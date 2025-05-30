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

        /// <inheritdoc cref="IListEx{T}.Add(T)"/>
        public new int Add( T item )
        {
            base.Add( item );
            return ( Count - 1 );
        }

        int IListEx.Add( object? item )
        {
            try
            {
                return ( (IList) this ).Add( item );
            }
            catch( ArgumentNullException )
            {
                throw new ArgumentNullException( nameof( item ) );
            }
            catch( ArgumentException ex )
            {
                throw new ArgumentException( ex.Message, nameof( item ), ex );
            }
        }

        void ICollectionEx.Add( object? item )
        {
            try
            {
                ( (IList) this ).Add( item );
            }
            catch( ArgumentNullException )
            {
                throw new ArgumentNullException( nameof( item ) );
            }
            catch( ArgumentException ex )
            {
                throw new ArgumentException( ex.Message, nameof( item ), ex );
            }
        }

        void ICollectionEx.AddRange( IEnumerable collection )
        {
            // The casted collection is converted to array to enforce that exceptions
            // are thrown immediately if the cast fails and therefore ensure that this
            // object is not modified in case of an exception.
            var castedCollection = collection.Cast<T>().ToArray();

            base.AddRange( castedCollection );
        }

        void IListEx.InsertRange( int index, IEnumerable collection )
        {
            // The casted collection is converted to array to enforce that exceptions
            // are thrown immediately if the cast fails and therefore ensure that this
            // object is not modified in case of an exception.
            var castedCollection = collection.Cast<T>().ToArray();

            base.InsertRange( index, castedCollection );
        }

        bool IListEx.Remove( object? item )
        {
            if( ( (IList) this ).IndexOf( item ) >= 0 )
            {
                ( (IList) this ).Remove( item );
                return true;
            }
            else
            {
                return false;
            }
        }

        bool ICollectionEx.Remove( object? item ) => ( (IListEx) this ).Remove( item );

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
                ( (IList) this ).Remove( item );
            }
        }

        /// <inheritdoc/>
        public bool Replace( T oldItem, T newItem )
        {
            var index = IndexOf( oldItem );
            if( index >= 0 )
            {
                Replace( index, newItem );
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void Replace( int index, T newItem )
        {
            if( ( index < 0 ) || ( index >= Count ) )
            {
                throw new ArgumentOutOfRangeException( nameof( index ) );
            }

            base.RemoveAt( index );
            base.Insert( index, newItem );
        }

        bool ICollectionEx.Replace( object? oldItem, object? newItem )
        {
            var index = ( (IList) this ).IndexOf( oldItem );
            if( index >= 0 )
            {
                if( oldItem != newItem )
                {
                    ( (IListEx) this ).Replace( index, newItem );
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        void IListEx.Replace( int index, object? newItem )
        {
            try
            {
                Replace( index, (T) newItem! );
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

        /// <inheritdoc/>
        public bool Move( T item, int newIndex )
        {
            var oldIndex = IndexOf( item );
            if( oldIndex >= 0 )
            {
                Move( oldIndex, newIndex );
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public void Move( int oldIndex, int newIndex )
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
                return;
            }

            var movedItem = this[ oldIndex ];

            base.RemoveAt( oldIndex );
            base.Insert( newIndex, movedItem );
        }

        bool IListEx.Move( object? item, int newIndex )
        {
            return IsCompatible( item ) && Move( (T) item!, newIndex );
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

        bool IListEx.Contains( object? item ) => ( (IList) this ).Contains( item );

        bool ICollectionEx.Contains( object? item ) => ( (IList) this ).Contains( item );

        bool IReadOnlyCollectionEx<T>.Contains( object? item ) => ( (IList) this ).Contains( item );

        int IReadOnlyListEx<T>.IndexOf( object? item ) => ( (IList) this ).IndexOf( item );

        int IReadOnlyListEx<T>.IndexOf( object? item, int index )
        {
            return IsCompatible( item ) ? base.IndexOf( (T) item!, index ) : -1;
        }

        int IReadOnlyListEx<T>.IndexOf( object? item, int index, int count )
        {
            return IsCompatible( item ) ? base.IndexOf( (T) item!, index, count ) : -1;
        }

        int IListEx.IndexOf( object? item, int index )
        {
            return IsCompatible( item ) ? base.IndexOf( (T) item!, index ) : -1;
        }

        int IListEx.IndexOf( object? item, int index, int count )
        {
            return IsCompatible( item ) ? base.IndexOf( (T) item!, index, count ) : -1;
        }

        int IReadOnlyListEx<T>.LastIndexOf( object? item )
        {
            return IsCompatible( item ) ? base.LastIndexOf( (T) item! ) : -1;
        }

        int IReadOnlyListEx<T>.LastIndexOf( object? item, int index )
        {
            return IsCompatible( item ) ? base.LastIndexOf( (T) item!, index ) : -1;
        }

        int IReadOnlyListEx<T>.LastIndexOf( object? item, int index, int count )
        {
            return IsCompatible( item ) ? base.LastIndexOf( (T) item!, index, count ) : -1;
        }

        int IListEx.LastIndexOf( object? item )
        {
            return IsCompatible( item ) ? base.LastIndexOf( (T) item! ) : -1;
        }

        int IListEx.LastIndexOf( object? item, int index )
        {
            return IsCompatible( item ) ? base.LastIndexOf( (T) item!, index ) : -1;
        }

        int IListEx.LastIndexOf( object? item, int index, int count )
        {
            return IsCompatible( item ) ? base.LastIndexOf( (T) item!, index, count ) : -1;
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        internal static bool IsCompatible( object? value )
        {
            return ( value is T ) || ( ( value is null ) && ( default( T ) is null ) );
        }
    }
}
