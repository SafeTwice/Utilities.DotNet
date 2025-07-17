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
    /// Implements an observable collection of items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the collection.</typeparam>
    [DebuggerDisplay( "Count = {Count}" )]
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
        //                               FINALIZER
        //===========================================================================

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~ObservableCollection()
        {
            Dispose( false );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public void Add( T item )
        {
            var index = AddItem( item );

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
            foreach( var itemToAdd in collection )
            {
                Add( itemToAdd );
            }
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
            int index = m_list.IndexOf( item );
            if( index < 0 )
            {
                return false;
            }

            m_list.RemoveAt( index );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item, index ) );

            return true;
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
            foreach( var itemToRemove in collection )
            {
                Remove( itemToRemove );
            }
        }

        void ICollectionEx.RemoveRange( IEnumerable collection )
        {
            foreach( var item in collection )
            {
                ( (ICollectionEx) this ).Remove( item );
            }
        }

        /// <inheritdoc/>
        public bool Replace( T oldItem, T newItem )
        {
            int index = m_list.IndexOf( oldItem );
            if( index < 0 )
            {
                return false;
            }

            if( !oldItem?.Equals( newItem ) ?? ( newItem is not null ) )
            {
                if( CanAddItem( newItem ) )
                {
                    m_list.Replace( index, newItem );
                }
                else
                {
                    m_list.RemoveAt( index );

                    NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, oldItem, index ) );

                    return true;
                }
            }

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem, index ) );

            return true;
        }

        bool ICollectionEx.Replace( object? oldItem, object? newItem )
        {
            if( ( (IList) m_list ).Contains( oldItem ) )
            {
                try
                {
                    return Replace( (T) oldItem!, (T) newItem! );
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
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void Clear()
        {
            while( m_list.Count > 0 )
            {
                var itemToRemove = m_list[ 0 ];

                m_list.RemoveAt( 0 );

                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, itemToRemove, 0 ) );
            }
        }

        /// <inheritdoc/>
        public bool Contains( T item )
        {
            return m_list.Contains( item );
        }

        bool ICollectionEx.Contains( object? item )
        {
            return ( (IListEx) m_list ).Contains( item );
        }

        bool IReadOnlyCollectionEx<T>.Contains( object? item )
        {
            return ( (IListEx) m_list ).Contains( item );
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
        /// Adds the specified item to the collection.
        /// </summary>
        /// <param name="item">The item to add to the collection.</param>
        /// <returns>The zero-based index at which the item was inserted, or -1 if the item could not be added.</returns>
        private protected int AddItem( T item )
        {
            if( !CanAddItem( item ) )
            {
                return -1;
            }

            var insertionIndex = Count;

            m_list.Add( item );

            return insertionIndex;
        }

        /// <summary>
        /// Raises the <see cref="CollectionChanged"/> event to notify subscribers of changes to the collection.
        /// </summary>
        /// <param name="e">The event data containing information about the change to the collection.</param>
        protected private void NotifyCollectionChanged( NotifyCollectionChangedEventArgs e )
        {
            CollectionChanged?.Invoke( this, e );
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
            if( disposing )
            {
                m_list.Clear();
            }
        }

        //===========================================================================
        //                          PROTECTED ATTRIBUTES
        //===========================================================================

        protected private readonly ListEx<T> m_list;
    }
}
