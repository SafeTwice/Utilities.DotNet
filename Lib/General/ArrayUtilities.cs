/// @file
/// @copyright  Copyright (c) 2019-2021 SafeTwice S.L. All rights reserved.
/// @license    MIT (https://opensource.org/licenses/MIT)

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
        /// Fills an <paramref name="array"/> with the given <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the array</typeparam>
        /// <param name="array">An array</param>
        /// <param name="value">Value to be set</param>
        public static void Fill<T>( this T[] array, T value )
        {
            for( int i = 0; i < array.Length; i++ )
            {
                array[ i ] = value;
            }
        }

        /// <summary>
        /// Creates a new array filled with the elements of <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">Type of the elements of the array</typeparam>
        /// <param name="array">An array</param>
        /// <returns>A shallow clone of the input array</returns>
        public static T[] ShallowClone<T>( this T[] array )
        {
            T[] clonedArray = new T[ array.Length ];

            array.CopyTo( clonedArray, 0 );

            return clonedArray;
        }
    }
}
