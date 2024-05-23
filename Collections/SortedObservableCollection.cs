/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Implements an observable and sorted collection of items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A comparer implementation is required to sort and to perform comparisons.
    /// The default comparer (<see cref="Comparer{T}.Default"/>) checks whether the collection items type <typeparamref name="T"/>
    /// implements <see cref="IComparable{T}"/> or <see cref="IComparable"/> and uses that implementation, if available.
    /// If <typeparamref name="T"/> does not implement either interface, a <see cref="IComparer{T}"/> instance must be passed
    /// in a constructor overload that accepts a comparer parameter.
    /// </para>
    /// <para>
    /// If the collection items type <typeparamref name="T"/> implements <see cref="INotifyPropertyChanged"/>, the collection will be
    /// automatically re-sorted when an item changes. Otherwise, the <see cref="UpdateSortOrder"/> method must be called each time
    /// that a collection item changes.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">Type of the items in the collection.</typeparam>
    public class SortedObservableCollection<T> : IObservableCollection<T> where T : notnull
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public int Count => m_sortedList.Count;

        /// <inheritdoc/>
        public bool IsReadOnly => false;

        /// <inheritdoc/>
        public bool IsSynchronized => ( (ICollection) m_sortedList ).IsSynchronized;

        /// <inheritdoc/>
        public object SyncRoot => ( (ICollection) m_sortedList ).SyncRoot;

        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableCollection{T}"/> class that uses the
        /// default <see cref="IComparer{T}"/>.
        /// </summary>
        public SortedObservableCollection()
        {
            m_sortedList = new();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableCollection{T}"/> class that uses the
        /// specified <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing collection items.</param>
        public SortedObservableCollection( IComparer<T> comparer )
        {
            m_sortedList = new( comparer );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableCollection{T}"/> class with the items from the specified collection,
        /// and that uses the default <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        public SortedObservableCollection( IEnumerable<T> items )
        {
            m_sortedList = new();

            foreach( var item in items )
            {
                Add( item );
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableCollection{T}"/> class with the items from the specified collection,
        /// and that uses the specified <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing collection items.</param>
        public SortedObservableCollection( IEnumerable<T> items, IComparer<T> comparer )
        {
            m_sortedList = new( comparer );

            foreach( var item in items )
            {
                Add( item );
            }
        }

        //===========================================================================
        //                               FINALIZER
        //===========================================================================

        /// <summary>
        /// Finalizes an instance of the <see cref="SortedObservableCollection{T}"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        ~SortedObservableCollection()
        {
            DetachItems();
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public void Add( T item )
        {
            AddItem( item );

            int index = m_sortedList.IndexOfValue( item );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, index ) );
        }

        /// <inheritdoc/>
        public bool Remove( T item )
        {
            int index = m_sortedList.IndexOfValue( item );
            if( index < 0 )
            {
                return false;
            }

            if( item is INotifyPropertyChanged notifyPropertyChangedItem )
            {
                notifyPropertyChangedItem.PropertyChanged -= Item_PropertyChangedEvent;
            }

            m_sortedList.RemoveAt( index );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item, index ) );
            return true;
        }

        /// <inheritdoc/>
        public void Clear()
        {
            if( m_sortedList.Count == 0 )
            {
                return;
            }

            DetachItems();

            m_sortedList.Clear();

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
        }

        /// <inheritdoc/>
        public bool Contains( T value )
        {
            return m_sortedList.ContainsValue( value );
        }

        /// <inheritdoc/>
        public void CopyTo( T[] array, int index )
        {
            m_sortedList.Values.CopyTo( array, index );
        }

        void ICollection.CopyTo( Array array, int index )
        {
            ( (ICollection) m_sortedList.Values ).CopyTo( array, index );
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return m_sortedList.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_sortedList.Values.GetEnumerator();
        }

        /// <summary>
        /// Updates the sort order of the specified item if needed.
        /// </summary>
        /// <remarks>
        /// For collection items that implement <see cref="INotifyPropertyChanged"/>, the sort order is updated automatically when the item changes.
        /// Otherwise, when an item changes such that its order may change then this method must be called to update the sort order of the changed item.
        /// </remarks>
        /// <param name="item">Item to update its sort order.</param>
        /// <exception cref="ArgumentException">Thrown when the specified item is not contained in the collection.</exception>
        public void UpdateSortOrder( T item )
        {
            int oldIndex = m_sortedList.IndexOfValue( item );

            if( oldIndex < 0 )
            {
                throw new ArgumentException( nameof( item ) );
            }

            bool needsReorder = false;
            
            if( ( oldIndex > 0 ) && m_sortedList.Comparer.Compare( item, m_sortedList.Values[ oldIndex - 1 ] ) < 0 )
            {
                needsReorder = true;
            }
            else if( ( oldIndex < m_sortedList.Count - 1 ) && m_sortedList.Comparer.Compare( item, m_sortedList.Values[ oldIndex + 1 ] ) > 0 )
            {
                needsReorder = true;
            }

            if( needsReorder )
            {
                m_sortedList.RemoveAt( oldIndex );
                m_sortedList.Add( item, item );

                int newIndex = m_sortedList.IndexOfValue( item );
                if( oldIndex != newIndex )
                {
                    NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Move, item, newIndex, oldIndex ) );
                }
            }
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        private protected void AddItem( T item )
        {
            m_sortedList.Add( item, item );

            if( item is INotifyPropertyChanged notifyPropertyChangedItem )
            {
                notifyPropertyChangedItem.PropertyChanged += Item_PropertyChangedEvent;
            }
        }

        private protected void Item_PropertyChangedEvent( object? sender, PropertyChangedEventArgs e )
        {
            UpdateSortOrder( (T) sender! );
        }

        private protected void NotifyCollectionChanged( NotifyCollectionChangedEventArgs e )
        {
            CollectionChanged?.Invoke( this, e );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void DetachItems()
        {
            foreach( var item in m_sortedList.Values )
            {
                if( item is INotifyPropertyChanged notifyPropertyChangedItem )
                {
                    notifyPropertyChangedItem.PropertyChanged -= Item_PropertyChangedEvent;
                }
            }
        }

        //===========================================================================
        //                           PROTECTED ATTRIBUTES
        //===========================================================================

        private protected SortedList<T, T> m_sortedList;
    }
}
