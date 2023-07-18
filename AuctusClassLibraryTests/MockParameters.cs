using Skyline.DataMiner.Scripting;

#pragma warning disable S1118 // Utility classes should not have public constructors
#pragma warning disable S3218 // Inner class members should not shadow outer class "static" or type members

namespace Auctus.Class.Library.Tests
{
    internal class MockParameters
    {
        public const int mockparameter_100 = 100;

        public static class Write
        {
            public const int mockparameter_101 = 101;
        }

        public class Mockbasictable
        {
            public const int tablePid = 1000;
            public const int indexColumn = 0;
            public const int indexColumnPid = 1001;

            public class Pid
            {
                public const int mockbasictableindex_1001 = 1001;
                public const int mockbasictablestringvalue_1002 = 1002;
                public const int mockbasictabledoublevalue_1003 = 1003;
                public const int mockbasictabledatetimevalue_1004 = 1004;
                public const int mockbasictabledisplaykey_1005 = 1005;

                public class Write
                {
                    public const int mockbasictablebutton_2006 = 2006;
                }
            }

            public class Idx
            {
                public const int mockbasictableindex_1001 = 0;
                public const int mockbasictablestringvalue_1002 = 1;
                public const int mockbasictabledoublevalue_1003 = 2;
                public const int mockbasictabledatetimevalue_1004 = 3;
                public const int mockbasictabledisplaykey_1005 = 4;
            }
        }

        public class MockbasictableQActionRow : QActionTableRow
        {
            public object Mockbasictableinstance_1001
            { get { if (Columns.ContainsKey(0)) { return Columns[0]; } else { return null; } } set { if (Columns.ContainsKey(0)) { Columns[0] = value; } else { Columns.Add(0, value); } } }

            public object Mockbasictablestringvalue_1002
            { get { if (Columns.ContainsKey(1)) { return Columns[1]; } else { return null; } } set { if (Columns.ContainsKey(1)) { Columns[1] = value; } else { Columns.Add(1, value); } } }

            public object Mockbasictabledoublevalue_1003
            { get { if (Columns.ContainsKey(2)) { return Columns[2]; } else { return null; } } set { if (Columns.ContainsKey(2)) { Columns[2] = value; } else { Columns.Add(2, value); } } }

            public object Mockbasictabledatetimevalue_1004
            { get { if (Columns.ContainsKey(3)) { return Columns[3]; } else { return null; } } set { if (Columns.ContainsKey(3)) { Columns[3] = value; } else { Columns.Add(3, value); } } }

            public object Mockbasictabledisplaykey_1005
            { get { if (Columns.ContainsKey(4)) { return Columns[4]; } else { return null; } } set { if (Columns.ContainsKey(4)) { Columns[4] = value; } else { Columns.Add(4, value); } } }

            public object Mockbasictablebutton_2006
            { get { if (Columns.ContainsKey(5)) { return Columns[5]; } else { return null; } } set { if (Columns.ContainsKey(5)) { Columns[5] = value; } else { Columns.Add(5, value); } } }

            public MockbasictableQActionRow() : base(0, 6)
            {
            }

            public MockbasictableQActionRow(object[] oRow) : base(0, 6, oRow)
            {
            }

            public static implicit operator MockbasictableQActionRow(object[] source)
            {
                return new MockbasictableQActionRow(source);
            }

            public static implicit operator object[](MockbasictableQActionRow source)
            {
                return source.ToObjectArray();
            }
        }
    }
}