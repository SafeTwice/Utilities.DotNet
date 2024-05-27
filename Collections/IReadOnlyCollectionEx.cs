/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of the <see cref="IReadOnlyCollection{T}"/> interface that provides additional methods.
    /// </summary>
    public interface IReadOnlyCollectionEx<T> : IReadOnlyCollection<T>
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Determines whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">Object to locate in the collection.</param>
        /// <returns><c>true</c> if item is found in the collection; otherwise, <c>false</c>.</returns>
        bool Contains( T item );

        /// <summary>
        /// Copies the elements of the collection to an array, starting at a particular array index.
        /// </summary>
        /// <param name="array">One-dimensional <see cref="System.Array">Array</see> that is the destination of the elements copied from the collection.
        ///                     The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">Zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="arrayIndex"/> is less than 0.</exception>
        void CopyTo( T[] array, int arrayIndex );
    }
}
