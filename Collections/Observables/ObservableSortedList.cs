/// @file
/// @copyright  Copyright (c) 2022-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

#pragma warning disable S1696

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
    public class ObservableSortedList<T> : ObservableSortedCollection<T>, IObservableList<T>, IObservableList
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
            set => ( (IListEx) this ).Replace( index, value );
        }

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

        /// <inheritdoc cref="IListEx{T}.Add(T)"/>
        public new int Add( T item )
        {
            int index = AddItem( item );
            if( index >= 0 )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, index ) );
            }

            return index;
        }

        int IListEx.Add( object? item )
        {
            try
            {
                return Add( (T) item! );
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

        int IList.Add( object? value )
        {
            try
            {
                return Add( (T) value! );
            }
            catch( InvalidCastException )
            {
                throw new ArgumentException( $"Incompatible item type", nameof( value ) );
            }
            catch( NullReferenceException )
            {
                throw new ArgumentNullException( nameof( value ) );
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
            try
            {
                Add( (T) value! );
            }
            catch( InvalidCastException )
            {
                throw new ArgumentException( $"Incompatible item type", nameof( value ) );
            }
            catch( NullReferenceException )
            {
                throw new ArgumentNullException( nameof( value ) );
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

        void IListEx.InsertRange( int index, IEnumerable collection )
        {
            ( (ICollectionEx) this ).AddRange( collection );
        }

        bool IListEx.Remove( object? item ) => ( (ICollectionEx) this ).Remove( item );

        void IList.Remove( object? value ) => ( (ICollectionEx) this ).Remove( value );

        /// <inheritdoc/>
        public void RemoveAt( int index )
        {
            if( ( index < 0 ) || ( index >= Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( index ) );
            }

            var item = RemoveItemAt( index );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item, index ) );
        }

        /// <inheritdoc/>
        public void RemoveRange( int index, int count )
        {
            if( index < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( index ) );
            }
            else if( count < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( count ) );
            }
            else if( ( ( index >= Count ) || ( ( index + count ) > Count ) ) )
            {
                throw new ArgumentException( "Index and count do not denote a valid range of items" );
            }

            for( int i = 0; i < count; i++ )
            {
                RemoveAt( index );
            }
        }

        /// <inheritdoc/>
        public void Replace( int index, T newItem )
        {
            if( ( index < 0 ) || ( index >= Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( index ) );
            }

            var oldItem = RemoveItemAt( index );
            AddItem( newItem );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem ) );
        }

        void IListEx.Replace( int index, object? newItem )
        {
            if( ( index < 0 ) || ( index >= Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( index ) );
            }

            try
            {
                Replace( index, (T) newItem! );
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
        public bool Move( T item, int newIndex )
        {
            return m_list.Contains( item );
        }

        /// <inheritdoc/>
        public void Move( int oldIndex, int newIndex )
        {
            // Ignored since the list is ordered.
        }

        bool IListEx.Move( object? item, int newIndex )
        {
            return ( (IListEx) m_list ).Contains( item );
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

        bool IListEx.Contains( object? item ) => ( (IListEx) m_list ).Contains( item );

        bool IList.Contains( object? value ) => ( (IList) m_list ).Contains( value );

        /// <inheritdoc/>
        public int IndexOf( T item ) => m_list.IndexOf( item );

        /// <inheritdoc/>
        public int IndexOf( T item, int index ) => m_list.IndexOf( item, index );

        /// <inheritdoc/>
        public int IndexOf( T item, int index, int count ) => m_list.IndexOf( item, index, count );

        int IReadOnlyListEx<T>.IndexOf( object? item ) => ( (IListEx) m_list ).IndexOf( item );

        int IReadOnlyListEx<T>.IndexOf( object? item, int index ) => ( (IListEx) m_list ).IndexOf( item, index );

        int IReadOnlyListEx<T>.IndexOf( object? item, int index, int count ) => ( (IListEx) m_list ).IndexOf( item, index, count );

        int IList.IndexOf( object? value ) => ( (IListEx) m_list ).IndexOf( value );

        int IListEx.IndexOf( object? item, int index ) => ( (IListEx) m_list ).IndexOf( item, index );

        int IListEx.IndexOf( object? item, int index, int count ) => ( (IListEx) m_list ).IndexOf( item, index, count );

        /// <inheritdoc/>
        public int LastIndexOf( T item ) => m_list.LastIndexOf( item );

        /// <inheritdoc/>
        public int LastIndexOf( T item, int index ) => m_list.LastIndexOf( item, index );

        /// <inheritdoc/>
        public int LastIndexOf( T item, int index, int count ) => m_list.LastIndexOf( item, index, count );

        int IReadOnlyListEx<T>.LastIndexOf( object? item ) => ( (IListEx) m_list ).LastIndexOf( item );

        int IReadOnlyListEx<T>.LastIndexOf( object? item, int index ) => ( (IListEx) m_list ).LastIndexOf( item, index );

        int IReadOnlyListEx<T>.LastIndexOf( object? item, int index, int count ) => ( (IListEx) m_list ).LastIndexOf( item, index, count );

        int IListEx.LastIndexOf( object? item ) => ( (IListEx) m_list ).LastIndexOf( item );

        int IListEx.LastIndexOf( object? item, int index ) => ( (IListEx) m_list ).LastIndexOf( item, index );

        int IListEx.LastIndexOf( object? item, int index, int count ) => ( (IListEx) m_list ).LastIndexOf( item, index, count );
    }
}
