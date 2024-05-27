﻿/// @file
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
    /// Implements an observable collection of items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the collection.</typeparam>
    public class ObservableCollection<T> : IObservableCollection<T>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public int Count => m_list.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public bool IsSynchronized => ( (ICollection) m_list ).IsSynchronized;

        /// <inheritdoc/>
        public object SyncRoot => ( (ICollection) m_list ).SyncRoot;

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
            m_list = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class with the items from the specified collection.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        public ObservableCollection( IEnumerable<T> items )
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

        /// <inheritdoc/>
        public void AddRange( IEnumerable<T> collection )
        {
            var initialIndex = m_list.Count;

            m_list.AddRange( collection );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, collection.ToList(), initialIndex ) );
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

        /// <inheritdoc/>
        public void RemoveRange( IEnumerable<T> collection )
        {
            foreach( var item in collection )
            {
                m_list.Remove( item );
            }

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, collection.ToList() ) );
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

        private protected void NotifyCollectionChanged( NotifyCollectionChangedEventArgs e )
        {
            CollectionChanged?.Invoke( this, e );
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private protected readonly List<T> m_list;
    }
}
