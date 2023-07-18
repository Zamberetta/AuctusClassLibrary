using System;

#pragma warning disable S4070 // Non-flags enums should not be marked with "FlagsAttribute"

namespace Auctus.DataMiner.Library.Common.Type
{
    public static class TimeSpanExtensions
    {
        [Flags]
        public enum Format : long
        {
            Years = 31536000000,
            Weeks = 604800000,
            Days = 86400000,
            Hours = 3600000,
            Minutes = 60000,
            Seconds = 1000,
            Milliseconds = 1,

            All = LongDate | LongTime,
            Date = Weeks | Days,
            LongDate = Years | Weeks | Days,
            Time = Hours | Minutes | Seconds,
            ShortTime = Hours | Minutes,
            LongTime = Hours | Minutes | Seconds | Milliseconds,
        }

        /// <summary>Converts the TimeSpan representation to a readable string.</summary>
        /// <param name="simplified">If set to <c>true</c>, 5h 5m 43s; Otherwise, 5 hours 5 minutes 43 seconds.</param>
        /// <param name="format">The desired format.</param>
        /// <returns>
        ///   Default: 3 years 48 weeks 6 days 5 hours 5 minutes 43 seconds 12 milliseconds
        ///   <br />
        ///   Simplified: 3y 48w 6d 5h 5m 43s 12ms
        /// </returns>
        public static string ToReadableString(this TimeSpan span, bool simplified = false, Format format = Format.All)
        {
            var totalMilliseconds = span.Duration().TotalMilliseconds;

            var years = format.CalculateComponent(Format.Years, simplified, ref totalMilliseconds);
            var weeks = format.CalculateComponent(Format.Weeks, simplified, ref totalMilliseconds);
            var days = format.CalculateComponent(Format.Days, simplified, ref totalMilliseconds);
            var hours = format.CalculateComponent(Format.Hours, simplified, ref totalMilliseconds);
            var minutes = format.CalculateComponent(Format.Minutes, simplified, ref totalMilliseconds);
            var seconds = format.CalculateComponent(Format.Seconds, simplified, ref totalMilliseconds);
            var milliseconds = format.CalculateComponent(Format.Milliseconds, simplified, ref totalMilliseconds);

            var formatted = string.Format("{0}{1}{2}{3}{4}{5}{6}",
            years,
            weeks,
            days,
            hours,
            minutes,
            seconds,
            milliseconds);

            if (string.IsNullOrEmpty(formatted))
            {
                return simplified ? "0s" : "0 seconds";
            }

            if (formatted.EndsWith(" "))
            {
                formatted = formatted.Substring(0, formatted.Length - 1);
            }

            return formatted;
        }

        private static string CalculateComponent(this Format format, Format targetFormat, bool simplified, ref double totalMilliseconds)
        {
            var total = 0d;

            if (format.HasFlag(targetFormat))
            {
                var calculatedTotal = targetFormat == Format.Milliseconds ? totalMilliseconds : Math.Floor(totalMilliseconds / (double)targetFormat);

                if (calculatedTotal > 0)
                {
                    total = calculatedTotal;
                    totalMilliseconds -= total * (double)targetFormat;
                }
            }

            if (total == 0)
            {
                return string.Empty;
            }

            var componentName = targetFormat.ComponentName(simplified);

            if (simplified)
            {
                return $"{total:0}{componentName} ";
            }
            else
            {
                return $"{total:0} {componentName}{total.Pluralise()} ";
            }
        }

        private static string ComponentName(this Format format, bool simplified)
        {
            string componentName;

            switch (format)
            {
                case Format.Years:
                    componentName = simplified ? "y" : "year";
                    break;

                case Format.Weeks:
                    componentName = simplified ? "w" : "week";
                    break;

                case Format.Days:
                    componentName = simplified ? "d" : "day";
                    break;

                case Format.Hours:
                    componentName = simplified ? "h" : "hour";
                    break;

                case Format.Minutes:
                    componentName = simplified ? "m" : "minute";
                    break;

                case Format.Seconds:
                    componentName = simplified ? "s" : "second";
                    break;

                case Format.Milliseconds:
                    componentName = simplified ? "ms" : "millisecond";
                    break;

                default:
                    componentName = string.Empty;
                    break;
            }

            return componentName;
        }

        private static string Pluralise(this double value)
        {
            if (value > 1)
            {
                return "s";
            }

            return string.Empty;
        }
    }
}