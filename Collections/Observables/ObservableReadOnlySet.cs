
using System.Collections.Generic;

/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt
namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Implements an observable read-only set of items that wraps another observable set.
    /// </summary>
    /// <typeparam name="T">Type of the items in the set.</typeparam>
    public class ObservableReadOnlySet<T> : ObservableReadOnlyCollection<T>, IObservableReadOnlySet<T>
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableReadOnlySet{T}"/> class that is a
        /// read-only wrapper around the specified set.
        /// </summary>
        /// <param name="set">Set to wrap.</param>
        public ObservableReadOnlySet( IObservableReadOnlySet<T> set ) : base( set )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public bool IsSubsetOf( IEnumerable<object> other ) => _Set.IsSubsetOf( other );

        /// <inheritdoc/>
        public bool IsSupersetOf( IEnumerable<object> other ) => _Set.IsSupersetOf( other );

        /// <inheritdoc/>
        public bool IsProperSubsetOf( IEnumerable<object> other ) => _Set.IsProperSubsetOf( other );

        /// <inheritdoc/>
        public bool IsProperSupersetOf( IEnumerable<object> other ) => _Set.IsProperSupersetOf( other );

        /// <inheritdoc/>
        public bool Overlaps( IEnumerable<object> other ) => _Set.Overlaps( other );

        /// <inheritdoc/>
        public bool SetEquals( IEnumerable<object> other ) => _Set.SetEquals( other );

        //===========================================================================
        //                           PRIVATE PROPERTIES
        //===========================================================================

        private IObservableReadOnlySet<T> _Set => (IObservableReadOnlySet<T>) m_collection;
    }
}
