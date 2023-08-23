/// @file
/// @copyright  Copyright (c) 2022 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.Numbers
{
    /// <summary>
    /// Long integer number which value (and the result from arithmetic operations) are saturated between the given bounds.
    /// </summary>
    public readonly struct SaturatedLong : IEquatable<long>, IEquatable<SaturatedLong>, IComparable<long>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        public long Value => m_value;
        public long Minimum => m_minimum;
        public long Maximum => m_maximum;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other">Object to copy</param>
        public SaturatedLong( SaturatedLong other )
        {
            m_minimum = other.m_minimum;
            m_maximum = other.m_maximum;
            m_value = other.m_value;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="value">m_value</param>
        /// <param name="min">Bound minimum</param>
        /// <param name="max">Bound maximum</param>
        public SaturatedLong( long value, long min, long max )
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
        /// Constructor with bounds set to the whole long integer range.
        /// </summary>
        /// <param name="value">m_value</param>
        public SaturatedLong( long value ) : this( value, long.MinValue, long.MaxValue )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public static implicit operator long( SaturatedLong o )
        {
            return o.m_value;
        }

        public static SaturatedLong operator ++( SaturatedLong o )
        {
            checked
            {
                try
                {
                    long newValue = o.m_value + 1;
                    return new SaturatedLong( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedLong( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedLong operator --( SaturatedLong o )
        {
            checked
            {
                try
                {
                    long newValue = o.m_value - 1;
                    return new SaturatedLong( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedLong( o.m_minimum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedLong operator +( SaturatedLong o, long value )
        {
            checked
            {
                try
                {
                    long newValue = o.m_value + value;
                    return new SaturatedLong( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedLong( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedLong operator -( SaturatedLong o, long value )
        {
            checked
            {
                try
                {
                    long newValue = o.m_value - value;
                    return new SaturatedLong( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedLong( o.m_minimum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedLong operator *( SaturatedLong o, long value )
        {
            checked
            {
                try
                {
                    long newValue = o.m_value * value;
                    return new SaturatedLong( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedLong( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedLong operator /( SaturatedLong o, long value )
        {
            long newValue = o.m_value / value;
            return new SaturatedLong( newValue, o.m_minimum, o.m_maximum );
        }

        public static SaturatedLong operator +( long value, SaturatedLong o )
        {
            checked
            {
                try
                {
                    long newValue = value + o.m_value;
                    return new SaturatedLong( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedLong( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedLong operator -( long value, SaturatedLong o )
        {
            checked
            {
                try
                {
                    long newValue = value - o.m_value;
                    return new SaturatedLong( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedLong( o.m_minimum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedLong operator *( long value, SaturatedLong o )
        {
            checked
            {
                try
                {
                    long newValue = value * o.m_value;
                    return new SaturatedLong( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedLong( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedLong operator /( long value, SaturatedLong o )
        {
            long newValue = value / o.m_value;
            return new SaturatedLong( newValue, o.m_minimum, o.m_maximum );
        }

        public static bool operator ==( SaturatedLong o, long value )
        {
            return o.m_value == value;
        }

        public static bool operator !=( SaturatedLong o, long value )
        {
            return o.m_value != value;
        }

        public static bool operator <( SaturatedLong o, long value )
        {
            return o.m_value < value;
        }

        public static bool operator >( SaturatedLong o, long value )
        {
            return o.m_value > value;
        }

        public static bool operator ==( long value, SaturatedLong o )
        {
            return value == o.m_value;
        }

        public static bool operator !=( long value, SaturatedLong o )
        {
            return value != o.m_value;
        }

        public static bool operator <( long value, SaturatedLong o )
        {
            return value < o.m_value;
        }

        public static bool operator >( long value, SaturatedLong o )
        {
            return value > o.m_value;
        }

        public bool Equals( long other )
        {
            return m_value == other;
        }

        public bool Equals( SaturatedLong other )
        {
            return ( m_value == other.m_value ) &&
                   ( m_minimum == other.m_minimum ) &&
                   ( m_maximum == other.m_maximum );
        }

        public override bool Equals( object? other )
        {
            if( other == null )
            {
                return false;
            }
            else if( other is SaturatedLong o )
            {
                return Equals( o );
            }
            else if( other is long i )
            {
                return Equals( i );
            }
            else
            {
                return false;
            }
        }

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

        public int CompareTo( long value )
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

        public override string ToString()
        {
            return m_value.ToString();
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly long m_value;
        private readonly long m_minimum;
        private readonly long m_maximum;
    }
}
