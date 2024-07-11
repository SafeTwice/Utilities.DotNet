/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of the <see cref="ICollection"/> interface that provides additional methods.
    /// </summary>
    public interface ICollectionEx : ICollection
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">Item to be added to the collection.</param>
        /// <returns><c>true</c> if the item could be added; <c>false</c> otherwise.</returns>
        bool Add( object item );

        /// <summary>
        /// Removes the first occurrence of a specific item from the collection.
        /// </summary>
        /// <param name="item">Item to be removed.</param>
        /// <returns><c>true</c> if the item was present and removed; <c>false</c> otherwise.</returns>
        bool Remove( object item );

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines whether the collection contains a specific item.
        /// </summary>
        /// <param name="item">The item to locate in the collection.</param>
        /// <returns><c>true</c> if item is found; otherwise, <c>false</c>.</returns>
        bool Contains( object item );

        /// <summary>
        /// Adds the items of the specified collection to the collection.
        /// </summary>
        /// <param name="collection">Collection whose items will be added to the collection.</param>
        /// <returns><c>true</c> if all the items could be added; <c>false</c> otherwise.</returns>
        bool AddRange( IEnumerable collection );

        /// <summary>
        /// Removes the items of the specified collection from the collection.
        /// </summary>
        /// <param name="collection">Collection whose items will be removed from the collection.</param>
        /// <returns><c>true</c> if all the items were present and removed; <c>false</c> otherwise.</returns>
        bool RemoveRange( IEnumerable collection );
    }
}
