using Auctus.DataMiner.Library.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skyline.DataMiner.Automation;
using Skyline.DataMiner.Net;
using Skyline.DataMiner.Net.Messages;
using Skyline.DataMiner.Net.Messages.Advanced;
using System;
using System.Linq;

namespace Auctus.DataMiner.Library.Automation.Tests
{
    [TestClass]
    public class SLNetTests
    {
        [TestMethod]
        public void SendDmsFileChange_Test()
        {
            var mock = new Mock<Engine>();
            var logMessage = string.Empty;

            mock.Setup(x => x.SendSLNetSingleResponseMessage(It.IsAny<DMSMessage>())).Returns(() => new SetDataMinerInfoResponseMessage() { iRet = 0 });
            mock.Setup(x => x.GetUserConnection().ServerDetails).Returns(() => new ServerDetails() { AgentID = 1234 });
            mock.Setup(x => x.Log(It.IsAny<string>(), It.IsAny<LogType>(), It.IsAny<int>()))
                .Callback<string, LogType, int>((message, logType, logLevel) =>
                {
                    logMessage = $"{logType}|{logLevel}|{message}";
                });

            SLNet.SendDmsFileChange(mock.Object, @"C:\SkylineDataMiner\ProtocolScripts\Epic.dll").Should().NotBeNull();

            SLNet.SendDmsFileChange(mock.Object, string.Empty).Should().BeNull();

            logMessage.Should().Contain("SendDmsFileChange|Exception => System.ArgumentException File Path cannot be null, empty or white space.");
        }

        [TestMethod]
        public void ChangeCommunicationState_Test()
        {
            var mock = new Mock<Engine>();
            SetDataMinerInfoMessage setDataMinerInfoMessage = null;

            mock.Setup(x => x.GetUserConnection().ServerDetails).Returns(() => new ServerDetails() { AgentID = 1234 });
            mock.Setup(x => x.SendSLNetSingleResponseMessage(It.IsAny<DMSMessage>()))
                .Returns(() => new SetDataMinerInfoResponseMessage() { iRet = 0 })
                .Callback<DMSMessage>((dmsMessage) =>
                {
                    setDataMinerInfoMessage = (SetDataMinerInfoMessage)dmsMessage;
                });

            SLNet.ChangeCommunicationState(mock.Object, 1234, 456, true, false, 2).Should().NotBeNull();

            setDataMinerInfoMessage.IInfo1.Should().Be(2);
            setDataMinerInfoMessage.Uia1.Uia.Last().Should().Be(1);

            SLNet.ChangeCommunicationState(mock.Object, 1234, 456, false, true, 0).Should().NotBeNull();

            setDataMinerInfoMessage.IInfo1.Should().Be(-1);
            setDataMinerInfoMessage.Uia1.Uia.Last().Should().Be(0);

            mock.Setup(x => x.SendSLNetSingleResponseMessage(It.IsAny<DMSMessage>()))
                .Returns(() =>
                {
                    throw new ArgumentException("Unhandled exception has occurred.");
                });

            SLNet.ChangeCommunicationState(mock.Object, 1234, 456, true, false, 2).Should().BeNull();
        }

        [TestMethod]
        public void GetInfo_Single_Test()
        {
            var mock = new Mock<Engine>();
            GetInfoMessage getInfoMessage = null;

            mock.Setup(x => x.SendSLNetSingleResponseMessage(It.IsAny<DMSMessage>()))
                .Callback<DMSMessage>((dmsMessage) =>
                {
                    getInfoMessage = (GetInfoMessage)dmsMessage;
                });

            SLNet.GetInfo<GetLicensesResponseMessage>(mock.Object, InfoTypeSingle.Licenses);

            getInfoMessage.Type.Should().Be(InfoType.Licenses);

            mock.Setup(x => x.SendSLNetSingleResponseMessage(It.IsAny<DMSMessage>()))
                .Returns(() =>
                {
                    throw new ArgumentException("Unhandled exception has occurred.");
                });

            SLNet.GetInfo<GetLicensesResponseMessage>(mock.Object, InfoTypeSingle.Licenses).Should().BeNull();
        }

        [TestMethod]
        public void GetInfo_Array_Test()
        {
            var mock = new Mock<Engine>();
            GetInfoMessage getInfoMessage = null;

            mock.Setup(x => x.SendSLNetMessage(It.IsAny<DMSMessage>()))
                .Callback<DMSMessage>((dmsMessage) =>
                {
                    getInfoMessage = (GetInfoMessage)dmsMessage;
                });

            SLNet.GetInfo<GetProtocolsResponseMessage>(mock.Object, InfoTypeArray.Protocols);

            getInfoMessage.Type.Should().Be(InfoType.Protocols);

            mock.Setup(x => x.SendSLNetMessage(It.IsAny<DMSMessage>()))
                .Returns(() =>
                {
                    throw new ArgumentException("Unhandled exception has occurred.");
                });

            SLNet.GetInfo<GetProtocolsResponseMessage>(mock.Object, InfoTypeArray.Protocols).Should().BeEmpty();
        }

        [TestMethod]
        public void AddDocument_Test()
        {
            var mock = new Mock<Engine>();
            var logMessage = string.Empty;
            var validDocumentResponse = true;

            mock.Setup(x => x.SendSLNetMessage(It.IsAny<DMSMessage>()));
            mock.Setup(x => x.GetUserConnection().ServerDetails).Returns(() => new ServerDetails() { AgentID = 1234 });
            mock.Setup(x => x.SendSLNetSingleResponseMessage(It.IsAny<DMSMessage>()))
                .Returns(() => validDocumentResponse ? new AddDocumentResponseMessage() { Ret = 1 } : null);
            mock.Setup(x => x.Log(It.IsAny<string>(), It.IsAny<LogType>(), It.IsAny<int>()))
                .Callback<string, LogType, int>((message, logType, logLevel) =>
                {
                    logMessage = $"{logType}|{logLevel}|{message}";
                });

            SLNet.AddDocument(mock.Object, @"DemoProtocol\New Folder", "epic.dll", new byte[163840]).Should().NotBeNull();

            SLNet.AddDocument(mock.Object, string.Empty, "epic.dll", new byte[1]).Should().BeNull();
            logMessage.Should().Contain("AddDocument|Exception => System.ArgumentException Document Path cannot be null, empty or white space.");

            SLNet.AddDocument(mock.Object, @"DemoProtocol\New Folder", string.Empty, new byte[1]).Should().BeNull();
            logMessage.Should().Contain("AddDocument|Exception => System.ArgumentException Document Name cannot be null, empty or white space.");

            SLNet.AddDocument(mock.Object, @"DemoProtocol\New Folder", "epic.dll", new byte[0]).Should().BeNull();
            logMessage.Should().Contain("AddDocument|Exception => System.ArgumentException Document cannot be empty.");

            validDocumentResponse = false;

            SLNet.AddDocument(mock.Object, @"DemoProtocol\New Folder", "epic.dll", new byte[1]).Should().BeNull();
            logMessage.Should().Contain("AddDocument|Exception => System.ArgumentException Failed to Add Document");

            mock.Setup(x => x.SendSLNetSingleResponseMessage(It.IsAny<DMSMessage>()))
                .Returns(() =>
                {
                    throw new ArgumentException("Unhandled exception has occurred.");
                });

            SLNet.AddDocument(mock.Object, @"DemoProtocol\New Folder", "epic.dll", new byte[1]).Should().BeNull();
            logMessage.Should().Contain("AddDocument|Exception => System.ArgumentException Failed to Add Document");
        }
    }
}