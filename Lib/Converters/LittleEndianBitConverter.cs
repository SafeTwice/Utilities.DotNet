/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.Converters
{
    /// <summary>
    /// Bit converter that converts from numeric values stored little-endian order.
    /// </summary>
    public class LittleEndianBitConverter : BitConverter
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <value>
        /// Global converter instance.
        /// </value>
        public static BitConverter Instance { get; } = new LittleEndianBitConverter();

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        public LittleEndianBitConverter() : base( !System.BitConverter.IsLittleEndian )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc cref="BitConverter.ToUInt16(byte[], int)"/>
        public new static ushort ToUInt16( byte[] value, int startIndex )
        {
            return Instance.ToUInt16( value, startIndex );
        }

        /// <inheritdoc cref="BitConverter.ToUInt32(byte[], int)"/>
        public new static uint ToUInt32( byte[] value, int startIndex )
        {
            return Instance.ToUInt32( value, startIndex );
        }

        /// <inheritdoc cref="BitConverter.ToUInt64(byte[], int)"/>
        public new static ulong ToUInt64( byte[] value, int startIndex )
        {
            return Instance.ToUInt64( value, startIndex );
        }

        /// <inheritdoc cref="BitConverter.ToInt16(byte[], int)"/>
        public new static short ToInt16( byte[] value, int startIndex )
        {
            return Instance.ToInt16( value, startIndex );
        }

        /// <inheritdoc cref="BitConverter.ToInt32(byte[], int)"/>
        public new static int ToInt32( byte[] value, int startIndex )
        {
            return Instance.ToInt32( value, startIndex );
        }

        /// <inheritdoc cref="BitConverter.ToInt64(byte[], int)"/>
        public new static long ToInt64( byte[] value, int startIndex )
        {
            return Instance.ToInt64( value, startIndex );
        }

        /// <inheritdoc cref="BitConverter.ToSingle(byte[], int)"/>
        public new static float ToSingle( byte[] value, int startIndex )
        {
            return Instance.ToSingle( value, startIndex );
        }

        /// <inheritdoc cref="BitConverter.ToDouble(byte[], int)"/>
        public new static double ToDouble( byte[] value, int startIndex )
        {
            return Instance.ToDouble( value, startIndex );
        }
    }
}
