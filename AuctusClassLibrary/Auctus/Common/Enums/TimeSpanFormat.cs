using System;

#pragma warning disable S4070 // Non-flags enums should not be marked with "FlagsAttribute"

namespace Auctus.DataMiner.Library.Auctus.Common
{
    /// <summary>
    ///   Enum used in conjunction with the ToReadableString TimeSpan method.
    /// </summary>
    [Flags]
    public enum TimeSpanFormat : long
    {
        /// <summary>
        /// Years.
        /// </summary>
        Years = 31536000000,

        /// <summary>
        /// Weeks.
        /// </summary>
        Weeks = 604800000,

        /// <summary>
        /// Days.
        /// </summary>
        Days = 86400000,

        /// <summary>
        /// Hours.
        /// </summary>
        Hours = 3600000,

        /// <summary>
        /// Minutes.
        /// </summary>
        Minutes = 60000,

        /// <summary>
        /// Seconds.
        /// </summary>
        Seconds = 1000,

        /// <summary>
        /// Milliseconds.
        /// </summary>
        Milliseconds = 1,

        /// <summary>
        /// Years, Weeks, Days, Hours, Minutes, Seconds and Milliseconds.
        /// </summary>
        All = LongDate | LongTime,

        /// <summary>
        /// Weeks and Days.
        /// </summary>
        Date = Weeks | Days,

        /// <summary>
        /// Years, Weeks and Days.
        /// </summary>
        LongDate = Years | Weeks | Days,

        /// <summary>
        /// Hours, Minutes and Seconds.
        /// </summary>
        Time = Hours | Minutes | Seconds,

        /// <summary>
        /// Hours and Minutes.
        /// </summary>
        ShortTime = Hours | Minutes,

        /// <summary>
        /// Hours, Minutes, Seconds and Milliseconds.
        /// </summary>
        LongTime = Hours | Minutes | Seconds | Milliseconds,
    }
}