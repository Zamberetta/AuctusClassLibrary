using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Auctus.DataMiner.Library.Common.Type.Tests
{
    [TestClass]
    public class IntegerExtensionsTests
    {
        private enum Level
        {
            Low = 0,
            Medium = 15,
            High = 30,
        }

        [TestMethod]
        public void ToBool_Test()
        {
            0.ToBool().Should().BeFalse();
            123.ToBool().Should().BeFalse();
            1.ToBool().Should().BeTrue();
        }

        [TestMethod]
        public void ToEnum_Test()
        {
            0.ToEnum<Level>().Should().Be(Level.Low);
            30.ToEnum<Level>().Should().Be(Level.High);
            123.ToEnum<Level>().Should().Be(Level.Low);
        }
    }
}