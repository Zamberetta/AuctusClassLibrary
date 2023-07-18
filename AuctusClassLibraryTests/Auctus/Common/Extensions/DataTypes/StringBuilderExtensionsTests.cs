using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Auctus.DataMiner.Library.Common.Type.Tests
{
    [TestClass]
    public class StringBuilderExtensionsTests
    {
        [TestMethod]
        public void LastAppended_Test()
        {
            var sb1 = new StringBuilder("Code is like humor.");
            var sb2 = new StringBuilder("It’s not a bug;");

            sb1.LastAppended().Should().BeNull();
            sb2.LastAppended().Should().BeNull();

            sb1.Append("When you have to explain it,", false);
            sb2.Append("it’s an undocumented feature.", true);

            sb1.LastAppended().Should().BeNull();
            sb2.LastAppended().Should().Be("it’s an undocumented feature.");

            sb1.AppendLine("it’s bad.", true);

            sb1.LastAppended().Should().Be("it’s bad.");
            sb2.LastAppended().Should().Be("it’s an undocumented feature.");
        }

        [TestMethod]
        public void LastIndexOf_Test()
        {
            var sb = new StringBuilder();
            sb.Append("Code is like humor.");

            sb.LastIndexOf('.').Should().Be(18);

            sb.AppendLine("When you have to explain it,");

            sb.LastIndexOf(',').Should().Be(46);

            sb.Append("it’s bad.");

            sb.LastIndexOf('’').Should().Be(51);

            sb.LastIndexOf('^').Should().Be(-1);
        }
    }
}