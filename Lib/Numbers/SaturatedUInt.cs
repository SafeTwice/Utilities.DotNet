﻿/// @file
/// @copyright  Copyright (c) 2022 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.Numbers
{
    /// <summary>
    /// Unsigned integer number which value (and the result from arithmetic operations) are saturated between the given bounds.
    /// </summary>
    public readonly struct SaturatedUInt : IEquatable<uint>, IEquatable<SaturatedUInt>, IComparable<uint>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Current value.
        /// </summary>
        public uint Value => m_value;

        /// <summary>
        /// Minimum value.
        /// </summary>
        public uint Minimum => m_minimum;


        /// <summary>
        /// Maximum value.
        /// </summary>
        public uint Maximum => m_maximum;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other">Object to copy.</param>
        public SaturatedUInt( SaturatedUInt other )
        {
            m_minimum = other.m_minimum;
            m_maximum = other.m_maximum;
            m_value = other.m_value;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="min">Bound minimum.</param>
        /// <param name="max">Bound maximum.</param>
        public SaturatedUInt( uint value, uint min, uint max )
        {
            if( min > max )
            {
                throw new ArgumentException( "bound minimum cannot be greater than bound maximum" );
            }

            m_minimum = min;
            m_maximum = max;

            if( value < min )
            {
                m_value = min;
            }
            else if( value > max )
            {
                m_value = max;
            }
            else
            {
                m_value = value;
            }
        }

        /// <summary>
        /// Constructor with bounds set to the whole integer range.
        /// </summary>
        /// <param name="value">Value.</param>
        public SaturatedUInt( uint value ) : this( value, uint.MinValue, uint.MaxValue )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Converts <paramref name="o"/> to an integer.
        /// </summary>
        /// <param name="o">Instance to convert.</param>
        public static implicit operator uint( SaturatedUInt o )
        {
            return o.m_value;
        }

        /// <summary>
        /// Increments the current value of <paramref name="o"/> by one.
        /// </summary>
        /// <param name="o">A saturated number.</param>
        /// <returns>A new instance which current value is the current value of <paramref name="o"/> incremented by one,
        ///          and having the same minimum and maximum values than <paramref name="o"/>.</returns>
        public static SaturatedUInt operator ++( SaturatedUInt o )
        {
            checked
            {
                try
                {
                    uint newValue = o.m_value + 1;
                    return new SaturatedUInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedUInt( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        /// <summary>
        /// Decrements the current value of <paramref name="o"/> by one.
        /// </summary>
        /// <param name="o">A saturated number.</param>
        /// <returns>A new instance which current value is the current value of <paramref name="o"/> decremented by one,
        ///          and having the same minimum and maximum values than <paramref name="o"/>.</returns>
        public static SaturatedUInt operator --( SaturatedUInt o )
        {
            checked
            {
                try
                {
                    uint newValue = o.m_value - 1;
                    return new SaturatedUInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedUInt( o.m_minimum, o.m_minimum, o.m_maximum );
                }
            }
        }

        /// <summary>
        /// Increments the current value of <paramref name="o"/> by <paramref name="value"/>.
        /// </summary>
        /// <param name="o">A saturated number.</param>
        /// <param name="value">Value to increment by.</param>
        /// <returns>A new instance which current value is the current value of <paramref name="o"/> incremented by <paramref name="value"/>,
        ///          and having the same minimum and maximum values than <paramref name="o"/>.</returns>
        public static SaturatedUInt operator +( SaturatedUInt o, uint value )
        {
            checked
            {
                try
                {
                    uint newValue = o.m_value + value;
                    return new SaturatedUInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedUInt( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        /// <summary>
        /// Decrements the current value of <paramref name="o"/> by <paramref name="value"/>.
        /// </summary>
        /// <param name="o">A saturated number.</param>
        /// <param name="value">Value to decrement by.</param>
        /// <returns>A new instance which current value is the current value of <paramref name="o"/> decremented by <paramref name="value"/>,
        ///          and having the same minimum and maximum values than <paramref name="o"/>.</returns>
        public static SaturatedUInt operator -( SaturatedUInt o, uint value )
        {
            checked
            {
                try
                {
                    uint newValue = o.m_value - value;
                    return new SaturatedUInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedUInt( o.m_minimum, o.m_minimum, o.m_maximum );
                }
            }
        }

        /// <summary>
        /// Multiplies the current value of <paramref name="o"/> by <paramref name="value"/>.
        /// </summary>
        /// <param name="o">A saturated number.</param>
        /// <param name="value">Value to multiply by.</param>
        /// <returns>A new instance which current value is the current value of <paramref name="o"/> multiplied by <paramref name="value"/>,
        ///          and having the same minimum and maximum values than <paramref name="o"/>.</returns>
        public static SaturatedUInt operator *( SaturatedUInt o, uint value )
        {
            checked
            {
                try
                {
                    uint newValue = o.m_value * value;
                    return new SaturatedUInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedUInt( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        /// <summary>
        /// Divides the current value of <paramref name="o"/> by <paramref name="value"/>.
        /// </summary>
        /// <param name="o">A saturated number.</param>
        /// <param name="value">Value to divide by.</param>
        /// <returns>A new instance which current value is the current value of <paramref name="o"/> divided by <paramref name="value"/>,
        ///          and having the same minimum and maximum values than <paramref name="o"/>.</returns>
        public static SaturatedUInt operator /( SaturatedUInt o, uint value )
        {
            uint newValue = o.m_value / value;
            return new SaturatedUInt( newValue, o.m_minimum, o.m_maximum );
        }

        /// <summary>
        /// Increments the current value of <paramref name="o"/> by <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to increment by.</param>
        /// <param name="o">A saturated number.</param>
        /// <returns>A new instance which current value is the current value of <paramref name="o"/> incremented by <paramref name="value"/>,
        ///          and having the same minimum and maximum values than <paramref name="o"/>.</returns>
        public static SaturatedUInt operator +( uint value, SaturatedUInt o )
        {
            checked
            {
                try
                {
                    uint newValue = value + o.m_value;
                    return new SaturatedUInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedUInt( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        /// <summary>
        /// Decrements <paramref name="value"/> by the current value of <paramref name="o"/>.
        /// </summary>
        /// <param name="value">Value to be decremented.</param>
        /// <param name="o">A saturated number.</param>
        /// <returns>A new instance which current value is <paramref name="value"/> decremented by the current value of <paramref name="o"/>,
        ///          and having the same minimum and maximum values than <paramref name="o"/>.</returns>
        public static SaturatedUInt operator -( uint value, SaturatedUInt o )
        {
            checked
            {
                try
                {
                    uint newValue = value - o.m_value;
                    return new SaturatedUInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedUInt( o.m_minimum, o.m_minimum, o.m_maximum );
                }
            }
        }

        /// <summary>
        /// Multiplies the current value of <paramref name="o"/> by <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Value to multiply by.</param>
        /// <param name="o">A saturated number.</param>
        /// <returns>A new instance which current value is the current value of <paramref name="o"/> multiplied by <paramref name="value"/>,
        ///          and having the same minimum and maximum values than <paramref name="o"/>.</returns>
        public static SaturatedUInt operator *( uint value, SaturatedUInt o )
        {
            checked
            {
                try
                {
                    uint newValue = value * o.m_value;
                    return new SaturatedUInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedUInt( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        /// <summary>
        /// Divides <paramref name="value"/> by the current value of <paramref name="o"/>.
        /// </summary>
        /// <param name="value">Value to be divided.</param>
        /// <param name="o">A saturated number.</param>
        /// <returns>A new instance which current value is <paramref name="value"/> divided by the current value of <paramref name="o"/>,
        ///          and having the same minimum and maximum values than <paramref name="o"/>.</returns>
        public static SaturatedUInt operator /( uint value, SaturatedUInt o )
        {
            uint newValue = value / o.m_value;
            return new SaturatedUInt( newValue, o.m_minimum, o.m_maximum );
        }

        /// <summary>
        /// Compares the current value of <paramref name="o"/> with <paramref name="value"/> for equality.
        /// </summary>
        /// <param name="o">A saturated number.</param>
        /// <param name="value">A value.</param>
        /// <returns><c>true</c> if the current value of <paramref name="o"/> and <paramref name="value"/> are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==( SaturatedUInt o, uint value )
        {
            return o.m_value == value;
        }

        /// <summary>
        /// Compares the current value of <paramref name="o"/> with <paramref name="value"/> for inequality.
        /// </summary>
        /// <param name="o">A saturated number.</param>
        /// <param name="value">A value.</param>
        /// <returns><c>true</c> if the current value of <paramref name="o"/> and <paramref name="value"/> are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=( SaturatedUInt o, uint value )
        {
            return o.m_value != value;
        }

        /// <summary>
        /// Compares the current value of <paramref name="o"/> with <paramref name="value"/> for inferiority.
        /// </summary>
        /// <param name="o">A saturated number.</param>
        /// <param name="value">A value.</param>
        /// <returns><c>true</c> if the current value of <paramref name="o"/> is less than <paramref name="value"/>, <c>false</c> otherwise.</returns>
        public static bool operator <( SaturatedUInt o, uint value )
        {
            return o.m_value < value;
        }

        /// <summary>
        /// Compares the current value of <paramref name="o"/> with <paramref name="value"/> for superiority.
        /// </summary>
        /// <param name="o">A saturated number.</param>
        /// <param name="value">A value.</param>
        /// <returns><c>true</c> if the current value of <paramref name="o"/> is greater than <paramref name="value"/>, <c>false</c> otherwise.</returns>
        public static bool operator >( SaturatedUInt o, uint value )
        {
            return o.m_value > value;
        }

        /// <summary>
        /// Compares the current value of <paramref name="o"/> with <paramref name="value"/> for equality.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="o">A saturated number.</param>
        /// <returns><c>true</c> if the current value of <paramref name="o"/> and <paramref name="value"/> are equal, <c>false</c> otherwise.</returns>
        public static bool operator ==( uint value, SaturatedUInt o )
        {
            return value == o.m_value;
        }

        /// <summary>
        /// Compares the current value of <paramref name="o"/> with <paramref name="value"/> for inequality.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="o">A saturated number.</param>
        /// <returns><c>true</c> if the current value of <paramref name="o"/> and <paramref name="value"/> are not equal, <c>false</c> otherwise.</returns>
        public static bool operator !=( uint value, SaturatedUInt o )
        {
            return value != o.m_value;
        }

        /// <summary>
        /// Compares <paramref name="value"/> with the current value of <paramref name="o"/> for inferiority.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="o">A saturated number.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is less than the current value of <paramref name="o"/>, <c>false</c> otherwise.</returns>
        public static bool operator <( uint value, SaturatedUInt o )
        {
            return value < o.m_value;
        }

        /// <summary>
        /// Compares <paramref name="value"/> with the current value of <paramref name="o"/> for superiority.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <param name="o">A saturated number.</param>
        /// <returns><c>true</c> if <paramref name="value"/> is greater than the current value of <paramref name="o"/>, <c>false</c> otherwise.</returns>
        public static bool operator >( uint value, SaturatedUInt o )
        {
            return value > o.m_value;
        }

        /// <summary>
        /// Compares the current value of this object with <paramref name="value"/> for equality.
        /// </summary>
        /// <param name="value">A value.</param>
        /// <returns><c>true</c> if the current value of this object and <paramref name="value"/> are equal, <c>false</c> otherwise.</returns>
        public bool Equals( uint value )
        {
            return m_value == value;
        }

        /// <summary>
        /// Compares this object with <paramref name="other"/> for equality.
        /// </summary>
        /// <remarks>
        /// Two SaturatedLong objects are equal if their current, minimum and maximum values are equal.
        /// </remarks>
        /// <param name="other">Another SaturatedLong.</param>
        /// <returns><c>true</c> if this object and <paramref name="other"/> are equal, <c>false</c> otherwise.</returns>
        public bool Equals( SaturatedUInt other )
        {
            return ( m_value == other.m_value ) &&
                   ( m_minimum == other.m_minimum ) &&
                   ( m_maximum == other.m_maximum );
        }

        /// <inheritdoc/>
        public override bool Equals( object? other )
        {
            if( other == null )
            {
                return false;
            }
            else if( other is SaturatedUInt o )
            {
                return Equals( o );
            }
            else if( other is uint i )
            {
                return Equals( i );
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = ( hash * 23 ) + m_value.GetHashCode();
                hash = ( hash * 23 ) + m_minimum.GetHashCode();
                hash = ( hash * 23 ) + m_maximum.GetHashCode();
                return hash;
            }
        }

        /// <inheritdoc/>
        public int CompareTo( uint value )
        {
            if( m_value > value )
            {
                return 1;
            }
            else if( m_value < value )
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return m_value.ToString();
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly uint m_value;
        private readonly uint m_minimum;
        private readonly uint m_maximum;
    }
}
