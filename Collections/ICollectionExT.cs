/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
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
        /// Adds the items of the specified collection to the collection.
        /// </summary>
        /// <param name="collection">Collection whose items will be added to the collection.</param>
        void AddRange( IEnumerable<T> collection );

        /// <summary>
        /// Removes the items of the specified collection from the collection.
        /// </summary>
        /// <remarks>
        /// Items in the collection to be removed not present in the collection will be ignored.
        /// </remarks>
        /// <param name="collection">Collection whose items will be removed from the collection.</param>
        void RemoveRange( IEnumerable<T> collection );

        /// <summary>
        /// Replaces an item in the collection with another item.
        /// </summary>
        /// <param name="oldItem">Item to replace.</param>
        /// <param name="newItem">Replacing item.</param>
        /// <returns><see langword="true"/> if the item to be replaced is present (the operation succeeded);
        ///          <see langword="false"/> otherwise (the operation failed).</returns>
        bool Replace( T oldItem, T newItem );
    }
}
