/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Represents a read-only view of a set of objects that provides notifications when items are added or removed,
    /// or when the whole set is cleared.
    /// </summary>
    /// <remarks>
    /// This interface does not provide methods to modify the set, but this does not mean that the set is immutable,
    /// the set can still be modified by other means.
    /// </remarks>
    /// <typeparam name="T">The type of the items in the set.</typeparam>
    public interface IObservableReadOnlySet<out T> : IObservableReadOnlyCollection<T>, IReadOnlySetEx<T>
    {
    }
}
