using Auctus.DataMiner.Library.Auctus.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Auctus.DataMiner.Library.Common.Type.Tests
{
    [TestClass]
    public class TimeSpanExtensionsTests
    {
        [TestMethod]
        public void ToReadableString_Test()
        {
            var timespan = TimeSpan.MaxValue;

            timespan.ToReadableString(false, TimeSpanFormat.All).Should().Be("29247 years 6 weeks 2 days 2 hours 48 minutes 5 seconds 477 milliseconds");
            timespan.ToReadableString(true, TimeSpanFormat.All).Should().Be("29247y 6w 2d 2h 48m 5s 477ms");

            timespan.ToReadableString(false, TimeSpanFormat.Days | TimeSpanFormat.Seconds).Should().Be("10675199 days 10085 seconds");
            timespan.ToReadableString(true, TimeSpanFormat.Days | TimeSpanFormat.Seconds).Should().Be("10675199d 10085s");

            timespan.ToReadableString(false, TimeSpanFormat.Milliseconds).Should().Be("922337203685477 milliseconds");
            timespan.ToReadableString(true, TimeSpanFormat.Milliseconds).Should().Be("922337203685477ms");

            TimeSpan.Zero.ToReadableString(false, TimeSpanFormat.Milliseconds).Should().Be("0 seconds");
            TimeSpan.Zero.ToReadableString(true, TimeSpanFormat.Milliseconds).Should().Be("0s");
        }
    }
}