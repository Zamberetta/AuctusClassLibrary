using Auctus.Class.Library.Tests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skyline.DataMiner.Net.Messages;
using Skyline.DataMiner.Scripting;
using System;

namespace Auctus.DataMiner.Library.Protocol.Tests
{
    [TestClass]
    public class UserTests
    {
        private MockProtocol mockProtocol = null;

        [TestInitialize]
        public void TestInitialize()
        {
            mockProtocol = new MockProtocol();
        }

        [TestMethod]
        public void GetUser_Test()
        {
            User.GetUser(mockProtocol).Should().Be("bross");

            var mock = new Mock<SLProtocol>();

            mock.Setup(x => x.NotifyDataMiner((int)NotifyType.GetUser, It.IsAny<object>(), It.IsAny<object>())).Returns(() =>
            {
                return null;
            });

            User.GetUser(mock.Object).Should().BeEmpty();

            mock.Setup(x => x.NotifyDataMiner((int)NotifyType.GetUser, It.IsAny<object>(), It.IsAny<object>())).Returns(() =>
            {
                throw new ArgumentException("Unhandled exception has occurred.");
            });

            User.GetUser(mock.Object).Should().BeEmpty();
        }

        [TestMethod]
        public void GetUserInfo_Test()
        {
            var userInfo = User.GetUserInfo(mockProtocol);

            userInfo.FullName.Should().Be("Bob Ross");
            userInfo.Groups.Should().HaveCount(2).And.Contain("Painter");

            mockProtocol.UserGroups = Array.Empty<string>();

            userInfo = User.GetUserInfo(mockProtocol);

            userInfo.Telephone.Should().Be("01234567891");
            userInfo.Groups.Should().BeEmpty();

            userInfo = User.GetUserInfo(mockProtocol, "bross");

            userInfo.Email.Should().Be("bob.ross@happy-accidents.com");
            userInfo.Groups.Should().BeEmpty();

            var mock = new Mock<SLProtocol>();

            mock.Setup(x => x.NotifyDataMiner((int)NotifyType.GetUser, It.IsAny<object>(), It.IsAny<object>())).Returns(() =>
            {
                return null;
            });

            User.GetUserInfo(mock.Object).Should().BeNull();

            mock.Setup(x => x.NotifyDataMiner((int)NotifyType.GetUser, It.IsAny<object>(), It.IsAny<object>())).Returns(() =>
            {
                return "bross";
            });

            mock.Setup(x => x.NotifyDataMiner((int)NotifyType.GetUserInfo, It.IsAny<object>(), It.IsAny<object>())).Returns(() =>
            {
                return null;
            });

            User.GetUserInfo(mock.Object).Should().BeNull();

            mock.Setup(x => x.NotifyDataMiner((int)NotifyType.GetUserInfo, It.IsAny<object>(), It.IsAny<object>())).Returns(() =>
            {
                return new object[0];
            });

            User.GetUserInfo(mock.Object).Should().BeNull();
        }
    }
}