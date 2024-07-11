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
        //                                PROPERTIES
        //===========================================================================

        /// <inheritdoc cref="ICollection{T}.Count" />
        new int Count { get; } // Needed to disambiguate between ICollection<> and IReadOnlyCollection<> and avoid error CS0229

        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Adds the items of the specified collection to the end of the collection.
        /// </summary>
        /// <param name="collection">Collection whose items will be added to the end of the collection.</param>
        void AddRange( IEnumerable<T> collection );

        /// <summary>
        /// Removes the items of the specified collection from the collection.
        /// </summary>
        /// <param name="collection">Collection whose items will be removed from the collection.</param>
        void RemoveRange( IEnumerable<T> collection );
    }
}
