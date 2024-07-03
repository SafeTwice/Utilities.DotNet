/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Implements a read-only list of items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the list.</typeparam>
    public class ReadOnlyList<T> : ReadOnlyCollection<T>, IReadOnlyListEx<T>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public T this[ int index ] => _List[ index ];

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the read-only list.
        /// </summary>
        /// <param name="list">List to wrap.</param>
        public ReadOnlyList( IReadOnlyListEx<T> list ) : base( list )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public IReadOnlyListEx<T> GetRange( int index, int count ) => _List.GetRange( index, count );

        /// <inheritdoc/>
        public IReadOnlyListEx<T> Slice( int start, int length ) => _List.Slice( start, length );

        /// <inheritdoc/>
        public int IndexOf( object item ) => _List.IndexOf( item );

        /// <inheritdoc/>
        public int IndexOf( object item, int index ) => _List.IndexOf( item, index );

        /// <inheritdoc/>
        public int IndexOf( object item, int index, int count ) => _List.IndexOf( item, index, count );

        /// <inheritdoc/>
        public int LastIndexOf( object item ) => _List.LastIndexOf( item );

        /// <inheritdoc/>
        public int LastIndexOf( object item, int index ) => _List.LastIndexOf( item, index );

        /// <inheritdoc/>
        public int LastIndexOf( object item, int index, int count ) => _List.LastIndexOf( item, index, count );

        //===========================================================================
        //                           PRIVATE PROPERTIES
        //===========================================================================

        private IReadOnlyListEx<T> _List => (IReadOnlyListEx<T>) m_collection;
    }
}
