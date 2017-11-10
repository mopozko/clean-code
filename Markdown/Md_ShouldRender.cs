using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    public class Md_ShouldRender
    {
        private Md Md;
        [SetUp]
        public void Initialize()
        {
            Md = new Md();
        }
        public void RenderToHtml_Equality(string inputMd, string outputHtml)
        {
            Md.RenderToHtml(inputMd)
                .Should()
                .Be(outputHtml);
        }

        [Test]
        public void RenderToHtml_SetEm()
        {
            var input = "qwer_qwer_qwer";
            var output = "qwer<em>qwer</em>qwer";
            RenderToHtml_Equality(input, output);
        }
        [Test]
        public void RenderToHtml_SetStrong()
        {
            var input = "qwer__qwer__qwer";
            var output = "qwer<strong>qwer</strong>qwer";
            RenderToHtml_Equality(input, output);
        }
        [Test]
        public void RenderToHtml_CommandEscaping()
        {
            var input = "qwer\\_qwer\\_qwer";
            var output = "qwer_qwer_qwer";
            RenderToHtml_Equality(input, output);
        }

        [Test]
        public void asd()
        {
            var input = "asd_asd__a__sd_asd_ASd__a_1_1";
            var output = "asd<em>asd__a__sd</em>asd_ASd__a_1_1";
            RenderToHtml_Equality(input, output);
        }
        [Test]
        public void asd1()
        {
            var input = "asd__asd_a_sd__asd_ASd__a_1_1";
            var output = "asd<strong>asd<em>a</em>sd</strong>asd_ASd__a_1_1";
            RenderToHtml_Equality(input, output);
        }
        [Test]
        public void RenderToHtml_EmInstrong()
        {
            var input = "qwer__qwer_qwer_qwer__qwer";
            var output = "qwer" +
                         "<strong>" +
                            "qwer" +
                                "<em>qwer</em>" +
                            "qwer" +
                         "</strong>" +
                         "qwer";
            RenderToHtml_Equality(input, output);
        }
        [Test]
        public void RenderToHtml_IgnoreDoubleUnderscoresInSingle()
        {
            var input = "qwer_qwer__qwer__qwer_qwer";
            var output = "qwer" +
                         "<em>" +
                             "qwer__qwer__qwer" +
                         "</em>" +
                         "qwer";
             RenderToHtml_Equality(input, output);
        }

        [Test, Timeout(1000)]
        public void RenderToHtml_TimeoutTest()
        {
            var input = "qwer__qwer_qwer_qwer__qwer";
            var output = "qwer" +
                         "<strong>" +
                         "qwer" +
                         "<em>qwer</em>" +
                         "qwer" +
                         "</strong>" +
                         "qwer";
            for (var i =0; i < 10000; i++)
                RenderToHtml_Equality(input, output);
        }



        [Test]
        public void RenderToHtml_IgnoreUnderscoresInStringWithNumbers()
        {
            var input = "qwer_123_123_qwer";
            var output = "qwer_123_123_qwer";
            RenderToHtml_Equality(input, output);
        }

        [Test]
        public void RenderToHtml_WhitespaceAfterUnderscores()
        {
            var input = "qwer_ qwer_ ";
            var output = "qwer_ qwer_ ";
            RenderToHtml_Equality(input, output);
        }

        [Test]
        public void RenderToHtml_IgnoreNotDoubleUnderscores()
        {
            var input = "qwer__qwer_qwer";
            var output = "qwer__qwer_qwer";
            RenderToHtml_Equality(input, output);
        }
    }
}