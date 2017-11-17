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

    public abstract class TagToken : MarkupToken
    {
        protected TagToken(MarkupToken token) 
            : base(token.Content, token.StartIndex, token.FinishIndex) {}
        public abstract void Open(StringBuilder builder);
        public abstract void Close(StringBuilder builder);
    }

    public class MarkdownTagToken : TagToken
    {
        public MarkdownTagToken(MarkupToken token): base(token) { }

        public override void Open(StringBuilder builder)
        {
            builder.Append(Content);
        }

        public override void Close(StringBuilder builder)
        {
            builder.Append(Content);
        }
    }


    public class HtmlTagToken : TagToken
    {
        public string TagName { get; }
        public HtmlTagToken(MarkupToken token) : base(token)
        {
            this.TagName = token.Content;
        }

        public override  void Open(StringBuilder builder)
        {
            builder.Append($"<{TagName}>");
        }

        public override void Close(StringBuilder builder)
        {
            builder.Append($"</{TagName}>");
        }

    }
}