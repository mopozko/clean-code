using System.Collections.Generic;

namespace Markdown
{
    public interface ITokenParser
    {
        IEnumerable<SingleMarkupToken> Parse(string input);
    }
}