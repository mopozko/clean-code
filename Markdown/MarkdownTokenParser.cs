using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Markdown
{
    public class MarkdownTokenParser : ITokenParser
    {
        private Dictionary<string, ISingleTokenParsingRool> SingleTokenValidators { get; }
        private AhoCorasickTree Tree { get;}
        public MarkdownTokenParser()
        {
            SingleTokenValidators = new Dictionary<string, ISingleTokenParsingRool>
            {
                {"_", new DefaultSingleTokenParsingRool()},
                {"__", new DefaultSingleTokenParsingRool()}
            };
            Tree = new AhoCorasickTree(SingleTokenValidators.Keys);
        }

        public IEnumerable<SingleMarkupToken> RemoveIntersectionTokens(IEnumerable<SingleMarkupToken> allTokens, string text)
        {
            return allTokens
                .GroupBy(x => x.StartIndex, x => x)
                .Select(x => x.OrderByDescending(y => y.TokenName.Length).First())
                .Where(x => IsValidSingleToken(text, x));
        }

        public IEnumerable<SingleMarkupToken> Parse(string text)
        {
            var allTokens = Tree.Find(text);
            return RemoveIntersectionTokens(allTokens, text);
        }
        
        private bool IsValidSingleToken(string text, SingleMarkupToken token)
        {
            return SingleTokenValidators[token.TokenName].IsValidToken(token,text);
        }
    }
}