using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HtmlNode
    {
        public MarkupToken Value;
        public HtmlNode Perent;
        public List<HtmlNode> Content;

        public HtmlNode()
        {
            Content = new List<HtmlNode>();
        }

        public HtmlNode(HtmlNode perent, MarkupToken value)
        {
            this.Value = value;
            Content = new List<HtmlNode>();
            this.Perent = perent;
        }

        public void AddNode(MarkupToken node)
        {
            var perent = Content.FirstOrDefault(x => x.Value.StartIndex < node.StartIndex && x.Value.FinishIndex > node.FinishIndex);
            if (perent == null)
                Content.Add(new HtmlNode(this, node));
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