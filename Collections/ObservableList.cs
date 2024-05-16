/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

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
        public int Count => m_sortedList.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public bool IsFixedSize => false;

        /// <inheritdoc/>
        public bool IsSynchronized => ( (ICollection) m_sortedList ).IsSynchronized;

        /// <inheritdoc/>
        public object SyncRoot => ( (ICollection) m_sortedList ).SyncRoot;

        /// <inheritdoc/>
        public T this[ int index ]
        {
            get => m_sortedList[ index ];
            set
            {
                var oldItem = m_sortedList[ index ];
                m_sortedList[ index ] = value;
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
            m_sortedList = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableList{T}"/> class with the items from the specified collection.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        public ObservableList( IEnumerable<T> items )
        {
            m_sortedList = new List<T>( items );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public void Add( T item )
        {
            m_sortedList.Add( item );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, ( m_sortedList.Count - 1 ) ) );
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
                return ( m_sortedList.Count - 1 );
            }
        }

        /// <inheritdoc/>
        public void Insert( int index, T item )
        {
            m_sortedList.Insert( index, item );

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
        public bool Remove( T item )
        {
            int index = m_sortedList.IndexOf( item );
            if( index < 0 )
            {
                return false;
            }

            m_sortedList.RemoveAt( index );

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

            var item = m_sortedList[ index ];

            m_sortedList.RemoveAt( index );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item, index ) );
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if( m_sortedList.Count == 0 )
            {
                return;
            }

            m_sortedList.Clear();

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
        }

        /// <inheritdoc/>
        public bool Contains( T value )
        {
            return m_sortedList.Contains( value );
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
        public int IndexOf( T value )
        {
            return m_sortedList.IndexOf( value );
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
        public void CopyTo( T[] array, int index )
        {
            m_sortedList.CopyTo( array, index );
        }

        void ICollection.CopyTo( Array array, int index )
        {
            ( (ICollection) m_sortedList ).CopyTo( array, index );
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return m_sortedList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_sortedList.GetEnumerator();
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

        private List<T> m_sortedList;
    }
}
