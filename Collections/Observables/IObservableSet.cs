/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Represents a set of unique objects that provides notifications when items
    /// are added, removed, moved or replaced, or when the whole set is cleared.
    /// </summary>
    public interface IObservableSet : IObservableCollection, ISetEx
    {
    }
}
