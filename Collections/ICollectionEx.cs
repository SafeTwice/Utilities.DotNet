/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of the <see cref="ICollection{T}"/> interface that provides additional methods.
    /// </summary>
    public interface ICollectionEx<T> : ICollection<T>, IReadOnlyCollectionEx<T>
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Adds the elements of the specified collection to the end of the collection.
        /// </summary>
        /// <param name="collection">Collection whose elements will be added to the end of the list.</param>
        void AddRange( IEnumerable<T> collection );

        /// <summary>
        /// Removes the elements of the specified collection from the collection.
        /// </summary>
        /// <param name="collection">Collection whose elements will be removed from the list.</param>
        void RemoveRange( IEnumerable<T> collection );
    }
}
