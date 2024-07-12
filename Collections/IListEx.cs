/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;

namespace Utilities.DotNet.Collections
{
    /// <summary>
    /// Extension of the <see cref="IList{T}"/> interface that provides additional methods.
    /// </summary>
    /// <typeparam name="T">Type of the items in the list.</typeparam>
    public interface IListEx<T> : IList<T>, ICollectionEx<T>, IReadOnlyListEx<T>
    {
        //===========================================================================
        //                                PROPERTIES
        //===========================================================================

        /// <inheritdoc cref="IList{T}.this[int]" />
        new T this[ int index ] { get; set; } // Needed to disambiguate between IList<> and IReadOnlyList<> and avoid error CS0121

        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source list.
        /// </summary>
        /// <param name="index">The zero-based list index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>Shallow copy of a range of elements in the source list.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="index"/> or <paramref name="count"/> are less than 0.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="index"/> and <paramref name="count"/> do not denote a valid range of
        ///                                            elements in the list.</exception>
        new IListEx<T> GetRange( int index, int count );

        /// <summary>
        /// Inserts the elements of a collection into the list at the specified index.
        /// </summary>
        /// <param name="index">Zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">Collection whose elements should be inserted into the list.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is less than 0 or greater than
        ///                                                      <see cref="ICollection{T}.Count">Count</see>.</exception>
        void InsertRange( int index, IEnumerable<T> collection );

        /// <summary>
        /// Removes a range of elements from the list.
        /// </summary>
        /// <param name="index">Zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">Number of elements to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="index"/> or <paramref name="count"/> are less than 0.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="index"/> and <paramref name="count"/> do not denote a valid range of
        ///                                            elements in the list.</exception>
        void RemoveRange( int index, int count );

        /// <summary>
        /// Searches for the specified item and returns the zero-based index of its first occurrence within the range of elements in the list that
        /// extends from the specified index to the last element.
        /// </summary>
        /// <remarks>
        /// The list is searched forward starting at <paramref name="index"/> and ending at the last element.
        /// </remarks>
        /// <param name="item">Object to locate in the list.</param>
        /// <param name="index">Zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <returns>Zero-based index of the first occurrence of item within the specified range of elements in the list, if found; otherwise, -1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is outside the range of valid indexes for the list.</exception>
        int IndexOf( T item, int index );

        /// <summary>
        /// Searches for the specified item and returns the zero-based index of the first occurrence within the range of elements in the list that starts
        /// at the specified index and contains the specified number of elements.
        /// </summary>
        /// <remarks>
        /// The list is searched forward starting at <paramref name="index"/> and ending at <paramref name="index"/> plus <paramref name="count"/> minus 1,
        /// if <paramref name="count"/> is greater than 0.
        /// </remarks>
        /// <param name="item">Object to locate in the list.</param>
        /// <param name="index">Zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">Number of elements in the section to search.</param>
        /// <returns>Zero-based index of the first occurrence of item within the specified range of elements in the list, if found; otherwise, -1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is outside the range of valid indexes for the list,
        ///                                                      <paramref name="count"/> is less than 0, or <paramref name="index"/> and <paramref name="count"/>
        ///                                                      do not denote a valid range of elements in the list.</exception>
        int IndexOf( T item, int index, int count );

        /// <summary>
        /// Searches for the specified item and returns the zero-based index of the last occurrence within the entire list.
        /// </summary>
        /// <remarks>
        /// The list is searched backward starting at the last element and ending at the first element.
        /// </remarks>
        /// <param name="item">Object to locate in the list.</param>
        /// <returns>Zero-based index of the last occurrence of item within the entire the list, if found; otherwise, -1.</returns>
        int LastIndexOf( T item );

        /// <summary>
        /// Searches for the specified item and returns the zero-based index of the last occurrence within the range of elements in the list
        /// that extends from the first element to the specified index.
        /// </summary>
        /// <remarks>
        /// The list is searched backward starting at <paramref name="index"/> and ending at the first element.
        /// </remarks>
        /// <param name="item">Object to locate in the list.</param>
        /// <param name="index">Zero-based starting index of the backward search.</param>
        /// <returns>Zero-based index of the last occurrence of item within the specified range of elements in the list, if found; otherwise, -1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is outside the range of valid indexes for the list.</exception>
        int LastIndexOf( T item, int index );

        /// <summary>
        /// Searches for the specified item and returns the zero-based index of the last occurrence within the range of elements in the list
        /// that contains the specified number of elements and ends at the specified index.
        /// </summary>
        /// <remarks>
        /// The list is searched backward starting at <paramref name="index"/> and ending at <paramref name="index"/> minus <paramref name="count"/> plus 1,
        /// if <paramref name="count"/> is greater than 0.
        /// </remarks>
        /// <param name="item">Object to locate in the list.</param>
        /// <param name="index">Zero-based starting index of the backward search.</param>
        /// <param name="count">Number of elements in the section to search.</param>
        /// <returns>Zero-based index of the last occurrence of item within the specified range of elements in the list, if found; otherwise, -1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is outside the range of valid indexes for the list,
        ///                                                      <paramref name="count"/> is less than 0, or <paramref name="index"/> and <paramref name="count"/>
        ///                                                      do not denote a valid range of elements in the list.</exception>
        int LastIndexOf( T item, int index, int count );

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source list.
        /// </summary>
        /// <param name="start">Zero-based list index at which the range starts.</param>
        /// <param name="length">Length of the range.</param>
        /// <returns>shallow copy of a range of elements in the source list.</returns>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="start"/> and <paramref name="length"/> do not denote a valid range of
        ///                                            elements in the list.</exception>
        new IListEx<T> Slice( int start, int length );
    }
}
