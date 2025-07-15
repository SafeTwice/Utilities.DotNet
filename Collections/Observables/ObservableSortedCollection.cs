/// @file
/// @copyright  Copyright (c) 2022-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#pragma warning disable S1696

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
    [DebuggerDisplay( "Count = {Count}" )]
    public class ObservableSortedCollection<T> : IObservableCollection<T>, IObservableCollection
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
        /// Finalizer.
        /// </summary>
        [ExcludeFromCodeCoverage]
        ~ObservableSortedCollection()
        {
            Dispose( false );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public void Add( T item )
        {
            int index = AddItem( item );
            if( index >= 0 )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, index ) );
            }
        }

        void ICollectionEx.Add( object? item )
        {
            try
            {
                Add( (T) item! );
            }
            catch( InvalidCastException )
            {
                throw new ArgumentException( $"Incompatible item type", nameof( item ) );
            }
            catch( NullReferenceException )
            {
                throw new ArgumentNullException( nameof( item ) );
            }
        }

        /// <inheritdoc/>
        public void AddRange( IEnumerable<T> collection )
        {
#if BULK_NOTIFY_RANGE_ACTIONS
            var addedItems = new ListEx<T>();

            foreach( var item in collection )
            {
                if( AddItem( item ) >= 0 )
                {
                    addedItems.Add( item );
                }
            }

            if( addedItems.Count > 0 )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, addedItems ) );
            }
#else
            foreach( var itemToAdd in collection )
            {
                Add( itemToAdd );
            }
#endif
        }

        void ICollectionEx.AddRange( IEnumerable collection )
        {
            // The casted collection is converted to array to enforce that exceptions
            // are thrown immediately if the cast fails and therefore ensure that this
            // object is not modified in case of an exception.
            var castedCollection = collection.Cast<T>().ToArray();

            AddRange( castedCollection );
        }

        /// <inheritdoc/>
        public bool Remove( T item )
        {
            int index = RemoveItem( item );
            if( index >= 0 )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item, index ) );
                return true;
            }
            else
            {
                return false;
            }
        }

        bool ICollectionEx.Remove( object? item )
        {
            if( ( (IList) m_list ).IndexOf( item ) >= 0 )
            {
                return Remove( (T) item! );
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void RemoveRange( IEnumerable<T> collection )
        {
#if BULK_NOTIFY_RANGE_ACTIONS
            var removedItems = new ListEx<T>();

            foreach( var item in collection )
            {
                if( RemoveItem( item ) >= 0 )
                {
                    removedItems.Add( item );
                }
            }

            if( removedItems.Count > 0 )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, removedItems ) );
            }
#else
            foreach( var itemToRemove in collection )
            {
                Remove( itemToRemove );
            }
#endif
        }

        void ICollectionEx.RemoveRange( IEnumerable collection )
        {
#if BULK_NOTIFY_RANGE_ACTIONS
            var removedItems = new ListEx<object?>();

            foreach( var item in collection )
            {
                if( ( (IList) m_list ).IndexOf( item ) >= 0 )
                {
                    RemoveItem( (T) item! );
                    removedItems.Add( item );
                }
            }

            if( removedItems.Count > 0 )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, removedItems ) );
            }
#else
            foreach( var itemToRemove in collection )
            {
                ( (ICollectionEx) this ).Remove( itemToRemove );
            }
#endif
        }

        /// <inheritdoc/>
        public bool Replace( T oldItem, T newItem )
        {
            var oldIndex = m_list.IndexOf( oldItem );
            if( oldIndex < 0 )
            {
                return false;
            }

            int newIndex;
            if( !oldItem?.Equals( newItem ) ?? ( newItem is not null ) )
            {
                RemoveItem( oldItem );
                newIndex = AddItem( newItem );
            }
            else
            {
                newIndex = oldIndex;
            }

            // NotifyCollectionChangedEventArgs constructors only allow Replace actions to have the same old and new indexes,
            // therefore if the indexes are different, we have instead to notify a Remove action for the old item followed by
            // an Add action for the new item.

            if( oldIndex == newIndex )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem, oldIndex ) );
            }
            else
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, oldItem, oldIndex ) );

                if( newIndex >= 0 )
                {
                    NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, newItem, newIndex ) );
                }
            }

            return true;
        }

        bool ICollectionEx.Replace( object? oldItem, object? newItem )
        {
            var oldIndex = ( (IList) m_list ).IndexOf( oldItem );
            if( oldIndex < 0 )
            {
                return false;
            }

            try
            {
                T castedOldItem = (T) oldItem!;
                T castedNewItem = (T) newItem!;

                int newIndex;
                if( !oldItem?.Equals( newItem ) ?? ( newItem is not null ) )
                {
                    RemoveItem( castedOldItem );
                    newIndex = AddItem( castedNewItem );
                }
                else
                {
                    newIndex = oldIndex;
                }

                // NotifyCollectionChangedEventArgs constructors only allow Replace actions to have the same old and new indexes,
                // therefore if the indexes are different, we have instead to notify a Remove action for the old item followed by
                // an Add action for the new item.

                if( oldIndex == newIndex )
                {
                    NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem, oldIndex ) );
                }
                else
                {
                    NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, oldItem, oldIndex ) );

                    if( newIndex >= 0 )
                    {
                        NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, newItem, newIndex ) );
                    }
                }

                return true;
            }
            catch( InvalidCastException )
            {
                throw new ArgumentException( $"Incompatible item type", nameof( newItem ) );
            }
            catch( NullReferenceException )
            {
                throw new ArgumentNullException( nameof( newItem ) );
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
#if BULK_NOTIFY_RANGE_ACTIONS
            if( m_list.Count == 0 )
            {
                return;
            }

            var removedItems = new ListEx<T>( m_list );

            m_list.Clear();

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, removedItems, 0 ) );
#else
            while( m_list.Count > 0 )
            {
                var itemToRemove = m_list[ 0 ];

                m_list.RemoveAt( 0 );

                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, itemToRemove, 0 ) );
            }
