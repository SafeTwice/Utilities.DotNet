/// @file
/// @copyright  Copyright (c) 2022-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

#pragma warning disable S1696

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Implements an observable set of unique items.
    /// </summary>
    /// <typeparam name="T">Type of the items in the set.</typeparam>
    [DebuggerDisplay( "Count = {Count}" )]
    public class ObservableSet<T> : ObservableCollection<T>, IObservableSet<T>, IObservableCollection
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class that uses the
        /// default <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        public ObservableSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class with the items from the specified collection,
        /// and that uses the default <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        public ObservableSet( IEnumerable<T> items ) : base( items )
        {
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
        public new bool Add( T item )
        {
            var index = AddItem( item );

            if( index >= 0 )
            {
                NotifyCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, index ) );
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void UnionWith( IEnumerable<T> other )
        {
            AddRange( other );
        }

        /// <inheritdoc/>
        public void IntersectWith( IEnumerable<T> other )
        {
            var itemsToRemove = m_list.Where( item => !other.Contains( item ) ).ToList();
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
            var itemsToAdd = other.Where( item => !m_list.Contains( item ) ).ToList();

            RemoveRange( other );
            AddRange( itemsToAdd );
        }

        /// <inheritdoc/>
        public bool IsSubsetOf( IEnumerable<T> other )
        {
            var otherHash = other.ToHashSet();
            return otherHash.IsSupersetOf( m_list );
        }

        /// <inheritdoc/>
        public bool IsProperSubsetOf( IEnumerable<T> other )
        {
            var otherHash = other.ToHashSet();
            return otherHash.IsProperSupersetOf( m_list );
        }

        /// <inheritdoc/>
        public bool IsSupersetOf( IEnumerable<T> other )
        {
            var otherHash = other.ToHashSet();
            return otherHash.IsSubsetOf( m_list );
        }

        /// <inheritdoc/>
        public bool IsProperSupersetOf( IEnumerable<T> other )
        {
            var otherHash = other.ToHashSet();
            return otherHash.IsProperSubsetOf( m_list );
        }

        /// <inheritdoc/>
        public bool Overlaps( IEnumerable<T> other )
        {
            return m_list.Any( item => other.Contains( item ) );
        }

        /// <inheritdoc/>
        public bool SetEquals( IEnumerable<T> other )
        {
            var otherHash = other.ToHashSet();
            return otherHash.SetEquals( m_list );
        }

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

        private protected override bool CanAddItem( T item )
        {
            return !m_list.Contains( item );
        }
    }
}
