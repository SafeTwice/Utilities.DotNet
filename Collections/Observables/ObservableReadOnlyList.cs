/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Implements an observable read-only list of items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the list.</typeparam>
    public class ObservableReadOnlyList<T> : ObservableReadOnlyCollection<T>, IObservableReadOnlyList<T>
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
        /// Initializes a new instance of the <see cref="ObservableReadOnlyList{T}"/> class that is a
        /// read-only wrapper around the specified collection.
        /// </summary>
        /// <param name="list">List to wrap.</param>
        public ObservableReadOnlyList( IObservableReadOnlyList<T> list ) : base( list )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public IObservableList<T> GetRange( int index, int count )
        {
            return new ObservableList<T>( List.GetRange( index, count ) );
        }

        /// <inheritdoc/>
        IListEx<T> IReadOnlyListEx<T>.GetRange( int index, int count )
        {
            return GetRange( index, count );
        }

        /// <inheritdoc/>
        public IObservableList<T> Slice( int start, int length )
        {
            return GetRange( start, length );
        }

        /// <inheritdoc/>
        IListEx<T> IReadOnlyListEx<T>.Slice( int start, int length )
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

        private IObservableReadOnlyList<T> List => (IObservableReadOnlyList<T>) m_collection;
    }
}
