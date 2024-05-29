﻿/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Implements an observable and sorted list of items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the list.</typeparam>
    public class ObservableSortedList<T> : ObservableSortedCollection<T>, IObservableList<T>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        /// <remarks>
        /// The setter replaces the item at the specified index with the specified item, but since the list
        /// is ordered, the new item may be moved to a different position.
        /// </remarks>
        public T this[ int index ]
        {
            get => m_list[ index ];
            set
            {
                RemoveAt( index );
                Add( value );
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The setter replaces the item at the specified index with the specified item, but since the list
        /// is ordered, the new item may be moved to a different position.
        /// </remarks>
        object? IList.this[ int index ]
        {
            get => this[ index ];
            set => this[ index ] = ( value is T castValue ) ? castValue : throw new InvalidCastException();
        }

        /// <inheritdoc/>
        bool IObservableList<T>.IsReadOnly => false;

        /// <inheritdoc/>
        bool IList.IsReadOnly => false;

        /// <inheritdoc/>
        bool IList.IsFixedSize => false;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedList{T}"/> class that uses the
        /// default <see cref="IComparer{T}"/>.
        /// </summary>
        public ObservableSortedList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedList{T}"/> class that uses the
        /// specified <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing list items.</param>
        public ObservableSortedList( IComparer<T> comparer ) : base( comparer )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedList{T}"/> class with the items from the specified collection,
        /// and that uses the default <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        public ObservableSortedList( IEnumerable<T> items ) : base( items )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedList{T}"/> class with the items from the specified collection,
        /// and that uses the specified <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing list items.</param>
        public ObservableSortedList( IEnumerable<T> items, IComparer<T> comparer ) : base( items, comparer )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        int IList.Add( object? value )
        {
            if( value is T obj )
            {
                Add( obj );
                return IndexOf( obj );
            }
            else
            {
                return -1;
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Since the list is ordered, the index is ignored and the new item is inserted in its corresponding sorted position.
        /// </remarks>
        public void Insert( int index, T item )
        {
            Add( item );
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Since the list is ordered, the index is ignored and the new item is inserted in its corresponding sorted position.
        /// </remarks>
        void IList.Insert( int index, object? value )
        {
            if( value is T obj )
            {
                Add( obj );
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Since the list is ordered, the index is ignored and the new items are inserted in its corresponding sorted positions.
        /// </remarks>
        public void InsertRange( int index, IEnumerable<T> collection )
        {
            AddRange( collection );
        }

        void IList.Remove( object? value )
        {
            if( value is T obj )
            {
                Remove( obj );
            }
        }

        /// <inheritdoc/>
        public void RemoveAt( int index )
        {
            if( index < 0 || index >= Count )
            {
                throw new ArgumentOutOfRangeException( nameof( index ) );
            }

            var item = m_list[ index ];

            if( item is INotifyPropertyChanged notifyPropertyChangedItem )
            {
                notifyPropertyChangedItem.PropertyChanged -= Item_PropertyChangedEvent;
            }

            m_list.RemoveAt( index );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item, index ) );
        }

        /// <inheritdoc/>
        public void RemoveRange( int index, int count )
        {
            var removedItems = m_list.GetRange( index, count );

            m_list.RemoveRange( index, count );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, removedItems, index ) );
        }

        bool IList.Contains( object? value )
        {
            if( value is T obj )
            {
                return Contains( obj );
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public IObservableList<T> GetRange( int index, int count )
        {
            return new ObservableList<T>( m_list.GetRange( index, count ) );
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
            return m_list.IndexOf( value );
        }

        /// <inheritdoc/>
        public int IndexOf( T item, int index )
        {
            return m_list.IndexOf( item, index );
        }

        /// <inheritdoc/>
        public int IndexOf( T item, int index, int count )
        {
            return m_list.IndexOf( item, index, count );
        }

        int IList.IndexOf( object? value )
        {
            if( value is T obj )
            {
                return IndexOf( obj );
            }
            else
            {
                return -1;
            }
        }

        /// <inheritdoc/>
        public int LastIndexOf( T value )
        {
            return m_list.LastIndexOf( value );
        }

        /// <inheritdoc/>
        public int LastIndexOf( T item, int index )
        {
            return m_list.LastIndexOf( item, index );
        }

        /// <inheritdoc/>
        public int LastIndexOf( T item, int index, int count )
        {
            return m_list.LastIndexOf( item, index, count );
        }
    }
}
