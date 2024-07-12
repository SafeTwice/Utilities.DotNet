/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Implements an observable and sorted set of items.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A comparer implementation is required to sort and to perform comparisons.
    /// The default comparer (<see cref="Comparer{T}.Default"/>) checks whether the set items type <typeparamref name="T"/>
    /// implements <see cref="IComparable{T}"/> or <see cref="IComparable"/> and uses that implementation, if available.
    /// If <typeparamref name="T"/> does not implement either interface, a <see cref="IComparer{T}"/> instance must be passed
    /// in a constructor overload that accepts a comparer parameter.
    /// </para>
    /// <para>
    /// If the set items type <typeparamref name="T"/> implements <see cref="INotifyPropertyChanged"/>, the set will be
    /// automatically re-sorted when an item changes. Otherwise, the <see cref="ObservableSortedCollection{T}.UpdateSortOrder"/>
    /// method must be called each time that a set item changes.
    /// </para>
    /// </remarks>
    /// <typeparam name="T">Type of the items in the set.</typeparam>
    public class ObservableSortedSet<T> : ObservableSortedCollection<T>, IObservableSet<T>
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class that uses the
        /// default <see cref="IComparer{T}"/>.
        /// </summary>
        public ObservableSortedSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class with the
        /// specified <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="comparer"><see cref="IComparer{T}"/> implementation to use when comparing values in the set.</param>
        public ObservableSortedSet( IComparer<T> comparer ) : base( comparer )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class with the items from the specified collection,
        /// and that uses the default <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        public ObservableSortedSet( IEnumerable<T> items ) : base( items )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableCollection{T}"/> class with the items from the specified collection,
        /// and that uses the specified <see cref="IComparer{T}"/>.
        /// </summary>
        /// <param name="items">Collection of initial items.</param>
        /// <param name="comparer"><see cref="IComparer{T}"/> implementation to use when comparing values in the set.</param>
        public ObservableSortedSet( IEnumerable<T> items, IComparer<T> comparer ) : base( items, comparer )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /*bool IReadOnlyCollectionEx<T>.Contains( object item )
        {
            return ( (ICollectionEx) this ).Contains( item );
        }*/

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
        public bool IsSupersetOf( IEnumerable<T> other )
        {
            var otherHash = other.ToHashSet();
            return otherHash.IsSubsetOf( m_list );
        }

        /// <inheritdoc/>
        public bool IsProperSubsetOf( IEnumerable<T> other )
        {
            var otherHash = other.ToHashSet();
            return otherHash.IsProperSupersetOf( m_list );
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
            var otherHash = other.ToHashSet();
            return otherHash.Overlaps( m_list );
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

        private protected override bool ContainsItem( T item )
        {
            return ( m_list.BinarySearch( item, Comparer ) >= 0 );
        }

        private protected override bool CanAddItem( T item )
        {
            return !ContainsItem( item );
        }
    }
}
