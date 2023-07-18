namespace Auctus.DataMiner.Library.Common.Type
{
    using Newtonsoft.Json;
    using System;

    public static class ObjectExtensions
    {
        /// <summary>Converts the object representation to a string.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="safe">If set to <c>true</c>, null's will be handled.</param>
        public static string ToStr(this object value, bool safe = true)
        {
            if (safe)
            {
                return Convert.ToString(value);
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>Converts the object representation to an integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="safe">If set to <c>true</c>, default is returned for invalid conversions.</param>
        public static int ToInt(this object value, bool safe = true)
        {
            var stringValue = ToStr(value);

            if (!int.TryParse(stringValue, out int intValue))
            {
                if (safe)
                {
                    return default;
                }
                else
                {
                    throw new ArgumentException("Error occurred when attempting to parse object as int.");
                }
            }

            return intValue;
        }

        /// <summary>Converts the object representation to an double.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="safe">If set to <c>true</c>, default is returned for invalid conversions.</param>
        public static double ToDouble(this object value, bool safe = true)
        {
            var stringValue = ToStr(value);

            if (!double.TryParse(stringValue, out double doubleValue))
            {
                if (safe)
                {
                    return default;
                }
                else
                {
                    throw new ArgumentException("Error occurred when attempting to parse object as double.");
                }
            }

            return doubleValue;
        }

        /// <summary>Converts the object representation to a boolean.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="safe">If set to <c>true</c>, default is returned for invalid conversions.</param>
        /// <returns>
        ///   <c>true</c> if 1 else returns <c>false</c>.
        /// </returns>
        public static bool ToBool(this object value, bool safe = true)
        {
            return value.ToInt(safe) == 1;
        }

        /// <summary>Converts the object representation to a DateTime.</summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="safe">If set to <c>true</c>, default is returned for invalid conversions.</param>
        public static DateTime FromOADate(this object value, bool safe = true)
        {
            return DateTime.FromOADate(value.ToDouble(safe));
        }

        /// <summary>Converts the object representation to its corresponding enumeration.</summary>
        /// <typeparam name="TEnumType">The target enum type.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="safe">If set to <c>true</c>, default is returned for invalid conversions.</param>
        public static TEnumType ToEnum<TEnumType>(this object value, bool safe = true) where TEnumType : struct
        {
            var stringValue = ToStr(value);

            if (int.TryParse(stringValue, out int intValue))
            {
                return intValue.ToEnum<TEnumType>();
            }

            return stringValue.ToEnum<TEnumType>(safe);
        }

        /// <summary>Converts the object to a JSON serialised string.</summary>
        /// <param name="value">The value to serialise.</param>
        /// <param name="indented">If set to <c>true</c>, child objects will be indented.</param>
        public static string ToJson(this object value, bool indented = true)
        {
            return JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }

        /// <summary>Determines if the underlying object is null.</summary>
        /// <param name="obj">The value to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified object is null; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNull(this object obj)
        {
            return obj == null || DBNull.Value.Equals(obj);
        }
    }
}