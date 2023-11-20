using Auctus.DataMiner.Library.Auctus.Common.Models;
using Auctus.DataMiner.Library.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skyline.DataMiner.Net;
using Skyline.DataMiner.Net.Messages;
using Skyline.DataMiner.Net.Messages.Advanced;
using Skyline.DataMiner.Scripting;
using System;
using System.Linq;
using static FluentAssertions.FluentActions;

namespace Auctus.DataMiner.Library.Protocol.Tests
{
    [TestClass]
    public class SLNetTests
    {
        [TestMethod]
        public void SendDmsFileChange_Test()
        {
            var mock = new Mock<SLProtocol>();
            var logMessage = string.Empty;

            mock.Setup(x => x.SLNet.SendSingleResponseMessage(It.IsAny<DMSMessage>())).Returns(() => new SetDataMinerInfoResponseMessage() { iRet = 0 });
            mock.Setup(x => x.GetUserConnection().ServerDetails).Returns(() => new ServerDetails() { AgentID = 1234 });
            mock.Setup(x => x.Log(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Callback<int, int, string>((iType, iLevel, message) =>
                {
                    logMessage = $"{iType}|{iLevel}|{message}";
                });

            SLNet.SendDmsFileChange(mock.Object, @"C:\SkylineDataMiner\ProtocolScripts\Epic.dll").Should().NotBeNull();

            SLNet.SendDmsFileChange(mock.Object, string.Empty).Should().BeNull();

            logMessage.Should().Contain("SendDmsFileChange|Exception => System.ArgumentException File Path cannot be null, empty or white space.");
        }

        [TestMethod]
        public void ChangeCommunicationState_Test()
        {
            var mock = new Mock<SLProtocol>();
            SetDataMinerInfoMessage setDataMinerInfoMessage = null;

            mock.Setup(x => x.GetUserConnection().ServerDetails).Returns(() => new ServerDetails() { AgentID = 1234 });
            mock.Setup(x => x.SLNet.SendSingleResponseMessage(It.IsAny<DMSMessage>()))
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

            mock.Setup(x => x.SLNet.SendSingleResponseMessage(It.IsAny<DMSMessage>()))
                .Returns(() =>
                {
                    throw new ArgumentException("Unhandled exception has occurred.");
                });

            SLNet.ChangeCommunicationState(mock.Object, 1234, 456, true, false, 2).Should().BeNull();
        }

        [TestMethod]
        public void GetInfo_Single_Test()
        {
            var mock = new Mock<SLProtocol>();
            GetInfoMessage getInfoMessage = null;

            mock.Setup(x => x.SLNet.SendSingleResponseMessage(It.IsAny<DMSMessage>()))
                .Callback<DMSMessage>((dmsMessage) =>
                {
                    getInfoMessage = (GetInfoMessage)dmsMessage;
                });

            SLNet.GetInfo<GetLicensesResponseMessage>(mock.Object, InfoTypeSingle.Licenses);

            getInfoMessage.Type.Should().Be(InfoType.Licenses);

            mock.Setup(x => x.SLNet.SendSingleResponseMessage(It.IsAny<DMSMessage>()))
                .Returns(() =>
                {
                    throw new ArgumentException("Unhandled exception has occurred.");
                });

            SLNet.GetInfo<GetLicensesResponseMessage>(mock.Object, InfoTypeSingle.Licenses).Should().BeNull();
        }

        [TestMethod]
        public void GetInfo_Array_Test()
        {
            var mock = new Mock<SLProtocol>();
            GetInfoMessage getInfoMessage = null;

            mock.Setup(x => x.SLNet.SendMessage(It.IsAny<DMSMessage>()))
                .Callback<DMSMessage>((dmsMessage) =>
                {
                    getInfoMessage = (GetInfoMessage)dmsMessage;
                });

            SLNet.GetInfo<GetProtocolsResponseMessage>(mock.Object, InfoTypeArray.Protocols);

            getInfoMessage.Type.Should().Be(InfoType.Protocols);

            mock.Setup(x => x.SLNet.SendMessage(It.IsAny<DMSMessage>()))
                .Returns(() =>
                {
                    throw new ArgumentException("Unhandled exception has occurred.");
                });

            SLNet.GetInfo<GetProtocolsResponseMessage>(mock.Object, InfoTypeArray.Protocols).Should().BeEmpty();
        }

        [TestMethod]
        public void GetAvailableAlarmTemplates_Test()
        {
            var mock = new Mock<SLProtocol>();
            var logMessage = string.Empty;

            mock.Setup(x => x.Log(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Callback<int, int, string>((iType, iLevel, message) =>
                {
                    logMessage = $"{iType}|{iLevel}|{message}";
                });

            SLNet.GetAvailableAlarmTemplates(mock.Object, string.Empty, "1.0.0.1").Should().BeNull();
            logMessage.Should().Contain("Exception => System.ArgumentException Protocol Name cannot be null, empty or white space.");

            SLNet.GetAvailableAlarmTemplates(mock.Object, "Test Protocol", null).Should().BeNull();
            logMessage.Should().Contain("Exception => System.ArgumentException Protocol Version cannot be null, empty or white space.");

            mock.Setup(x => x.SLNet.SendSingleResponseMessage(It.IsAny<DMSMessage>())).Returns(() => new GetAvailableAlarmTemplatesResponse() { Templates = new AlarmTemplateMetaInfo[] { } });

            SLNet.GetAvailableAlarmTemplates(mock.Object, "Test Protocol", "1.0.0.1").Should().NotBeNull();
        }

        [TestMethod]
        public void UpdateAlarmTemplate_Test()
        {
            var mock = new Mock<SLProtocol>();

            Invoking(() => SLNet.UpdateAlarmTemplate(mock.Object, null, string.Empty, UpdateAlarmTemplateType.New)).Should().Throw<ArgumentException>();

            mock.Setup(x => x.SLNet.SendMessage(It.IsAny<DMSMessage>()))
                .Returns(() => new DMSMessage[0]);

            Invoking(() => SLNet.UpdateAlarmTemplate(mock.Object, null, "Alarm Template", UpdateAlarmTemplateType.New)).Should().NotThrow();
        }

        [TestMethod]
        public void DeleteDocument_Test()
        {
            var mock = new Mock<SLProtocol>();

            Invoking(() => SLNet.DeleteDocument(mock.Object, string.Empty, "MockDocument.xml")).Should().Throw<ArgumentException>();
            Invoking(() => SLNet.DeleteDocument(mock.Object, "MockElement", null)).Should().Throw<ArgumentException>();

            mock.Setup(x => x.SLNet.SendMessage(It.IsAny<DMSMessage>()))
                .Returns(() => new DMSMessage[0]);

            Invoking(() => SLNet.DeleteDocument(mock.Object, "MockElement", "MockDocument.xml")).Should().NotThrow();
        }

        [TestMethod]
        public void GetAvailableDocuments_Test()
        {
            var mock = new Mock<SLProtocol>();
            var logMessage = string.Empty;

            mock.Setup(x => x.Log(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Callback<int, int, string>((iType, iLevel, message) =>
                {
                    logMessage = $"{iType}|{iLevel}|{message}";
                });

            SLNet.GetAvailableDocuments(mock.Object, string.Empty).Should().BeEmpty();
            logMessage.Should().Contain("Exception => System.ArgumentException Folder cannot be null, empty or white space.");

            mock.Setup(x => x.SLNet.SendSingleResponseMessage(It.IsAny<DMSMessage>()))
                .Returns(() => null);

            SLNet.GetAvailableDocuments(mock.Object, "MockProtocol").Should().BeEmpty();
            logMessage.Should().Contain("Exception => System.ArgumentException Failed to get a valid documents response.");

            var firstDocument = new DmaDocument(
                        name: "AlarmTemplate.xml",
                        description: "Alarm Template",
                        comments: "Template requires validation",
                        date: new DateTime(2023, 11, 19, 12, 0, 0, 0, DateTimeKind.Local),
                        isHyperlink: false,
                        hyperlink: string.Empty
                    );

            var secondDocument = new DmaDocument(
                        name: "https://skyline.be/sites/default/files/inline-images/logo_skylinecommunications_0.svg",
                        description: string.Empty,
                        comments: string.Empty,
                        date: new DateTime(2001, 04, 1, 12, 58, 23, 0, DateTimeKind.Local),
                        isHyperlink: true,
                        hyperlink: "https://skyline.be/sites/default/files/inline-images/logo_skylinecommunications_0.svg"
                    );

            var response = new GetDocumentsResponseMessage()
            {
                Sa = new SA(new string[]
                {
                    firstDocument.Name,
                    firstDocument.Description,
                    firstDocument.Comments,
                    firstDocument.Date.ToString(),
                    firstDocument.IsHyperlink.ToString().ToUpper(),
                    secondDocument.Name,
                    secondDocument.Description,
                    secondDocument.Comments,
                    secondDocument.Date.ToString(),
                    $"{secondDocument.IsHyperlink.ToString().ToUpper()};{secondDocument.Hyperlink}",
                }),
            };

            mock.Setup(x => x.SLNet.SendSingleResponseMessage(It.IsAny<DMSMessage>()))
                .Returns(() => response);

            var documents = SLNet.GetAvailableDocuments(mock.Object, "MockProtocol");

            documents.Should().NotBeEmpty();
            documents.First().Should().BeEquivalentTo(firstDocument);
            documents.Last().Should().BeEquivalentTo(secondDocument);
        }

        [TestMethod]
        public void GetDocument_Test()
        {
            var mock = new Mock<SLProtocol>();
            var logMessage = string.Empty;

            mock.Setup(x => x.Log(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Callback<int, int, string>((iType, iLevel, message) =>
                {
                    logMessage = $"{iType}|{iLevel}|{message}";
                });

            SLNet.GetDocument(mock.Object, string.Empty, "AlarmTemplate.xml").Should().BeEmpty();
            logMessage.Should().Contain("Exception => System.ArgumentException Document Path cannot be null, empty or white space.");

            SLNet.GetDocument(mock.Object, "MockProtocol", null).Should().BeEmpty();
            logMessage.Should().Contain("Exception => System.ArgumentException Document Name cannot be null, empty or white space.");

            var fileSize = 35000;
            var random = new Random();
            var documentBytes = new byte[fileSize];
            random.NextBytes(documentBytes);

            mock.Setup(x => x.GetUserConnection().ServerDetails)
                .Returns(() => new ServerDetails() { AgentID = 1234 });

            mock.Setup(x => x.SLNet.SendSingleResponseMessage(It.IsAny<GetBinaryFileMessage>()))
                .Returns(() => new GetBinaryFileResponseMessage() { FileNr = 1, Size = fileSize });

            mock.Setup(x => x.SLNet.SendSingleResponseMessage(It.IsAny<PullDocumentMessage>()))
                .Returns<PullDocumentMessage>((message) =>
                {
                    var bytes = new byte[message.Length];
                    Buffer.BlockCopy(documentBytes, message.Start, bytes, 0, message.Length);
                    return new PullDocumentResponseMessage() { Ba = new BA(bytes), Size = message.Length };
                });

            SLNet.GetDocument(mock.Object, "MockProtocol", "AlarmTemplate.xml").Should().Equal(documentBytes);

            fileSize = 163840;
            documentBytes = new byte[fileSize];
            random.NextBytes(documentBytes);

            SLNet.GetDocument(mock.Object, "MockProtocol", "AlarmTemplate.xml").Should().Equal(documentBytes);
        }

        [TestMethod]
        public void AddDocument_Test()
        {
            var mock = new Mock<SLProtocol>();
            var logMessage = string.Empty;
            var validDocumentResponse = true;

            mock.Setup(x => x.SLNet.SendMessage(It.IsAny<DMSMessage>()));
            mock.Setup(x => x.GetUserConnection().ServerDetails).Returns(() => new ServerDetails() { AgentID = 1234 });
            mock.Setup(x => x.SLNet.SendSingleResponseMessage(It.IsAny<DMSMessage>()))
                .Returns(() => validDocumentResponse ? new AddDocumentResponseMessage() { Ret = 1 } : null);
            mock.Setup(x => x.Log(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Callback<int, int, string>((iType, iLevel, message) =>
                {
                    logMessage = $"{iType}|{iLevel}|{message}";
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

            validDocumentResponse = true;

            mock.Setup(x => x.SLNet.SendSingleResponseMessage(It.IsAny<DMSMessage>()))
                .Returns(() =>
                {
                    throw new ArgumentException("Unhandled exception has occurred.");
                });

            SLNet.AddDocument(mock.Object, @"DemoProtocol\New Folder", "epic.dll", new byte[1]).Should().BeNull();
            logMessage.Should().Contain("AddDocument|Exception => System.ArgumentException Unhandled exception has occurred");
        }
    }
}