using Auctus.DataMiner.Library.Common.Comparators;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataMiner.Library.Common.Type
{
    /// <summary>
    ///   Extension methods for the StringBuilder type.
    /// </summary>
    public static class StringBuilderExtensions
    {
        private static Dictionary<StringBuilder, string> _lastStrings = new Dictionary<StringBuilder, string>(ObjectReferenceEqualityComparer.Instance);

        /// <summary>Retrieves the last remembered appended string for this instance.</summary>
        /// <param name="sb">The StringBuilder instance.</param>
        public static string LastAppended(this StringBuilder sb)
        {
            string s;

            _lastStrings.TryGetValue(sb, out s);

            return s;
        }

        /// <summary>Appends the specified string to this instance.</summary>
        /// <param name="sb">The StringBuilder instance that will be appended to.</param>
        /// <param name="value">The string to append.</param>
        /// <param name="remember">If set to <c>true</c> the string value will be stored.</param>
        public static StringBuilder Append(this StringBuilder sb, string value, bool remember)
        {
            if (remember)
            {
                _lastStrings[sb] = value;
            }

            return sb.Append(value);
        }

        /// <summary>Appends the specified string followed by the default line terminator to this instance.</summary>
        /// <param name="sb">The StringBuilder instance that will be appended to.</param>
        /// <param name="value">The string to append.</param>
        /// <param name="remember">If set to <c>true</c> the string value will be stored.</param>
        public static StringBuilder AppendLine(this StringBuilder sb, string value, bool remember)
        {
            if (remember)
            {
                _lastStrings[sb] = value;
            }

            return sb.AppendLine(value);
        }

        /// <summary>Returns the zero-based index position of the last occurrence of a specified Unicode character within this instance.</summary>
        /// <param name="sb">The StringBuilder instance.</param>
        /// <param name="character">The Unicode character to seek.</param>
        /// <returns>
        ///   The zero-based index position of value if that character is found, or -1 if it is not.
        /// </returns>
        public static int LastIndexOf(this StringBuilder sb, char character)
        {
            for (int i = sb.Length - 1; i >= 0; i--)
            {
                if (sb[i] == character)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}