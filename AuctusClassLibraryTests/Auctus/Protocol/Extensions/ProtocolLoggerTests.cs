using Auctus.Class.Library.Tests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skyline.DataMiner.Scripting;
using System;

namespace Auctus.DataMiner.Library.Protocol.Tests
{
    [TestClass]
    public class ProtocolLoggerTests
    {
        private MockProtocol mockProtocol = null;

        [TestInitialize]
        public void TestInitialize()
        {
            mockProtocol = new MockProtocol();
        }

        [TestMethod]
        [DataRow("Nothing to see here!", LogType.Information, LogLevel.DevelopmentLogging, "1|-1|QA123|TR456|Logger_String_Test|Nothing to see here!")]
        [DataRow("Starting particle accelerator.", LogType.DebugInfo, LogLevel.NoLogging, "4|0|QA123|TR456|Logger_String_Test|Starting particle accelerator.")]
        [DataRow("For your information.", LogType.Information, LogLevel.Level1, "1|1|QA123|TR456|Logger_String_Test|For your information.")]
        [DataRow("Cleanup in progress!", LogType.Allways, LogLevel.Level2, "8|2|QA123|TR456|Logger_String_Test|Cleanup in progress!")]
        [DataRow("An unexpected error occurred!", LogType.Error, LogLevel.LogEverything, "2|5|QA123|TR456|Logger_String_Test|An unexpected error occurred!")]
        public void Logger_String_Test(string message, LogType logType, LogLevel logLevel, string expected)
        {
            mockProtocol.Logger(message, logType, logLevel);

            mockProtocol.LastLogEntry.Should().Be(expected);
        }

        [TestMethod]
        public void Logger_Exception_Test()
        {
            mockProtocol.Logger(new Exception("An unexpected error occurred!"));

            mockProtocol.LastLogEntry.Should().Be("2|0|QA123|TR456|Logger_Exception_Test|Exception => System.Exception An unexpected error occurred!\r\n");
        }

        [TestMethod]
        public void Logger_1_Arg_Test()
        {
            mockProtocol.Logger("The temperature is {0}°C.", 20.4, LogType.Allways, LogLevel.DevelopmentLogging);

            mockProtocol.LastLogEntry.Should().Be("8|-1|QA123|TR456|Logger_1_Arg_Test|The temperature is 20.4°C.");
        }

        [TestMethod]
        public void Logger_2_Arg_Test()
        {
            var dt = new DateTime(1, 1, 1, 13, 24, 21);

            mockProtocol.Logger("At {0:t}, the temperature is {1}°C.", dt, 21.3, LogType.Allways, LogLevel.Level1);

            mockProtocol.LastLogEntry.Should().Be("8|1|QA123|TR456|Logger_2_Arg_Test|At 1:24 PM, the temperature is 21.3°C.");
        }

        [TestMethod]
        public void Logger_3_Arg_Test()
        {
            var dt = new DateTime(1, 1, 1, 9, 52, 45);

            mockProtocol.Logger("At {0:t}, the temperature is {1}°C and {2}.", dt, 17.1, "slightly overcast", LogType.Information, LogLevel.Level2);

            mockProtocol.LastLogEntry.Should().Be("1|2|QA123|TR456|Logger_3_Arg_Test|At 9:52 AM, the temperature is 17.1°C and slightly overcast.");
        }

        [TestMethod]
        public void Logger_Array_Arg_Test()
        {
            var date = new DateTime(2023, 03, 20);
            var hiTime = new TimeSpan(14, 17, 32);
            var hiTemp = 62.1m;
            var loTime = new TimeSpan(3, 16, 10);
            var loTemp = 54.8m;

            mockProtocol.Logger("Temperature on 20/03/2023:   14:17:32: 62.1 degrees (hi)   03:16:10: 54.8 degrees (lo)", null, LogType.Information, LogLevel.LogEverything);

            mockProtocol.LastLogEntry.Should().Be("1|5|QA123|TR456|Logger_Array_Arg_Test|Temperature on 20/03/2023:   14:17:32: 62.1 degrees (hi)   03:16:10: 54.8 degrees (lo)");

            mockProtocol.Logger("Temperature on {0:d}:{1,11}: {2} degrees (hi){3,11}: {4} degrees (lo)", new object[] { date, hiTime, hiTemp, loTime, loTemp }, LogType.Information, LogLevel.LogEverything);

            mockProtocol.LastLogEntry.Should().Be("1|5|QA123|TR456|Logger_Array_Arg_Test|Temperature on 20/03/2023:   14:17:32: 62.1 degrees (hi)   03:16:10: 54.8 degrees (lo)");
        }

        [TestMethod]
        [DataRow(true, LogType.Information, LogLevel.DevelopmentLogging, "1|-1|QA123|TR456|Logger_Bool_Test|True")]
        [DataRow(false, LogType.DebugInfo, LogLevel.NoLogging, "4|0|QA123|TR456|Logger_Bool_Test|False")]
        public void Logger_Bool_Test(bool value, LogType logType, LogLevel logLevel, string expected)
        {
            mockProtocol.Logger(value, logType, logLevel);

            mockProtocol.LastLogEntry.Should().Be(expected);
        }

