/// @file
/// @copyright  Copyright (c) 2022-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Implements an observable read-only collection of items that wraps another observable collection.
    /// </summary>
    /// <typeparam name="T">Type of the items in the collection.</typeparam>
    [DebuggerDisplay( "Count = {Count}" )]
    public class ObservableReadOnlyCollection<T> : IObservableReadOnlyCollection<T>
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
            Dispose( false );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public bool Contains( object item ) => m_collection.Contains( item );

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => m_collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => m_collection.GetEnumerator();

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
            m_collection.CollectionChanged -= Collection_CollectionChangedEvent;
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

        protected private readonly IObservableReadOnlyCollection<T> m_collection;
    }
}
