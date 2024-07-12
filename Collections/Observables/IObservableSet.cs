/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Represents a set of unique objects that provides notifications when items are
    /// added, removed, moved or replaced, or when the whole set is cleared.
    /// </summary>
    /// <typeparam name="T">The type of the items in the set.</typeparam>
    public interface IObservableSet<T> : ISet<T>, IObservableCollection<T>, IObservableReadOnlySet<T>
    {
        /// <inheritdoc cref="ISet{T}.Add(T)"/>
        new bool Add( T item ); // Needed to disambiguate between ISet<> and ICollectionEx<> and avoid error CS0121
    }
}
