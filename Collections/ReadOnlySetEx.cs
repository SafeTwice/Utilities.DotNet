/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;
using System.Diagnostics;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Implements a read-only set of items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the set.</typeparam>
    public class ReadOnlySetEx<T> : ReadOnlyCollectionEx<T>, IReadOnlySetEx<T>
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the read-only set.
        /// </summary>
        /// <param name="set">Set to wrap.</param>
        public ReadOnlySetEx( IReadOnlySetEx<T> set ) : base( set )
        {
            Debug.Assert( ReferenceEquals( @set, Set ) );
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public bool IsSubsetOf( IEnumerable<object> other ) => Set.IsSubsetOf( other );

        /// <inheritdoc/>
        public bool IsSupersetOf( IEnumerable<object> other ) => Set.IsSupersetOf( other );

        /// <inheritdoc/>
        public bool IsProperSubsetOf( IEnumerable<object> other ) => Set.IsProperSubsetOf( other );

        /// <inheritdoc/>
        public bool IsProperSupersetOf( IEnumerable<object> other ) => Set.IsProperSupersetOf( other );

        /// <inheritdoc/>
        public bool Overlaps( IEnumerable<object> other ) => Set.Overlaps( other );

        /// <inheritdoc/>
        public bool SetEquals( IEnumerable<object> other ) => Set.SetEquals( other );

        //===========================================================================
        //                           PRIVATE PROPERTIES
        //===========================================================================

        private IReadOnlySetEx<T> Set => (IReadOnlySetEx<T>) m_collection;
    }
}
