/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of the <see cref="IList{T}"/> interface that provides additional methods.
    /// </summary>
    public interface IListEx<T> : IList<T>
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <inheritdoc cref="List{T}.AddRange(IEnumerable{T})"/>
        void AddRange( IEnumerable<T> collection );

        /// <inheritdoc cref="List{T}.IndexOf(T, int)"/>
        int IndexOf( T item, int index );

        /// <inheritdoc cref="List{T}.IndexOf(T, int, int)"/>
        int IndexOf( T item, int index, int count );

        /// <inheritdoc cref="List{T}.LastIndexOf(T)"/>
        int LastIndexOf( T item );

        /// <inheritdoc cref="List{T}.LastIndexOf(T, int)"/>
        int LastIndexOf( T item, int index );

        /// <inheritdoc cref="List{T}.LastIndexOf(T, int, int)"/>
        int LastIndexOf( T item, int index, int count );
    }
}
