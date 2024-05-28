/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Implements an observable read-only collection of items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the collection.</typeparam>
    public class ObservableReadOnlyCollection<T> : IObservableReadOnlyCollection<T> where T : notnull
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public int Count => m_collection.Count;

        //===========================================================================
        //                             PUBLIC EVENTS
        //===========================================================================

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableReadOnlyCollection{T}"/> class that is a
        /// read-only wrapper around the specified collection.
        /// </summary>
        /// <param name="collection">Collection to wrap.</param>
        public ObservableReadOnlyCollection( IObservableReadOnlyCollection<T> collection )
        {
            m_collection = collection;

            m_collection.CollectionChanged += Collection_CollectionChangedEvent;
        }

        //===========================================================================
        //                               FINALIZER
        //===========================================================================

        /// <summary>
        /// Finalizes an instance of the <see cref="ObservableSortedCollection{T}"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        ~ObservableReadOnlyCollection()
        {
            m_collection.CollectionChanged -= Collection_CollectionChangedEvent;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public bool Contains( T item )
        {
            return m_collection.Contains( item );
        }

        /// <inheritdoc/>
        public void CopyTo( T[] array, int arrayIndex )
        {
            m_collection.CopyTo( array, arrayIndex );
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            return m_collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_collection.GetEnumerator();
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void Collection_CollectionChangedEvent( object? sender, NotifyCollectionChangedEventArgs e )
        {
            CollectionChanged?.Invoke( this, e );
        }

        //===========================================================================
        //                           PROTECTED ATTRIBUTES
        //===========================================================================

        private protected IObservableReadOnlyCollection<T> m_collection;
    }
}
