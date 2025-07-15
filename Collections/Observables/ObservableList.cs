/// @file
/// @copyright  Copyright (c) 2022-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

#pragma warning disable S1696

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
            set => ( (IListEx) this ).Replace( index, value );
        }

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

        /// <inheritdoc cref="IListEx{T}.Add(T)"/>
        public new int Add( T item )
        {
            base.Add( item );
            return ( m_list.Count - 1 );
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
        public void Insert( int index, T item )
        {
            if( ( index < 0 ) || ( index > Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( index ) );
            }

            m_list.Insert( index, item );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, index ) );
        }

        void IList.Insert( int index, object? value )
        {
            try
            {
                Insert( index, (T) value! );
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
        public void InsertRange( int index, IEnumerable<T> collection )
        {
#if BULK_NOTIFY_RANGE_ACTIONS
            m_list.InsertRange( index, collection );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, collection.ToList(), index ) );
#else
            if( ( index < 0 ) || ( index > Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( index ) );
            }

            int i = 0;
            foreach( var item in collection )
            {
                Insert( index + i, item );
                i++;
            }
#endif
        }

        void IListEx.InsertRange( int index, IEnumerable collection )
        {
            // The casted collection is converted to array to enforce that exceptions
            // are thrown immediately if the cast fails and therefore ensure that this
            // object is not modified in case of an exception.
            var castedCollection = collection.Cast<T>().ToArray();

            InsertRange( index, castedCollection );
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

            var item = m_list[ index ];

            m_list.RemoveAt( index );

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

#if BULK_NOTIFY_RANGE_ACTIONS
            if( count == 0 )
            {
                return;
            }

            var removedItems = m_list.GetRange( index, count );

            m_list.RemoveRange( index, count );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, removedItems, index ) );
#else
            for( int i = 0; i < count; i++ )
            {
                RemoveAt( index );
            }
#endif
        }

        /// <inheritdoc/>
        public void Replace( int index, T newItem )
        {
            if( ( index < 0 ) || ( index >= Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( index ) );
            }

            var oldItem = m_list[ index ];

            m_list.Replace( index, newItem );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem, index ) );
        }

        void IListEx.Replace( int index, object? newItem )
        {
            if( ( index < 0 ) || ( index >= Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( index ) );
            }

            var oldItem = m_list[ index ];

            ( (IListEx) m_list ).Replace( index, newItem );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem, index ) );
        }

        /// <inheritdoc/>
        public bool Move( T item, int newIndex )
        {
            var oldIndex = m_list.IndexOf( item );
            if( oldIndex >= 0 )
            {
                Move( oldIndex, newIndex );
                return true;
            }
            else
            {
                return false;
            }
        }

        bool IListEx.Move( object? item, int newIndex )
        {
            var oldIndex = ( (IListEx) m_list ).IndexOf( item );
            if( oldIndex >= 0 )
            {
                Move( oldIndex, newIndex );
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void Move( int oldIndex, int newIndex )
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
                return;
            }

            var movedItem = m_list[ oldIndex ];

            m_list.RemoveAt( oldIndex );
            m_list.Insert( newIndex, movedItem );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Move, movedItem, newIndex, oldIndex ) );
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
