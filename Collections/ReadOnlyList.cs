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
        public T this[ int index ] => List[ index ];

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
        public IListEx<T> GetRange( int index, int count )
        {
            return new ListEx<T>( List.GetRange( index, count ) );
        }

        /// <inheritdoc/>
        public IListEx<T> Slice( int start, int length )
        {
            return GetRange( start, length );
        }

        /// <inheritdoc/>
        public int IndexOf( T value )
        {
            return List.IndexOf( value );
        }

        /// <inheritdoc/>
        public int IndexOf( T item, int index )
        {
            return List.IndexOf( item, index );
        }

        /// <inheritdoc/>
        public int IndexOf( T item, int index, int count )
        {
            return List.IndexOf( item, index, count );
        }

        /// <inheritdoc/>
        public int LastIndexOf( T value )
        {
            return List.LastIndexOf( value );
        }

        /// <inheritdoc/>
        public int LastIndexOf( T item, int index )
        {
            return List.LastIndexOf( item, index );
        }

        /// <inheritdoc/>
        public int LastIndexOf( T item, int index, int count )
        {
            return List.LastIndexOf( item, index, count );
        }

        //===========================================================================
        //                           PRIVATE PROPERTIES
        //===========================================================================

        private IReadOnlyListEx<T> List => (IReadOnlyListEx<T>) m_collection;
    }
}
