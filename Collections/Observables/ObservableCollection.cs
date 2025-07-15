/// @file
/// @copyright  Copyright (c) 2022-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

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
            var index = m_list.Count;

            m_list.Add( item );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, index ) );
        }

        void ICollectionEx.Add( object? item )
        {
            var index = m_list.Count;

            try
            {
                ( (IList) m_list ).Add( item );
            }
            catch( ArgumentNullException )
            {
                throw new ArgumentNullException( nameof( item ) );
            }
            catch( ArgumentException ex )
            {
                throw new ArgumentException( ex.Message, nameof( item ), ex );
            }

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, index ) );
        }

        /// <inheritdoc/>
        public void AddRange( IEnumerable<T> collection )
        {
#if BULK_NOTIFY_RANGE_ACTIONS
            if( !collection.Any() )
            {
                return;
            }

            var initialIndex = m_list.Count;

            m_list.AddRange( collection );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, collection.ToList(), initialIndex ) );
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
#if BULK_NOTIFY_RANGE_ACTIONS
            var removedItems = new ListEx<T>();

            foreach( var item in collection )
            {
                if( m_list.Remove( item ) )
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
                if( ( (ICollectionEx) m_list ).Remove( item ) )
                {
                    removedItems.Add( item );
                }
            }

            if( removedItems.Count > 0 )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, removedItems ) );
            }
#else
            foreach( var item in collection )
            {
                ( (ICollectionEx) this ).Remove( item );
            }
#endif
        }

        /// <inheritdoc/>
        public bool Replace( T oldItem, T newItem )
        {
            int index = m_list.IndexOf( oldItem );
            if( index < 0 )
            {
                return false;
            }

            m_list.Replace( index, newItem );

            NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem, index ) );

            return true;
        }

        bool ICollectionEx.Replace( object? oldItem, object? newItem )
        {
            var index = ( (IList) m_list ).IndexOf( oldItem );
            if( index >= 0 )
            {
                ( (IListEx) m_list ).Replace( index, newItem );

                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem, index ) );

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
                m_list.Clear();
            }
        }

        //===========================================================================
        //                          PROTECTED ATTRIBUTES
        //===========================================================================

        protected private readonly ListEx<T> m_list;
    }
}
