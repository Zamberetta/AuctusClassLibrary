using Auctus.Class.Library.Tests;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Skyline.DataMiner.Net.Exceptions;
using Skyline.DataMiner.Net.Messages;
using Skyline.DataMiner.Scripting;
using System;
using System.Collections.Generic;
using static Auctus.Class.Library.Tests.MockParameters;
using static FluentAssertions.FluentActions;

namespace Auctus.DataMiner.Library.Protocol.Tests
{
    [TestClass]
    public class QActionTableExtensionsTests
    {
        private MockProtocol mockProtocol = null;
        private QActionTable mockBasicTable = null;
        private MockbasictableQActionRow firstRow = null;
        private MockbasictableQActionRow secondRow = null;
        private MockbasictableQActionRow thirdRow = null;
        private MockbasictableQActionRow fourthRow = null;

        [TestInitialize]
        public void TestInitialize()
        {
            mockProtocol = new MockProtocol();
            mockBasicTable = new QActionTable(mockProtocol, Mockbasictable.tablePid, "Mock Basic Table");

            var mockTable = mockProtocol.InitialiseMockTable(Mockbasictable.tablePid);
            mockTable.GetColumn(1, 1002);
            mockTable.GetColumn(2, 1003);
            mockTable.GetColumn(3, 1004);

            firstRow = new MockbasictableQActionRow
            {
                Mockbasictablestringvalue_1002 = "ABC",
                Mockbasictabledoublevalue_1003 = 123.456,
                Mockbasictabledatetimevalue_1004 = DateTime.Now,
                Mockbasictabledisplaykey_1005 = "ABC/123.456",
            };

            secondRow = new MockbasictableQActionRow
            {
                Mockbasictablestringvalue_1002 = "DEF",
                Mockbasictabledoublevalue_1003 = 234.567,
                Mockbasictabledatetimevalue_1004 = DateTime.Now,
                Mockbasictabledisplaykey_1005 = "DEF/234.567",
            };

            thirdRow = new MockbasictableQActionRow
            {
                Mockbasictableinstance_1001 = "3",
                Mockbasictablestringvalue_1002 = "GHI",
                Mockbasictabledoublevalue_1003 = 342.734,
                Mockbasictabledatetimevalue_1004 = DateTime.Now,
                Mockbasictabledisplaykey_1005 = "GHI/342.734",
            };

            fourthRow = new MockbasictableQActionRow
            {
                Mockbasictableinstance_1001 = "4",
                Mockbasictablestringvalue_1002 = "JKL",
                Mockbasictabledoublevalue_1003 = 456.789,
                Mockbasictabledatetimevalue_1004 = DateTime.Now,
                Mockbasictabledisplaykey_1005 = "JKL/456.789",
            };

            mockBasicTable.AddRow(firstRow);
            mockBasicTable.AddRow(secondRow);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            mockBasicTable.DeleteRow(mockBasicTable.Keys);
        }

        private ParameterChangeEventMessage QActionTableRowsToParameterValue(QActionTableRow[] rows)
        {
            var columnCount = rows.Length > 0 ? rows[0].ColumnCount : 0;
            var columns = new ParameterValue
            {
                ValueType = ParameterValueType.Array,
                ArrayValue = new ParameterValue[columnCount],
            };

            for (int rowIndex = 0; rowIndex < rows.Length; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    if (columns.ArrayValue[columnIndex] == null)
                    {
                        columns.ArrayValue[columnIndex] = new ParameterValue
                        {
                            ValueType = ParameterValueType.Array,
                            ArrayValue = new ParameterValue[rows.Length],
                        };
                    }

                    columns.ArrayValue[columnIndex].ArrayValue[rowIndex] = new ParameterValue
                    {
                        ValueType = ParameterValueType.Array,
                        ArrayValue = new ParameterValue[]
                        {
                            GetParameterValue(rows[rowIndex].Columns.ContainsKey(columnIndex) ? rows[rowIndex].Columns[columnIndex] : null)
                        },
                    };
                }
            }

            return new ParameterChangeEventMessage()
            {
                NewValue = columns,
            };
        }

