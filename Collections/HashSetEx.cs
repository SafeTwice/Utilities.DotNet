/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable S1696

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of <see cref="HashSet{T}"/> that implements <see cref="ISetEx{T}"/>.
    /// </summary>
    public class HashSetEx<T> : HashSet<T>, ISetEx<T>, ISetEx
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot { get; } = new();

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance that is empty and has the default initial capacity.
        /// </summary>
        public HashSetEx()
        {
        }

        /// <summary>
        /// Initializes a new instance that contains elements copied from the specified collection and
        /// has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">Collection whose elements are copied to the new set.</param>
        public HashSetEx( IEnumerable<T> collection ) : base( collection )
        {
        }

        /// <summary>
        /// Initializes a new instance that is empty, has the default initial capacity,
        /// and uses the specified equality comparer.
        /// </summary>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the set.</param>
        public HashSetEx( IEqualityComparer<T> comparer ) : base( comparer )
        {
        }

        /// <summary>
        /// Initializes a new instance that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">Number of elements that the new set can initially store.</param>
        public HashSetEx( int capacity ) : base( capacity )
        {
        }

        /// <summary>
        /// Initializes a new instance that uses the specified equality comparer,
        /// contains elements copied from the specified collection and
        /// has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">Collection whose elements are copied to the new set.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the set,
        ///                        or <see langword="null"/> to use the default <see cref="EqualityComparer{T}"/> implementation for the set type.</param>
        public HashSetEx( IEnumerable<T> collection, IEqualityComparer<T> comparer ) : base( collection, comparer )
        {
        }

        /// <summary>
        /// Initializes a new instance that uses the specified equality comparer, is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">Number of elements that the new set can initially store.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the set.</param>
        public HashSetEx( int capacity, IEqualityComparer<T> comparer ) : base( capacity, comparer )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        bool ISetEx.Add( object? item )
        {
            try
            {
                return base.Add( (T) item! );
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

        void ICollectionEx.Add( object? item ) => ( (ISetEx) this ).Add( item );

        /// <inheritdoc/>
        public void AddRange( IEnumerable<T> collection )
        {
            foreach( var item in collection )
            {
                base.Add( item );
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

        bool ICollectionEx.Remove( object? item )
        {
            return IsCompatible( item ) && Remove( (T) item! );
        }

        /// <inheritdoc/>
        public void RemoveRange( IEnumerable<T> collection )
        {
            foreach( var item in collection )
            {
                base.Remove( item );
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
            if( !Contains( oldItem ) )
            {
                return false;
            }
            else if( Comparer.Equals( oldItem, newItem ) )
            {
                return true;
            }

            Remove( oldItem );
            Add( newItem );

            return true;
        }

        bool ICollectionEx.Replace( object? oldItem, object? newItem )
        {
            if( !( (ICollectionEx) this ).Contains( oldItem ) )
            {
                return false;
            }

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

        bool ICollectionEx.Contains( object? item )
        {
            return IsCompatible( item ) && base.Contains( (T) item! );
        }

        bool IReadOnlyCollectionEx<T>.Contains( object? item )
        {
            return IsCompatible( item ) && base.Contains( (T) item! );
        }

        void ICollection.CopyTo( Array array, int index )
        {
            var items = new T[ Count ];
            CopyTo( items, index );
            Array.Copy( items, array, Count );
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
        //                            PRIVATE METHODS
        //===========================================================================

        internal bool IsCompatible( object? value )
        {
            return ( value is T ) || ( ( value is null ) && ( default( T ) is null ) );
        }
    }
}
