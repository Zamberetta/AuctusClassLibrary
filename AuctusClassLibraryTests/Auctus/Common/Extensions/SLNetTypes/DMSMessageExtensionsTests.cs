using Auctus.DataMiner.Library.Common.SLNetType;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skyline.DataMiner.Net.Messages;
using System.Linq;

namespace Auctus.DataMiner.Library.Common.Type.Tests
{
    [TestClass]
    public class DMSMessageExtensionsTests
    {
        [TestMethod]
        public void CastDMSMessage_Test()
        {
            var dmsMessage1 = new Mock<GetProtocolsResponseMessage>().Object;
            var dmsMessage2 = new Mock<GetProtocolsResponseMessage>().Object;
            var dmsMessage3 = new Mock<GetProtocolsResponseMessage>().Object;

            var dmsMessageArray = new DMSMessage[] { dmsMessage1, dmsMessage2, dmsMessage3 };
            var cast = dmsMessageArray.CastDMSMessage<GetProtocolsResponseMessage>().ToArray();

            cast.Should().BeOfType(typeof(GetProtocolsResponseMessage[])).And.HaveCount(3);
        }
    }
}