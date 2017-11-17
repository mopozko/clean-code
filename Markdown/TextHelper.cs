namespace Markdown
{
    public static class TextHelper
    {
        public static string GetPreviousWord(string context, SingleMarkupToken token, int lenght)
        {
            return context.Substring(token.StartIndex - lenght, lenght);
        }

        public static string GetNextWord(string context, SingleMarkupToken token, int lenght)
        {
            return context.Substring(token.StartIndex + token.TokenName.Length, lenght);
        }

        public static char GetPreviousChar(string context, SingleMarkupToken token)
        {
            return context[token.StartIndex - 1];
        }

        public static char GetNextChar(string context, SingleMarkupToken token)
        {
            return context[token.StartIndex + token.TokenName.Length];
        }

        public static bool OnTheLeftIs(string context, SingleMarkupToken token, string word)
        {
            return GetPreviousWord(context, token, word.Length) == word;
        }

        public static bool OnTheLeftIs(string context, SingleMarkupToken token, char c)
        {
            return GetPreviousChar(context, token) == c;
        }
        public static bool OnTheRightIs(string context, SingleMarkupToken token, string word)
        {
            return GetNextWord(context, token, word.Length) == word;
        }

        public static bool OnTheRightIs(string context, SingleMarkupToken token, char c)
        {
            return GetNextChar(context, token) == c;
        }
    }
}