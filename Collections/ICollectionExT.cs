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
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">Item to be added to the collection.</param>
        /// <returns><c>true</c> if the item could be added; <c>false</c> otherwise.</returns>
        new bool Add( T item );

        /// <summary>
        /// Adds the items of the specified collection to the collection.
        /// </summary>
        /// <param name="collection">Collection whose items will be added to the collection.</param>
        /// <returns><c>true</c> if all the items could be added; <c>false</c> otherwise.</returns>
        bool AddRange( IEnumerable<T> collection );

        /// <summary>
        /// Removes the items of the specified collection from the collection.
        /// </summary>
        /// <param name="collection">Collection whose items will be removed from the collection.</param>
        /// <returns><c>true</c> if all the items were present and removed; <c>false</c> otherwise.</returns>
        bool RemoveRange( IEnumerable<T> collection );

        /// <summary>
        /// Replaces an item in the collection with another item.
        /// </summary>
        /// <param name="oldItem">Item to replace.</param>
        /// <param name="newItem">Replacing item.</param>
        /// <returns><c>true</c> if the item could be replaced; <c>false</c> otherwise.</returns>
        bool Replace( T oldItem, T newItem );
    }
}
