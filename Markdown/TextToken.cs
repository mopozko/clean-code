namespace Markdown
{
    public class TextToken : MarkupToken
    {
        public string Text { get; }

        public TextToken(string text, int startIndex) : base(text,startIndex, startIndex + text.Length - 1)
        {
            this.Text = text;
        }
    }
}