using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class MarkupTree
    {
        public MarkupNode RootNode;

        public MarkupTree(IEnumerable<MarkupToken> tags)
        {
            RootNode = new MarkupNode();
            foreach (var tag in tags.OrderBy(x => x.StartIndex))
                RootNode.AddNode(tag);
        }

    }
    public class MarkupNode
    {
        public MarkupToken Value;
        public List<MarkupNode> Content;
        public MarkupNode Parent;
        public MarkupNode()
        {
            Content = new List<MarkupNode>();
            Parent = null;
        }

        public MarkupNode(MarkupToken value, MarkupNode parent)
        {
            this.Value = value;
            Content = new List<MarkupNode>();
            Parent = parent;
        }

        public void AddNode(MarkupToken node)
        {
            var parent = Content.FirstOrDefault(x => x.Value.StartIndex < node.StartIndex && x.Value.FinishIndex > node.FinishIndex);
            if (parent == null)
                Content.Add(new MarkupNode(node,this));
            else
            {
               parent.AddNode(node);
            }
        }
    }
}