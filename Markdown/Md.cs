using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Markdown
{
	public class Md
    {
        private readonly Dictionary<string, Func<int,int, MarkupToken>> TagMatching;

        private readonly List<ISingliTokenValidator> SingleTokenValidators;

        public Md()
        {
            SingleTokenValidators = new List<ISingliTokenValidator>
            {
                new UnderscoresEscaping(),
                new UnderscoresInWordWithNumbers(),
                new WhitespaceAfterUnderscores()
            };
            TagMatching = new Dictionary<string, Func<int,int, MarkupToken>>
            {
                {"__",(start,finish) => new StrongTagToken(start,finish)},
                {"_",(start,finish) => new EmTagToken(start,finish)}
            };
        }

        public MarkupToken GetHtmlTagTokenByMdTagName(string mdName, int startIndex, int finishIndex)
        {
            return TagMatching[mdName](startIndex,finishIndex);
        }

        private bool TryParseTagToken(string text, int startIndex, ref SingleMarkupToken token)
        {
            foreach (var tagName in TagMatching.Keys)
            {
                if(startIndex + tagName.Length > text.Length)
                    continue;

                if (!text.Substring(startIndex, tagName.Length).Equals(tagName))
                    continue;

                var tempToken = new SingleMarkupToken(tagName, startIndex);
                if (!IsValidSingleToken(text, tempToken))
                    continue;

                token = tempToken;
                return true;
            }
            return false;
        }

        private IEnumerable<MarkupToken> ParseAllTokens(string text)
        {
            var index = 0;
            var startTextTokenIndex = 0;
            var openningTags = new Stack<SingleMarkupToken>();

            while (index < text.Length)
            {
                SingleMarkupToken token = null;
                if (TryParseTagToken(text, index, ref token))
                {
                    yield return ParseTextToken(startTextTokenIndex, token.StartIndex, text);
                    startTextTokenIndex = index + token.TokenName.Length;

                    if (openningTags.Any() && openningTags.Peek().TokenName == token.TokenName)
                    {
                        var openingTag = openningTags.Pop();
                        yield return GetHtmlTagTokenByMdTagName(token.TokenName, openingTag.StartIndex, token.StartIndex);
                    }
                    else openningTags.Push(token);

                    index += token.TokenName.Length;
                }
                else index++;
            }

            yield return ParseTextToken(startTextTokenIndex, text.Length, text);
            //return not closed tokens as text token
            foreach (var singleToken in openningTags)
                yield return new TextToken(singleToken.TokenName, singleToken.StartIndex);
        }
        
        public TextToken ParseTextToken(int startIndex, int finishIndex, string text)
        {
            var lenght = finishIndex - startIndex;
            var content = text.Substring(startIndex, lenght);
            return new TextToken(content.Replace("\\_","_"), startIndex);
        }

        private bool IsValidSingleToken(string text,SingleMarkupToken token)
        {
            return SingleTokenValidators
                .All(validator => validator.IsValidToken(text, token));
        }

        public string RenderToHtml(string markdown)
        {
            var allTokens = ParseAllTokens(markdown).ToList();
            var tree = new HtmlTree(allTokens);
            return tree.Print();
		}
    }
}