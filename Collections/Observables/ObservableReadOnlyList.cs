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
        public T this[ int index ] => _List[ index ];

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
        public IObservableReadOnlyList<T> GetRange( int index, int count ) => _List.GetRange( index, count );

        IReadOnlyListEx<T> IReadOnlyListEx<T>.GetRange( int index, int count ) => _List.GetRange( index, count );

        /// <inheritdoc/>
        public IObservableReadOnlyList<T> Slice( int start, int length ) => _List.Slice( start, length );

        IReadOnlyListEx<T> IReadOnlyListEx<T>.Slice( int start, int length ) => _List.Slice( start, length );

        /// <inheritdoc/>
        public int IndexOf( object value ) => _List.IndexOf( value );

        /// <inheritdoc/>
        public int IndexOf( object item, int index ) => _List.IndexOf( item, index );

        /// <inheritdoc/>
        public int IndexOf( object item, int index, int count ) => _List.IndexOf( item, index, count );

        /// <inheritdoc/>
        public int LastIndexOf( object value ) => _List.LastIndexOf( value );

        /// <inheritdoc/>
        public int LastIndexOf( object item, int index ) => _List.LastIndexOf( item, index );

        /// <inheritdoc/>
        public int LastIndexOf( object item, int index, int count ) => _List.LastIndexOf( item, index, count );

        //===========================================================================
        //                           PRIVATE PROPERTIES
        //===========================================================================

        private IObservableReadOnlyList<T> _List => (IObservableReadOnlyList<T>) m_collection;
    }
}
