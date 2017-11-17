namespace Markdown
{
    public interface ISingleTokenValidator
    {
        bool IsValidToken(string text, SingleMarkupToken token);
    }

    public class UnderscoresEscaping : ISingleTokenValidator
    {
        public bool IsValidToken(string text, SingleMarkupToken token)
        {
            if (token.StartIndex == 0) return true;

            return !TextHelper.OnTheLeftIs(text, token, '\\');
        }
    }

    public class WhitespaceAfterUnderscores : ISingleTokenValidator
    {
        public bool IsValidToken(string text, SingleMarkupToken token)
        {
            if (token.StartIndex + token.TokenName.Length == text.Length) return true;
            return !TextHelper.OnTheRightIs(text,token, ' ');
        }
    }


    public class UnderscoresInWordWithNumbers : ISingleTokenValidator
    {
        public bool IsValidToken(string text, SingleMarkupToken token)
        {
            if (token.StartIndex == 0) return true;
            if (token.StartIndex + token.TokenName.Length == text.Length) return true;
            var leftChar = TextHelper.GetPreviousChar(text,token);
            var rigthChar = TextHelper.GetNextChar(text, token);
            return !(char.IsLetterOrDigit(leftChar) && char.IsDigit(rigthChar)
                  || char.IsLetterOrDigit(rigthChar) && char.IsDigit(leftChar));
        }
    }
}