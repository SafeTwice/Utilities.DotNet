/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of the <see cref="IReadOnlyCollection{T}"/> interface that provides additional methods.
    /// </summary>
    public interface IReadOnlyCollectionEx<out T> : IReadOnlyCollection<T>
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Determines whether the collection contains the specified item.
        /// </summary>
        /// <param name="item">Object to locate in the collection.</param>
        /// <returns><c>true</c> if item is found in the collection; otherwise, <c>false</c>.</returns>
        bool Contains( object item );
    }
}
