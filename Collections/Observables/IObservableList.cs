/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections;
using System.Collections.Generic;

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Represents a collection of objects which can be individually accessed by index, that provides notifications when items are
    /// added, removed, moved or replaced, or when the whole collection is cleared.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    public interface IObservableList<T> : IListEx<T>, IObservableCollection<T>, IObservableReadOnlyList<T>
    {
    }
}
