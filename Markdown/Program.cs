using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Markdown
{
	class Program
	{
		static void Main(string[] args)
        {
            var md = new Md();
            var input = "qwer__qwer_qwer_qwer__qwer";

            for (var i = 0; i < 10000000; i++)
            {
                md.RenderToHtml(input);
            }
        }
	}
}
