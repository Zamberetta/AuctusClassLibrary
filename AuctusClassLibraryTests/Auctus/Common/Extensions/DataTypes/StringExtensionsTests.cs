using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static FluentAssertions.FluentActions;

namespace Auctus.DataMiner.Library.Common.Type.Tests
{
    [TestClass()]
    public class StringExtensionsTests
    {
        private enum Level
        {
            Low = 0,
            Medium = 15,
            High = 30,
        }

        [TestMethod]
        public void ToEnum_Test()
        {
            var validString = "Medium";
            var invalidString = "Friend";
            string nullString = null;

            validString.ToEnum<Level>(true).Should().Be(Level.Medium);
            validString.ToEnum<Level>(false).Should().Be(Level.Medium);

            invalidString.ToEnum<Level>(true).Should().Be(Level.Low);
            Invoking(() => invalidString.ToEnum<Level>(false)).Should().Throw<ArgumentException>();

            nullString.ToEnum<Level>(true).Should().Be(Level.Low);
            Invoking(() => nullString.ToEnum<Level>(false)).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void ParseReadableTimeSpan_Test()
        {
            var timespan = TimeSpan.MaxValue;

            timespan.ToReadableString(false, TimeSpanExtensions.Format.All).ParseReadableTimeSpan().Should().Be(timespan);
            timespan.ToReadableString(true, TimeSpanExtensions.Format.All).ParseReadableTimeSpan().Should().Be(timespan);

            timespan = new TimeSpan(1, 1, 1, 1, 1);

            timespan.ToReadableString(false, TimeSpanExtensions.Format.All).ParseReadableTimeSpan().Should().Be(timespan);
            timespan.ToReadableString(true, TimeSpanExtensions.Format.All).ParseReadableTimeSpan().Should().Be(timespan);

            timespan = TimeSpan.FromDays(12345);

            timespan.ToReadableString(false, TimeSpanExtensions.Format.Days).ParseReadableTimeSpan().Should().Be(timespan);
            timespan.ToReadableString(true, TimeSpanExtensions.Format.Days).ParseReadableTimeSpan().Should().Be(timespan);

            timespan = new TimeSpan(12, 34, 56);

            timespan.ToReadableString(false, TimeSpanExtensions.Format.Time).ParseReadableTimeSpan().Should().Be(timespan);
            timespan.ToReadableString(true, TimeSpanExtensions.Format.Time).ParseReadableTimeSpan().Should().Be(timespan);

            string.Empty.ParseReadableTimeSpan().Should().Be(TimeSpan.Zero);
        }
    }
}