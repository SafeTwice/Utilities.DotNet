/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Represents a collection of objects which can be individually accessed by index, that provides notifications when items are
    /// added, removed, moved or replaced, or when the whole collection is cleared.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    public interface IObservableList<T> : IListEx<T>, IObservableCollection<T>, IObservableReadOnlyList<T>
    {
        /// <summary>
        /// Creates a shallow copy of a range of elements in the source list.
        /// </summary>
        /// <param name="index">The zero-based list index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>Shallow copy of a range of elements in the source list.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="index"/> or <paramref name="count"/> are less than 0.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="index"/> and <paramref name="count"/> do not denote a valid range of
        ///                                            elements in the list.</exception>
        new IObservableList<T> GetRange( int index, int count );

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source list.
        /// </summary>
        /// <param name="start">Zero-based list index at which the range starts.</param>
        /// <param name="length">Length of the range.</param>
        /// <returns>shallow copy of a range of elements in the source list.</returns>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="start"/> and <paramref name="length"/> do not denote a valid range of
        ///                                            elements in the list.</exception>
        new IObservableList<T> Slice( int start, int length );
    }
}
