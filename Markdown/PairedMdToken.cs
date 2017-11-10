namespace Markdown
{
    public class PairedMdToken : PairedTagToken
    {
        public string TokenName { get; }
        public PairedMdToken(string tagName, int startIndex, int finishIndex) : base(startIndex, finishIndex)
        {
            this.TokenName = tagName;
        }
    }
}