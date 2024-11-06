/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension methods for collections.
    /// </summary>
    public static class Extensions
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> into a <see cref="ListEx{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to convert.</param>
        /// <returns>A <see cref="ListEx{T}"/> containing the elements of the sequence.</returns>
        public static ListEx<T> ToListEx<T>( this IEnumerable<T> sequence )
        {
            return new ListEx<T>( sequence );
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> into a <see cref="HashSetEx{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to convert.</param>
        /// <returns>A <see cref="HashSetEx{T}"/> containing the elements of the sequence.</returns>
        public static HashSetEx<T> ToHashSetEx<T>( this IEnumerable<T> sequence )
        {
            return new HashSetEx<T>( sequence );
        }

        /// <summary>
        /// Wraps an <see cref="ICollectionEx{T}"/> into a <see cref="ReadOnlyCollectionEx{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to wrap.</param>
        /// <returns>A read-only view wrapping the elements of the collection.</returns>
        public static ReadOnlyCollectionEx<T> WrapAsReadOnlyCollection<T>( this ICollectionEx<T> collection )
        {
            return new ReadOnlyCollectionEx<T>( collection );
        }

        /// <summary>
        /// Wraps an <see cref="IListEx{T}"/> into a <see cref="ReadOnlyListEx{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to wrap.</param>
        /// <returns>A read-only view wrapping the elements of the list.</returns>
        public static ReadOnlyListEx<T> WrapAsReadOnlyList<T>( this IListEx<T> list )
        {
            return new ReadOnlyListEx<T>( list );
        }

        /// <summary>
        /// Wraps an <see cref="ISetEx{T}"/> into a <see cref="ReadOnlySetEx{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the set.</typeparam>
        /// <param name="set">The set to wrap.</param>
        /// <returns>A read-only view wrapping the elements of the set.</returns>
        public static ReadOnlySetEx<T> WrapAsReadOnlySetEx<T>( this ISetEx<T> set )
        {
            return new ReadOnlySetEx<T>( set );
        }
    }
}
