/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of the <see cref="IList{T}"/> interface that provides additional methods.
    /// </summary>
    public interface IListEx<T> : IList<T>
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <inheritdoc cref="List{T}.AddRange(IEnumerable{T})"/>
        void AddRange( IEnumerable<T> collection );
    }
}
