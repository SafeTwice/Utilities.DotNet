/// @file
/// @copyright  Copyright (c) 2022-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections;
using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Represents a collection of objects which can be individually accessed by index, that provides notifications when items are
    /// added, removed, moved or replaced, or when the whole collection is cleared.
    /// </summary>
    /// <typeparam name="T">The type of the items in the list.</typeparam>
    public interface IObservableList<T> : IListEx<T>, IList, IObservableCollection<T>, IObservableReadOnlyList<T>
    {
        /// <inheritdoc cref="IList{T}.this[int]" />
        new T this[ int index ] { get; set; } // Needed to disambiguate between IList<> and IList and avoid error CS0121

        /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
        new bool IsReadOnly { get; } // Needed to disambiguate between IObservableCollection<> and IList and avoid error CS0229

        /// <inheritdoc cref="IList{T}.RemoveAt(int)" />
        new void RemoveAt( int index ); // Needed to disambiguate between IList<> and IList and avoid error CS0121

        /// <inheritdoc cref="ICollection{T}.Clear()" />
        new void Clear(); // Needed to disambiguate between ICollection<> and IList and avoid error CS0121
    }
}
