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
    /// Implements an observable collection of items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the collection.</typeparam>
    public class ObservableCollection<T> : IObservableCollection<T>, IObservableCollection
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public int Count => m_list.Count;

        /// <inheritdoc/>
        bool ICollection<T>.IsReadOnly => false;

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => ( (ICollection) m_list ).IsSynchronized;

        /// <inheritdoc/>
        object ICollection.SyncRoot => ( (ICollection) m_list ).SyncRoot;

        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class.
        /// </summary>
        public ObservableCollection()
        {
            m_list = new ListEx<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class with the items from the specified collection.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        public ObservableCollection( IEnumerable<T> items )
        {
            m_list = new ListEx<T>( items );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public bool Add( T item )
        {
            var initialIndex = m_list.Count;

            m_list.Add( item );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, initialIndex ) );

            return true;
        }

        void ICollection<T>.Add( T item )
        {
            Add( item );
        }

        bool ICollectionEx.Add( object item )
        {
            if( item is T obj )
            {
                Add( obj );
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool AddRange( IEnumerable<T> collection )
        {
            var initialIndex = m_list.Count;

            m_list.AddRange( collection );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, collection.ToList(), initialIndex ) );

            return true;
        }

        bool ICollectionEx.AddRange( IEnumerable collection )
        {
            var itemsToAdd = new ListEx<T>();

            foreach( var item in collection )
            {
                if( item is T obj )
                {
                    itemsToAdd.Add( obj );
                }
                else
                {
                    return false;
                }
            }

            AddRange( itemsToAdd );

            return true;
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

        bool ICollectionEx.Remove( object item )
        {
            if( item is T obj )
            {
                return Remove( obj );
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool RemoveRange( IEnumerable<T> collection )
        {
            bool result = true;
            var removedItems = new ListEx<T>();

            foreach( var item in collection )
            {
                if( m_list.Remove( item ) )
                {
                    removedItems.Add( item );
                }
                else
                {
                    result = false;
                }
            }

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, removedItems ) );

            return result;
        }

        bool ICollectionEx.RemoveRange( IEnumerable collection )
        {
            bool partialResult = true;
            var itemsToRemove = new ListEx<T>();

            foreach( var item in collection )
            {
                if( item is T obj )
                {
                    itemsToRemove.Add( obj );
                }
                else
                {
                    partialResult = false;
                }
            }

            return RemoveRange( itemsToRemove ) && partialResult;
        }

        /// <inheritdoc/>
        public bool Replace( T oldItem, T newItem )
        {
            int index = m_list.IndexOf( oldItem );
            if( index < 0 )
            {
                return false;
            }

            m_list.RemoveAt( index );
            m_list.Insert( index, newItem );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem, index ) );

            return true;
        }

        bool ICollectionEx.Replace( object oldItem, object newItem )
        {
            if( ( oldItem is T oldObj ) && ( newItem is T newObj ) )
            {
                Replace( oldObj, newObj );
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if( m_list.Count == 0 )
            {
                return;
            }

            var deletedItems = new ListEx<T>( m_list );

            m_list.Clear();

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, deletedItems, 0 ) );
        }

        /// <inheritdoc/>
        public bool Contains( T value )
        {
            return m_list.Contains( value );
        }

        bool ICollectionEx.Contains( object item )
        {
            if( item is T obj )
            {
                return Contains( obj );
            }
            else
            {
                return false;
            }
        }

        bool IReadOnlyCollectionEx<T>.Contains( object item )
        {
            return ( (ICollectionEx) this ).Contains( item );
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
        //                            PROTECTED METHODS
        //===========================================================================

        protected private void NotifyCollectionChanged( NotifyCollectionChangedEventArgs e )
        {
            CollectionChanged?.Invoke( this, e );
        }

        //===========================================================================
        //                          PROTECTED ATTRIBUTES
        //===========================================================================

        protected private readonly List<T> m_list;
    }
}
