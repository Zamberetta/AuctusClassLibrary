﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Auctus.DataMiner.Library.Common.Comparators
{
    /// <summary>
    /// An <see cref="IEqualityComparer{Object}"/> that uses reference equality (<see cref="object.ReferenceEquals(object, object)"/>)
    /// instead of value equality (<see cref="object.Equals(object)"/>) when comparing two object instances.
    /// </summary>
    /// <remarks>
    /// The <see cref="ObjectReferenceEqualityComparer"/> type cannot be instantiated. Instead, use the <see cref="Instance"/> property
    /// to access the singleton instance of this type.
    /// </remarks>
    public sealed class ObjectReferenceEqualityComparer : IEqualityComparer<object>, IEqualityComparer
    {
        private ObjectReferenceEqualityComparer()
        { }

        /// <summary>
        /// Gets the singleton <see cref="ObjectReferenceEqualityComparer"/> instance.
        /// </summary>
        public static ObjectReferenceEqualityComparer Instance { get; } = new ObjectReferenceEqualityComparer();

        /// <summary>
        /// Determines whether two object references refer to the same object instance.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// <see langword="true"/> if both <paramref name="x"/> and <paramref name="y"/> refer to the same object instance
        /// or if both are <see langword="null"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// This API is a wrapper around <see cref="object.ReferenceEquals(object, object)"/>.
        /// It is not necessarily equivalent to calling <see cref="object.Equals(object, object)"/>.
        /// </remarks>
        public new bool Equals(object x, object y) => ReferenceEquals(x, y);

        /// <summary>
        /// Returns a hash code for the specified object. The returned hash code is based on the object
        /// identity, not on the contents of the object.
        /// </summary>
        /// <param name="obj">The object for which to retrieve the hash code.</param>
        /// <returns>A hash code for the identity of <paramref name="obj"/>.</returns>
        /// <remarks>
        /// This API is a wrapper around <see cref="RuntimeHelpers.GetHashCode(object)"/>.
        /// It is not necessarily equivalent to calling <see cref="object.GetHashCode()"/>.
        /// </remarks>
        public int GetHashCode(object obj)
        {
            return RuntimeHelpers.GetHashCode(obj);
        }
    }
}