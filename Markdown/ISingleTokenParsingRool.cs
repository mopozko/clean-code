using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public interface ISingleTokenParsingRool
    {
        bool IsValidToken(SingleMarkupToken token, string text);
    }

    public class DefaultSingleTokenParsingRool : ISingleTokenParsingRool
    {
        private readonly List<ISingleTokenValidator> singleTokenValidators = new List<ISingleTokenValidator>
        {
            new UnderscoresEscaping(),
            new UnderscoresInWordWithNumbers(),
            new WhitespaceAfterUnderscores()
        };
        public bool IsValidToken(SingleMarkupToken token, string text)
        {
            return singleTokenValidators
                .All(validator => validator.IsValidToken(text, token));
        }
    }
}