/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Represents a collection of objects which can be individually accessed by index,
    /// that provides notifications when items are added, removed, moved or replaced,
    /// or when the whole collection is cleared.
    /// </summary>
    public interface IObservableList : IObservableCollection, IListEx
    {
    }
}
