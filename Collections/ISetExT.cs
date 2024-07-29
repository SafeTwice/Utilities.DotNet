/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of the <see cref="ISet{T}"/> interface that provides additional methods.
    /// </summary>
    /// <typeparam name="T">Type of the items in the set.</typeparam>
    public interface ISetEx<T> : ISet<T>, ICollectionEx<T>
    {
    }
}
