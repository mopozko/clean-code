using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Markdown
{
	public class Md
    {
        private readonly Dictionary<string, Func<PairedTagToken, HtmlTagToken>> TagMatching;

        private readonly List<ISingliTokenValidator> SingleTokenValidators;

        public Md()
        {
            SingleTokenValidators = new List<ISingliTokenValidator>
            {
                new EscapingUnderscores(),
                new UnderscoresInWordWithNumbers(),
                new WhitespaceAfterUnderscores()
            };
            TagMatching = new Dictionary<string, Func<PairedTagToken, HtmlTagToken>>
            {
                {"__",token => new StrongTagToken(token.StartIndex,token.FinishIndex)},
                {"_",token => new EmTagToken(token.StartIndex,token.FinishIndex)}
            };
        }

        public HtmlTagToken GetHtmlTagTokenByMdTagName(string mdName, PairedTagToken token)
        {
            return TagMatching[mdName](token);
        }
        
        private IEnumerable<SingleMdToken> ParseSingleTokens(string input)
        {
            var index = 0;
            while (index < input.Length)
            {
                var find = false;
                foreach (var tagName in TagMatching.Keys)
                {
                    if (index + tagName.Length > input.Length)
                        continue;
                    if (!input.Substring(index, tagName.Length).Equals(tagName))
                        continue;
                    var token = new SingleMdToken(tagName, index);
                    if (IsValidSingleToken(input, token))
                        yield return token;
                    index += tagName.Length;
                    find = true;
                    break;
                }
                if (!find) index++;
            }   
        }

        private IEnumerable<PairedTagToken> GroopTokensToPairs(IEnumerable<SingleMdToken> tokens)
        {
            var openningTags = new Stack<SingleMdToken>();
            foreach (var token in tokens)
            {
                if (openningTags.Count == 0)
                    openningTags.Push(token);
                else if (openningTags.Peek().TokenName == token.TokenName)
                {
                    var openingToken = openningTags.Pop();
                    yield return GetHtmlTagTokenByMdTagName(token.TokenName, new PairedTagToken(openingToken.StartIndex, token.StartIndex));
                }
                else openningTags.Push(token);
            }
            foreach (var singleToken in openningTags)
                yield return new TextToken(singleToken.TokenName,singleToken.StartIndex);
        }

        public TextToken ParseTextToken(SingleMdToken startToken, int finishIndex, string text)
        {
            var startIndex = startToken.StartIndex + startToken.TokenName.Length;
            var lenght = finishIndex - startIndex;
            var content = text.Substring(startIndex, lenght);
            return new TextToken(content,startIndex);
        }

        public IEnumerable<TextToken> ParseTextTokens(string text, List<SingleMdToken> tokens)
        {
            tokens.Add(new SingleMdToken("",0));
            tokens.Add(new SingleMdToken("",text.Length));
            tokens = tokens.OrderBy(x => x.StartIndex).ToList();
            for (var i = 0; i < tokens.Count - 1; i++)
            {
                var startTag = tokens[i];
                var finishTag = tokens[i + 1];
                yield return ParseTextToken(startTag, finishTag.StartIndex, text);
            }
        }

        private bool IsValidSingleToken(string text,SingleMdToken token)
        {
            return SingleTokenValidators
                .All(validator => validator.IsValidToken(text, token));
        }

        public string RenderToHtml(string markdown)
        {
            var allTokens = ParseSingleTokens(markdown).ToList();
            var textTokens = ParseTextTokens(markdown, allTokens)
                .Select(x => new TextToken(x.Text.Replace("\\_", "_"),x.StartIndex));
            var groopingTokens = GroopTokensToPairs(allTokens);
            var allTags = new List<PairedTagToken>();
            allTags.AddRange(groopingTokens);
            allTags.AddRange(textTokens);
            var tree = new HtmlTree(allTags);
            return tree.Print();
		}
    }
}