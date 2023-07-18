using System;
using System.Collections.Generic;
using System.Linq;
using static Auctus.Class.Library.Tests.MockTable;

namespace Auctus.Class.Library.Tests
{
    public class MockTable
    {
        /// <summary>
        /// ElementName, Table
        /// </summary>
        public Dictionary<string, Dictionary<int, Table>> Tables;

        public MockTable()
        {
            Tables = new Dictionary<string, Dictionary<int, Table>>();
        }

        public Table GetTable(string elementName, int tablePid, int index = 0, int displayIndex = 0)
        {
            if (!Tables.TryGetValue(elementName, out Dictionary<int, Table> tables))
            {
                tables = new Dictionary<int, Table>();

                Tables.Add(elementName, tables);
            }

            if (!tables.TryGetValue(tablePid, out Table table))
            {
                table = new Table() { TablePid = tablePid, Index = index, DisplayIndex = displayIndex, Columns = new Dictionary<int, Column>() };

                tables.Add(tablePid, table);
            }

            return table;
        }

        public Table GetTableByColumnPid(string elementName, int columnPid)
        {
            if (Tables.TryGetValue(elementName, out Dictionary<int, Table> tables))
            {
                foreach (var table in tables.Values)
                {
                    foreach (var column in table.Columns)
                    {
                        if (column.Value.ColumnPid == columnPid)
                        {
                            return table;
                        }
                    }
                }
            }

            throw new InvalidOperationException("Sequence contains no matching element");
        }

        public class Table
        {
            public int TablePid { get; set; }

            public int Index { get; set; }

            public int DisplayIndex { get; set; }

            /// <summary>
            /// IDX, Column
            /// </summary>
            public Dictionary<int, Column> Columns { get; set; }
        }

        public class Column
        {
            public int IDX { get; set; }

            public int ColumnPid { get; set; }

            /// <summary>
            /// Key, CellValue
            /// </summary>
            public Dictionary<string, object> Cells { get; set; }
        }
    }

    public static class MockTableExtensions
    {
        public static Column GetColumn(this Table table, int idx, int columnPid = -1)
        {
            if (!table.Columns.TryGetValue(idx, out Column tableColumn))
            {
                tableColumn = new Column { IDX = idx, ColumnPid = columnPid, Cells = new Dictionary<string, object>() };

                table.Columns.Add(idx, tableColumn);
            }

            return tableColumn;
        }

        public static Column GetColumnByPid(this Table table, int columnPid)
        {
            return table.Columns.Values.First(x => x.ColumnPid == columnPid);
        }

        public static string GetNextKey(this Table table)
        {
            var column = table.GetColumn(table.Index);
            var lastKey = column.Cells.Keys.OrderByDescending(x => x).FirstOrDefault();

            if (lastKey == null)
            {
                return "1";
            }

            if (!int.TryParse(lastKey, out int currentIncrement))
            {
                throw new ArgumentException("Failed to retrieve next key, table does not appear to be of type 'autoincrement'.");
            }

            return Convert.ToString(currentIncrement + 1);
        }

        public static bool ContainsKey(this Table table, string key)
        {
            var column = table.GetColumn(table.Index);
            return key != null && column.Cells.Keys.Contains(key);
        }

        public static void ClearTable(this Table table)
        {
            foreach (var column in table.Columns)
            {
                column.Value.Cells.Clear();
            }
        }

        public static void AddCell(this Column column, string key, object cellValue)
        {
            column.Cells.Add(key, ValueConversion(cellValue));
        }

        public static bool UpdateCell(this Column column, string key, object cellValue, bool createEntry = false)
        {
            if (column.Cells.ContainsKey(key))
            {
                column.Cells[key] = ValueConversion(cellValue);
                return true;
            }
            else if (createEntry)
            {
                AddCell(column, key, cellValue);
                return true;
            }

            return false;
        }

        private static object ValueConversion(object cellValue)
        {
            if (cellValue == null) return null;

            var type = Type.GetTypeCode(cellValue.GetType());

            switch (type)
            {
                case TypeCode.Empty:
                case TypeCode.DBNull:
                    return null;

                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return Convert.ToDouble(cellValue);

                case TypeCode.DateTime:
                    return Math.Round(((DateTime)cellValue).ToOADate(), 8);

                case TypeCode.Object:
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.String:
                default:
                    return Convert.ToString(cellValue);
            }
        }
    }
}