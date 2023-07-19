namespace Auctus.DataMiner.Library.Protocol
{
    using Skyline.DataMiner.Net.Exceptions;
    using Skyline.DataMiner.Net.Messages;
    using Skyline.DataMiner.Scripting;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class QActionTableExtensions
    {
        /// <summary>Removes all rows from the specified table.</summary>
        /// <param name="table">The target QActionTable.</param>
        /// <returns>
        ///   The number of remaining rows in the table.
        /// </returns>
        public static int ClearTable(this QActionTable table)
        {
            return table.Protocol.ClearTable(table.TableId);
        }

        /// <summary>Removes all rows from the specified table.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="tableId">The ID of the table parameter.</param>
        /// <returns>
        ///   The number of remaining rows in the table.
        /// </returns>
        public static int ClearTable(this SLProtocol protocol, int tableId)
        {
            return protocol.DeleteRow(tableId, protocol.GetKeys(tableId));
        }

        /// <summary>Retrieves all rows for the specified table.</summary>
        /// <typeparam name="TQActionTableRow">The QActionTableRow type representing the table row.</typeparam>
        /// <param name="table">The target QActionTable.</param>
        public static List<TQActionTableRow> GetRows<TQActionTableRow>(this QActionTable table)
            where TQActionTableRow : QActionTableRow, new()
        {
            return table.Protocol.GetRows<TQActionTableRow>(table.TableId);
        }

        /// <summary>Retrieves all rows for the specified table.</summary>
        /// <typeparam name="TQActionTableRow">The QActionTableRow type representing the table row.</typeparam>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="tableId">The ID of the table parameter.</param>
        public static List<TQActionTableRow> GetRows<TQActionTableRow>(this SLProtocol protocol, int tableId)
            where TQActionTableRow : QActionTableRow, new()
        {
            return protocol.GetRows<TQActionTableRow>(protocol.DataMinerID, protocol.ElementID, tableId);
        }

        /// <summary>Retrieves all rows for the specified table.</summary>
        /// <typeparam name="TQActionTableRow">The QActionTableRow type representing the table row.</typeparam>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="dataMinerId">The ID of the DataMiner Agent the element/service is hosted on.</param>
        /// <param name="elementId">The ID of the element/service.</param>
        /// <param name="tableId">The ID of the table parameter.</param>
        public static List<TQActionTableRow> GetRows<TQActionTableRow>(this SLProtocol protocol, int dataMinerId, int elementId, int tableId)
            where TQActionTableRow : QActionTableRow, new()
        {
            try
            {
                var rows = new List<TQActionTableRow>();

                var message = new GetPartialTableMessage
                {
                    DataMinerID = dataMinerId,
                    ElementID = elementId,
                    ParameterID = tableId,
                    Filters = new[]
                    {
                        "forceFullTable=true",
                    },
                };

                var response = protocol.SLNet.SendSingleResponseMessage(message) as ParameterChangeEventMessage;

                if (response == null)
                {
                    throw new ArgumentException($"Parameter not found ({dataMinerId}/{elementId}/{tableId}).");
                }

                if (response.NewValue == null || response.NewValue.ArrayValue == null)
                {
                    return rows;
                }

                var tableColumns = response.NewValue.ArrayValue;
                var columnCount = tableColumns.Length;
                var rowCount = columnCount > 0 ? tableColumns[0].ArrayValue.Length : 0;
                var rowValues = new Dictionary<int, object>[rowCount];

                for (var columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    var columnCells = tableColumns[columnIndex].ArrayValue;

                    for (var rowIndex = 0; rowIndex < columnCells.Length; rowIndex++)
                    {
                        if (rowValues[rowIndex] == null)
                        {
                            rowValues[rowIndex] = new Dictionary<int, object>();
                        }

                        rowValues[rowIndex].Add(columnIndex, columnCells[rowIndex].IsEmpty ? null : columnCells[rowIndex].ArrayValue[0].InteropValue);
                    }
                }

                foreach (var rowDict in rowValues)
                {
                    rows.Add(new TQActionTableRow
                    {
                        Columns = rowDict
                    });
                }

                return rows;
            }
            catch (DataMinerException e)
            {
                if (e.ErrorCode == -2147220916 || (e.ErrorCode == -2147024891 && e.Message == "No such element."))
                {
                    // 0x80070005: Access is denied.
                    // 0x8004024C, SL_NO_SUCH_ELEMENT, "The element is unknown."
                    throw new ArgumentException($"Element not found ({dataMinerId}/{elementId}).");
                }
                else if (e.ErrorCode == -2147220935)
                {
                    // 0x80040239, SL_FAILED_NOT_FOUND, The object or file was not found.
                    throw new ArgumentException($"Parameter not found ({dataMinerId}/{elementId}/{tableId}).");
                }

                throw;
            }
        }

        /// <summary>Retrieves all rows for the selected columns.</summary>
        /// <typeparam name="TQActionTableRow">The QActionTableRow type representing the table row.</typeparam>
        /// <param name="table">The target QActionTable.</param>
        /// <param name="columnIndexes">The column indexes that data should be retrieved for.</param>
        public static List<TQActionTableRow> GetRowsForSelectedColumns<TQActionTableRow>(this QActionTable table, uint[] columnIndexes)
            where TQActionTableRow : QActionTableRow, new()
        {
            return table.Protocol.GetRowsForSelectedColumns<TQActionTableRow>(table.TableId, columnIndexes);
        }

        /// <summary>Retrieves all rows for the selected columns.</summary>
        /// <typeparam name="TQActionTableRow">The QActionTableRow type representing the table row.</typeparam>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="tableId">The ID of the table parameter.</param>
        /// <param name="columnIndexes">The column indexes that data should be retrieved for.</param>
        public static List<TQActionTableRow> GetRowsForSelectedColumns<TQActionTableRow>(this SLProtocol protocol, int tableId, uint[] columnIndexes)
            where TQActionTableRow : QActionTableRow, new()
        {
            var tableColumns = (object[])protocol.NotifyProtocol((int)NotifyType.NT_GET_TABLE_COLUMNS, tableId, columnIndexes);

            if (tableColumns == null || !(tableColumns[0] is object[] firstColumn))
                throw new ArgumentException("No data was returned for table: " + tableId);

            var rows = new List<TQActionTableRow>();

            for (var rowIndex = 0; rowIndex < firstColumn.Length; rowIndex++)
            {
                var dict = new Dictionary<int, object>();

                for (var columnIndex = 0; columnIndex < columnIndexes.Length; columnIndex++)
                {
                    dict.Add((int)columnIndexes[columnIndex], ((object[])tableColumns[columnIndex])[rowIndex]);
                }

                rows.Add(new TQActionTableRow
                {
                    Columns = dict
                });
            }

            return rows;
        }

        /// <summary>Retrieves all cells for the selected column.</summary>
        /// <param name="table">The target QActionTable.</param>
        /// <param name="columnIdx">The target column IDX.</param>
        public static object[] GetColumn(this QActionTable table, int columnIdx)
        {
            return table.Protocol.GetColumn(table.TableId, columnIdx);
        }

        /// <summary>Retrieves all cells for the selected column.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="tableId">The ID of the table parameter.</param>
        /// <param name="columnIdx">The target column IDX.</param>
        public static object[] GetColumn(this SLProtocol protocol, int tableId, int columnIdx)
        {
            var tableColumns = (object[])protocol.NotifyProtocol((int)NotifyType.NT_GET_TABLE_COLUMNS, tableId, new uint[] { (uint)columnIdx });

            return tableColumns[0] as object[];
        }

        /// <summary>Retrieves the cell data for the specified row and column.</summary>
        /// <param name="table">The target QActionTable.</param>
        /// <param name="key">The primary key of the row.</param>
        /// <param name="columnIdx">The target column IDX.</param>
        public static object GetCell(this QActionTable table, string key, int columnIdx)
        {
            return table.Protocol.GetCell(table.TableId, key, columnIdx);
        }

        /// <summary>Retrieves the cell data for the specified row and column.</summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="tableId">The ID of the table parameter.</param>
        /// <param name="key">The primary key of the row.</param>
        /// <param name="columnIdx">The target column IDX.</param>
        public static object GetCell(this SLProtocol protocol, int tableId, string key, int columnIdx)
        {
            return protocol.GetParameterIndexByKey(tableId, key, columnIdx + 1); // + 1 is to rectify for 1-based indexing this method uses.
        }

        /// <summary>Update cells within a column using a collection of keys and values.</summary>
        /// <typeparam name="TColumnType">The column type.</typeparam>
        /// <param name="table">The target QActionTable.</param>
        /// <param name="columnPid">The ID of the column parameter.</param>
        /// <param name="keyValues">The collection of primary keys and values to set.</param>
        public static void SetColumn<TColumnType>(this QActionTable table, int columnPid, IDictionary<string, TColumnType> keyValues)
        {
            table.Protocol.SetColumn(table.TableId, columnPid, keyValues);
        }

        /// <summary>Update cells within a column using a collection of keys and values.</summary>
        /// <typeparam name="TColumnType">The column type.</typeparam>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="tableId">The ID of the table parameter.</param>
        /// <param name="columnPid">The ID of the column parameter.</param>
        /// <param name="keyValues">The collection of primary keys and values to set.</param>
        public static void SetColumn<TColumnType>(this SLProtocol protocol, int tableId, int columnPid, IDictionary<string, TColumnType> keyValues)
        {
            if (keyValues == null || keyValues.Count == 0)
                return;

            var tableInfo = new object[] { tableId, columnPid };

            var values = keyValues.Values is ICollection<object> ? keyValues.Values as ICollection<object> : keyValues.Values.Cast<object>();

            var tableData = new object[] { keyValues.Keys.ToArray<object>(), (values == null ? Array.Empty<object>() : values).ToArray() };

            protocol.NotifyProtocol((int)NotifyType.NT_FILL_ARRAY_WITH_COLUMN, tableInfo, tableData);
        }

        /// <summary>Sets the cell data for the specified row and column.</summary>
        /// <typeparam name="TCellType">The cell type.</typeparam>
        /// <param name="table">The target QActionTable.</param>
        /// <param name="key">The primary key of the row.</param>
        /// <param name="columnIdx">The target column IDX.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>
        ///   <c>true</c> if the cell value has changed; otherwise, <c>false</c>.
        /// </returns>
        public static bool SetCell<TCellType>(this QActionTable table, string key, int columnIdx, TCellType value)
        {
            return table.Protocol.SetCell(table.TableId, key, columnIdx, value);
        }

        /// <summary>Sets the cell data for the specified row and column.</summary>
        /// <typeparam name="TCellType">The cell type.</typeparam>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="tableId">The ID of the table parameter.</param>
        /// <param name="key">The primary key of the row.</param>
        /// <param name="columnIdx">The target column IDX.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>
        ///   <c>true</c> if the cell value has changed; otherwise, <c>false</c>.
        /// </returns>
        public static bool SetCell<TCellType>(this SLProtocol protocol, int tableId, string key, int columnIdx, TCellType value)
        {
            return protocol.SetParameterIndexByKey(tableId, key, columnIdx + 1, value); // + 1 is to rectify for 1-based indexing this method uses.
        }

        /// <summary>Retrieves all keys matching the specified value found in the target column.
        /// <br/><br/>
        /// Note:<br/>
        /// In order for this method to work, the column must either be a foreign key column or it must have the option 'indexColumn'.
        /// </summary>
        /// <param name="table">The target QActionTable.</param>
        /// <param name="columnPid">The ID of the column parameter.</param>
        /// <param name="value">The value to find within the target column.</param>
        /// <param name="caseSensitive">If set to <c>true</c> [case sensitive].</param>
        public static string[] GetKeysForIndex(this QActionTable table, int columnPid, string value, bool caseSensitive = false)
        {
            return table.Protocol.GetKeysForIndex(columnPid, value, caseSensitive);
        }

        /// <summary>Retrieves all keys matching the specified value found in the target column.
        /// <br/><br/>
        /// Note:<br/>
        /// In order for this method to work, the column must either be a foreign key column or it must have the option 'indexColumn'.
        /// </summary>
        /// <param name="protocol">Instance that implements SLProtocol.</param>
        /// <param name="columnPid">The ID of the column parameter.</param>
        /// <param name="value">The value to find within the target column.</param>
        /// <param name="caseSensitive">If set to <c>true</c> [case sensitive].</param>
        public static string[] GetKeysForIndex(this SLProtocol protocol, int columnPid, string value, bool caseSensitive = false)
        {
            var notifyType = caseSensitive ? NotifyType.NT_GET_KEYS_FOR_INDEX_CASED : NotifyType.GetKeysForIndex;

            return protocol.NotifyProtocol((int)notifyType, columnPid, value) as string[];
        }

        /// <summary>Replace all the rows in the target table with the specified data.</summary>
        /// <typeparam name="TQActionTableRow">The QActionTableRow type representing the table row.</typeparam>
        /// <param name="table">The target QActionTable.</param>
        /// <param name="rows">The row data.</param>
        /// <param name="timeInfo">Time stamp.</param>
        public static object FillArray<TQActionTableRow>(this QActionTable table, List<TQActionTableRow> rows, DateTime? timeInfo = null)
            where TQActionTableRow : QActionTableRow, new()
        {
            return table.FillArray(rows.ToArray<QActionTableRow>(), timeInfo);
        }

        /// <summary>Update and Append rows in the target table using the specified data.</summary>
        /// <typeparam name="TQActionTableRow">The QActionTableRow type representing the table row.</typeparam>
        /// <param name="table">The target QActionTable.</param>
        /// <param name="rows">The row data.</param>
        /// <param name="timeInfo">Time stamp.</param>
        public static object FillArrayNoDelete<TQActionTableRow>(this QActionTable table, List<TQActionTableRow> rows, DateTime? timeInfo = null)
            where TQActionTableRow : QActionTableRow, new()
        {
            return table.FillArrayNoDelete(rows.ToArray<QActionTableRow>(), timeInfo);
        }
    }
}