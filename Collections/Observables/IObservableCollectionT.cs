/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Represents a collection of objects that provides notifications when items are added or removed, or when the whole collection is cleared.
    /// </summary>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public interface IObservableCollection<T> : IObservableReadOnlyCollection<T>, ICollectionEx<T>
    {
    }
}
