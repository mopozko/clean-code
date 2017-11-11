using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HtmlTree
    {
        private HtmlNode RootNode;

        public HtmlTree(IEnumerable<MarkupToken> tags)
        {
            RootNode = new HtmlNode();
            foreach (var tag in tags.OrderBy(x => x.StartIndex))
                RootNode.AddNode(tag);
        }

        public string Print()
        {
            var builder = new StringBuilder();
            RootNode.Print(builder);
            return builder.ToString();
        }

        private class HtmlNode
        {
            MarkupToken Value;
            List<HtmlNode> Content;

            public HtmlNode()
            {
                Content = new List<HtmlNode>();
            }

            public HtmlNode(MarkupToken value)
            {
                this.Value = value;
                Content = new List<HtmlNode>();
            }

            public void AddNode(MarkupToken node)
            {
                var perent = Content.FirstOrDefault(x => x.Value.StartIndex < node.StartIndex && x.Value.FinishIndex > node.FinishIndex);
                if (perent == null)
                    Content.Add(new HtmlNode(node));
                else
                {
                    if (node is StrongTagToken token && perent.Value is EmTagToken)
                    {
                        perent.AddNode(new TextToken("__", token.StartIndex));
                        perent.AddNode(new TextToken("__", token.FinishIndex));
                    }
                    else perent.AddNode(node);
                }
            }

            public void Print(StringBuilder builder)
            {
                var content = Content.OrderBy(x => x.Value.StartIndex);
                switch (Value)
                {
                    case null:
                    {
                        foreach (var tag in content)
                            tag.Print(builder);
                        break;
                    }
                    case TextToken value:
                    {
                        builder.Append(value.Text);
                        break;
                    }
                    case HtmlTagToken value:
                    {
                        value.Open(builder);
                        foreach (var tag in content)
                            tag.Print(builder);
                        value.Close(builder);
                        break;
                    }
                }
            }
        }
    }
}