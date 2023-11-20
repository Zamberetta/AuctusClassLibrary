using Auctus.Class.Library.Tests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using static FluentAssertions.FluentActions;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace Auctus.DataMiner.Library.Common.Type.Tests
{
    [TestClass]
    public class EnumExtensionsTests
    {
        [TestMethod]
        public void GetEnumAttribute_Enum_Test()
        {
            MockEnumeration.abc.GetEnumAttribute<DefaultValueAttribute>().Should().BeNull();
            MockEnumeration.abc.GetEnumAttribute<DescriptionAttribute>().Description.Should().Be("ABC");
        }

        [TestMethod]
        public void GetEnumAttribute_Field_Test()
        {
            typeof(MockEnumeration).GetEnumAttribute<DescriptionAttribute>("Invalid").Should().BeNull();
            typeof(MockEnumeration).GetEnumAttribute<DefaultValueAttribute>("abc").Should().BeNull();
            typeof(MockEnumeration).GetEnumAttribute<DescriptionAttribute>("abc").Description.Should().Be("ABC");
        }

        [TestMethod]
        public void GetEnumAttributes_Test()
        {
            typeof(MockEnumeration).GetEnumAttributes<DefaultValueAttribute>().Should().BeEmpty();
            typeof(MockEnumeration).GetEnumAttributes<DescriptionAttribute>().Should().HaveCount(3);
        }

        [TestMethod]
        public void GetEnumMemberName_Test()
        {
            var invalidAttribute = new DefaultValueAttribute("ABC");
            var validAttribute = new DescriptionAttribute("ABC");

            invalidAttribute.GetEnumMemberName(typeof(MockEnumeration)).Should().BeNull();
            validAttribute.GetEnumMemberName(typeof(MockEnumeration)).Should().Be("abc");
        }

        [TestMethod]
        public void GetEnumMemberValue_Test()
        {
            var invalidAttribute = new DefaultValueAttribute("ABC");
            var validAttribute = new DescriptionAttribute("ABC");

            Invoking(() => invalidAttribute.GetEnumMemberValue<MockEnumeration>()).Should().Throw<InvalidEnumArgumentException>();

            validAttribute.GetEnumMemberValue<MockEnumeration>().Should().Be(MockEnumeration.abc);
        }
    }
}