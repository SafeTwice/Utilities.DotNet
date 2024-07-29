/// @file
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
    /// <remarks>
    /// <para>
    /// A comparer implementation is required to sort and to perform comparisons.
    /// The default comparer (<see cref="Comparer{T}.Default"/>) checks whether the list items type <typeparamref name="T"/>
    /// implements <see cref="IComparable{T}"/> or <see cref="IComparable"/> and uses that implementation, if available.
    /// If <typeparamref name="T"/> does not implement either interface, a <see cref="IComparer{T}"/> instance must be passed
    /// in a constructor overload that accepts a comparer parameter.
    /// </para>
    /// <para>
    /// If the list items type <typeparamref name="T"/> implements <see cref="INotifyPropertyChanged"/>, the list will be
    /// automatically re-sorted when an item changes. Otherwise, the <see cref="ObservableSortedCollection{T}.UpdateSortOrder"/>
    /// method must be called each time that a list item changes.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">Type of the items in the list.</typeparam>
    public class ObservableSortedList<T> : ObservableSortedCollection<T>, IObservableList<T>, IListEx
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
            set => Replace( index, value );
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
        //bool IObservableList<T>.IsReadOnly => false;

        bool IList.IsReadOnly => false;

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
                int index = AddItem( obj );

                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, obj, index ) );

                return index;
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

        bool IListEx.InsertRange( int index, IEnumerable collection )
        {
            var itemsToInsert = new ListEx<T>();

            foreach( var item in collection )
            {
                if( item is T obj )
                {
                    itemsToInsert.Add( obj );
                }
                else
                {
                    return false;
                }
            }

            AddRange( itemsToInsert );

            return true;
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
            if( ( index < 0 ) || ( index >= Count ) )
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

        /// <inheritdoc/>
        public bool Replace( int index, T newItem )
        {
            if( ( index < 0 ) || ( index >= Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( index ) );
            }

            return Replace( this[ index ], newItem );
        }

        bool IListEx.Replace( int index, object newItem )
        {
            if( newItem is T obj )
            {
                return Replace( index, obj );
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool Move( T item, int newIndex )
        {
            var oldIndex = IndexOf( item );
            if( oldIndex < 0 )
            {
                return false;
            }

            return Move( oldIndex, newIndex );
        }

        /// <inheritdoc/>
        public bool Move( int oldIndex, int newIndex )
        {
            if( ( oldIndex < 0 ) || ( oldIndex >= Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( oldIndex ) );
            }

            if( ( newIndex < 0 ) || ( newIndex > Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( newIndex ) );
            }

            if( oldIndex < newIndex )
            {
                newIndex--;
            }

            return ( oldIndex == newIndex );
        }

        bool IListEx.Move( object item, int newIndex )
        {
            if( item is T obj )
            {
                return Move( obj, newIndex );
            }
            else
            {
                return false;
            }
        }

        bool IList.Contains( object? value )
        {
            return ( value is T obj ) && Contains( obj );
        }

        /// <inheritdoc/>
        public ObservableList<T> GetRange( int index, int count )
        {
            return new ObservableList<T>( m_list.GetRange( index, count ) );
        }

        IObservableList<T> IObservableList<T>.GetRange( int index, int count ) => GetRange( index, count );

        IReadOnlyListEx<T> IReadOnlyListEx<T>.GetRange( int index, int count ) => GetRange( index, count );

        IObservableReadOnlyList<T> IObservableReadOnlyList<T>.GetRange( int index, int count ) => GetRange( index, count );

        IListEx<T> IListEx<T>.GetRange( int index, int count ) => GetRange( index, count );

        IListEx IListEx.GetRange( int index, int count ) => GetRange( index, count );

        /// <inheritdoc/>
        public IObservableList<T> Slice( int start, int length ) => GetRange( start, length );

        IObservableList<T> IObservableList<T>.Slice( int start, int length ) => GetRange( start, length );

        IReadOnlyListEx<T> IReadOnlyListEx<T>.Slice( int start, int length ) => GetRange( start, length );

        IObservableReadOnlyList<T> IObservableReadOnlyList<T>.Slice( int start, int length ) => GetRange( start, length );

        IListEx<T> IListEx<T>.Slice( int start, int length ) => GetRange( start, length );

        IListEx IListEx.Slice( int start, int length ) => GetRange( start, length );

        /// <inheritdoc/>
        public int IndexOf( T item )
        {
            return m_list.IndexOf( item );
        }

        int IList.IndexOf( object? value )
        {
            return ( value is T obj ) ? IndexOf( obj ) : -1;
        }

        int IReadOnlyListEx<T>.IndexOf( object item )
        {
            return ( item is T obj ) ? IndexOf( obj ) : -1;
        }

        /// <inheritdoc/>
        public int IndexOf( T item, int index )
        {
            return m_list.IndexOf( item, index );
        }

        int IReadOnlyListEx<T>.IndexOf( object item, int index )
        {
            return ( item is T obj ) ? IndexOf( obj, index ) : -1;
        }

        /// <inheritdoc/>
        public int IndexOf( T item, int index, int count )
        {
            return m_list.IndexOf( item, index, count );
        }

        int IReadOnlyListEx<T>.IndexOf( object item, int index, int count )
        {
            return ( item is T obj ) ? IndexOf( obj, index, count ) : -1;
        }

        /// <inheritdoc/>
        public int LastIndexOf( T item )
        {
            return m_list.LastIndexOf( item );
        }

        int IReadOnlyListEx<T>.LastIndexOf( object item )
        {
            return ( item is T obj ) ? LastIndexOf( obj ) : -1;
        }

        /// <inheritdoc/>
        public int LastIndexOf( T item, int index )
        {
            return m_list.LastIndexOf( item, index );
        }

        int IReadOnlyListEx<T>.LastIndexOf( object item, int index )
        {
            return ( item is T obj ) ? LastIndexOf( obj, index ) : -1;
        }

        /// <inheritdoc/>
        public int LastIndexOf( T item, int index, int count )
        {
            return m_list.LastIndexOf( item, index, count );
        }

        int IReadOnlyListEx<T>.LastIndexOf( object item, int index, int count )
        {
            return ( item is T obj ) ? LastIndexOf( obj, index, count ) : -1;
        }

        int IListEx.IndexOf( object item, int index ) => ( (IReadOnlyListEx<T>) this ).IndexOf( item, index );

        int IListEx.IndexOf( object item, int index, int count ) => ( (IReadOnlyListEx<T>) this ).IndexOf( item, index, count );

        int IListEx.LastIndexOf( object item ) => ( (IReadOnlyListEx<T>) this ).LastIndexOf( item );

        int IListEx.LastIndexOf( object item, int index ) => ( (IReadOnlyListEx<T>) this ).LastIndexOf( item, index );

        int IListEx.LastIndexOf( object item, int index, int count ) => ( (IReadOnlyListEx<T>) this ).LastIndexOf( item, index, count );
    }
}
