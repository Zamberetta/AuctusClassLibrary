using Skyline.DataMiner.Net;
using Skyline.DataMiner.Net.Messages;
using Skyline.DataMiner.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auctus.Class.Library.Tests
{
    internal class MockProtocol : SLProtocol
    {
        private readonly MockTable MockTables = new MockTable();

        public string LastLogEntry = string.Empty;

        public string[] UserGroups = new string[]
        {
            "Administrator",
            "Painter",
        };

        public MockTable.Table InitialiseMockTable(int tablePid, int index = 0, int displayIndex = 0)
        {
            return MockTables.GetTable(ElementName, tablePid, index, displayIndex);
        }

        public Dictionary<int, ConnectivityConnection> ConnectivityConnections => throw new NotImplementedException();

        public Dictionary<int, ConnectivityInterface> ConnectivityInterfaces => throw new NotImplementedException();

        public double Leave => throw new NotImplementedException();

        public double Clear => throw new NotImplementedException();

        public int IsActive => throw new NotImplementedException();

        public SLNetConnection SLNet { get; set; } = new SLNetConnection();

        public int QActionID { get; set; } = 123;
        public string ElementProtocolVersion { get; set; }
        public string ProtocolVersion { get; set; } = "1.0.0.1";
        public string ProtocolName { get; set; } = "Mock Protocol";
        public string MetricsKey { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string UserCookie { get; set; } = "dca9699739ac415ea31df250f377b0dc";

        public string UserInfo => throw new NotImplementedException();

        public string ElementName { get; set; } = "Mock Element";

        public int ElementID { get; set; } = 567;

        public int DataMinerID { get; set; } = 1234;

        public void AddRow(int tableId, object[] row, bool[] keyMask)
        {
            throw new NotImplementedException();
        }

        public int AddRow(int tableId, object[] row)
        {
            var table = MockTables.GetTable(ElementName, tableId);
            var key = Convert.ToString(row[table.Index]);

            if (string.IsNullOrWhiteSpace(key))
            {
                key = table.GetNextKey();
            }

            for (var i = 0; i < row.Length; i++)
            {
                object cellValue;

                if (i == table.Index)
                {
                    cellValue = key;
                }
                else if (i == table.DisplayIndex)
                {
                    cellValue = Convert.ToString(row[i]);
                }
                else
                {
                    cellValue = row[i];
                }

                var column = table.GetColumn(i);

                column.AddCell(key, cellValue);
            }

            return table.Columns[table.Index].Cells.Count;
        }

        public int AddRow(int tableId, string row)
        {
            throw new NotImplementedException();
        }

        public string AddRowReturnKey(int tableId, object[] row)
        {
            throw new NotImplementedException();
        }

        public string AddRowReturnKey(int tableId)
        {
            throw new NotImplementedException();
        }

        public int CheckTrigger(int iID)
        {
            throw new NotImplementedException();
        }

        public object ClearAllKeys(int tableId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteConnectivityConnection(int id, int dmaID, int eleID, bool deleteBothConnections)
        {
            throw new NotImplementedException();
        }

        public bool DeleteConnectivityConnectionProperty(int id, int dmaID, int eleID, bool both)
        {
            throw new NotImplementedException();
        }

        public bool DeleteConnectivityConnectionProperty(int id, int dmaID, int eleID)
        {
            throw new NotImplementedException();
        }

        public bool DeleteConnectivityConnectionProperty(int id, int connId, int dmaID, int eleID, bool both)
        {
            throw new NotImplementedException();
        }

        public bool DeleteConnectivityConnectionProperty(int id, int connId, int dmaID, int eleID)
        {
            throw new NotImplementedException();
        }

        public bool DeleteConnectivityInterfaceProperty(int id, int dmaID, int eleID)
        {
            throw new NotImplementedException();
        }

        public bool DeleteConnectivityInterfaceProperty(int id, int itfId, int dmaID, int eleID)
        {
            throw new NotImplementedException();
        }

        public int DeleteRow(int tableId, string[] rows)
        {
            var table = MockTables.GetTable(ElementName, tableId);

            if ((table.Columns.Count - 1) < table.Index)
            {
                return 0;
            }

            foreach (var column in table.Columns)
            {
                foreach (var row in rows)
                {
                    column.Value.Cells.Remove(row);
                }
            }

            return table.Columns[table.Index].Cells.Count;
        }

        public int DeleteRow(int tableId, int row)
        {
            throw new NotImplementedException();
        }

        public int DeleteRow(int tableId, string rowKey)
        {
            throw new NotImplementedException();
        }

        public void DisposeResources()
        {
            throw new NotImplementedException();
        }

        public ExecuteScriptResponseMessage ExecuteScript(ExecuteScriptMessage message)
        {
            throw new NotImplementedException();
        }

        public ExecuteScriptResponseMessage ExecuteScript(string scriptName)
        {
            throw new NotImplementedException();
        }

        public bool Exists(int tableId, string key)
        {
            throw new NotImplementedException();
        }

        public object FillArray(int tableId, List<object[]> rows, NotifyProtocol.SaveOption option, DateTime? timeInfo)
        {
            throw new NotImplementedException();
        }

        public object FillArray(int tableId, List<object[]> rows, NotifyProtocol.SaveOption option)
        {
            throw new NotImplementedException();
        }

        public object FillArray(int tableId, List<object[]> columns, DateTime? timeInfo)
        {
            throw new NotImplementedException();
        }

        public object FillArray(int tableId, List<object[]> columns)
        {
            throw new NotImplementedException();
        }

        public object FillArray(int tableId, object[] columns, DateTime? timeInfo)
        {
            var table = MockTables.GetTable(ElementName, tableId);

            table.ClearTable();

            foreach (var column in columns)
            {
                if (column == null) continue;

                object[] row;

                if (column.GetType().BaseType == typeof(QActionTableRow))
                {
                    row = ((QActionTableRow)column).ToObjectArray();
                }
                else
                {
                    row = (object[])column;
                }

                AddRow(tableId, row);
            }

            return true;
        }

        public object FillArray(int tableId, object[] columns)
        {
            throw new NotImplementedException();
        }

        public object FillArrayNoDelete(int tableId, List<object[]> columns, DateTime? timeInfo)
        {
            throw new NotImplementedException();
        }

        public object FillArrayNoDelete(int tableId, List<object[]> columns)
        {
            throw new NotImplementedException();
        }

        public object FillArrayNoDelete(int tableId, object[] columns, DateTime? timeInfo)
        {
            var table = MockTables.GetTable(ElementName, tableId);

            foreach (var column in columns)
            {
                if (column == null) continue;

                object[] row;

                if (column.GetType().BaseType == typeof(QActionTableRow))
                {
                    row = ((QActionTableRow)column).ToObjectArray();
                }
                else
                {
                    row = (object[])column;
                }

                var key = (string)row[table.Index];

                if (table.ContainsKey(key))
                {
                    SetRow(tableId, key, row);
                    continue;
                }

                AddRow(tableId, row);
            }

            return true;
        }

        public object FillArrayNoDelete(int tableId, object[] columns)
        {
            throw new NotImplementedException();
        }

        public object FillArrayWithColumn(int tableId, int columnPid, object[] keys, object[] values, DateTime? timeInfo)
        {
            throw new NotImplementedException();
        }

        public object FillArrayWithColumn(int tableId, int columnPid, object[] keys, object[] values)
        {
            throw new NotImplementedException();
        }

        public ConnectivityConnection GetConnectivityConnection(int DMAId, int EId, string name, bool exported)
        {
            throw new NotImplementedException();
        }

        public ConnectivityConnection GetConnectivityConnection(int DMAId, int EId, string name)
        {
            throw new NotImplementedException();
        }

        public ConnectivityConnection GetConnectivityConnection(int DMAId, int EId, int ConnectionId, bool exported)
        {
            throw new NotImplementedException();
        }

        public ConnectivityConnection GetConnectivityConnection(int DMAId, int EId, int ConnectionId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, ConnectivityConnection> GetConnectivityConnections(int DMAId, int EId, string nameFilter, bool exported)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, ConnectivityConnection> GetConnectivityConnections(int DMAId, int EId, string nameFilter)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, ConnectivityConnection> GetConnectivityConnections(int DMAId, int EId, bool exported)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, ConnectivityConnection> GetConnectivityConnections(int DMAId, int EId)
        {
            throw new NotImplementedException();
        }

        public ConnectivityInterface GetConnectivityInterface(int DMAId, int EId, string name, bool customName, bool exported)
        {
            throw new NotImplementedException();
        }

        public ConnectivityInterface GetConnectivityInterface(int DMAId, int EId, string name, bool customName)
        {
            throw new NotImplementedException();
        }

        public ConnectivityInterface GetConnectivityInterface(int DMAId, int EId, int ItfId, bool exported)
        {
            throw new NotImplementedException();
        }

        public ConnectivityInterface GetConnectivityInterface(int DMAId, int EId, int ItfId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, ConnectivityInterface> GetConnectivityInterfaces(int DMAId, int EId, string nameFilter, bool customName, bool exported)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, ConnectivityInterface> GetConnectivityInterfaces(int DMAId, int EId, string nameFilter, bool customName)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, ConnectivityInterface> GetConnectivityInterfaces(int DMAId, int EId, bool exported)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, ConnectivityInterface> GetConnectivityInterfaces(int DMAId, int EId)
        {
            throw new NotImplementedException();
        }

        public object GetData(string from, int iID)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, ConnectivityInterface> GetExternalConnectivityInterfaces(int DMAId, int EId)
        {
            throw new NotImplementedException();
        }

        public object GetInputParameter(int j)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, ConnectivityInterface> GetInternalConnectivityInterfaces(int DMAId, int EId)
        {
            throw new NotImplementedException();
        }

        public int GetKeyPosition(int iPID, string key)
        {
            throw new NotImplementedException();
        }

        public string[] GetKeys(int tableId, NotifyProtocol.KeyType type)
        {
            var table = MockTables.GetTable(ElementName, tableId);

            var targetColumn = 9999;

            switch (type)
            {
                case Skyline.DataMiner.Scripting.NotifyProtocol.KeyType.Index:
                    targetColumn = table.Index;
                    break;

                case Skyline.DataMiner.Scripting.NotifyProtocol.KeyType.DisplayKey:
                    targetColumn = table.DisplayIndex;
                    break;
            }

            if ((table.Columns.Count - 1) < targetColumn)
            {
                return new string[0];
            }

            return table.Columns[targetColumn].Cells.Values.OfType<string>().ToArray();
        }

        public string[] GetKeys(int tableId)
        {
            return GetKeys(tableId, Skyline.DataMiner.Scripting.NotifyProtocol.KeyType.Index);
        }

        public string GetLocalDatabaseType()
        {
            throw new NotImplementedException();
        }

        public object GetParameter(int iID)
        {
            throw new NotImplementedException();
        }

        public object GetParameterByData(string data)
        {
            throw new NotImplementedException();
        }

        public object GetParameterByName(string name)
        {
            throw new NotImplementedException();
        }

        public object GetParameterDescription(int iID)
        {
            throw new NotImplementedException();
        }

        public object GetParameterIndex(int iID, int iX, int iY)
        {
            throw new NotImplementedException();
        }

        public object GetParameterIndexByKey(int iPID, string key, int iY)
        {
            var table = MockTables.GetTable(ElementName, iPID);
            var column = table.GetColumn(iY - 1);

            if (column.Cells.ContainsKey(key))
            {
                return column.Cells[key];
            }

            return null;
        }

        public object GetParameterItemData(string data)
        {
            throw new NotImplementedException();
        }

        public object GetParameters(object ids)
        {
            throw new NotImplementedException();
        }

        public object GetRow(int iPID, int iRow)
        {
            throw new NotImplementedException();
        }

        public object GetRow(int iPID, string key)
        {
            throw new NotImplementedException();
        }

        public int GetTriggerParameter()
        {
            return 456;
        }

        public IConnection GetUserConnection()
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty(int iID)
        {
            throw new NotImplementedException();
        }

        public void Log(string message, LogType logType, LogLevel logLevel)
        {
            LastLogEntry = $"{logType}|{logLevel}|{message}";
            Console.WriteLine(LastLogEntry);
        }

        public void Log(string message, LogLevel logLevel)
        {
            LastLogEntry = $"{logLevel}|{message}";
            Console.WriteLine(LastLogEntry);
        }

        public void Log(string message, LogType logType)
        {
            LastLogEntry = $"{logType}|{message}";
            Console.WriteLine(LastLogEntry);
        }

        public void Log(string message)
        {
            LastLogEntry = $"{message}";
            Console.WriteLine(LastLogEntry);
        }

        public int Log(int iType, int iLevel, string message)
        {
            LastLogEntry = $"{iType}|{iLevel}|{message}";
            Console.WriteLine(LastLogEntry);
            return 0;
        }

        public object NewRow()
        {
            throw new NotImplementedException();
        }

        public object NotifyClient(int iType, object value1, object value2)
        {
            throw new NotImplementedException();
        }

        public object NotifyDataMiner(int iType, object value1, object value2)
        {
            switch (iType)
            {
                case (int)NotifyType.GetUser:
                    return NtGetUser((string)value1);

                case (int)NotifyType.GetUserInfo:
                    return NtGetUserInfo((string)value1);

                default:
                    throw new NotImplementedException();
            }
        }

        private object NtGetUser(string userCookie)
        {
            if (UserCookie != userCookie)
            {
                return string.Empty;
            }

            return "bross";
        }

        private object NtGetUserInfo(string accountName)
        {
            var userInfo = new string[]
            {
                accountName,
                "Bob Ross",
                "bob.ross@happy-accidents.com",
                "01234567891",
                "07654321987",
            };

            userInfo = userInfo.Concat(UserGroups).ToArray();

            return new object[]
            {
                userInfo,
            };
        }

        public int NotifyDataMinerQueued(int iType, object value1, object value2)
        {
            throw new NotImplementedException();
        }

        public object NotifyProtocol(int iType, object value1, object value2)
        {
            switch (iType)
            {
                case (int)NotifyType.AddRow:
                    return AddRow((int)value1, (object[])value2);

                case (int)NotifyType.DeleteRow:
                    return DeleteRow((int)value1, (string[])value2);

                case (int)NotifyType.FillArray:
                    return FillArray((int)((object[])value1)[0], (object[])value2, (DateTime)((object[])value1)[2]);

                case (int)NotifyType.FillArrayNoDelete:
                    return FillArrayNoDelete((int)((object[])value1)[0], (object[])value2, (DateTime)((object[])value1)[2]);

                case (int)NotifyType.GetKeysForIndex:
                    return NtGetKeysForIndex((int)value1, (string)value2, false);

                case (int)NotifyType.NT_FILL_ARRAY_WITH_COLUMN:
                    return NtFillArrayWithColumn((object[])value1, (object[])value2);

                case (int)NotifyType.NT_GET_TABLE_COLUMNS:
                    return NtGetTableColumns((int)value1, (uint[])value2);

                case (int)NotifyType.NT_GET_KEYS_SLPROTOCOL:
                    return GetKeys((int)value1);

                case (int)NotifyType.NT_GET_KEYS_FOR_INDEX_CASED:
                    return NtGetKeysForIndex((int)value1, (string)value2, true);

                default:
                    throw new NotImplementedException();
            }
        }

        private object NtFillArrayWithColumn(object[] tableInfo, object[] tableData)
        {
            var tablePid = (int)tableInfo[0];
            var columnPid = (int)tableInfo[1];

            var table = MockTables.GetTable(ElementName, tablePid);
            var column = table.GetColumnByPid(columnPid);

            var rowCount = tableData.Length > 0 ? ((object[])tableData[0]).Length : 0;

            for (int i = 0; i < rowCount; i++)
            {
                var key = Convert.ToString(((object[])tableData[0])[i]);
                var value = ((object[])tableData[1])[i];

                column.UpdateCell(key, value);
            }

            return null;
        }

        private object NtGetTableColumns(int tablePid, uint[] columnIndexes)
        {
            var table = MockTables.GetTable(ElementName, tablePid);
            var columns = new object[columnIndexes.Length];

            for (var i = 0; i < columnIndexes.Length; i++)
            {
                uint columnIndex = columnIndexes[i];
                var column = table.GetColumn((int)columnIndex);
                columns[i] = column.Cells.Values.ToArray();
            }

            return columns;
        }

        private object NtGetKeysForIndex(int columnPid, string value, bool caseSensitive)
        {
            var stringComparison = caseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase;
            var table = MockTables.GetTableByColumnPid(ElementName, columnPid);
            var column = table.GetColumnByPid(columnPid);
            var keys = new List<string>();

            foreach (var cell in column.Cells)
            {
                if (string.Equals(Convert.ToString(cell.Value), value, stringComparison))
                {
                    keys.Add(cell.Key);
                }
            }

            return keys.ToArray();
        }

        public object OldRow()
        {
            throw new NotImplementedException();
        }

        public object Row(object theArray, int iRow)
        {
            throw new NotImplementedException();
        }

        public int RowCount(int tableId)
        {
            var table = MockTables.GetTable(ElementName, tableId);

            if ((table.Columns.Count - 1) < table.Index)
            {
                return 0;
            }

            return table.Columns[table.Index].Cells.Count;
        }

        public int RowCount(object theArray)
        {
            return RowCount((int)theArray);
        }

        public int RowIndex()
        {
            throw new NotImplementedException();
        }

        public string RowKey()
        {
            throw new NotImplementedException();
        }

        public object RowPingTimestamp()
        {
            throw new NotImplementedException();
        }

        public object RowRTT()
        {
            throw new NotImplementedException();
        }

        public int SendToDisplay(int iID, int[] iXs, int[] iYs)
        {
            throw new NotImplementedException();
        }

        public int SendToDisplay(int iID, int iX, int iY)
        {
            throw new NotImplementedException();
        }

        public int SendToDisplay(int iID)
        {
            throw new NotImplementedException();
        }

        public int SetParameter(int iID, object value, ValueType timeInfo)
        {
            throw new NotImplementedException();
        }

        public int SetParameter(int iID, object value)
        {
            throw new NotImplementedException();
        }

        public void SetParameterBinary(int pid, byte[] data)
        {
            throw new NotImplementedException();
        }

        public int SetParameterByData(string data, object value)
        {
            throw new NotImplementedException();
        }

        public int SetParameterByName(string name, object value)
        {
            throw new NotImplementedException();
        }

        public int SetParameterDescription(int iID, object value)
        {
            throw new NotImplementedException();
        }

        public bool SetParameterIndex(int iID, int iX, int iY, object value, ValueType timeInfo)
        {
            throw new NotImplementedException();
        }

        public bool SetParameterIndex(int iID, int iX, int iY, object value)
        {
            throw new NotImplementedException();
        }

        public bool SetParameterIndexByKey(int iID, string key, int iY, object value, ValueType timeInfo)
        {
            throw new NotImplementedException();
        }

        public bool SetParameterIndexByKey(int iID, string key, int iY, object value)
        {
            var table = MockTables.GetTable(ElementName, iID);
            var column = table.GetColumn(iY - 1);

            return column.UpdateCell(key, value);
        }

        public int SetParameterItemData(string data, object value)
        {
            throw new NotImplementedException();
        }

        public object SetParameters(int[] ids, object[] values, DateTime[] timeInfos)
        {
            throw new NotImplementedException();
        }

        public object SetParameters(int[] ids, object[] values)
        {
            throw new NotImplementedException();
        }

        public object SetParametersByData(string[] datas, object[] values)
        {
            throw new NotImplementedException();
        }

        public object SetParametersByName(string[] names, object[] values)
        {
            throw new NotImplementedException();
        }

        public object SetParametersIndex(int[] ids, int[] iXs, int[] iYs, object[] values, DateTime[] timeInfos)
        {
            throw new NotImplementedException();
        }

        public object SetParametersIndex(int[] ids, int[] iXs, int[] iYs, object[] values)
        {
            throw new NotImplementedException();
        }

        public object SetParametersIndexByKey(int[] ids, string[] keys, int[] iYs, object[] values, DateTime[] timeInfos)
        {
            throw new NotImplementedException();
        }

        public object SetParametersIndexByKey(int[] ids, string[] keys, int[] iYs, object[] values)
        {
            throw new NotImplementedException();
        }

        public object SetParametersItemData(string[] datas, object[] values)
        {
            throw new NotImplementedException();
        }

        public int SetReadParameterByName(string name, object value)
        {
            throw new NotImplementedException();
        }

        public object SetReadParametersByName(string[] names, object[] values)
        {
            throw new NotImplementedException();
        }

        public object SetRow(int iPID, int iRow, object row, ValueType timeInfo, bool bOverrideBehaviour)
        {
            throw new NotImplementedException();
        }

        public object SetRow(int iPID, int iRow, object row, ValueType timeInfo)
        {
            throw new NotImplementedException();
        }

        public object SetRow(int iPID, int iRow, object row, bool bOverrideBehaviour)
        {
            throw new NotImplementedException();
        }

        public object SetRow(int iPID, int iRow, object row)
        {
            throw new NotImplementedException();
        }

        public object SetRow(int iPID, string key, object row, ValueType timeInfo, bool bOverrideBehaviour)
        {
            throw new NotImplementedException();
        }

        public object SetRow(int iPID, string key, object row, ValueType timeInfo)
        {
            throw new NotImplementedException();
        }

        public object SetRow(int iPID, string key, object row, bool bOverrideBehaviour)
        {
            throw new NotImplementedException();
        }

        // TODO: What exception do you get if a row does not exist?
        // TODO: What is acutually returned from SetRow?
        public object SetRow(int iPID, string key, object row)
        {
            var table = MockTables.GetTable(ElementName, iPID);

            if (!table.ContainsKey(key))
            {
                throw new ArgumentException("Row does not exist.");
            }

            var cells = (object[])row;

            for (var i = 0; i < cells.Length; i++)
            {
                object cellValue;

                if (i == table.Index)
                {
                    cellValue = key;
                }
                else if (i == table.DisplayIndex)
                {
                    cellValue = Convert.ToString(cells[i]);
                }
                else
                {
                    cellValue = cells[i];
                }

                var column = table.GetColumn(i);

                column.UpdateCell(key, cellValue);
            }

            return cells;
        }

        public int SetWriteParameterByName(string name, object value)
        {
            throw new NotImplementedException();
        }

        public object SetWriteParametersByName(string[] names, object[] values)
        {
            throw new NotImplementedException();
        }

        public void ShowInformationMessage(string message, string caption)
        {
            throw new NotImplementedException();
        }

        public void ShowInformationMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}