/// @file
/// @copyright  Copyright (c) 2022 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.Net.Numbers
{
    /// <summary>
    /// Integer number which value (and the result from arithmetic operations) are saturated between the given bounds.
    /// </summary>
    public readonly struct SaturatedInt : IEquatable<int>, IEquatable<SaturatedInt>, IComparable<int>
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        public int Value => m_value;
        public int Minimum => m_minimum;
        public int Maximum => m_maximum;

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other">Object to copy</param>
        public SaturatedInt( SaturatedInt other )
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
        public SaturatedInt( int value, int min, int max )
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
        /// <param name="value">m_value</param>
        public SaturatedInt( int value ) : this( value, int.MinValue, int.MaxValue )
        {
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public static implicit operator int( SaturatedInt o )
        {
            return o.m_value;
        }

        public static SaturatedInt operator ++( SaturatedInt o )
        {
            checked
            {
                try
                {
                    int newValue = o.m_value + 1;
                    return new SaturatedInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedInt( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedInt operator --( SaturatedInt o )
        {
            checked
            {
                try
                {
                    int newValue = o.m_value - 1;
                    return new SaturatedInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedInt( o.m_minimum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedInt operator +( SaturatedInt o, int value )
        {
            checked
            {
                try
                {
                    int newValue = o.m_value + value;
                    return new SaturatedInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedInt( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedInt operator -( SaturatedInt o, int value )
        {
            checked
            {
                try
                {
                    int newValue = o.m_value - value;
                    return new SaturatedInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedInt( o.m_minimum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedInt operator *( SaturatedInt o, int value )
        {
            checked
            {
                try
                {
                    int newValue = o.m_value * value;
                    return new SaturatedInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedInt( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedInt operator /( SaturatedInt o, int value )
        {
            int newValue = o.m_value / value;
            return new SaturatedInt( newValue, o.m_minimum, o.m_maximum );
        }

        public static SaturatedInt operator +( int value, SaturatedInt o )
        {
            checked
            {
                try
                {
                    int newValue = value + o.m_value;
                    return new SaturatedInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedInt( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedInt operator -( int value, SaturatedInt o )
        {
            checked
            {
                try
                {
                    int newValue = value - o.m_value;
                    return new SaturatedInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedInt( o.m_minimum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedInt operator *( int value, SaturatedInt o )
        {
            checked
            {
                try
                {
                    int newValue = value * o.m_value;
                    return new SaturatedInt( newValue, o.m_minimum, o.m_maximum );
                }
                catch( Exception )
                {
                    return new SaturatedInt( o.m_maximum, o.m_minimum, o.m_maximum );
                }
            }
        }

        public static SaturatedInt operator /( int value, SaturatedInt o )
        {
            int newValue = value / o.m_value;
            return new SaturatedInt( newValue, o.m_minimum, o.m_maximum );
        }

        public static bool operator ==( SaturatedInt o, int value )
        {
            return o.m_value == value;
        }

        public static bool operator !=( SaturatedInt o, int value )
        {
            return o.m_value != value;
        }

        public static bool operator <( SaturatedInt o, int value )
        {
            return o.m_value < value;
        }

        public static bool operator >( SaturatedInt o, int value )
        {
            return o.m_value > value;
        }

        public static bool operator ==( int value, SaturatedInt o )
        {
            return value == o.m_value;
        }

        public static bool operator !=( int value, SaturatedInt o )
        {
            return value != o.m_value;
        }

        public static bool operator <( int value, SaturatedInt o )
        {
            return value < o.m_value;
        }

        public static bool operator >( int value, SaturatedInt o )
        {
            return value > o.m_value;
        }

        public bool Equals( int other )
        {
            return m_value == other;
        }

        public bool Equals( SaturatedInt other )
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
            else if( other is SaturatedInt o )
            {
                return Equals( o );
            }
            else if( other is int i )
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

        public int CompareTo( int value )
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

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly int m_value;
        private readonly int m_minimum;
        private readonly int m_maximum;
    }
}
