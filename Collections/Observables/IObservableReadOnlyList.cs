/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Represents a read-only view of a collection of objects that provides notifications when items are added or removed,
    /// or when the whole collection is cleared.
    /// </summary>
    /// <remarks>
    /// This interface does not provide methods to modify the collection, but this does not mean that the collection is immutable,
    /// the collection can still be modified by other means.
    /// </remarks>
    /// <typeparam name="T">The type of the items in the collection.</typeparam>
    public interface IObservableReadOnlyList<out T> : IObservableReadOnlyCollection<T>, IReadOnlyListEx<T>
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <inheritdoc cref="IReadOnlyListEx{T}.GetRange(int, int)"/>
        new IObservableReadOnlyList<T> GetRange( int index, int count );

        /// <inheritdoc cref="IReadOnlyListEx{T}.Slice(int, int)"/>
        new IObservableReadOnlyList<T> Slice( int start, int length );
    }
}
