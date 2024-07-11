/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of <see cref="List{T}"/> that implements <see cref="IListEx{T}"/>.
    /// </summary>
    public class ListEx<T> : List<T>, IListEx<T>, ICollectionEx
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Initializes a new instance that is empty and has the default initial capacity.
        /// </summary>
        public ListEx()
        {
        }

        /// <summary>
        /// Initializes a new instance that contains elements copied from the specified collection and
        /// has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">Collection whose elements are copied to the new list.</param>
        public ListEx( IEnumerable<T> collection ) : base( collection )
        {
        }

        /// <summary>
        /// Initializes a new instance that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">Number of elements that the new list can initially store.</param>
        public ListEx( int capacity ) : base( capacity )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        bool ICollectionEx<T>.TryAdd( T item )
        {
            Add( item );
            return true;
        }

        /// <inheritdoc/>
        public new IListEx<T> GetRange( int index, int count )
        {
            return new ListEx<T>( base.GetRange( index, count ) );
        }

        IReadOnlyListEx<T> IReadOnlyListEx<T>.GetRange( int index, int count )
        {
            return GetRange( index, count );
        }

        /// <inheritdoc/>
#if NET8_0_OR_GREATER
        public new IListEx<T> Slice( int start, int length )
#else
        public IListEx<T> Slice( int start, int length )
#endif
        {
            return GetRange( start, length );
        }

        IReadOnlyListEx<T> IReadOnlyListEx<T>.Slice( int start, int length )
        {
            return GetRange( start, length );
        }

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

        bool ICollectionEx<T>.AddRange( IEnumerable<T> collection )
        {
            base.AddRange( collection );
            return true;
        }

        bool ICollectionEx.AddRange( IEnumerable collection )
        {
            bool result = true;
            var itemsToAdd = new List<T>();

            foreach( var item in collection )
            {
                if( item is T obj )
                {
                    itemsToAdd.Add( obj );
                }
                else
                {
                    result = false;
                }
            }

            AddRange( itemsToAdd );

            return result;
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
            var itemsToRemove = new List<T>();

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

        bool ICollectionEx.Contains( object item )
        {
            return ( item is T obj ) && Contains( obj );
        }

        bool IReadOnlyCollectionEx<T>.Contains( object item )
        {
            return ( item is T obj ) && Contains( obj );
        }

        int IReadOnlyListEx<T>.IndexOf( object item )
        {
            return ( item is T obj ) ? IndexOf( obj ) : -1;
        }

        int IReadOnlyListEx<T>.IndexOf( object item, int index )
        {
            return ( item is T obj ) ? IndexOf( obj, index ) : -1;
        }

        int IReadOnlyListEx<T>.IndexOf( object item, int index, int count )
        {
            return ( item is T obj ) ? IndexOf( obj, index, count ) : -1;
        }

        int IReadOnlyListEx<T>.LastIndexOf( object item )
        {
            return ( item is T obj ) ? LastIndexOf( obj ) : -1;
        }

        int IReadOnlyListEx<T>.LastIndexOf( object item, int index )
        {
            return ( item is T obj ) ? LastIndexOf( obj, index ) : -1;
        }

        int IReadOnlyListEx<T>.LastIndexOf( object item, int index, int count )
        {
            return ( item is T obj ) ? LastIndexOf( obj, index, count ) : -1;
        }
    }
}
