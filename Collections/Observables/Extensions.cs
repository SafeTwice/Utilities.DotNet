/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;

namespace Utilities.DotNet.Collections.Observables
{
    /// <summary>
    /// Extension methods for observable collections.
    /// </summary>
    public static class Extensions
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> into a <see cref="ObservableCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to convert.</param>
        /// <returns>An <see cref="ObservableCollection{T}"/> containing the elements of the sequence.</returns>
        public static ObservableCollection<T> ToObservableCollection<T>( this IEnumerable<T> sequence )
        {
            return new ObservableCollection<T>( sequence );
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> into a <see cref="ObservableList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to convert.</param>
        /// <returns>An <see cref="ObservableList{T}"/> containing the elements of the sequence.</returns>
        public static ObservableList<T> ToObservableList<T>( this IEnumerable<T> sequence )
        {
            return new ObservableList<T>( sequence );
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> into a <see cref="ObservableSet{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to convert.</param>
        /// <returns>An <see cref="ObservableSet{T}"/> containing the elements of the sequence.</returns>
        public static ObservableSet<T> ToObservableSet<T>( this IEnumerable<T> sequence )
        {
            return new ObservableSet<T>( sequence );
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> into a <see cref="ObservableSortedCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to convert.</param>
        /// <returns>An <see cref="ObservableSortedCollection{T}"/> containing the elements of the sequence.</returns>
        public static ObservableSortedCollection<T> ToObservableSortedCollection<T>( this IEnumerable<T> sequence )
        {
            return new ObservableSortedCollection<T>( sequence );
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> into a <see cref="ObservableSortedList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to convert.</param>
        /// <returns>An <see cref="ObservableSortedList{T}"/> containing the elements of the sequence.</returns>
        public static ObservableSortedList<T> ToObservableSortedList<T>( this IEnumerable<T> sequence )
        {
            return new ObservableSortedList<T>( sequence );
        }

        /// <summary>
        /// Converts an <see cref="IEnumerable{T}"/> into a <see cref="ObservableSortedSet{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="sequence">The sequence to convert.</param>
        /// <returns>An <see cref="ObservableSortedSet{T}"/> containing the elements of the sequence.</returns>
        public static ObservableSortedSet<T> ToObservableSortedSet<T>( this IEnumerable<T> sequence )
        {
            return new ObservableSortedSet<T>( sequence );
        }

        /// <summary>
        /// Wraps an <see cref="IObservableCollection{T}"/> into a <see cref="ObservableReadOnlyCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to wrap.</param>
        /// <returns>A read-only view wrapping the elements of the collection.</returns>
        public static ObservableReadOnlyCollection<T> WrapAsObservableReadOnlyCollection<T>( this IObservableCollection<T> collection )
        {
            return new ObservableReadOnlyCollection<T>( collection );
        }

        /// <summary>
        /// Wraps an <see cref="IObservableList{T}"/> into a <see cref="ObservableReadOnlyList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the list.</typeparam>
        /// <param name="list">The list to wrap.</param>
        /// <returns>A read-only view wrapping the elements of the list.</returns>
        public static ObservableReadOnlyList<T> WrapAsObservableReadOnlyList<T>( this IObservableList<T> list )
        {
            return new ObservableReadOnlyList<T>( list );
        }

        /// <summary>
        /// Wraps an <see cref="IObservableSet{T}"/> into a <see cref="ObservableReadOnlySet{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the set.</typeparam>
        /// <param name="set">The set to wrap.</param>
        /// <returns>A read-only view wrapping the elements of the set.</returns>
        public static ObservableReadOnlySet<T> WrapAsObservableReadOnlySet<T>( this IObservableSet<T> set )
        {
            return new ObservableReadOnlySet<T>( set );
        }
    }
}
