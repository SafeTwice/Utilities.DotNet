/// @file
/// @copyright  Copyright (c) 2019-2021 SafeTwice S.L. All rights reserved.
/// @license    MIT (https://opensource.org/licenses/MIT)

using System;
using System.Text;

namespace Utilities.Net
{
    /// <summary>
    /// Array utilities.
    /// </summary>
    public static class ArrayUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Creates an array of the given <paramref name="size"/> filled with <paramref name="initValue"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the array</typeparam>
        /// <param name="size">Size of the array</param>
        /// <param name="initValue">Initial value for the elements</param>
        /// <returns>An array</returns>
        public static T[] CreateArray<T>( uint size, T initValue )
        {
            T[] result = new T[ size ];

            for( int i = 0; i < size; i++ )
            {
                result[ i ] = initValue;
            }

            return result;
        }

        /// <summary>
        /// Creates an array of the given <paramref name="size"/> filled with values provided by <paramref name="valueProvider"/>.
        /// </summary>
        /// <remarks>
        /// The function <paramref name="valueProvider"/> is called once for each value of the array.
        /// </remarks>
        /// <typeparam name="T">Type of the elements of the array</typeparam>
        /// <param name="size">Size of the array</param>
        /// <param name="valueProvider">Function to provide the initial values for the array</param>
        /// <returns>An array</returns>
        public static T[] CreateArray<T>( uint size, Func<T> valueProvider )
        {
            T[] result = new T[ size ];

            for( int i = 0; i < size; i++ )
            {
                result[ i ] = valueProvider();
            }

            return result;
        }

        /// <summary>
        /// Creates an array of the given <paramref name="size"/> filled with values provided by <paramref name="valueProvider"/>.
        /// </summary>
        /// <remarks>
        /// The function <paramref name="valueProvider"/> is called once for each value of the array, passing the array index as parameter.
        /// </remarks>
        /// <typeparam name="T">Type of the elements of the array</typeparam>
        /// <param name="size">Size of the array</param>
        /// <param name="valueProvider">Function to provide the initial values for the array</param>
        /// <returns>An array</returns>
        public static T[] CreateArray<T>( uint size, Func<int,T> valueProvider )
        {
            T[] result = new T[ size ];

            for( int i = 0; i < size; i++ )
            {
                result[ i ] = valueProvider( i );
            }

            return result;
        }

        /// <summary>
        /// Fills an <paramref name="array"/> with the given <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the array</typeparam>
        /// <param name="array">An array</param>
        /// <param name="value">Value to be set</param>
        public static void Fill<T>( this T[] array, T value )
        {
            array.Fill( value, 0, array.Length );
        }

        /// <summary>
        /// Fills range of elements of an <paramref name="array"/> with the given <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the array</typeparam>
        /// <param name="array">An array</param>
        /// <param name="value">Value to be set</param>
        /// <param name="startIndex">Index at which filling begins</param>
        /// <param name="count">Number of elements to fill</param>
        public static void Fill<T>( this T[] array, T value, int startIndex, int count )
        {
            var endIndex = ( startIndex + count );
            for( int i = startIndex; i < endIndex; i++ )
            {
                array[ i ] = value;
            }
        }

        /// <summary>
        /// Creates a new array filled with a range of elements from <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the array</typeparam>
        /// <param name="array">An array</param>
        /// <param name="startIndex">Index at which element copying from the source array begins</param>
        /// <param name="count">Number of elements to copy</param>
        /// <returns>A shallow copy of the specified range of elements from the input array</returns>
        public static T[] Subarray<T>( this T[] array, int startIndex, int count )
        {
            T[] subarray = new T[ count ];

            Array.Copy( array, startIndex, subarray, 0, count );

            return subarray;
        }

        /// <summary>
        /// Creates a new array filled with the elements of <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the array</typeparam>
        /// <param name="array">An array</param>
        /// <returns>A shallow clone of the input array</returns>
        public static T[] ShallowClone<T>( this T[] array )
        {
            return array.Subarray( 0, array.Length );
        }

        /// <summary>
        /// Creates a new array filled with a range of elements from <paramref name="array"/> in reverse order.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the array</typeparam>
        /// <param name="array">An array</param>
        /// <param name="startIndex">Index at which element copying from the source array begins</param>
        /// <param name="count">Number of elements to copy</param>
        /// <returns>A shallow copy of the specified range of elements from the input array in reverse order</returns>
        public static T[] ReversedSubarray<T>( this T[] array, int startIndex, int count )
        {
            var subarray = array.Subarray( startIndex, count );

            Array.Reverse( subarray );

            return subarray;
        }

        /// <summary>
        /// Creates a new array filled with the elements of <paramref name="array"/> in reverse order.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the array</typeparam>
        /// <param name="array">An array</param>
        /// <returns>A shallow copy of the input array in reverse order</returns>
        public static T[] ReversedShallowClone<T>( this T[] array )
        {
            return array.ReversedSubarray( 0, array.Length );
        }

        /// <summary>
        /// Decodes an ASCII string from a byte array.
        /// </summary>
        /// <param name="array">Array to decode the string from</param>
        /// <returns>An string decoded from its ASCII representation in the array</returns>
        public static string DecodeASCII( this byte[] array )
        {
            return array.DecodeASCII( 0, array.Length );
        }

        /// <summary>
        /// Decodes an ASCII string from a byte array.
        /// </summary>
        /// <param name="array">Array to decode the string from</param>
        /// <param name="offset">Offset to start decoding from</param>
        /// <param name="length">Length to decode</param>
        /// <returns>An string decoded from its ASCII representation in the array</returns>
        public static string DecodeASCII( this byte[] array, int offset, int length )
        {
            var firstNULindex = Array.IndexOf( array, (byte) 0, offset, length );
            if( firstNULindex != -1 )
            {
                length = firstNULindex - offset;
            }
            return Encoding.ASCII.GetString( array, offset, length );
        }

        /// <summary>
        /// Encodes the ASCII representation of a string into a byte array.
        /// </summary>
        /// <param name="str">String to encode</param>
        /// <returns>A byte array with the ASCII representation of the string</returns>
        public static byte[] EncodeASCII( this string str )
        {
            return Encoding.ASCII.GetBytes( str );
        }

        /// <summary>
        /// Converts a byte array to its corresponding hexadecimal representation.
        /// </summary>
        /// <param name="value">A byte array</param>
        /// <param name="separator">An optional string to include between byte's representations</param>
        /// <returns>String containing the hexadecimal representation of the input array</returns>
        public static string ToHexString( this byte[] value, string separator = "" )
        {
            if( value.Length == 0 )
            {
                return string.Empty;
            }

            StringBuilder result = new StringBuilder( ( value.Length * 2 ) + ( ( value.Length - 1 ) * separator.Length ) );
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
    }
}
