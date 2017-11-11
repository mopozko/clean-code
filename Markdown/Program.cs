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
            var input = "qwer_qwer_qwer";

            Console.WriteLine(md.RenderToHtml(input));
            
        }
	}
}
