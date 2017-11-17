using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Markdown
{
	public class Md
    {        
        private ITokenParser TokenParser { get; }
        private ITreeReader TreeReader { get; }
        public Md()
        {
            TokenParser = new MarkdownTokenParser();         
            TreeReader = new HtmlTreeReader();
        }
        
        private MarkupToken SingleToDoubleMarkupToken(SingleMarkupToken open, SingleMarkupToken close)
        {
            return new MarkupToken(open.TokenName,open.StartIndex,close.StartIndex);
        }

        private IEnumerable<MarkupToken> ParseAllTokens(string text)
        {
            var startTextTokenIndex = 0;

            var openningTags = new List<SingleMarkupToken>();

            foreach (var token in TokenParser.Parse(text))
            {
                yield return ParseTextToken(startTextTokenIndex, token.StartIndex, text);

                startTextTokenIndex = token.StartIndex + token.TokenName.Length;

                var openingTag = openningTags.FirstOrDefault(x => x.TokenName == token.TokenName);
                if (openingTag != null)
                {
                    yield return SingleToDoubleMarkupToken(openingTag, token);
                    openningTags.Remove(openingTag);
                }
                else openningTags.Add(token);
            }

            yield return ParseTextToken(startTextTokenIndex, text.Length, text);
            
            //return not closed tokens as text token
            foreach (var singleToken in openningTags)
                yield return new TextToken(singleToken.TokenName, singleToken.StartIndex);
        }
        
        private TextToken ParseTextToken(int startIndex, int finishIndex, string text)
        {
            var lenght = finishIndex - startIndex;
            var content = text.Substring(startIndex, lenght);
            return new TextToken(content.Replace("\\_","_"), startIndex);
        }

        public string RenderToHtml(string markdown)
        {
            var allTokens = ParseAllTokens(markdown).ToList();
            var tree = new MarkupTree(allTokens);
            return TreeReader.Read(tree);
		}
    }
}