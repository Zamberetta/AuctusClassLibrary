using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skyline.DataMiner.Automation;
using System;

namespace Auctus.DataMiner.Library.Automation.Tests
{
    [TestClass]
    public class EngineLoggerTests
    {
        private Engine mockEngine = null;
        private string logMessage = string.Empty;

        [TestInitialize]
        public void TestInitialize()
        {
            var mock = new Mock<Engine>();

            mock.Setup(x => x.Log(It.IsAny<string>(), It.IsAny<LogType>(), It.IsAny<int>()))
                .Callback<string, LogType, int>((message, logType, logLevel) =>
                {
                    logMessage = $"{logType}|{logLevel}|{message}";
                });

            mockEngine = mock.Object;
        }

        [TestMethod]
        [DataRow("Nothing to see here!", LogType.None, -1, "None|-1|ID0|Logger_String_Test|Nothing to see here!")]
        [DataRow("Starting particle accelerator.", LogType.Debug, 0, "Debug|0|ID0|Logger_String_Test|Starting particle accelerator.")]
        [DataRow("For your information.", LogType.Information, 1, "Information|1|ID0|Logger_String_Test|For your information.")]
        [DataRow("Cleanup in progress!", LogType.Always, 2, "Always|2|ID0|Logger_String_Test|Cleanup in progress!")]
        [DataRow("An unexpected error occurred!", LogType.Error, 5, "Error|5|ID0|Logger_String_Test|An unexpected error occurred!")]
        public void Logger_String_Test(string message, LogType logType, int logLevel, string expected)
        {
            mockEngine.Logger(message, logType, logLevel);

            logMessage.Should().Be(expected);
        }

        [TestMethod]
        public void Logger_Exception_Test()
        {
            mockEngine.Logger(new Exception("An unexpected error occurred!"));

            logMessage.Should().Be("Error|1|ID0|Logger_Exception_Test|Exception => System.Exception An unexpected error occurred!\r\n");
        }

        [TestMethod]
        public void Logger_1_Arg_Test()
        {
            mockEngine.Logger("The temperature is {0}°C.", 20.4, LogType.Information, -1);

            logMessage.Should().Be("Information|-1|ID0|Logger_1_Arg_Test|The temperature is 20.4°C.");
        }

        [TestMethod]
        public void Logger_2_Arg_Test()
        {
            var dt = new DateTime(1, 1, 1, 13, 24, 21);

            mockEngine.Logger("At {0:t}, the temperature is {1}°C.", dt, 21.3, LogType.Always, 1);

            logMessage.Should().Be("Always|1|ID0|Logger_2_Arg_Test|At 1:24 PM, the temperature is 21.3°C.");
        }

        [TestMethod]
        public void Logger_3_Arg_Test()
        {
            var dt = new DateTime(1, 1, 1, 9, 52, 45);

            mockEngine.Logger("At {0:t}, the temperature is {1}°C and {2}.", dt, 17.1, "slightly overcast", LogType.None, 2);

            logMessage.Should().Be("None|2|ID0|Logger_3_Arg_Test|At 9:52 AM, the temperature is 17.1°C and slightly overcast.");
        }

        [TestMethod]
        public void Logger_Array_Arg_Test()
        {
            var date = "20/03/2023";
            var hiTime = new TimeSpan(14, 17, 32);
            var hiTemp = 62.1m;
            var loTime = new TimeSpan(3, 16, 10);
            var loTemp = 54.8m;

            mockEngine.Logger("Temperature on 20/03/2023: 14:17:32: 62.1 degrees (hi) 03:16:10: 54.8 degrees (lo)", null, LogType.Information, -1);

            logMessage.Should().Be("Information|-1|ID0|Logger_Array_Arg_Test|Temperature on 20/03/2023: 14:17:32: 62.1 degrees (hi) 03:16:10: 54.8 degrees (lo)");

            mockEngine.Logger("Temperature on {0}: {1}: {2} degrees (hi) {3}: {4} degrees (lo)", new object[] { date, hiTime, hiTemp, loTime, loTemp }, LogType.Information, -1);

            logMessage.Should().Be("Information|-1|ID0|Logger_Array_Arg_Test|Temperature on 20/03/2023: 14:17:32: 62.1 degrees (hi) 03:16:10: 54.8 degrees (lo)");
        }

        [TestMethod]
        [DataRow(true, LogType.None, -1, "None|-1|ID0|Logger_Bool_Test|True")]
        [DataRow(false, LogType.Debug, 0, "Debug|0|ID0|Logger_Bool_Test|False")]
        public void Logger_Bool_Test(bool value, LogType logType, int logLevel, string expected)
        {
            mockEngine.Logger(value, logType, logLevel);

            logMessage.Should().Be(expected);
        }

