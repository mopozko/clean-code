namespace Markdown
{
    public class SingleMdToken : SingleTagToken
    {
        public string TokenName { get; }
        public SingleMdToken(string tokenName, int startIndex) : base(startIndex)
        {
            this.TokenName = tokenName;
        }
    }
}