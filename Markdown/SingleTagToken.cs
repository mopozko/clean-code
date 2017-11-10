namespace Markdown
{
    public class SingleTagToken
    {
        public int StartIndex { get; }

        public SingleTagToken(int startIndex)
        {
            StartIndex = startIndex;
        }
    }
}