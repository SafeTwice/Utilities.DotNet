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
        /// Adds an item to the end of the collection.
        /// </summary>
        /// <param name="item">Item to be added.</param>
        void Add( object item );

        /// <summary>
        /// Removes the first occurrence of a specific item from the collection.
        /// </summary>
        /// <param name="item">Item to be removed.</param>
        void Remove( object item );

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
        /// Adds the items of the specified collection to the end of the collection.
        /// </summary>
        /// <param name="collection">Collection whose items will be added to the end of the collection.</param>
        void AddRange( IEnumerable collection );

        /// <summary>
        /// Removes the items of the specified collection from the collection.
        /// </summary>
        /// <param name="collection">Collection whose items will be removed from the collection.</param>
        void RemoveRange( IEnumerable collection );
    }
}
