/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.Net.Converters
{
    /// <summary>
    /// Bit converter that converts from numeric values stored big-endian order.
    /// </summary>
    public class BigEndianBitConverter : BitConverter
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        public static readonly BitConverter Instance = new BigEndianBitConverter();

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        public BigEndianBitConverter() : base( System.BitConverter.IsLittleEndian )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public new static ushort ToUInt16( byte[] value, int startIndex )
        {
            return Instance.ToUInt16( value, startIndex );
        }

        public new static uint ToUInt32( byte[] value, int startIndex )
        {
            return Instance.ToUInt32( value, startIndex );
        }

        public new static ulong ToUInt64( byte[] value, int startIndex )
        {
            return Instance.ToUInt64( value, startIndex );
        }

        public new static short ToInt16( byte[] value, int startIndex )
        {
            return Instance.ToInt16( value, startIndex );
        }

        public new static int ToInt32( byte[] value, int startIndex )
        {
            return Instance.ToInt32( value, startIndex );
        }

        public new static long ToInt64( byte[] value, int startIndex )
        {
            return Instance.ToInt64( value, startIndex );
        }

        public new static float ToSingle( byte[] value, int startIndex )
        {
            return Instance.ToSingle( value, startIndex );
        }

        public new static double ToDouble( byte[] value, int startIndex )
        {
            return Instance.ToDouble( value, startIndex );
        }
    }
}