        private ParameterValue GetParameterValue(object cellValue)
        {
            var parameterValue = new ParameterValue();

            if (cellValue == null)
            {
                parameterValue.ValueType = ParameterValueType.Empty;
                return parameterValue;
            }

            var type = Type.GetTypeCode(cellValue.GetType());

            switch (type)
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    parameterValue.ValueType = ParameterValueType.Empty;
                    break;

                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                    parameterValue.ValueType = ParameterValueType.Int32;
                    parameterValue.Int32Value = Convert.ToInt32(cellValue);
                    break;

                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    parameterValue.ValueType = ParameterValueType.Double;
                    parameterValue.DoubleValue = Convert.ToDouble(cellValue);
                    break;

                case TypeCode.DateTime:
                    parameterValue.ValueType = ParameterValueType.Date;
                    parameterValue.DateValue = (DateTime)cellValue;
                    break;

                default:
                    parameterValue.ValueType = ParameterValueType.String;
                    parameterValue.StringValue = Convert.ToString(cellValue);
                    break;
            }

            return parameterValue;
        }

        [TestMethod]
        public void ClearTable_QActionTable_Test()
        {
            mockBasicTable.RowCount.Should().Be(2);
            mockBasicTable.ClearTable().Should().Be(0);
        }

        [TestMethod]
        public void ClearTable_Protocol_Test()
        {
            mockProtocol.RowCount(Mockbasictable.tablePid).Should().Be(2);
            mockProtocol.ClearTable(Mockbasictable.tablePid).Should().Be(0);
        }

        [TestMethod]
        public void GetRows_Test()
        {
            var mock = new Mock<SLNetConnection>();

            mock.Setup(x => x.SendSingleResponseMessage(It.IsAny<DMSMessage>())).Returns(() =>
            {
                return new ParameterChangeEventMessage();
            });

            mockProtocol.SLNet = mock.Object;

            var getRowsQActionTable = mockBasicTable.GetRows<MockbasictableQActionRow>();

            getRowsQActionTable.Should().BeEmpty();

            mock.Setup(x => x.SendSingleResponseMessage(It.IsAny<DMSMessage>())).Returns(() =>
            {
                return QActionTableRowsToParameterValue(new QActionTableRow[0]);
            });

            getRowsQActionTable = mockBasicTable.GetRows<MockbasictableQActionRow>();

            getRowsQActionTable.Should().BeEmpty();

            mock.Setup(x => x.SendSingleResponseMessage(It.IsAny<DMSMessage>())).Returns(() =>
            {
                return QActionTableRowsToParameterValue(new[] { thirdRow, fourthRow });
            });

            mockProtocol.SLNet = mock.Object;

            getRowsQActionTable = mockBasicTable.GetRows<MockbasictableQActionRow>();

            getRowsQActionTable.Should().NotBeEmpty().And.HaveCount(2);
            getRowsQActionTable.Should().ContainEquivalentOf(thirdRow, options => options.Excluding(x => x.Columns));
            getRowsQActionTable.Should().ContainEquivalentOf(fourthRow, options => options.Excluding(x => x.Columns));

            var getRowsProtocol = mockProtocol.GetRows<MockbasictableQActionRow>(Mockbasictable.tablePid);

            getRowsQActionTable.Should().BeEquivalentTo(getRowsProtocol);

            var dataMinerId = 9876;
            var elementId = 543;

            mockProtocol.DataMinerID = dataMinerId;
            mockProtocol.ElementID = elementId;

            var getRowsRemote = mockProtocol.GetRows<MockbasictableQActionRow>(dataMinerId, elementId, Mockbasictable.tablePid);

            getRowsQActionTable.Should().BeEquivalentTo(getRowsRemote);
        }

        [TestMethod]
        public void GetRows_Exceptions_Test()
        {
            var mock = new Mock<SLNetConnection>();

            mock.Setup(x => x.SendSingleResponseMessage(It.IsAny<DMSMessage>())).Returns(() =>
            {
                return null;
            });

            mockProtocol.SLNet = mock.Object;

            Invoking(() => mockBasicTable.GetRows<MockbasictableQActionRow>()).Should().Throw<ArgumentException>();

            mock.Setup(x => x.SendSingleResponseMessage(It.IsAny<DMSMessage>())).Returns(() =>
            {
                throw new DataMinerException("No such element.", -2147024891);
            });

            mockProtocol.SLNet = mock.Object;

            Invoking(() => mockBasicTable.GetRows<MockbasictableQActionRow>())
                .Should().Throw<ArgumentException>()
                .And.Message.Should().StartWith("Element not found");

            mock.Setup(x => x.SendSingleResponseMessage(It.IsAny<DMSMessage>())).Returns(() =>
            {
                throw new DataMinerException("", -2147220916);
            });

            mockProtocol.SLNet = mock.Object;

            Invoking(() => mockBasicTable.GetRows<MockbasictableQActionRow>())
                .Should().Throw<ArgumentException>()
                .And.Message.Should().StartWith("Element not found");

            mock.Setup(x => x.SendSingleResponseMessage(It.IsAny<DMSMessage>())).Returns(() =>
            {
                throw new DataMinerException("", -2147220935);
            });

            mockProtocol.SLNet = mock.Object;

            Invoking(() => mockBasicTable.GetRows<MockbasictableQActionRow>())
                .Should().Throw<ArgumentException>()
                .And.Message.Should().StartWith("Parameter not found");

            mock.Setup(x => x.SendSingleResponseMessage(It.IsAny<DMSMessage>())).Returns(() =>
            {
                throw new DataMinerException("Unhandled exception has occurred.");
            });

            mockProtocol.SLNet = mock.Object;

            Invoking(() => mockBasicTable.GetRows<MockbasictableQActionRow>())
                .Should().Throw<Exception>();
        }

        [TestMethod]
        public void GetRowsForSelectedColumns_QActionTable_Test()
        {
            var rows = mockBasicTable.GetRowsForSelectedColumns<MockbasictableQActionRow>(new uint[]
            {
                Mockbasictable.Idx.mockbasictablestringvalue_1002,
                Mockbasictable.Idx.mockbasictabledoublevalue_1003,
            });

            rows.Should().NotBeEmpty().And.HaveCount(2);

            foreach (var row in rows)
            {
                row.Mockbasictableinstance_1001.Should().BeNull();
                row.Mockbasictablestringvalue_1002.Should().NotBeNull().And.BeOfType(typeof(string));
                row.Mockbasictabledoublevalue_1003.Should().NotBeNull().And.BeOfType(typeof(double));
                row.Mockbasictabledatetimevalue_1004.Should().BeNull();
                row.Mockbasictabledisplaykey_1005.Should().BeNull();
            }
        }

        [TestMethod]
        public void GetRowsForSelectedColumns_Protocol_Test()
        {
            var rows = mockProtocol.GetRowsForSelectedColumns<MockbasictableQActionRow>(Mockbasictable.tablePid, new uint[]
            {
                Mockbasictable.Idx.mockbasictabledatetimevalue_1004,
                Mockbasictable.Idx.mockbasictabledisplaykey_1005,
            });

            rows.Should().NotBeEmpty().And.HaveCount(2);

            foreach (var row in rows)
            {
                row.Mockbasictableinstance_1001.Should().BeNull();
                row.Mockbasictablestringvalue_1002.Should().BeNull();
                row.Mockbasictabledoublevalue_1003.Should().BeNull();
                row.Mockbasictabledatetimevalue_1004.Should().NotBeNull().And.BeOfType(typeof(double));
                row.Mockbasictabledisplaykey_1005.Should().NotBeNull().And.BeOfType(typeof(string));
            }

            var mock = new Mock<SLProtocol>();

            mock.Setup(x => x.NotifyDataMiner((int)NotifyType.NT_GET_TABLE_COLUMNS, It.IsAny<object>(), It.IsAny<object>())).Returns(() =>
            {
                throw new ArgumentException("Unhandled exception has occurred.");
            });

            Invoking(() => mock.Object.GetRowsForSelectedColumns<MockbasictableQActionRow>(Mockbasictable.tablePid, new uint[]
            {
                Mockbasictable.Idx.mockbasictabledatetimevalue_1004,
                Mockbasictable.Idx.mockbasictabledisplaykey_1005,
            })).Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void GetColumn_QActionTable_Test()
        {
            var column = mockBasicTable.GetColumn(Mockbasictable.Idx.mockbasictablestringvalue_1002);

            column.Should().NotBeEmpty().And.HaveCount(2).And.AllBeOfType(typeof(string));

            column = mockBasicTable.GetColumn(Mockbasictable.Idx.mockbasictabledoublevalue_1003);

            column.Should().NotBeEmpty().And.HaveCount(2).And.AllBeOfType(typeof(double));
        }

        [TestMethod]
        public void GetColumn_Protocol_Test()
        {
            var column = mockProtocol.GetColumn(Mockbasictable.tablePid, Mockbasictable.Idx.mockbasictabledatetimevalue_1004);

            column.Should().NotBeEmpty().And.HaveCount(2).And.AllBeOfType(typeof(double));

            column = mockProtocol.GetColumn(Mockbasictable.tablePid, Mockbasictable.Idx.mockbasictabledisplaykey_1005);

            column.Should().NotBeEmpty().And.HaveCount(2).And.AllBeOfType(typeof(string));
        }

        [TestMethod]
        public void GetCell_QActionTable_Test()
        {
            mockBasicTable.GetCell("1", Mockbasictable.Idx.mockbasictablestringvalue_1002).Should().BeOfType(typeof(string));
            mockBasicTable.GetCell("2", Mockbasictable.Idx.mockbasictabledoublevalue_1003).Should().BeOfType(typeof(double));
            mockBasicTable.GetCell("2", 99).Should().BeNull();
        }

        [TestMethod]
        public void GetCell_Protocol_Test()
        {
            mockProtocol.GetCell(Mockbasictable.tablePid, "1", Mockbasictable.Idx.mockbasictabledatetimevalue_1004).Should().BeOfType(typeof(double));
            mockProtocol.GetCell(Mockbasictable.tablePid, "2", Mockbasictable.Idx.mockbasictabledisplaykey_1005).Should().BeOfType(typeof(string));
            mockProtocol.GetCell(Mockbasictable.tablePid, "2", 99).Should().BeNull();
        }

        // TODO: Check if new rows are added if a key does not exist in the table?
        [TestMethod]
        public void SetColumn_QActionTable_Test()
        {
            var targetColumnPid = Mockbasictable.Pid.mockbasictablestringvalue_1002;
            var targetColumnIdx = Mockbasictable.Idx.mockbasictablestringvalue_1002;
            var originalValue = mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "1", targetColumnIdx + 1);

            mockProtocol.SetColumn(Mockbasictable.tablePid, targetColumnPid, (Dictionary<string, DateTime>)null);

            mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "1", targetColumnIdx + 1).Should().Be(originalValue);

            mockProtocol.SetColumn(Mockbasictable.tablePid, targetColumnPid, new Dictionary<string, DateTime>() { });

            mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "1", targetColumnIdx + 1).Should().Be(originalValue);

            mockBasicTable.SetColumn(targetColumnPid, new Dictionary<string, string>()
            {
                { "1", "MNO" },
                { "2", "PQR" },
            });

            mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "1", targetColumnIdx + 1).Should().Be("MNO");
            mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "2", targetColumnIdx + 1).Should().Be("PQR");

            mockBasicTable.SetColumn(targetColumnPid, new Dictionary<string, object>()
            {
                { "1", "STU" },
                { "2", "VWX" },
            });

            mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "1", targetColumnIdx + 1).Should().Be("STU");
            mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "2", targetColumnIdx + 1).Should().Be("VWX");
        }

        [TestMethod]
        public void SetColumn_Protocol_Test()
        {
            var targetColumnPid = Mockbasictable.Pid.mockbasictabledatetimevalue_1004;
            var targetColumnIdx = Mockbasictable.Idx.mockbasictabledatetimevalue_1004;
            var dtNow = DateTime.Now;
            var oaDate = Math.Round(dtNow.ToOADate(), 8);

            var originalValue = mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "1", targetColumnIdx + 1);

            mockProtocol.SetColumn(Mockbasictable.tablePid, targetColumnPid, new Dictionary<string, DateTime>() { });

            mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "1", targetColumnIdx + 1).Should().Be(originalValue);

            mockProtocol.SetColumn(Mockbasictable.tablePid, targetColumnPid, new Dictionary<string, DateTime>()
            {
                { "1", dtNow },
                { "2", dtNow },
            });

            mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "1", targetColumnIdx + 1).Should().Be(oaDate);
            mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "2", targetColumnIdx + 1).Should().Be(oaDate);
        }

        [TestMethod]
        public void SetCell_QActionTable_Test()
        {
            var targetColumnIdx = Mockbasictable.Idx.mockbasictablestringvalue_1002;

            mockBasicTable.SetCell("1", targetColumnIdx, "STU").Should().BeTrue();
            mockBasicTable.SetCell("2", targetColumnIdx, "VXY").Should().BeTrue();

            mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "1", targetColumnIdx + 1).Should().Be("STU");
            mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "2", targetColumnIdx + 1).Should().Be("VXY");

            mockBasicTable.SetCell("3", targetColumnIdx, "Z").Should().BeFalse();
        }

        [TestMethod]
        public void SetCell_Protocol_Test()
        {
            var targetColumnIdx = Mockbasictable.Idx.mockbasictabledoublevalue_1003;

            mockProtocol.SetCell(Mockbasictable.tablePid, "1", targetColumnIdx, 567.890).Should().BeTrue();
            mockProtocol.SetCell(Mockbasictable.tablePid, "2", targetColumnIdx, 678.901).Should().BeTrue();

            mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "1", targetColumnIdx + 1).Should().Be(567.890);
            mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "2", targetColumnIdx + 1).Should().Be(678.901);

            mockProtocol.SetCell(Mockbasictable.tablePid, "3", targetColumnIdx, 789.012).Should().BeFalse();
        }

        [TestMethod]
        public void GetKeysForIndex_QActionTable_Test()
        {
            var keys = mockBasicTable.GetKeysForIndex(Mockbasictable.Pid.mockbasictablestringvalue_1002, "abc", false);
            keys.Should().HaveCount(1);

            mockBasicTable.AddRow(firstRow);

            keys = mockBasicTable.GetKeysForIndex(Mockbasictable.Pid.mockbasictablestringvalue_1002, "abc", false);
            keys.Should().HaveCount(2);

            keys = mockBasicTable.GetKeysForIndex(Mockbasictable.Pid.mockbasictablestringvalue_1002, "abc", true);
            keys.Should().BeEmpty();
        }

        [TestMethod]
        public void GetKeysForIndex_Protocol_Test()
        {
            var keys = mockProtocol.GetKeysForIndex(Mockbasictable.Pid.mockbasictabledoublevalue_1003, "234.567", false);
            keys.Should().HaveCount(1);

            mockProtocol.AddRow(Mockbasictable.tablePid, secondRow);

            keys = mockProtocol.GetKeysForIndex(Mockbasictable.Pid.mockbasictabledoublevalue_1003, "234.567", false);
            keys.Should().HaveCount(2);

            keys = mockProtocol.GetKeysForIndex(Mockbasictable.Pid.mockbasictabledoublevalue_1003, "789.012", true);
            keys.Should().BeEmpty();
        }

        [TestMethod]
        public void FillArray_QActionTable_Test()
        {
            mockBasicTable.RowCount.Should().Be(2);

            var rows = new List<MockbasictableQActionRow>() {
                firstRow,
                thirdRow,
                fourthRow,
            };

            mockBasicTable.FillArray(rows);

            mockBasicTable.RowCount.Should().Be(3);
        }

        [TestMethod]
        public void FillArrayNoDelete_QActionTable_Test()
        {
            mockBasicTable.AddRow(thirdRow);
            mockBasicTable.RowCount.Should().Be(3);

            var stringValue = (string)mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "3", Mockbasictable.Idx.mockbasictablestringvalue_1002 + 1);

            stringValue.Should().Be("GHI");

            var customRow = new MockbasictableQActionRow(thirdRow)
            {
                Mockbasictablestringvalue_1002 = "MNO"
            };

            var rows = new List<MockbasictableQActionRow>() {
                firstRow,
                secondRow,
                customRow,
            };

            mockBasicTable.FillArrayNoDelete(rows);

            mockBasicTable.RowCount.Should().Be(5);

            stringValue = (string)mockProtocol.GetParameterIndexByKey(Mockbasictable.tablePid, "3", Mockbasictable.Idx.mockbasictablestringvalue_1002 + 1);

            stringValue.Should().Be("MNO");
        }
    }
}