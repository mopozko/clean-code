using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class HtmlTreeReader : ITreeReader
    {
        private static readonly Dictionary<string, string> MarkdownToHtmlMatching = new Dictionary<string, string>
        {
            {"_","em" },
            {"__","strong" }
        };
        public string Read(MarkupTree tree)
        {
            var builder = new StringBuilder();
            Read(tree.RootNode, builder);
            return builder.ToString();
        }
        private void Read(MarkupNode node, StringBuilder builder)
        {
            var content = node.Content.OrderBy(x => x.Value.StartIndex).ToArray();
            var value = node.Value;
            var parentValue = node.Parent?.Value;
            if (value == null)
                ReadContent();
            else if (node.Value is TextToken textValue)
                builder.Append(textValue.Text);
            else
            {
                TagToken tag;
                if (parentValue != null && parentValue.Content == "_" && value.Content == "__")
                    tag = new MarkdownTagToken(value);
                else
                    tag = new HtmlTagToken(new MarkupToken(MarkdownToHtmlMatching[value.Content],value.StartIndex,value.FinishIndex));
                tag.Open(builder);
                ReadContent();
                tag.Close(builder);
            }

            void ReadContent()
            {
                foreach (var tag in content)
                    Read(tag, builder);
            }
        }
    }
}