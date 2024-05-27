/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Implements am observable list of items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the list.</typeparam>
    public class ObservableList<T> : IObservableList<T> where T : class
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public int Count => m_list.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public bool IsFixedSize => false;

        /// <inheritdoc/>
        public bool IsSynchronized => ( (ICollection) m_list ).IsSynchronized;

        /// <inheritdoc/>
        public object SyncRoot => ( (ICollection) m_list ).SyncRoot;

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

        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class.
        /// </summary>
        public ObservableList()
        {
            m_list = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class with the items from the specified collection.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        public ObservableList( IEnumerable<T> items )
        {
            m_list = new List<T>( items );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public void Add( T item )
        {
            var initialIndex = m_list.Count;

            m_list.Add( item );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, initialIndex ) );
        }

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
        public void AddRange( IEnumerable<T> collection )
        {
            var initialIndex = m_list.Count;

            m_list.AddRange( collection );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, collection.ToList(), initialIndex ) );
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

        /// <inheritdoc/>
        public bool Remove( T item )
        {
            int index = m_list.IndexOf( item );
            if( index < 0 )
            {
                return false;
            }

            m_list.RemoveAt( index );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item, index ) );
            return true;
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

        /// <inheritdoc/>
        public void Clear()
        {
            if( m_list.Count == 0 )
            {
                return;
            }

            m_list.Clear();

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
        }

        /// <inheritdoc/>
        public bool Contains( T value )
        {
            return m_list.Contains( value );
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

        /// <inheritdoc cref="IListEx{T}.GetRange(int, int)"/>
        public ObservableList<T> GetRange( int index, int count )
        {
            return new ObservableList<T>( m_list.GetRange( index, count ) );
        }

        /// <inheritdoc/>
        IListEx<T> IListEx<T>.GetRange( int index, int count )
        {
            return GetRange( index, count );
        }

        /// <inheritdoc cref="IListEx{T}.Slice(int, int)"/>
        public ObservableList<T> Slice( int start, int length )
        {
            return GetRange( start, length );
        }

        /// <inheritdoc/>
        IListEx<T> IListEx<T>.Slice( int start, int length )
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

        /// <inheritdoc/>
        public void CopyTo( T[] array, int index )
        {
            m_list.CopyTo( array, index );
        }

        void ICollection.CopyTo( Array array, int index )
        {
            ( (ICollection) m_list ).CopyTo( array, index );
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_list.GetEnumerator();
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void NotifyCollectionChanged( NotifyCollectionChangedEventArgs e )
        {
            CollectionChanged?.Invoke( this, e );
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly List<T> m_list;
    }
}
