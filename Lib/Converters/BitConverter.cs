/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.Net.Converters
{
    /// <summary>
    /// Bit converter that converts from numeric values stored big-endian or little-endian order.
    /// </summary>
    public class BitConverter
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        public BitConverter( bool reverse )
        {
            m_reverse = reverse;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public ushort Convert( ushort value )
        {
            if( m_reverse )
            {
                return ToUInt16( System.BitConverter.GetBytes( value ), 0 );
            }
            else
            {
                return value;
            }
        }

        public uint Convert( uint value )
        {
            if( m_reverse )
            {
                return ToUInt32( System.BitConverter.GetBytes( value ), 0 );
            }
            else
            {
                return value;
            }
        }

        public ulong Convert( ulong value )
        {
            if( m_reverse )
            {
                return ToUInt64( System.BitConverter.GetBytes( value ), 0 );
            }
            else
            {
                return value;
            }
        }

        public short Convert( short value )
        {
            if( m_reverse )
            {
                return ToInt16( System.BitConverter.GetBytes( value ), 0 );
            }
            else
            {
                return value;
            }
        }

        public int Convert( int value )
        {
            if( m_reverse )
            {
                return ToInt32( System.BitConverter.GetBytes( value ), 0 );
            }
            else
            {
                return value;
            }
        }

        public long Convert( long value )
        {
            if( m_reverse )
            {
                return ToInt64( System.BitConverter.GetBytes( value ), 0 );
            }
            else
            {
                return value;
            }
        }

        public ushort ToUInt16( byte[] value, int startIndex )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( ushort ) );
                startIndex = 0;
            }
            return System.BitConverter.ToUInt16( value, startIndex );
        }

        public uint ToUInt32( byte[] value, int startIndex )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( uint ) );
                startIndex = 0;
            }
            return System.BitConverter.ToUInt32( value, startIndex );
        }

        public ulong ToUInt64( byte[] value, int startIndex )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( ulong ) );
                startIndex = 0;
            }
            return System.BitConverter.ToUInt64( value, startIndex );
        }

        public short ToInt16( byte[] value, int startIndex )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( short ) );
                startIndex = 0;
            }
            return System.BitConverter.ToInt16( value, startIndex );
        }

        public int ToInt32( byte[] value, int startIndex )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( int ) );
                startIndex = 0;
            }
            return System.BitConverter.ToInt32( value, startIndex );
        }

        public long ToInt64( byte[] value, int startIndex )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( long ) );
                startIndex = 0;
            }
            return System.BitConverter.ToInt64( value, startIndex );
        }

        public float ToSingle( byte[] value, int startIndex )
        {
            if( m_reverse )
            {
                value = value.ReversedSubarray( startIndex, sizeof( float ) );
                startIndex = 0;
            }
            return System.BitConverter.ToSingle( value, startIndex );
        }

        public double ToDouble( byte[] value, int startIndex )
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

        private bool m_reverse;
    }
}
