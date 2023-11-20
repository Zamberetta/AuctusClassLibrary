using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static FluentAssertions.FluentActions;

namespace Auctus.DataMiner.Library.Common.Type.Tests
{
    [TestClass]
    public class ObjectExtensionsTests
    {
        private enum Level
        {
            Low = 0,
            Medium = 15,
            High = 30,
        }

        [TestMethod]
        public void ToStr_Test()
        {
            object validObjectString = "Hello World!";
            object nullObject = null;

            validObjectString.ToStr(true).Should().Be("Hello World!");
            validObjectString.ToStr(false).Should().Be("Hello World!");

            nullObject.ToStr(true).Should().Be(string.Empty);

            Invoking(() => nullObject.ToStr(false)).Should().Throw<NullReferenceException>();
        }

        [TestMethod]
        public void ToInt_Test()
        {
            object validObjectInt = 123;
            object validObjectString = "123";
            object validObjectDouble = 123.000d;
            object invalidObjectDouble = 123.456d;
            object nullObject = null;

            validObjectInt.ToInt(true).Should().Be(123);
            validObjectInt.ToInt(false).Should().Be(123);

            validObjectString.ToInt(true).Should().Be(123);
            validObjectString.ToInt(false).Should().Be(123);

            validObjectDouble.ToInt(true).Should().Be(123);
            validObjectDouble.ToInt(false).Should().Be(123);

            invalidObjectDouble.ToInt(true).Should().Be(0);
            Invoking(() => invalidObjectDouble.ToInt(false)).Should().Throw<ArgumentException>();

            nullObject.ToInt(true).Should().Be(0);
            Invoking(() => nullObject.ToInt(false)).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void ToDouble_Test()
        {
            object validObjectInt = 123;
            object validObjectString = "123.456";
            object validObjectDouble = 123.456d;
            object nullObject = null;

            validObjectInt.ToDouble(true).Should().Be(123d);
            validObjectInt.ToDouble(false).Should().Be(123d);

            validObjectString.ToDouble(true).Should().Be(123.456d);
            validObjectString.ToDouble(false).Should().Be(123.456d);

            validObjectDouble.ToDouble(true).Should().Be(123.456d);
            validObjectDouble.ToDouble(false).Should().Be(123.456d);

            nullObject.ToDouble(true).Should().Be(0d);
            Invoking(() => nullObject.ToDouble(false)).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void ToBool_Test()
        {
            object validObjectInt = 1;
            object validObjectString = "1";
            object invalidObjectString = "Hello";
            object validObjectDouble = 1d;
            object invalidObjectDouble = 1.234d;
            object nullObject = null;

            validObjectInt.ToBool(true).Should().Be(true);
            validObjectInt.ToBool(false).Should().Be(true);

            validObjectString.ToBool(true).Should().Be(true);
            validObjectString.ToBool(false).Should().Be(true);
            invalidObjectString.ToBool(true).Should().Be(false);

            validObjectDouble.ToBool(true).Should().Be(true);
            validObjectDouble.ToBool(false).Should().Be(true);
            invalidObjectDouble.ToBool(true).Should().Be(false);

            nullObject.ToBool(true).Should().Be(false);
            Invoking(() => invalidObjectString.ToBool(false)).Should().Throw<ArgumentException>();
            Invoking(() => invalidObjectDouble.ToBool(false)).Should().Throw<ArgumentException>();
            Invoking(() => nullObject.ToBool(false)).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void FromOADate_Test()
        {
            object validObjectInt = 1234;
            object validObjectString = "1234.56";
            object validObjectDouble = 1234.56d;
            object nullObject = null;

            validObjectInt.FromOADate(true).Should().Be(new DateTime(1903, 5, 18, 0, 0, 0, DateTimeKind.Local));
            validObjectInt.FromOADate(false).Should().Be(new DateTime(1903, 5, 18, 0, 0, 0, DateTimeKind.Local));

            validObjectString.FromOADate(true).Should().Be(new DateTime(1903, 5, 18, 13, 26, 24, DateTimeKind.Local));
            validObjectString.FromOADate(false).Should().Be(new DateTime(1903, 5, 18, 13, 26, 24, DateTimeKind.Local));

            validObjectDouble.FromOADate(true).Should().Be(new DateTime(1903, 5, 18, 13, 26, 24, DateTimeKind.Local));
            validObjectDouble.FromOADate(false).Should().Be(new DateTime(1903, 5, 18, 13, 26, 24, DateTimeKind.Local));

            nullObject.FromOADate(true).Should().Be(new DateTime(1899, 12, 30, 0, 0, 0, DateTimeKind.Local));
            Invoking(() => nullObject.FromOADate(false)).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void ToEnum_Int_Test()
        {
            object validObjectInt = 30;
            object invalidObjectInt = 123;
            object validObjectString = "30";
            object invalidObjectString = "Friend";
            object validObjectDouble = 15d;
            object invalidObjectDouble = 123.456d;
            object nullObject = null;

            validObjectInt.ToEnum<Level>(true).Should().Be(Level.High);
            validObjectInt.ToEnum<Level>(false).Should().Be(Level.High);

            invalidObjectInt.ToEnum<Level>(true).Should().Be(Level.Low);
            invalidObjectInt.ToEnum<Level>(false).Should().Be(Level.Low);

            validObjectString.ToEnum<Level>(true).Should().Be(Level.High);
            validObjectString.ToEnum<Level>(false).Should().Be(Level.High);

            invalidObjectString.ToEnum<Level>(true).Should().Be(Level.Low);
            Invoking(() => invalidObjectString.ToEnum<Level>(false)).Should().Throw<ArgumentException>();

            validObjectDouble.ToEnum<Level>(true).Should().Be(Level.Medium);
            validObjectDouble.ToEnum<Level>(false).Should().Be(Level.Medium);

            invalidObjectDouble.ToEnum<Level>(true).Should().Be(Level.Low);
            Invoking(() => invalidObjectDouble.ToEnum<Level>(false)).Should().Throw<ArgumentException>();

            nullObject.ToEnum<Level>(true).Should().Be(Level.Low);
            Invoking(() => nullObject.ToEnum<Level>(false)).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void ToEnum_String_Test()
        {
            object validObjectString = "Medium";
            object invalidObjectString = "Friend";
            object nullObject = null;

            validObjectString.ToEnum<Level>(true).Should().Be(Level.Medium);
            validObjectString.ToEnum<Level>(false).Should().Be(Level.Medium);

            invalidObjectString.ToEnum<Level>(true).Should().Be(Level.Low);
            Invoking(() => invalidObjectString.ToEnum<Level>(false)).Should().Throw<ArgumentException>();

            nullObject.ToEnum<Level>(true).Should().Be(Level.Low);
            Invoking(() => nullObject.ToEnum<Level>(false)).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void ToJson_Test()
        {
            object ObjectInt = 30;
            object ObjectString = "Carpe diem";
            object ObjectDouble = 123.456d;
            object ObjectEnum = Level.Medium;
            object ObjectArray = new object[] { "Hello", 123 };
            object nullObject = null;

            ObjectInt.ToJson(true).Should().Be("30");
            ObjectString.ToJson(true).Should().Be("\"Carpe diem\"");
            ObjectDouble.ToJson(true).Should().Be("123.456");
            ObjectEnum.ToJson(true).Should().Be("15");
            ObjectArray.ToJson(true).Should().Be("[\r\n  \"Hello\",\r\n  123\r\n]");
            ObjectArray.ToJson(false).Should().Be("[\"Hello\",123]");
            nullObject.ToJson(true).Should().Be("null");
        }

        [TestMethod]
        public void IsNull_Test()
        {
            DBNull.Value.IsNull().Should().BeTrue();

            ((object)null).IsNull().Should().BeTrue();

            ((string)null).IsNull().Should().BeTrue();

            Level.Medium.IsNull().Should().BeFalse();
        }
    }
}