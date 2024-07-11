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

        void ICollectionEx.Add( object item )
        {
            if( item is T obj )
            {
                Add( obj );
            }
            else
            {
                throw new InvalidCastException();
            }
        }

        void ICollectionEx.AddRange( IEnumerable collection )
        {
            foreach( var item in collection )
            {
                ( (ICollectionEx) this ).Add( item );
            }
        }

        void ICollectionEx.Remove( object item )
        {
            if( item is T obj )
            {
                Remove( obj );
            }
        }

        /// <inheritdoc/>
        public void RemoveRange( IEnumerable<T> collection )
        {
            foreach( var item in collection )
            {
                Remove( item );
            }
        }

        void ICollectionEx.RemoveRange( IEnumerable collection )
        {
            foreach( var item in collection )
            {
                ( (ICollectionEx) this ).Remove( item );
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
