namespace Markdown
{
    public class SingleMarkupToken
    {
        public string TokenName { get; }
        public int StartIndex { get; }
        public SingleMarkupToken(string tokenName, int startIndex)
        {
            this.TokenName = tokenName;
            this.StartIndex = startIndex;
        }
    }
}