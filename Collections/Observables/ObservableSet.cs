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
    /// Implements an observable set of items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The items added to the set will be compared for equality using an <see cref="IEqualityComparer{T}"/> implementation
    /// for the item's type <typeparamref name="T"/>.
    /// </para>
    /// <para>
    /// When using the default equality comparer with a <typeparamref name="T"/> that uses a custom equality comparison mechanism
    /// (e.g., by overriding <see cref="object.Equals(object)"/> or by implementing <see cref="IEquatable{T}"/>), ensure that
    /// <see cref="object.GetHashCode"/> is overridden such that equal objects have the same hash code, otherwise comparison will fail.
    /// </para>
    /// <para>
    /// Similarly, when providing a custom <see cref="IEqualityComparer{T}"/> implementation, ensure that <see cref="IEqualityComparer{T}.GetHashCode(T)"/>
    /// returns the same hash code for equal objects.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">Type of the items in the set.</typeparam>
    public class ObservableSet<T> : IObservableSet<T>, ICollectionEx
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public int Count => m_set.Count;

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> implementation used to determine equality for the set items.
        /// </summary>
        public IEqualityComparer<T> Comparer => m_set.Comparer;

        /// <inheritdoc/>
        bool ICollection<T>.IsReadOnly => false;

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => false;

        /// <inheritdoc/>
        object ICollection.SyncRoot => m_set;

        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class that uses the
        /// default <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public ObservableSet()
        {
            m_set = new HashSet<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class with the
        /// specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="comparer"><see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the set.</param>
        public ObservableSet( IEqualityComparer<T> comparer )
        {
            m_set = new HashSet<T>( comparer );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class with the items from the specified collection,
        /// and that uses the default <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        public ObservableSet( IEnumerable<T> items )
        {
            m_set = new HashSet<T>( items );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class with the items from the specified collection,
        /// and that uses the specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        /// <param name="comparer"><see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the set.</param>
        public ObservableSet( IEnumerable<T> items, IEqualityComparer<T> comparer )
        {
            m_set = new HashSet<T>( comparer );
            AddRange( items );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public bool Add( T item )
        {
            if( m_set.Add( item ) )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item ) );
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
            bool result = true;
            var addedItems = new List<T>();

            foreach( var item in collection )
            {
                if( m_set.Add( item ) )
                {
                    addedItems.Add( item );
                }
                else
                {
                    result = false;
                }
            }

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, addedItems ) );

            return result;
        }

        bool ICollectionEx.AddRange( IEnumerable collection )
        {
            var itemsToAdd = new List<T>();

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

            return AddRange( itemsToAdd );
        }

        /// <inheritdoc/>
        public bool Remove( T item )
        {
            if( m_set.Remove( item ) )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item ) );
                return true;
            }
            else
            {
                return false;
            }
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
                if( m_set.Remove( item ) )
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
        public bool Replace( T oldItem, T newItem )
        {
            if( !m_set.Contains( oldItem ) ||
                ( !m_set.Comparer.Equals( oldItem, newItem ) && m_set.Contains( newItem ) ) )
            {
                return false;
            }

            m_set.Remove( oldItem );
            m_set.Add( newItem );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem ) );

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
            if( m_set.Count == 0 )
            {
                return;
            }

            var deletedItems = new ListEx<T>( m_set );

            m_set.Clear();

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, deletedItems, 0 ) );
        }

        /// <inheritdoc/>
        public bool Contains( T value )
        {
            return m_set.Contains( value );
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
            m_set.CopyTo( array, index );
        }

        void ICollection.CopyTo( Array array, int index )
        {
            var items = new T[ m_set.Count ];
            m_set.CopyTo( items, index );
            Array.Copy( items, array, m_set.Count );
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return m_set.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_set.GetEnumerator();
        }

        /// <inheritdoc/>
        public void UnionWith( IEnumerable<T> other )
        {
            AddRange( other );
        }

        /// <inheritdoc/>
        public void IntersectWith( IEnumerable<T> other )
        {
            var itemsToRemove = m_set.Where( item => !other.Contains( item ) ).ToList();
            RemoveRange( itemsToRemove );
        }

        /// <inheritdoc/>
        public void ExceptWith( IEnumerable<T> other )
        {
            RemoveRange( other );
        }

        /// <inheritdoc/>
        public void SymmetricExceptWith( IEnumerable<T> other )
        {
            var itemsToAdd = other.Where( item => !m_set.Contains( item ) ).ToList();

            RemoveRange( other );
            AddRange( itemsToAdd );
        }

        /// <inheritdoc/>
        public bool IsSubsetOf( IEnumerable<T> other ) => m_set.IsSubsetOf( other );

        /// <inheritdoc/>
        public bool IsSupersetOf( IEnumerable<T> other ) => m_set.IsSupersetOf( other );

        /// <inheritdoc/>
        public bool IsProperSupersetOf( IEnumerable<T> other ) => m_set.IsProperSupersetOf( other );

        /// <inheritdoc/>
        public bool IsProperSubsetOf( IEnumerable<T> other ) => m_set.IsProperSubsetOf( other );

        /// <inheritdoc/>
        public bool Overlaps( IEnumerable<T> other ) => m_set.Overlaps( other );

        /// <inheritdoc/>
        public bool SetEquals( IEnumerable<T> other ) => m_set.SetEquals( other );

        bool IReadOnlySetEx<T>.IsSubsetOf( IEnumerable<object> other ) => IsSubsetOf( other.OfType<T>() );

        bool IReadOnlySetEx<T>.IsSupersetOf( IEnumerable<object> other )
        {
            try
            {
                return IsSupersetOf( other.Cast<T>() );
            }
            catch( InvalidCastException )
            {
                return false;
            }
        }
        bool IReadOnlySetEx<T>.IsProperSubsetOf( IEnumerable<object> other ) => IsProperSubsetOf( other.OfType<T>() );

        bool IReadOnlySetEx<T>.IsProperSupersetOf( IEnumerable<object> other )
        {
            try
            {
                return IsProperSupersetOf( other.Cast<T>() );
            }
            catch( InvalidCastException )
            {
                return false;
            }
        }
        bool IReadOnlySetEx<T>.Overlaps( IEnumerable<object> other ) => Overlaps( other.OfType<T>() );

        bool IReadOnlySetEx<T>.SetEquals( IEnumerable<object> other )
        {
            try
            {
                return SetEquals( other.Cast<T>() );
            }
            catch( InvalidCastException )
            {
                return false;
            }
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

        protected private readonly HashSet<T> m_set;
    }
}
