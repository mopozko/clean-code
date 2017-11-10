namespace Markdown
{
    public class TextToken : PairedTagToken
    {
        public string Text { get; }

        public TextToken(string text, int startIndex) : base(startIndex, startIndex + text.Length - 1)
        {
            this.Text = text;
        }
    }
}