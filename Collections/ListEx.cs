/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of <see cref="List{T}"/> that implements <see cref="IListEx{T}"/>.
    /// </summary>
    public class ListEx<T> : List<T>, IListEx<T>, IListEx
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
        public new bool Add( T item )
        {
            base.Add( item );
            return true;
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

        bool IListEx.InsertRange( int index, IEnumerable collection )
        {
            var itemsToInsert = new ListEx<T>();

            foreach( var item in collection )
            {
                if( item is T obj )
                {
                    itemsToInsert.Add( obj );
                }
                else
                {
                    return false;
                }
            }

            InsertRange( index, itemsToInsert );

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
            var index = IndexOf( oldItem );
            if( index >= 0 )
            {
                return Replace( index, newItem );
            }
            else
            {
                return false;
            }
        }

        bool ICollectionEx.Replace( object oldItem, object newItem )
        {
            if( ( oldItem is T oldObj ) && ( newItem is T newObj ) )
            {
                return Replace( oldObj, newObj );
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool Replace( int index, T newItem )
        {
            if( ( index < 0 ) || ( index >= Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( index ) );
            }

            RemoveAt( index );
            Insert( index, newItem );
            return true;
        }

        bool IListEx.Replace( int index, object newItem )
        {
            if( newItem is T obj )
            {
                return Replace( index, obj );
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool Move( T item, int newIndex )
        {
            var oldIndex = IndexOf( item );
            if( oldIndex >= 0 )
            {
                return Move( oldIndex, newIndex );
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public bool Move( int oldIndex, int newIndex )
        {
            if( ( oldIndex < 0 ) || ( oldIndex >= Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( oldIndex ) );
            }

            if( ( newIndex < 0 ) || ( newIndex > Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( newIndex ) );
            }

            if( oldIndex < newIndex )
            {
                newIndex--;
            }

            if( oldIndex == newIndex )
            {
                return true;
            }

            var item = this[ oldIndex ];
            RemoveAt( oldIndex );
            Insert( newIndex, item );

            return true;
        }

        bool IListEx.Move( object item, int newIndex )
        {
            if( item is T obj )
            {
                return Move( obj, newIndex );
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public new ListEx<T> GetRange( int index, int count )
        {
            return new ListEx<T>( base.GetRange( index, count ) );
        }

        /// <inheritdoc/>
        IListEx<T> IListEx<T>.GetRange( int index, int count ) => GetRange( index, count );

        IReadOnlyListEx<T> IReadOnlyListEx<T>.GetRange( int index, int count ) => GetRange( index, count );

        IListEx IListEx.GetRange( int index, int count ) => GetRange( index, count );

        /// <inheritdoc/>
#if NET8_0_OR_GREATER
        public new ListEx<T> Slice( int start, int length ) => GetRange( start, length );
#else
        public ListEx<T> Slice( int start, int length ) => GetRange( start, length );
#endif

        IListEx<T> IListEx<T>.Slice( int start, int length ) => GetRange( start, length );

        IReadOnlyListEx<T> IReadOnlyListEx<T>.Slice( int start, int length ) => GetRange( start, length );

        IListEx IListEx.Slice( int start, int length ) => GetRange( start, length );

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

        int IListEx.IndexOf( object item, int index ) => ( (IReadOnlyListEx<T>) this ).IndexOf( item, index );

        int IListEx.IndexOf( object item, int index, int count ) => ( (IReadOnlyListEx<T>) this ).IndexOf( item, index, count );

        int IListEx.LastIndexOf( object item ) => ( (IReadOnlyListEx<T>) this ).LastIndexOf( item );

        int IListEx.LastIndexOf( object item, int index ) => ( (IReadOnlyListEx<T>) this ).LastIndexOf( item, index );

        int IListEx.LastIndexOf( object item, int index, int count ) => ( (IReadOnlyListEx<T>) this ).LastIndexOf( item, index, count );
    }
}
