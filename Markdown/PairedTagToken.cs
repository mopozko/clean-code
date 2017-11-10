namespace Markdown
{
    public class PairedTagToken : SingleTagToken
    {
        public int FinishIndex { get; }
        public PairedTagToken(int startIndex, int finishIndex) : base(startIndex)
        {
            FinishIndex = finishIndex;
        }
    }
}