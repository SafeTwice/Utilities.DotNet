/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections;
using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Implements a read-only collection of items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the collection.</typeparam>
    public class ReadOnlyCollection<T> : IReadOnlyCollectionEx<T>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <inheritdoc/>
        public int Count => m_collection.Count;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the read-only collection.
        /// </summary>
        /// <param name="collection">Collection to wrap.</param>
        public ReadOnlyCollection( IReadOnlyCollectionEx<T> collection )
        {
            m_collection = collection;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public bool Contains( object item ) => m_collection.Contains( item );

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => m_collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => m_collection.GetEnumerator();

        //===========================================================================
        //                          PROTECTED ATTRIBUTES
        //===========================================================================

        protected private readonly IReadOnlyCollectionEx<T> m_collection;
    }
}
