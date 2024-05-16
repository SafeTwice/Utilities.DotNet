/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Implements am observable and sorted list of items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the list.</typeparam>
    public class SortedObservableList<T> : SortedObservableCollection<T>, IObservableList<T> where T : class
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
            get => m_sortedList.Values[ index ];
            set
            {
                RemoveAt( index );
                Add( value );
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The setter replaces the item at the specified index with the specified item, but since the list
        /// is ordered, the new item may be moved to a different position.
        /// </remarks>
        object? IList.this[ int index ]
        {
            get => this[ index ];
            set => this[ index ] = value as T ?? throw new InvalidCastException();
        }

        /// <inheritdoc/>
        public bool IsFixedSize => false;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableList{T}"/> class that uses the
        /// default <see cref="IComparer{T}"/>.
        /// </summary>
        public SortedObservableList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableList{T}"/> class that uses the
        /// specified <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing list items.</param>
        public SortedObservableList( IComparer<T> comparer ) : base( comparer )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableList{T}"/> class with the items from the specified collection,
        /// and that uses the default <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        public SortedObservableList( IEnumerable<T> items ) : base( items )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SortedObservableList{T}"/> class with the items from the specified collection,
        /// and that uses the specified <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        /// <param name="comparer">The <see cref="IComparer{T}"/> implementation to use when comparing list items.</param>
        public SortedObservableList( IEnumerable<T> items, IComparer<T> comparer ) : base( items, comparer )
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
                return IndexOf( obj );
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
            var obj = value as T;
            if( obj == null )
            {
                throw new InvalidCastException();
            }
            else
            {
                Add( obj );
            }
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

            var item = m_sortedList.Values[ index ];

            if( item is INotifyPropertyChanged notifyPropertyChangedItem )
            {
                notifyPropertyChangedItem.PropertyChanged -= Item_PropertyChangedEvent;
            }

            m_sortedList.RemoveAt( index );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item, index ) );
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
            return m_sortedList.IndexOfValue( value );
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
    }
}
