namespace Markdown
{
    public interface ISingliTokenValidator
    {
        bool IsValidToken(string text, SingleMarkupToken token);
    }

    public class UnderscoresEscaping : ISingliTokenValidator
    {
        public bool IsValidToken(string text, SingleMarkupToken token)
        {
            if (token.StartIndex == 0) return true;
            return text[token.StartIndex - 1] != '\\';
        }
    }

    public class WhitespaceAfterUnderscores : ISingliTokenValidator
    {
        public bool IsValidToken(string text, SingleMarkupToken token)
        {
            if (token.StartIndex + token.TokenName.Length == text.Length) return true;
            return text[token.StartIndex + token.TokenName.Length] != ' ';
        }
    }


    public class UnderscoresInWordWithNumbers : ISingliTokenValidator
    {
        public bool IsValidToken(string text, SingleMarkupToken token)
        {
            if (token.StartIndex == 0) return true;
            if (token.StartIndex + token.TokenName.Length == text.Length) return true;
            var leftChar = text[token.StartIndex - 1];
            var rigthChar = text[token.StartIndex + token.TokenName.Length];
            return !(char.IsLetterOrDigit(leftChar) && char.IsDigit(rigthChar)
                     || char.IsLetterOrDigit(rigthChar) && char.IsDigit(leftChar));
        }
    }
}