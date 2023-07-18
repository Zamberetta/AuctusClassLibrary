using Auctus.Class.Library.Tests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Auctus.DataMiner.Library.Protocol.Tests
{
    [TestClass]
    public class ProtocolDocumentsTests
    {
        private MockProtocol mockProtocol = null;
        private static readonly string DataMinerDocumentsDirectory = $@"C:\Skyline DataMiner\Documents";
        private readonly string TargetDirectory = $@"{DataMinerDocumentsDirectory}\ClassLibraryTest";

        [TestInitialize]
        public void TestInitialize()
        {
            mockProtocol = new MockProtocol
            {
                ProtocolName = "ClassLibraryTest"
            };
        }

        [TestMethod]
        public void GetDirectory_Test()
        {
            ProtocolDocuments.GetDirectory(mockProtocol).Should().Be($@"{TargetDirectory}\");
            ProtocolDocuments.GetDirectory(mockProtocol, "SubDirectory").Should().Be($@"{TargetDirectory}\SubDirectory\");
            ProtocolDocuments.GetDirectory(mockProtocol, "", "DemoProtocol").Should().Be($@"{DataMinerDocumentsDirectory}\DemoProtocol\");
        }
    }
}