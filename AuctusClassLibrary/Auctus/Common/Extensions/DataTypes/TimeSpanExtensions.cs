using Auctus.DataMiner.Library.Auctus.Common;
using System;

namespace Auctus.DataMiner.Library.Common.Type
{
    /// <summary>
    ///   Extension methods for the TimeSpan type.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>Converts the TimeSpan representation to a readable string.</summary>
        /// <param name="span">The target TimeSpan.</param>
        /// <param name="simplified">If set to <c>true</c>, 5h 5m 43s; Otherwise, 5 hours 5 minutes 43 seconds.</param>
        /// <param name="format">The desired format.</param>
        /// <returns>
        ///   Default: 3 years 48 weeks 6 days 5 hours 5 minutes 43 seconds 12 milliseconds
        ///   <br />
        ///   Simplified: 3y 48w 6d 5h 5m 43s 12ms
        /// </returns>
        public static string ToReadableString(this TimeSpan span, bool simplified = false, TimeSpanFormat format = TimeSpanFormat.All)
        {
            var totalMilliseconds = span.Duration().TotalMilliseconds;

            var years = format.CalculateComponent(TimeSpanFormat.Years, simplified, ref totalMilliseconds);
            var weeks = format.CalculateComponent(TimeSpanFormat.Weeks, simplified, ref totalMilliseconds);
            var days = format.CalculateComponent(TimeSpanFormat.Days, simplified, ref totalMilliseconds);
            var hours = format.CalculateComponent(TimeSpanFormat.Hours, simplified, ref totalMilliseconds);
            var minutes = format.CalculateComponent(TimeSpanFormat.Minutes, simplified, ref totalMilliseconds);
            var seconds = format.CalculateComponent(TimeSpanFormat.Seconds, simplified, ref totalMilliseconds);
            var milliseconds = format.CalculateComponent(TimeSpanFormat.Milliseconds, simplified, ref totalMilliseconds);

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

        private static string CalculateComponent(this TimeSpanFormat format, TimeSpanFormat targetFormat, bool simplified, ref double totalMilliseconds)
        {
            var total = 0d;

            if (format.HasFlag(targetFormat))
            {
                var calculatedTotal = targetFormat == TimeSpanFormat.Milliseconds ? totalMilliseconds : Math.Floor(totalMilliseconds / (double)targetFormat);

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

        private static string ComponentName(this TimeSpanFormat format, bool simplified)
        {
            string componentName = string.Empty;

            switch (format)
            {
                case TimeSpanFormat.Years:
                    componentName = simplified ? "y" : "year";
                    break;

                case TimeSpanFormat.Weeks:
                    componentName = simplified ? "w" : "week";
                    break;

                case TimeSpanFormat.Days:
                    componentName = simplified ? "d" : "day";
                    break;

                case TimeSpanFormat.Hours:
                    componentName = simplified ? "h" : "hour";
                    break;

                case TimeSpanFormat.Minutes:
                    componentName = simplified ? "m" : "minute";
                    break;

                case TimeSpanFormat.Seconds:
                    componentName = simplified ? "s" : "second";
                    break;

                case TimeSpanFormat.Milliseconds:
                    componentName = simplified ? "ms" : "millisecond";
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