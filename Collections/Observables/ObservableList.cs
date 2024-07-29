﻿/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Implements an observable list of items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the list.</typeparam>
    public class ObservableList<T> : ObservableCollection<T>, IObservableList<T>, IListEx
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public T this[ int index ]
        {
            get => m_list[ index ];
            set => Replace( index, value );
        }

        object? IList.this[ int index ]
        {
            get => this[ index ];
            set => this[ index ] = ( value is T castValue ) ? castValue : throw new InvalidCastException();
        }

        /// <inheritdoc/>
        //bool IObservableList<T>.IsReadOnly => false;

        /// <inheritdoc/>
        bool IList.IsReadOnly => false;

        /// <inheritdoc/>
        bool IList.IsFixedSize => false;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        public ObservableList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class with the items from the specified collection.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        public ObservableList( IEnumerable<T> items ) : base( items )
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
                return ( m_list.Count - 1 );
            }
            else
            {
                return -1;
            }
        }

        /// <inheritdoc/>
        public void Insert( int index, T item )
        {
            m_list.Insert( index, item );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, index ) );
        }

        void IList.Insert( int index, object? value )
        {
            if( value is T obj )
            {
                Insert( index, obj );
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        /// <inheritdoc/>
        public void InsertRange( int index, IEnumerable<T> collection )
        {
            m_list.InsertRange( index, collection );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, collection.ToList(), index ) );
        }

        bool IListEx.InsertRange( int index, IEnumerable collection )
        {
            var itemsToInsert = new List<T>();

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

            InsertRange( index, itemsToInsert );

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
            if( index < 0 || index >= Count )
            {
                throw new ArgumentOutOfRangeException( nameof( index ) );
            }

            var item = m_list[ index ];

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

            var oldItem = m_list[ index ];

            m_list.RemoveAt( index );
            m_list.Insert( index, newItem );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem, index ) );

            return true;
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
            var oldIndex = m_list.IndexOf( item );
            if( oldIndex >= 0 )
            {
                return Move( oldIndex, newIndex );
            }
            else
            {
                return false;
            }
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

            if( oldIndex == newIndex )
            {
                return true;
            }

            var item = m_list[ oldIndex ];
            m_list.RemoveAt( oldIndex );
            m_list.Insert( newIndex, item );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Move, item, newIndex, oldIndex ) );

            return true;
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
        public ObservableList<T> Slice( int start, int length ) => GetRange( start, length );

        IObservableList<T> IObservableList<T>.Slice( int start, int length ) => GetRange( start, length );

        IReadOnlyListEx<T> IReadOnlyListEx<T>.Slice( int start, int length ) => GetRange( start, length );

        IObservableReadOnlyList<T> IObservableReadOnlyList<T>.Slice( int start, int length ) => GetRange( start, length );

        IListEx<T> IListEx<T>.Slice( int start, int length ) => GetRange( start, length );

        IListEx IListEx.Slice( int start, int length ) => GetRange( start, length );

        /// <inheritdoc/>
        public int IndexOf( T value )
        {
            return m_list.IndexOf( value );
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
        public int LastIndexOf( T value )
        {
            return m_list.LastIndexOf( value );
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