#endif
        }

        /// <inheritdoc/>
        public bool Contains( T item )
        {
            return ContainsItem( item );
        }

        bool ICollectionEx.Contains( object? item )
        {
            return ListEx<T>.IsCompatible( item ) && ContainsItem( (T) item! );
        }

        bool IReadOnlyCollectionEx<T>.Contains( object? item )
        {
            return ListEx<T>.IsCompatible( item ) && ContainsItem( (T) item! );
        }

        /// <inheritdoc/>
        public void CopyTo( T[] array, int arrayIndex )
        {
            m_list.CopyTo( array, arrayIndex );
        }

        void ICollection.CopyTo( Array array, int index )
        {
            ( (ICollection) m_list ).CopyTo( array, index );
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => m_list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => m_list.GetEnumerator();

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
                throw new ArgumentException( "Item not contained in the collection", nameof( item ) );
            }

            if( ( ( oldIndex > 0 ) && ( Comparer.Compare( item, m_list[ oldIndex - 1 ] ) < 0 ) ) ||
                ( ( oldIndex < m_list.Count - 1 ) && ( Comparer.Compare( item, m_list[ oldIndex + 1 ] ) > 0 ) ) )
            {
                m_list.RemoveAt( oldIndex );
                int newIndex = AddItemNoAttach( item );

                if( oldIndex != newIndex )
                {
                    NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Move, item, newIndex, oldIndex ) );
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        /// <summary>
        /// Adds the specified item to the collection and attaches the property changed event handler if possible.
        /// </summary>
        /// <remarks>
        /// If the item implements <see cref="System.ComponentModel.INotifyPropertyChanged"/>,
        /// the method attaches a handler to its <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event.
        /// </remarks>
        /// <param name="item">The item to add to the collection.</param>
        /// <returns>The zero-based index at which the item was inserted, or -1 if the item could not be added.</returns>
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

        /// <summary>
        /// Removes the specified item from the collection and detaches any associated event handlers.
        /// </summary>
        /// <remarks>
        /// If the item implements <see cref="System.ComponentModel.INotifyPropertyChanged"/>,
        /// its  <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event handler is detached
        /// before removal.
        /// </remarks>
        /// <param name="item">The item to remove from the collection.</param>
        /// <returns>The zero-based index of the removed item if it was found in the collection; otherwise, -1.</returns>
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

        private protected T RemoveItemAt( int index )
        {
            T item = m_list[ index ];

            if( item is INotifyPropertyChanged notifyPropertyChangedItem )
            {
                notifyPropertyChangedItem.PropertyChanged -= Item_PropertyChangedEvent;
            }

            m_list.RemoveAt( index );

            return item;
        }

        /// <summary>
        /// Handles the <see cref="INotifyPropertyChanged.PropertyChanged"/> event for an item in the collection.
        /// </summary>
        /// <remarks>
        /// All items added to the collection that implement <see cref="INotifyPropertyChanged"/> must have their
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> event attached to this method.
        /// </remarks>
        /// <param name="sender">The item that raised the event.</param>
        /// <param name="e">The event data containing information about the property that changed.</param>
        private protected void Item_PropertyChangedEvent( object? sender, PropertyChangedEventArgs e )
        {
            UpdateSortOrder( (T) sender! );
        }

        /// <summary>
        /// Raises the <see cref="CollectionChanged"/> event to notify subscribers of changes to the collection.
        /// </summary>
        /// <param name="e">The event data containing information about the change to the collection.</param>
        private protected void NotifyCollectionChanged( NotifyCollectionChangedEventArgs e )
        {
            CollectionChanged?.Invoke( this, e );
        }

        /// <summary>
        /// Determines whether the specified item exists in the collection.
        /// </summary>
        /// <remarks>
        /// Derived classes can override this method to provide custom logic for determining whether an item
        /// is contained in the collection.
        /// </remarks>
        /// <param name="item">The item to locate in the collection.</param>
        /// <returns><see langword="true"/> if the item is found in the collection;
        ///           <see langword="false"/> otherwise.</returns>
        private protected virtual bool ContainsItem( T item )
        {
            return m_list.Contains( item );
        }

        /// <summary>
        /// Determines whether the specified item can be added to the collection.
        /// </summary>
        /// <remarks>Derived classes can override this method to provide custom logic for determining whether
        /// an item is eligible for addition to the collection.
        /// </remarks>
        /// <param name="item">The item to evaluate for addition to the collection.</param>
        /// <returns><see langword="true"/> if the item can be added to the collection;
        ///           <see langword="false"/> otherwise.</returns>
        private protected virtual bool CanAddItem( T item )
        {
            return true;
        }

        /// <summary>
        /// Derived classes must override this method to release resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources;
        ///                         <see langword="false"/> to release only unmanaged resources.</param>
        /// <remarks>
        /// <para>Overriding implementations must only dispose other objects when <paramref name="disposing"/> is <see langword="true"/>.</para>
        /// <para>Overriding implementations must call its base class implementation for this method passing the
        ///       <paramref name="disposing"/> parameter.</para>
        /// </remarks>
        protected virtual void Dispose( bool disposing )
        {
            DetachItems();

            if( disposing )
            {
                m_list.Clear();
            }
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

        protected private readonly ListEx<T> m_list = new();
    }
}
