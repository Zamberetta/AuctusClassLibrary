using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Auctus.DataMiner.Library.Common.Type
{
    /// <summary>
    ///   Extension methods for the enum type.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>Gets the target attribute assigned to an enum value.</summary>
        /// <typeparam name="T">Target Attribute.</typeparam>
        /// <param name="enumValue">Enum value.</param>
        public static T GetEnumAttribute<T>(this Enum enumValue) where T : Attribute
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var attributes = (T[])fieldInfo.GetCustomAttributes(typeof(T), false);

            return attributes.Length > 0 ? attributes[0] : null;
        }

        /// <summary>Gets the target attribute based on an enum type and field name.</summary>
        /// <typeparam name="T">Target Attribute.</typeparam>
        /// <param name="enumType">Type of the enum.</param>
        /// <param name="field">Enum field.</param>
        public static T GetEnumAttribute<T>(this System.Type enumType, string field) where T : Attribute
        {
            var fieldInfo = enumType.GetField(field);

            if (fieldInfo == null)
            {
                return null;
            }

            var attributes = (T[])fieldInfo.GetCustomAttributes(typeof(T), false);

            return attributes.Length > 0 ? attributes[0] : null;
        }

        /// <summary>Retrieves all attributes for a given type within the target enum type.</summary>
        /// <typeparam name="T">Target Attribute.</typeparam>
        /// <param name="enumType">Type of the enum.</param>
        public static IEnumerable<T> GetEnumAttributes<T>(this System.Type enumType) where T : Attribute
        {
            var fields = enumType.GetFields();

            foreach (var field in fields)
            {
                var attributes = (T[])field.GetCustomAttributes(typeof(T), false);

                if (attributes.Length > 0)
                {
                    yield return attributes[0];
                }
            }
        }

        /// <summary>Gets the name of the enum member based on the assigned attribute.</summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns>
        ///   Enum Member Name
        /// </returns>
        public static string GetEnumMemberName(this Attribute attribute, System.Type enumType)
        {
            var fields = enumType.GetFields();

            foreach (var field in fields)
            {
                var customAttribute = Attribute.GetCustomAttribute(field, attribute.GetType());

                if (customAttribute != null && customAttribute.Equals(attribute))
                {
                    return field.Name;
                }
            }

            return null;
        }

        /// <summary>Gets the enum member based on the assigned attribute.</summary>
        /// <typeparam name="T">Target Enum.</typeparam>
        /// <param name="attribute">The attribute.</param>
        public static T GetEnumMemberValue<T>(this Attribute attribute) where T : Enum
        {
            var fields = typeof(T).GetFields();

            foreach (var field in fields)
            {
                var customAttribute = Attribute.GetCustomAttribute(field, attribute.GetType());

                if (customAttribute != null && customAttribute.Equals(attribute))
                {
                    return (T)field.GetValue(null);
                }
            }

            throw new InvalidEnumArgumentException($"Failed to retrieve the associated enum member for the given attribute.");
        }
    }
}