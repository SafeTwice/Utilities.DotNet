/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Replacement of the IReadOnlySet interface that is covariant.
    /// </summary>
    public interface IReadOnlySetEx<out T> : IReadOnlyCollectionEx<T>
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Determines whether the current set is a proper (strict) subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is a proper subset of <paramref name="other"/>; otherwise <c>false</c>.</returns>
        bool IsProperSubsetOf( IEnumerable<object> other );

        /// <summary>
        /// Determines whether the current set is a proper (strict) superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is a proper superset of <paramref name="other"/>; otherwise <c>false</c>.</returns>
        bool IsProperSupersetOf( IEnumerable<object> other );

        /// <summary>
        /// Determines whether the current set is a subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is a subset of <paramref name="other"/>; otherwise <c>false</c>.</returns>
        bool IsSubsetOf( IEnumerable<object> other );

        /// <summary>
        /// Determines whether the current set is a superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is a superset of <paramref name="other"/>; otherwise <c>false</c>.</returns>
        bool IsSupersetOf( IEnumerable<object> other );

        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set and <paramref name="other"/> share at least one common item; otherwise <c>false</c>.</returns>
        bool Overlaps( IEnumerable<object> other );

        /// <summary>
        /// Determines whether the current set and the specified collection contain the same items.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns><c>true</c> if the current set is equal to <paramref name="other"/>; otherwise, <c>false</c>.</returns>
        bool SetEquals( IEnumerable<object> other );
    }
}