        [TestMethod]
        [DataRow('A', LogType.Information, LogLevel.DevelopmentLogging, "1|-1|QA123|TR456|Logger_Char_Test|A")]
        [DataRow('*', LogType.DebugInfo, LogLevel.NoLogging, "4|0|QA123|TR456|Logger_Char_Test|*")]
        public void Logger_Char_Test(char value, LogType logType, LogLevel logLevel, string expected)
        {
            mockProtocol.Logger(value, logType, logLevel);

            mockProtocol.LastLogEntry.Should().Be(expected);
        }

        [TestMethod]
        [DataRow(new char[] { 'A', '*' }, LogType.Information, LogLevel.DevelopmentLogging, "1|-1|QA123|TR456|Logger_Char_Array_Test|A*")]
        [DataRow(new char[] { 'B', '-' }, LogType.DebugInfo, LogLevel.NoLogging, "4|0|QA123|TR456|Logger_Char_Array_Test|B-")]
        public void Logger_Char_Array_Test(char[] value, LogType logType, LogLevel logLevel, string expected)
        {
            mockProtocol.Logger(value, logType, logLevel);

            mockProtocol.LastLogEntry.Should().Be(expected);
        }

        [TestMethod]
        public void Logger_Char_Sub_Array_Test()
        {
            mockProtocol.Invoking(x => x.Logger(null, 0, 1))
                .Should().Throw<ArgumentNullException>()
                .WithMessage("Buffer cannot be null.\r\nParameter name: buffer");

            mockProtocol.Invoking(x => x.Logger(new char[] { '1' }, -1, 0))
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Index cannot be less than 0.\r\nParameter name: index");

            mockProtocol.Invoking(x => x.Logger(new char[] { '1' }, 0, -1))
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Count cannot be less than 0.\r\nParameter name: count");

            mockProtocol.Invoking(x => x.Logger(new char[] { '2' }, 2, 2))
                .Should().Throw<ArgumentException>()
                .WithMessage("Count cannot be bigger than the offsetted buffer.");

            mockProtocol.Logger(new char[] { 'B', 'A', '*', '-' }, 1, 2, LogType.DebugInfo, 0);

            mockProtocol.LastLogEntry.Should().Be("4|0|QA123|TR456|Logger_Char_Sub_Array_Test|A*");
        }

        [TestMethod]
        public void Logger_Double_Test()
        {
            mockProtocol.Logger(1234.45d, LogType.Information, LogLevel.DevelopmentLogging);

            mockProtocol.LastLogEntry.Should().Be("1|-1|QA123|TR456|Logger_Double_Test|1234.45");
        }

        [TestMethod]
        public void Logger_Decimal_Test()
        {
            mockProtocol.Logger(6.9M, LogType.Information, LogLevel.DevelopmentLogging);

            mockProtocol.LastLogEntry.Should().Be("1|-1|QA123|TR456|Logger_Decimal_Test|6.9");
        }

        [TestMethod]
        public void Logger_Float_Test()
        {
            mockProtocol.Logger(5.75F, LogType.Information, LogLevel.DevelopmentLogging);

            mockProtocol.LastLogEntry.Should().Be("1|-1|QA123|TR456|Logger_Float_Test|5.75");
        }

        [TestMethod]
        public void Logger_Int_Test()
        {
            mockProtocol.Logger(-123456789, LogType.Information, LogLevel.DevelopmentLogging);

            mockProtocol.LastLogEntry.Should().Be("1|-1|QA123|TR456|Logger_Int_Test|-123456789");
        }

        [TestMethod]
        public void Logger_UInt_Test()
        {
            mockProtocol.Logger(4294967295, LogType.Information, LogLevel.DevelopmentLogging);

            mockProtocol.LastLogEntry.Should().Be("1|-1|QA123|TR456|Logger_UInt_Test|4294967295");
        }

        [TestMethod]
        public void Logger_Long_Test()
        {
            mockProtocol.Logger(-123456789890, LogType.Information, LogLevel.DevelopmentLogging);

            mockProtocol.LastLogEntry.Should().Be("1|-1|QA123|TR456|Logger_Long_Test|-123456789890");
        }

        [TestMethod]
        public void Logger_ULong_Test()
        {
            mockProtocol.Logger(18446744073709551615, LogType.Information, LogLevel.DevelopmentLogging);

            mockProtocol.LastLogEntry.Should().Be("1|-1|QA123|TR456|Logger_ULong_Test|18446744073709551615");
        }

        [TestMethod]
        public void Logger_Object_Test()
        {
            var dt = (object)new DateTime(1, 1, 1, 9, 52, 45);

            mockProtocol.Logger(dt, LogType.Information, LogLevel.DevelopmentLogging);

            mockProtocol.LastLogEntry.Should().Be("1|-1|QA123|TR456|Logger_Object_Test|01/01/0001 9:52:45 AM");

            mockProtocol.Logger(new object[] { "Hello!" }, LogType.Information, LogLevel.DevelopmentLogging);

            mockProtocol.LastLogEntry.Should().Be("1|-1|QA123|TR456|Logger_Object_Test|System.Object[]");

            mockProtocol.Logger((object)null, LogType.Information, LogLevel.DevelopmentLogging);

            mockProtocol.LastLogEntry.Should().Be("1|-1|QA123|TR456|Logger_Object_Test|NULL");
        }
    }
}