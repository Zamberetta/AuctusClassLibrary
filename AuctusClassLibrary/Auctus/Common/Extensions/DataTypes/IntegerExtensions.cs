using System;

namespace Auctus.DataMiner.Library.Common.Type
{
    public static class IntegerExtensions
    {
        /// <summary>Converts the integer representation to a boolean.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        ///   <c>true</c> if 1 else returns <c>false</c>.
        /// </returns>
        public static bool ToBool(this int value)
        {
            return value == 1;
        }

        /// <summary>Converts the integer representation to its corresponding enumeration.</summary>
        /// <typeparam name="TEnumType">The target enum type.</typeparam>
        /// <param name="value">The value to convert.</param>
        public static TEnumType ToEnum<TEnumType>(this int value) where TEnumType : struct
        {
            var enumName = Enum.GetName(typeof(TEnumType), value);
            return enumName.ToEnum<TEnumType>();
        }
    }
}