/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of <see cref="HashSet{T}"/> that implements <see cref="IReadOnlySetEx{T}"/>.
    /// </summary>
    public class HashSetEx<T> : HashSet<T>, ISetEx<T>, IReadOnlySetEx<T>, ICollectionEx
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
        /// Initializes a new instance that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">Number of elements that the new set can initially store.</param>
        public HashSetEx( int capacity ) : base( capacity )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        bool ICollectionEx.Add( object item )
        {
            if( item is T obj )
            {
                Add( obj );
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool AddRange( IEnumerable<T> collection )
        {
            bool result = true;

            foreach( var item in collection )
            {
                if( !Add( item ) )
                {
                    result = false;
                }
            }

            return result;
        }

        bool ICollectionEx.AddRange( IEnumerable collection )
        {
            var itemsToAdd = new ListEx<T>();

            foreach( var item in collection )
            {
                if( item is T obj )
                {
                    itemsToAdd.Add( obj );
                }
                else
                {
                    return false;
                }
            }

            AddRange( itemsToAdd );

            return true;
        }

        bool ICollectionEx.Remove( object item )
        {
            if( item is T obj )
            {
                return Remove( obj );
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool RemoveRange( IEnumerable<T> collection )
        {
            var result = true;

            foreach( var item in collection )
            {
                if( !Remove( item ) )
                {
                    result = false;
                }
            }

            return result;
        }

        bool ICollectionEx.RemoveRange( IEnumerable collection )
        {
            bool partialResult = true;
            var itemsToRemove = new ListEx<T>();

            foreach( var item in collection )
            {
                if( item is T obj )
                {
                    itemsToRemove.Add( obj );
                }
                else
                {
                    partialResult = false;
                }
            }

            return RemoveRange( itemsToRemove ) && partialResult;
        }

        /// <inheritdoc/>
        public bool Replace( T oldItem, T newItem )
        {
            if( !Contains( oldItem ) ||
                ( !Comparer.Equals( oldItem, newItem ) && Contains( newItem ) ) )
            {
                return false;
            }

            Remove( oldItem );
            Add( newItem );

            return true;
        }

        bool ICollectionEx.Replace( object oldItem, object newItem )
        {
            if( ( oldItem is T oldObj ) && ( newItem is T newObj ) )
            {
                Replace( oldObj, newObj );
                return true;
            }
            else
            {
                return false;
            }
        }

        bool ICollectionEx.Contains( object item )
        {
            return ( item is T obj ) && Contains( obj );
        }

        bool IReadOnlyCollectionEx<T>.Contains( object item )
        {
            return ( item is T obj ) && Contains( obj );
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
    }
}