        [TestMethod]
        [DataRow('A', LogType.None, -1, "None|-1|ID0|Logger_Char_Test|A")]
        [DataRow('*', LogType.Debug, 0, "Debug|0|ID0|Logger_Char_Test|*")]
        public void Logger_Char_Test(char value, LogType logType, int logLevel, string expected)
        {
            mockEngine.Logger(value, logType, logLevel);

            logMessage.Should().Be(expected);
        }

        [TestMethod]
        [DataRow(new char[] { 'A', '*' }, LogType.None, -1, "None|-1|ID0|Logger_Char_Array_Test|A*")]
        [DataRow(new char[] { 'B', '-' }, LogType.Debug, 0, "Debug|0|ID0|Logger_Char_Array_Test|B-")]
        public void Logger_Char_Array_Test(char[] value, LogType logType, int logLevel, string expected)
        {
            mockEngine.Logger(value, logType, logLevel);

            logMessage.Should().Be(expected);
        }

        [TestMethod]
        public void Logger_Char_Sub_Array_Test()
        {
            mockEngine.Invoking(x => x.Logger(null, 0, 1))
                .Should().Throw<ArgumentNullException>()
                .WithMessage("Buffer cannot be null.\r\nParameter name: buffer");

            mockEngine.Invoking(x => x.Logger(new char[] { '1' }, -1, 0))
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Index cannot be less than 0.\r\nParameter name: index");

            mockEngine.Invoking(x => x.Logger(new char[] { '1' }, 0, -1))
                .Should().Throw<ArgumentOutOfRangeException>()
                .WithMessage("Count cannot be less than 0.\r\nParameter name: count");

            mockEngine.Invoking(x => x.Logger(new char[] { '2' }, 2, 2))
                .Should().Throw<ArgumentException>()
                .WithMessage("Count cannot be bigger than the offsetted buffer.");

            mockEngine.Logger(new char[] { 'B', 'A', '*', '-' }, 1, 2, LogType.Debug, 0);

            logMessage.Should().Be("Debug|0|ID0|Logger_Char_Sub_Array_Test|A*");
        }

        [TestMethod]
        public void Logger_Double_Test()
        {
            mockEngine.Logger(1234.45d, LogType.Information, -1);

            logMessage.Should().Be("Information|-1|ID0|Logger_Double_Test|1234.45");
        }

        [TestMethod]
        public void Logger_Decimal_Test()
        {
            mockEngine.Logger(6.9M, LogType.Information, -1);

            logMessage.Should().Be("Information|-1|ID0|Logger_Decimal_Test|6.9");
        }

        [TestMethod]
        public void Logger_Float_Test()
        {
            mockEngine.Logger(5.75F, LogType.Information, -1);

            logMessage.Should().Be("Information|-1|ID0|Logger_Float_Test|5.75");
        }

        [TestMethod]
        public void Logger_Int_Test()
        {
            mockEngine.Logger(-123456789, LogType.Information, -1);

            logMessage.Should().Be("Information|-1|ID0|Logger_Int_Test|-123456789");
        }

        [TestMethod]
        public void Logger_UInt_Test()
        {
            mockEngine.Logger(4294967295, LogType.Information, -1);

            logMessage.Should().Be("Information|-1|ID0|Logger_UInt_Test|4294967295");
        }

        [TestMethod]
        public void Logger_Long_Test()
        {
            mockEngine.Logger(-123456789890, LogType.Information, -1);

            logMessage.Should().Be("Information|-1|ID0|Logger_Long_Test|-123456789890");
        }

        [TestMethod]
        public void Logger_ULong_Test()
        {
            mockEngine.Logger(18446744073709551615, LogType.Information, -1);

            logMessage.Should().Be("Information|-1|ID0|Logger_ULong_Test|18446744073709551615");
        }

        [TestMethod]
        public void Logger_Object_Test()
        {
            mockEngine.Logger((object)"01/01/0001 9:52:45 AM", LogType.Information, -1);

            logMessage.Should().Be($"Information|-1|ID0|Logger_Object_Test|01/01/0001 9:52:45 AM");

            mockEngine.Logger(new object[] { "Hello!" }, LogType.Information, -1);

            logMessage.Should().Be("Information|-1|ID0|Logger_Object_Test|System.Object[]");

            mockEngine.Logger((object)null, LogType.Information, -1);

            logMessage.Should().Be("Information|-1|ID0|Logger_Object_Test|NULL");
        }
    }
}