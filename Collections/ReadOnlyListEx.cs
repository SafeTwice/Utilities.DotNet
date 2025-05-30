/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Diagnostics;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Implements a read-only list of items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the list.</typeparam>
    public class ReadOnlyListEx<T> : ReadOnlyCollectionEx<T>, IReadOnlyListEx<T>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public T this[ int index ] => List[ index ];

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the read-only list.
        /// </summary>
        /// <param name="list">List to wrap.</param>
        public ReadOnlyListEx( IReadOnlyListEx<T> list ) : base( list )
        {
            Debug.Assert( ReferenceEquals( list, List ) );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public IReadOnlyListEx<T> GetRange( int index, int count ) => List.GetRange( index, count );

        /// <inheritdoc/>
        public IReadOnlyListEx<T> Slice( int start, int length ) => List.Slice( start, length );

        /// <inheritdoc/>
        public int IndexOf( object? item ) => List.IndexOf( item );

        /// <inheritdoc/>
        public int IndexOf( object? item, int index ) => List.IndexOf( item, index );

        /// <inheritdoc/>
        public int IndexOf( object? item, int index, int count ) => List.IndexOf( item, index, count );

        /// <inheritdoc/>
        public int LastIndexOf( object? item ) => List.LastIndexOf( item );

        /// <inheritdoc/>
        public int LastIndexOf( object? item, int index ) => List.LastIndexOf( item, index );

        /// <inheritdoc/>
        public int LastIndexOf( object? item, int index, int count ) => List.LastIndexOf( item, index, count );

        //===========================================================================
        //                           PRIVATE PROPERTIES
        //===========================================================================

        private IReadOnlyListEx<T> List => (IReadOnlyListEx<T>) m_collection;
    }
}
