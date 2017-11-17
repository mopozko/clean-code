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
            //var input = "_a __b_";
            //var input = "__фыв___ф\\__";

            //Console.WriteLine(md.RenderToHtml(input));

            var tree = new AhoCorasickTree(new []{"qwe","rty"});
            var input = "qwerty";
            Console.WriteLine(input);
            var res = tree.Find(input);
            foreach (var s in res)
            {
                Console.WriteLine($"{s.StartIndex} {s.TokenName}");
            }
            tree = new AhoCorasickTree(new[] { "_", "__" });
            input = "_q__q__q_";
            Console.WriteLine(input);
            res = tree.Find(input);
            foreach (var s in res)
            {
                Console.WriteLine($"{s.StartIndex} {s.TokenName}");
            }


            //var context = "asdfghj";
            //var token = new SingleMarkupToken("df",2);
            //Console.WriteLine(TextHelper.OnTheRightIs(context,token,"gh"));
        }
	}
}
