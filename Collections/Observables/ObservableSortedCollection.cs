/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Utilities.DotNet.Collections.Observables
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
    public class ObservableSortedCollection<T> : IObservableCollection<T>, ICollectionEx
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public int Count => m_list.Count;

        /// <summary>
        /// Gets the <see cref="IComparer{T}"/> implementation used to sort the collection items.
        /// </summary>
        public IComparer<T> Comparer { get; }

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
        /// Initializes a new instance of the <see cref="ObservableSortedCollection{T}"/> class that uses the
        /// default <see cref="IComparer{T}"/>.
        /// </summary>
        public ObservableSortedCollection()
        {
            Comparer = Comparer<T>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedCollection{T}"/> class that uses the
        /// specified <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing collection items.</param>
        public ObservableSortedCollection( IComparer<T> comparer )
        {
            Comparer = comparer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedCollection{T}"/> class with the items from the specified collection,
        /// and that uses the default <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        public ObservableSortedCollection( IEnumerable<T> items )
        {
            Comparer = Comparer<T>.Default;

            foreach( var item in items )
            {
                AddItem( item );
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableSortedCollection{T}"/> class with the items from the specified collection,
        /// and that uses the specified <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing collection items.</param>
        public ObservableSortedCollection( IEnumerable<T> items, IComparer<T> comparer )
        {
            Comparer = comparer;

            foreach( var item in items )
            {
                AddItem( item );
            }
        }

        //===========================================================================
        //                               FINALIZER
        //===========================================================================

        /// <summary>
        /// Finalizes an instance of the <see cref="ObservableSortedCollection{T}"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        ~ObservableSortedCollection()
        {
            DetachItems();
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public bool Add( T item )
        {
            int index = AddItem( item );

            if( index >= 0 )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, index ) );
                return true;
            }
            else
            {
                return false;
            }
        }

        void ICollection<T>.Add( T item )
        {
            if( !Add( item ) )
            {
                throw new InvalidOperationException();
            }
        }

        bool ICollectionEx.Add( object item )
        {
            if( item is T obj )
            {
                return Add( obj );
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool AddRange( IEnumerable<T> collection )
        {
            var result = true;
            var addedItems = new List<T>();

            foreach( var item in collection )
            {
                if( AddItem( item ) >= 0 )
                {
                    addedItems.Add( item );
                }
                else
                {
                    result = false;
                }
            }

            if( addedItems.Count > 0 )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, addedItems ) );
            }

            return result;
        }

        bool ICollectionEx.AddRange( IEnumerable collection )
        {
            bool result = true;
            var itemsToAdd = new List<T>();

            foreach( var item in collection )
            {
                if( item is T obj )
                {
                    itemsToAdd.Add( obj );
                }
                else
                {
                    result = false;
                }
            }

            return AddRange( itemsToAdd ) && result;
        }

        /// <inheritdoc/>
        public bool Remove( T item )
        {
            int index = RemoveItem( item );
            if( index < 0 )
            {
                return false;
            }

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
            var removedItems = new List<T>();

            foreach( var item in collection )
            {
                if( RemoveItem( item ) >= 0 )
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
            var itemsToRemove = new List<T>();

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
        public void Clear()
        {
            if( m_list.Count == 0 )
            {
                return;
            }

            DetachItems();

            m_list.Clear();

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
        }

        /// <inheritdoc/>
        public bool Contains( T value ) => ContainsItem( value );

        bool ICollectionEx.Contains( object item )
        {
            return ( item is T obj ) && ContainsItem( obj );
        }

        bool IReadOnlyCollectionEx<T>.Contains( object item )
        {
            return ( item is T obj ) && ContainsItem( obj );
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
            int oldIndex = m_list.IndexOf( item );

            if( oldIndex < 0 )
            {
                throw new ArgumentException( nameof( item ) );
            }

            bool needsReorder = false;

            if( ( oldIndex > 0 ) && Comparer.Compare( item, m_list[ oldIndex - 1 ] ) < 0 )
            {
                needsReorder = true;
            }
            else if( ( oldIndex < m_list.Count - 1 ) && Comparer.Compare( item, m_list[ oldIndex + 1 ] ) > 0 )
            {
                needsReorder = true;
            }

            if( needsReorder )
            {
                m_list.RemoveAt( oldIndex );
                int newIndex = AddItemNoAttach( item );

                if( oldIndex != newIndex )
                {
                    NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Move, item, newIndex, oldIndex ) );
                }
            }
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        private protected int AddItem( T item )
        {
            if( !CanAddItem( item ) )
            {
                return -1;
            }

            var insertionIndex = AddItemNoAttach( item );

            if( item is INotifyPropertyChanged notifyPropertyChangedItem )
            {
                notifyPropertyChangedItem.PropertyChanged += Item_PropertyChangedEvent;
            }

            return insertionIndex;
        }

        private protected int RemoveItem( T item )
        {
            int index = m_list.IndexOf( item );

            if( index >= 0 )
            {
                if( item is INotifyPropertyChanged notifyPropertyChangedItem )
                {
                    notifyPropertyChangedItem.PropertyChanged -= Item_PropertyChangedEvent;
                }

                m_list.RemoveAt( index );
            }

            return index;
        }

        private protected void Item_PropertyChangedEvent( object? sender, PropertyChangedEventArgs e )
        {
            UpdateSortOrder( (T) sender! );
        }

        private protected void NotifyCollectionChanged( NotifyCollectionChangedEventArgs e )
        {
            CollectionChanged?.Invoke( this, e );
        }

        private protected virtual bool ContainsItem( T item )
        {
            return m_list.Contains( item );
        }

        private protected virtual bool CanAddItem( T item )
        {
            return true;
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private int AddItemNoAttach( T item )
        {
            var insertionIndex = m_list.BinarySearch( item, Comparer );

            if( insertionIndex < 0 )
            {
                insertionIndex = ~insertionIndex;
            }

            m_list.Insert( insertionIndex, item );

            return insertionIndex;
        }

        private void DetachItems()
        {
            foreach( var item in m_list )
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

        protected private readonly List<T> m_list = new();
    }
}
