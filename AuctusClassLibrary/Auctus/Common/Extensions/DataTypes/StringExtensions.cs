using System;
using System.Text.RegularExpressions;

namespace Auctus.DataMiner.Library.Common.Type
{
    /// <summary>
    ///   Extension methods for the string type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>Converts the string representation to its corresponding enumeration.</summary>
        /// <typeparam name="TEnumType">The target enum type.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="safe">If set to <c>true</c>, default is returned for invalid conversions.</param>
        public static TEnumType ToEnum<TEnumType>(this string value, bool safe = true) where TEnumType : struct
        {
            TEnumType @enum;

            if (Enum.TryParse(value, true, out @enum))
            {
                return @enum;
            }

            if (safe)
            {
                return default;
            }
            else
            {
                throw new ArgumentException($"Cannot cast '{value}' to enum type '{typeof(TEnumType).Name}.'");
            }
        }

        /// <summary>Converts a readable time span to the built-in TimeSpan.</summary>
        /// <param name="value">The value to convert.</param>
        public static TimeSpan ParseReadableTimeSpan(this string value)
        {
            string pattern = @"(?<Years>\d+(?=.year|y))|(?<Weeks>\d+(?=.week|w))|(?<Days>\d+(?=.day|d))|(?<Hours>\d+(?=.hour|h))|(?<Minutes>\d+(?=.minute|m(?!s)))|(?<Seconds>\d+(?=.second|s))|(?<Milliseconds>\d+(?=.millisecond|ms))";

            var totalMilliseconds = 0D;

            var regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(500));

            foreach (Match match in regex.Matches(value))
            {
                for (var i = 0; i < match.Groups.Count; i++)
                {
                    var groupValue = match.Groups[i].Value;

                    if (string.IsNullOrWhiteSpace(groupValue))
                    {
                        continue;
                    }

                    var multiplier = 0D;

                    switch (regex.GroupNameFromNumber(i))
                    {
                        case "Years":
                            multiplier = 31536000000;
                            break;

                        case "Weeks":
                            multiplier = 604800000;
                            break;

                        case "Days":
                            multiplier = 86400000;
                            break;

                        case "Hours":
                            multiplier = 3600000;
                            break;

                        case "Minutes":
                            multiplier = 60000;
                            break;

                        case "Seconds":
                            multiplier = 1000;
                            break;

                        case "Milliseconds":
                            multiplier = 1;
                            break;

                        default:
                            continue;
                    }

                    totalMilliseconds += Convert.ToDouble(groupValue) * multiplier;
                }
            }

            if (totalMilliseconds == 922337203685477D)
            {
                return TimeSpan.MaxValue;
            }

            return TimeSpan.FromMilliseconds(totalMilliseconds);
        }
    }
}