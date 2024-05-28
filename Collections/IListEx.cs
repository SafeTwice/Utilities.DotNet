/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of the <see cref="IList{T}"/> interface that provides additional methods.
    /// </summary>
    public interface IListEx<T> : IList<T>, ICollectionEx<T>, IReadOnlyListEx<T>
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Inserts the elements of a collection into the list at the specified index.
        /// </summary>
        /// <param name="index">Zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">Collection whose elements should be inserted into the list.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than 0 or greater than
        ///                                                      <see cref="ICollection{T}.Count">Count</see>.</exception>
        void InsertRange( int index, IEnumerable<T> collection );

        /// <summary>
        /// Removes a range of elements from the list.
        /// </summary>
        /// <param name="index">Zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">Number of elements to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="index"/> or <paramref name="count"/> are less than 0.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="index"/> and <paramref name="count"/> do not denote a valid range of
        ///                                            elements in the list.</exception>
        void RemoveRange( int index, int count );

        /// <inheritdoc cref="IList{T}.IndexOf(T)"/>
        new int IndexOf( T item ); // Needed to disambiguate between IList<> and IReadOnlyListEx<> and avoid error CS0121
    }
}
