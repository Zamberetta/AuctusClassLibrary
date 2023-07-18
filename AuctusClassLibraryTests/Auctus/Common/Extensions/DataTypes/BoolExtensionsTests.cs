using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Auctus.DataMiner.Library.Common.Type.Tests
{
    [TestClass]
    public class BoolExtensionsTests
    {
        [TestMethod]
        public void ToInt_Test()
        {
            false.ToInt().Should().Be(0);
            true.ToInt().Should().Be(1);
        }
    }
}