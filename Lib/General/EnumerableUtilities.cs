/// @file
/// @copyright  Copyright (c) 2019-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.DotNet.Exceptions;

namespace Utilities.DotNet
{
    /// <summary>
    /// Enumerable utilities.
    /// </summary>
    public static class EnumerableUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================


        /// <summary>
        /// Converts a byte sequence to its corresponding hexadecimal representation.
        /// </summary>
        /// <param name="value">A byte sequence.</param>
        /// <param name="separator">An optional string to include between byte's representations.</param>
        /// <returns>String containing the hexadecimal representation of the input sequence.</returns>
        public static string ToHexString( this IEnumerable<byte> value, string separator = "" )
        {
            var length = value.Count();

            if( length == 0 )
            {
                return string.Empty;
            }

            var result = new StringBuilder( ( length * 2 ) + ( ( length - 1 ) * separator.Length ) );
            bool addSeparator = false;

            foreach( byte b in value )
            {
                if( addSeparator )
                {
                    result.Append( separator );
                }

                result.AppendFormat( "{0:X2}", b );

                addSeparator = true;
            }

            return result.ToString();
        }

        /// <summary>
        /// Performs the specified <see cref="Action{T}"/> on each element of an <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> with the elements to perform the <paramref name="action"/> on.</param>
        /// <param name="action">The <see cref="Action{T}"/> to perform on each element of <paramref name="source"/>.</param>
        public static void ForEach<TSource>( this IEnumerable<TSource> source, Action<TSource> action )
        {
            foreach( var element in source )
            {
                action( element );
            }
        }

        /// <summary>
        /// Performs the specified <see cref="Action{T, TParam}"/> on each element of an <see cref="IEnumerable{T}"/> using a parameter.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TParam">Type of the parameter passed to the action.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> with the elements to perform the <paramref name="action"/> on.</param>
        /// <param name="action">An <see cref="Action{T}"/> to perform on each element of <paramref name="source"/>.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}"/> with the parameters passed to the <paramref name="action"/> for each element.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="source"/> and <paramref name="parameters"/> do not have the same size.</exception>
        public static void ForEach<TSource, TParam>( this IEnumerable<TSource> source, Action<TSource, TParam> action, IEnumerable<TParam> parameters )
        {
            if( source.Count() != parameters.Count() )
            {
                throw new ArgumentException( "Parameters 'source' and 'parameters' must have the same size.", nameof( parameters ) );
            }

            var paramEnumerator = parameters.GetEnumerator();
            paramEnumerator.MoveNext();

            foreach( var element in source )
            {
                action.Invoke( element, paramEnumerator.Current );
                paramEnumerator.MoveNext();
            }
        }

        /// <summary>
        /// Projects each element of an <see cref="IEnumerable{T}"/> using the specified <see cref="Func{T1, T2, T3, TResult}"/> with a parameter.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">Type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <typeparam name="TParam">Type of the parameter passed to the selector.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> with the elements to perform the <paramref name="selector"/> on.</param>
        /// <param name="selector">A transform function to apply to each source element; the second parameter of the function provides the parameter
        ///                        for each source element.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}"/> with the parameters passed to the <paramref name="selector"/> for each source element.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="source"/> and <paramref name="parameters"/> do not have the same size.</exception>
        public static IEnumerable<TResult> Select<TSource, TResult, TParam>( this IEnumerable<TSource> source, Func<TSource, TParam, TResult> selector,
                                                                             IEnumerable<TParam> parameters )
        {
            if( source.Count() != parameters.Count() )
            {
                throw new ArgumentException( "Parameters 'source' and 'parameters' must have the same size.", nameof( parameters ) );
            }

            var paramEnumerator = parameters.GetEnumerator();
            paramEnumerator.MoveNext();

            foreach( var element in source )
            {
                yield return selector.Invoke( element, paramEnumerator.Current );
                paramEnumerator.MoveNext();
            }
        }

        /// <summary>
        /// Projects each element of an <see cref="IEnumerable{T}"/> using the specified <see cref="Func{T1, T2, T3, TResult}"/> with a parameter.
        /// </summary>
        /// <typeparam name="TSource">Type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">Type of the value returned by <paramref name="selector"/>.</typeparam>
        /// <typeparam name="TParam">Type of the parameter passed to the selector.</typeparam>
        /// <param name="source">An <see cref="IEnumerable{T}"/> with the elements to perform the <paramref name="selector"/> on.</param>
        /// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents
        ///                        the index of the source element; the third parameter of the function provides the parameter for each source element.</param>
        /// <param name="parameters">An <see cref="IEnumerable{T}"/> with the parameters passed to the <paramref name="selector"/> for each source element.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> whose elements are the result of invoking the transform function on each element of source.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="source"/> and <paramref name="parameters"/> do not have the same size.</exception>
        public static IEnumerable<TResult> Select<TSource, TResult, TParam>( this IEnumerable<TSource> source, Func<TSource, Int32, TParam, TResult> selector,
                                                                             IEnumerable<TParam> parameters )
        {
            if( source.Count() != parameters.Count() )
            {
                throw new ArgumentException( "Parameters 'source' and 'parameters' must have the same size.", nameof( parameters ) );
            }

            int index = 0;
            var paramEnumerator = parameters.GetEnumerator();
            paramEnumerator.MoveNext();

            foreach( var element in source )
            {
                yield return selector.Invoke( element, index, paramEnumerator.Current );
                index++;
                paramEnumerator.MoveNext();
            }
        }

        /// <summary>
        /// Casts all elements of an <see cref="IEnumerable{T}"/> to the specified type.
        /// </summary>
        /// <typeparam name="T">Target type of the cast.</typeparam>
        /// <param name="source"><see cref="IEnumerable{T}"/> whose elements to cast.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains the cast elements.</returns>
        /// <exception cref="InvalidCastExceptionEx">Thrown when an element could not be cast to <typeparamref name="T"/>.</exception>
        public static IEnumerable<T> CastAll<T>( this IEnumerable<object> source )
        {
            foreach( var item in source )
            {
                if( item is T castItem )
                {
                    yield return castItem;
                }
                else
                {
                    throw new InvalidCastExceptionEx( item, typeof( T ) );
                }
            }
        }
    }
}
