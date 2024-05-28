/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of <see cref="List{T}"/> that implements <see cref="IListEx{T}"/>.
    /// </summary>
    public class ListEx<T> : List<T>, IListEx<T>
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

#if NET8_0_OR_GREATER
        /// <inheritdoc/>
        public new IListEx<T> Slice( int start, int length )
        {
            return GetRange( start, length );
        }
#else
        /// <inheritdoc/>
        public IListEx<T> Slice( int start, int length )
        {
            return GetRange( start, length );
        }
#endif

        /// <inheritdoc/>
        public void RemoveRange( IEnumerable<T> collection )
        {
            foreach( var item in collection )
            {
                Remove( item );
            }
        }
    }
}
