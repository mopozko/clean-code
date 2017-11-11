using System.Text;

namespace Markdown
{
    public class MarkupToken
    {
        public string Content { get; }
        public int StartIndex { get; }
        public int FinishIndex { get; }
        public MarkupToken(string content, int startIndex, int finishIndex)
        {
            this.Content = content;
            StartIndex = startIndex;
            FinishIndex = finishIndex;
        }
    }

    public class HtmlTagToken : MarkupToken
    {
        public string TagName { get; }
        public HtmlTagToken(string tagName, int startIndex, int finishIndex) : base(tagName, startIndex, finishIndex)
        {
            this.TagName = tagName;
        }

        public void Open(StringBuilder builder)
        {
            builder.Append($"<{TagName}>");
        }

        public void Close(StringBuilder builder)
        {
            builder.Append($"</{TagName}>");
        }

    }

    public class EmTagToken : HtmlTagToken
    {
        public EmTagToken(int startIndex, int finishIndex) : base("em", startIndex, finishIndex) { }
    }

    public class StrongTagToken : HtmlTagToken
    {
        public StrongTagToken(int startIndex, int finishIndex) : base("strong", startIndex, finishIndex) { }
    }

}