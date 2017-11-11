using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HtmlTree
    {
        public HtmlNode RootNode;

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
    }
}