/// @file
/// @copyright  Copyright (c) 2022-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

#pragma warning disable S1696

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
    [DebuggerDisplay( "Count = {Count}" )]
    public class ObservableSet<T> : IObservableSet<T>, IObservableCollection
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
            m_set = new HashSetEx<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class with the
        /// specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="comparer"><see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the set.</param>
        public ObservableSet( IEqualityComparer<T> comparer )
        {
            m_set = new HashSetEx<T>( comparer );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class with the items from the specified collection,
        /// and that uses the default <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        public ObservableSet( IEnumerable<T> items )
        {
            m_set = new HashSetEx<T>( items );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class with the items from the specified collection,
        /// and that uses the specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        /// <param name="comparer"><see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the set.</param>
        public ObservableSet( IEnumerable<T> items, IEqualityComparer<T> comparer )
        {
            m_set = new HashSetEx<T>( items, comparer );
        }

        //===========================================================================
        //                               FINALIZER
        //===========================================================================

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~ObservableSet()
        {
            Dispose( false );
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
            Add( item );
        }

        void ICollectionEx.Add( object? item )
        {
            if( ( (ISetEx) m_set ).Add( item ) )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item ) );
            }
        }

        /// <inheritdoc/>
        public void AddRange( IEnumerable<T> collection )
        {
#if BULK_NOTIFY_RANGE_ACTIONS
            var addedItems = new ListEx<T>();

            foreach( var item in collection )
            {
                if( m_set.Add( item ) )
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

        bool ICollectionEx.Remove( object? item )
        {
            if( ( (ISetEx) m_set ).Remove( item ) )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item ) );
                return true;
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
                if( m_set.Remove( item ) )
                {
                    removedItems.Add( item );
                }
            }

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, removedItems ) );
#else
            foreach( var itemToRemove in collection )
            {
                Remove( itemToRemove );
            }
#endif
        }

        void ICollectionEx.RemoveRange( IEnumerable collection )
        {
            foreach( var itemToRemove in collection )
            {
                ( (ICollectionEx) this ).Remove( itemToRemove );
            }
        }

        /// <inheritdoc/>
        public bool Replace( T oldItem, T newItem )
        {
            if( !m_set.Contains( oldItem ) )
            {
                return false;
            }

            m_set.Remove( oldItem );
            if( m_set.Add( newItem ) )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem ) );
            }
            else
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, oldItem ) );
            }

            return true;
        }

        bool ICollectionEx.Replace( object? oldItem, object? newItem )
        {
            if( !( (ISetEx) m_set ).Contains( oldItem ) )
            {
                return false;
            }

            try
            {
                T castedOldItem = (T) oldItem!;
                T castedNewItem = (T) newItem!;

                if( m_set.Comparer.Equals( castedOldItem, castedNewItem ) )
                {
                    return true;
                }

                m_set.Remove( castedOldItem );
                if( m_set.Add( castedNewItem ) )
                {
                    NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem ) );
                }
                else
                {
                    NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, oldItem ) );
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
            if( m_set.Count == 0 )
            {
                return;
            }

            var removedItems = new ListEx<T>( m_set );

            m_set.Clear();

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, removedItems ) );
#else
            while( m_set.Count > 0 )
            {
                var itemToRemove = m_set.First();

                m_set.Remove( itemToRemove );

                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, itemToRemove ) );
            }
#endif
        }

        /// <inheritdoc/>
        public bool Contains( T item )
        {
            return m_set.Contains( item );
        }

        bool ICollectionEx.Contains( object? item )
        {
            return ( (ICollectionEx) m_set ).Contains( item );
        }

        bool IReadOnlyCollectionEx<T>.Contains( object? item )
        {
            return ( (ICollectionEx) m_set ).Contains( item );
        }

        /// <inheritdoc/>
        public void CopyTo( T[] array, int arrayIndex )
        {
            m_set.CopyTo( array, arrayIndex );
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

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        //===========================================================================
        //                            PROTECTED METHODS
        //===========================================================================

        protected private void NotifyCollectionChanged( NotifyCollectionChangedEventArgs e )
        {
            CollectionChanged?.Invoke( this, e );
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
            if( disposing )
            {
                m_set.Clear();
            }
        }

        //===========================================================================
        //                          PROTECTED ATTRIBUTES
        //===========================================================================

        protected private readonly HashSetEx<T> m_set;
    }
}
