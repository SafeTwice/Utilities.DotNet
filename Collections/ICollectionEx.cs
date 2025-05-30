/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
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
        /// <exception cref="ArgumentException">
        /// Thrown when the type of <paramref name="item"/> is not compatible with the collection.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the type of <paramref name="item"/> is not compatible with the list.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="item"/> is <see langword="null"/> but the collection does not accept <see langword="null"/> values.
        /// </exception>
        void Add( object? item );

        /// <summary>
        /// Adds the items of the specified collection to the collection.
        /// </summary>
        /// <param name="collection">Collection whose items will be added to the collection.</param>
        /// <exception cref="InvalidCastException">
        /// Thrown when the type of any item of <paramref name="collection"/> is not compatible with the list.
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// Thrown when the type of any item of <paramref name="collection"/> is <see langword="null"/> but the collection
        /// does not accept <see langword="null"/> values.
        /// </exception>
        void AddRange( IEnumerable collection );

        /// <summary>
        /// Removes the first occurrence of a specific item from the collection.
        /// </summary>
        /// <param name="item">Item to be removed.</param>
        /// <returns><see langword="true"/> if the item was present (the operation succeeded);
        ///          <see langword="false"/> otherwise (the operation failed).</returns>
        bool Remove( object? item );

        /// <summary>
        /// Removes the items of the specified collection from the collection.
        /// </summary>
        /// <remarks>
        /// Items in the collection to be removed not present in the collection will be ignored.
        /// </remarks>
        /// <param name="collection">Collection whose items will be removed from the collection.</param>
        void RemoveRange( IEnumerable collection );

        /// <summary>
        /// Replaces an item in the collection with another item.
        /// </summary>
        /// <param name="oldItem">Item to replace.</param>
        /// <param name="newItem">Replacing item.</param>
        /// <returns><see langword="true"/> if the item to be replaced is present (the operation succeeded);
        ///          <see langword="false"/> otherwise (the operation failed).</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the type of <paramref name="newItem"/> is not compatible with the list.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="newItem"/> is <see langword="null"/> but the collection does not accept <see langword="null"/> values.
        /// </exception>
        bool Replace( object? oldItem, object? newItem );

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        void Clear();

        /// <summary>
        /// Determines whether the collection contains a specific item.
        /// </summary>
        /// <param name="item">The item to locate in the collection.</param>
        /// <returns><see langword="true"/> if item is found;
        ///          <see langword="false"/> otherwise.</returns>
        bool Contains( object? item );
    }
}
