/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.Converters
{
    /// <summary>
    /// Bit converter that converts from numeric values stored big-endian or little-endian order.
    /// </summary>
    public class BitConverter
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="reverse">Indicates if the values to convert have different byte order (endianness) than the one used in the processing computer architecture.</param>
        public BitConverter( bool reverse )
        {
            m_reverse = reverse;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Converts a value stored using 2 bytes at the specified position of a byte array to a 16-bit unsigned integer.
        /// </summary>
        /// <param name="value">Array storing the value to be converted.</param>
        /// <param name="startIndex">Index in the array at which the value is stored.</param>
        /// <returns>Converted value.</returns>
        public ushort ToUInt16( byte[] value, int startIndex = 0 )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( ushort ) );
                startIndex = 0;
            }
            return System.BitConverter.ToUInt16( value, startIndex );
        }

        /// <summary>
        /// Converts a value stored using 4 bytes at the specified position of a byte array to a 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">Array storing the value to be converted.</param>
        /// <param name="startIndex">Index in the array at which the value is stored.</param>
        /// <returns>Converted value.</returns>
        public uint ToUInt32( byte[] value, int startIndex = 0 )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( uint ) );
                startIndex = 0;
            }
            return System.BitConverter.ToUInt32( value, startIndex );
        }

        /// <summary>
        /// Converts a value stored using 8 bytes at the specified position of a byte array to a 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">Array storing the value to be converted.</param>
        /// <param name="startIndex">Index in the array at which the value is stored.</param>
        /// <returns>Converted value.</returns>
        public ulong ToUInt64( byte[] value, int startIndex = 0 )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( ulong ) );
                startIndex = 0;
            }
            return System.BitConverter.ToUInt64( value, startIndex );
        }

        /// <summary>
        /// Converts a value stored using 2 bytes at the specified position of a byte array to a 16-bit signed integer.
        /// </summary>
        /// <param name="value">Array storing the value to be converted.</param>
        /// <param name="startIndex">Index in the array at which the value is stored.</param>
        /// <returns>Converted value.</returns>
        public short ToInt16( byte[] value, int startIndex = 0 )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( short ) );
                startIndex = 0;
            }
            return System.BitConverter.ToInt16( value, startIndex );
        }

        /// <summary>
        /// Converts a value stored using 4 bytes at the specified position of a byte array to a 32-bit signed integer.
        /// </summary>
        /// <param name="value">Array storing the value to be converted.</param>
        /// <param name="startIndex">Index in the array at which the value is stored.</param>
        /// <returns>Converted value.</returns>
        public int ToInt32( byte[] value, int startIndex = 0 )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( int ) );
                startIndex = 0;
            }
            return System.BitConverter.ToInt32( value, startIndex );
        }

        /// <summary>
        /// Converts a value stored using 8 bytes at the specified position of a byte array to a 64-bit signed integer.
        /// </summary>
        /// <param name="value">Array storing the value to be converted.</param>
        /// <param name="startIndex">Index in the array at which the value is stored.</param>
        /// <returns>Converted value.</returns>
        public long ToInt64( byte[] value, int startIndex = 0 )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( long ) );
                startIndex = 0;
            }
            return System.BitConverter.ToInt64( value, startIndex );
        }

        /// <summary>
        /// Converts a value stored using 4 bytes at the specified position of a byte array to a 32-bit floating-point number.
        /// </summary>
        /// <param name="value">Array storing the value to be converted.</param>
        /// <param name="startIndex">Index in the array at which the value is stored.</param>
        /// <returns>Converted value.</returns>
        public float ToSingle( byte[] value, int startIndex = 0 )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( float ) );
                startIndex = 0;
            }
            return System.BitConverter.ToSingle( value, startIndex );
        }

        /// <summary>
        /// Converts a value stored using 8 bytes at the specified position of a byte array to a 64-bit floating-point number.
        /// </summary>
        /// <param name="value">Array storing the value to be converted.</param>
        /// <param name="startIndex">Index in the array at which the value is stored.</param>
        /// <returns>Converted value.</returns>
        public double ToDouble( byte[] value, int startIndex = 0 )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( double ) );
                startIndex = 0;
            }
            return System.BitConverter.ToDouble( value, startIndex );
        }

        //===========================================================================
        //                           PRIVATE PROPERTIES
        //===========================================================================

        private readonly bool m_reverse;
    }
}
