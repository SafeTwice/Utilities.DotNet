/// @file
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
    public class ObservableList<T> : ObservableCollection<T>, IObservableList<T> where T : class
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public T this[ int index ]
        {
            get => m_list[ index ];
            set
            {
                var oldItem = m_list[ index ];
                m_list[ index ] = value;
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, value, oldItem, index ) );
            }
        }

        object? IList.this[ int index ]
        {
            get => this[ index ];
            set => this[ index ] = value as T ?? throw new InvalidCastException();
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
            var obj = value as T;
            if( obj == null )
            {
                return -1;
            }
            else
            {
                Add( obj );
                return ( m_list.Count - 1 );
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
            var obj = value as T;
            if( obj == null )
            {
                throw new InvalidCastException();
            }
            else
            {
                Insert( index, obj );
            }
        }

        /// <inheritdoc/>
        public void InsertRange( int index, IEnumerable<T> collection )
        {
            m_list.InsertRange( index, collection );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, collection.ToList(), index ) );
        }

        void IList.Remove( object? value )
        {
            var obj = value as T;
            if( obj != null )
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

        bool IList.Contains( object? value )
        {
            var obj = value as T;
            if( obj == null )
            {
                return false;
            }
            else
            {
                return Contains( obj );
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
            var obj = value as T;
            if( obj == null )
            {
                return -1;
            }
            else
            {
                return IndexOf( obj );
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
