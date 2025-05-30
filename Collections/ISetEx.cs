/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Represents a collection of unique items.
    /// </summary>
    public interface ISetEx : ICollectionEx
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Adds an item to the current set and returns a value to indicate if the element was successfully added.
        /// </summary>
        /// <param name="item">Item to be added to the set.</param>
        /// <returns><see langword="true"/> if the item was successfully added to the set (i.e., it was not already present);
        ///          <see langword="false"/> otherwise.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the type of <paramref name="item"/> is not compatible with the list.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="item"/> is <see langword="null"/> but the collection does not accept <see langword="null"/> values.
        /// </exception>
        new bool Add( object? item );
    }
}
