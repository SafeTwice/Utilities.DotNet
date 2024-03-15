/// @file
/// @copyright  Copyright (c) 2022 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using Utilities.DotNet.Numbers;
using Xunit;

namespace Utilities.DotNet.Test.Numbers
{
    public class SaturatedULongTest
    {
        [Fact]
        public void ConstructorBetweenBounds()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );

            Assert.Equal( 50UL, sn1.Value );
            Assert.Equal( 25UL, sn1.Minimum );
            Assert.Equal( 100UL, sn1.Maximum );
        }

        [Fact]
        public void ConstructorLowerThanMinimum()
        {
            var sn1 = new SaturatedULong( 5UL, 25UL, 100UL );

            Assert.Equal( 25UL, sn1.Value );
            Assert.Equal( 25UL, sn1.Minimum );
            Assert.Equal( 100UL, sn1.Maximum );
        }

        [Fact]
        public void ConstructorGreaterThanMaximum()
        {
            var sn1 = new SaturatedULong( 8000UL, 25UL, 100UL );

            Assert.Equal( 100UL, sn1.Value );
            Assert.Equal( 25UL, sn1.Minimum );
            Assert.Equal( 100UL, sn1.Maximum );
        }

        [Fact]
        public void ConstructorInvalidBounds()
        {
            Assert.Throws<ArgumentException>( () => new SaturatedULong( 50UL, 125UL, 100UL ) );
        }

        [Fact]
        public void ConstructorLimitBounds()
        {
            var sn1 = new SaturatedULong( 500UL );

            Assert.Equal( 500UL, sn1.Value );
            Assert.Equal( ulong.MinValue, sn1.Minimum );
            Assert.Equal( ulong.MaxValue, sn1.Maximum );
        }

        [Fact]
        public void ConstructorFromOther()
        {
            var sn1 = new SaturatedULong( 18000UL, 9000UL, 800000UL );
            var sn2 = new SaturatedULong( sn1 );

            Assert.Equal( 18000UL, sn2.Value );
            Assert.Equal( 9000UL, sn2.Minimum );
            Assert.Equal( 800000UL, sn2.Maximum );
        }

        [Fact]
        public void ImplicitConversionToLong()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );
            ulong value = sn1;

            Assert.Equal( 50UL, value );
            Assert.Equal( 25UL, sn1.Minimum );
            Assert.Equal( 100UL, sn1.Maximum );
        }

        [Fact]
        public void IncrementNoSaturation()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );
            sn1++;

            Assert.Equal( 51UL, sn1.Value );
            Assert.Equal( 25UL, sn1.Minimum );
            Assert.Equal( 100UL, sn1.Maximum );
        }

        [Fact]
        public void DecrementNoSaturation()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );
            sn1--;

            Assert.Equal( 49UL, sn1.Value );
            Assert.Equal( 25UL, sn1.Minimum );
            Assert.Equal( 100UL, sn1.Maximum );
        }

        [Fact]
        public void AdditionNoSaturationPre()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );
            var sn2 = sn1 + 25UL;

            Assert.Equal( 75UL, sn2.Value );
            Assert.Equal( 25UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void AdditionNoSaturationPost()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );
            var sn2 = 35UL + sn1;

            Assert.Equal( 85UL, sn2.Value );
            Assert.Equal( 25UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void SubtractionNoSaturationPre()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );
            var sn2 = sn1 - 5UL;

            Assert.Equal( 45UL, sn2.Value );
            Assert.Equal( 25UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void SubtractionNoSaturationPost()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );
            var sn2 = 90UL - sn1;

            Assert.Equal( 40UL, sn2.Value );
            Assert.Equal( 25UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationNoSaturationPre()
        {
            var sn1 = new SaturatedULong( 10UL, 5UL, 100UL );
            var sn2 = sn1 * 6UL;

            Assert.Equal( 60UL, sn2.Value );
            Assert.Equal( 5UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationNoSaturationPost()
        {
            var sn1 = new SaturatedULong( 15UL, 10UL, 100UL );
            var sn2 = 3UL * sn1;

            Assert.Equal( 45UL, sn2.Value );
            Assert.Equal( 10UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void DivisionNoSaturationPre()
        {
            var sn1 = new SaturatedULong( 80UL, 15UL, 100UL );
            var sn2 = sn1 / 4UL;

            Assert.Equal( 20UL, sn2.Value );
            Assert.Equal( 15UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void DivisionNoSaturationPost()
        {
            var sn1 = new SaturatedULong( 3UL, 0UL, 100UL );
            var sn2 = 150UL / sn1;

            Assert.Equal( 50UL, sn2.Value );
            Assert.Equal( 0UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void IncrementSaturation()
        {
            var sn1 = new SaturatedULong( 100UL, 25UL, 100UL );
            sn1++;

            Assert.Equal( 100UL, sn1.Value );
            Assert.Equal( 25UL, sn1.Minimum );
            Assert.Equal( 100UL, sn1.Maximum );
        }

        [Fact]
        public void DecrementSaturation()
        {
            var sn1 = new SaturatedULong( 25UL, 25UL, 100UL );
            sn1--;

            Assert.Equal( 25UL, sn1.Value );
            Assert.Equal( 25UL, sn1.Minimum );
            Assert.Equal( 100UL, sn1.Maximum );
        }

        [Fact]
        public void AdditionSaturationPre()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );
            var sn2 = sn1 + 25000UL;

            Assert.Equal( 100UL, sn2.Value );
            Assert.Equal( 25UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void AdditionSaturationPost()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );
            var sn2 = 35000UL + sn1;

            Assert.Equal( 100UL, sn2.Value );
            Assert.Equal( 25UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void SubtractionSaturationPre()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );
            var sn2 = sn1 - 500UL;

            Assert.Equal( 25UL, sn2.Value );
            Assert.Equal( 25UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void SubtractionSaturationPost()
        {
            var sn1 = new SaturatedULong( 80UL, 25UL, 100UL );
            var sn2 = 90UL - sn1;

            Assert.Equal( 25UL, sn2.Value );
            Assert.Equal( 25UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationSaturationPre()
        {
            var sn1 = new SaturatedULong( 10UL, 5UL, 100UL );
            var sn2 = sn1 * 60UL;

            Assert.Equal( 100UL, sn2.Value );
            Assert.Equal( 5UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationSaturationPost()
        {
            var sn1 = new SaturatedULong( 15UL, 10UL, 100UL );
            var sn2 = 300UL * sn1;

            Assert.Equal( 100UL, sn2.Value );
            Assert.Equal( 10UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void DivisionSaturationPre()
        {
            var sn1 = new SaturatedULong( 80UL, 15UL, 100UL );
            var sn2 = sn1 / 8UL;

            Assert.Equal( 15UL, sn2.Value );
            Assert.Equal( 15UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void DivisionSaturationPost()
        {
            var sn1 = new SaturatedULong( 30UL, 25UL, 100UL );
            var sn2 = 150UL / sn1;

            Assert.Equal( 25UL, sn2.Value );
            Assert.Equal( 25UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void IncrementOverflow()
        {
            var sn1 = new SaturatedULong( ulong.MaxValue );
            sn1++;

            Assert.Equal( ulong.MaxValue, sn1.Value );
            Assert.Equal( ulong.MinValue, sn1.Minimum );
            Assert.Equal( ulong.MaxValue, sn1.Maximum );
        }

        [Fact]
        public void DecrementOverflow()
        {
            var sn1 = new SaturatedULong( ulong.MinValue );
            sn1--;

            Assert.Equal( ulong.MinValue, sn1.Value );
            Assert.Equal( ulong.MinValue, sn1.Minimum );
            Assert.Equal( ulong.MaxValue, sn1.Maximum );
        }

        [Fact]
        public void AdditionOverflowPre()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );
            var sn2 = sn1 + ulong.MaxValue;

            Assert.Equal( 100UL, sn2.Value );
            Assert.Equal( 25UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void AdditionOverflowPost()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );
            var sn2 = ulong.MaxValue + sn1;

            Assert.Equal( 100UL, sn2.Value );
            Assert.Equal( 25UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void SubtractionOverflowPre()
        {
            var sn1 = new SaturatedULong( 50UL, 25UL, 100UL );
            var sn2 = sn1 - sn1.Maximum;

            Assert.Equal( 25UL, sn2.Value );
            Assert.Equal( 25UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void SubtractionOverflowPost()
        {
            var sn1 = new SaturatedULong( ulong.MaxValue, 500UL, ulong.MaxValue );
            var sn2 = 500UL - sn1;

            Assert.Equal( 500UL, sn2.Value );
            Assert.Equal( 500UL, sn2.Minimum );
            Assert.Equal( ulong.MaxValue, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPre()
        {
            var sn1 = new SaturatedULong( 10UL, 5UL, 100UL );
            var sn2 = sn1 * ( ulong.MaxValue / 2UL );

            Assert.Equal( 100UL, sn2.Value );
            Assert.Equal( 5UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPost()
        {
            var sn1 = new SaturatedULong( 15UL, 10UL, 100UL );
            var sn2 = ( ulong.MaxValue / 2UL ) * sn1;

            Assert.Equal( 100UL, sn2.Value );
            Assert.Equal( 10UL, sn2.Minimum );
            Assert.Equal( 100UL, sn2.Maximum );
        }

        [Fact]
        public void ComparisonOperatorsPre()
        {
            var sn1 = new SaturatedULong( 15UL, 10UL, 100UL );

            Assert.True( sn1 == 15UL );
            Assert.False( sn1 == 16UL );

            Assert.False( sn1 != 15UL );
            Assert.True( sn1 != 14UL );

            Assert.True( sn1 < 25UL );
            Assert.True( sn1 <= 16UL );
            Assert.True( sn1 <= 15UL );
            Assert.False( sn1 <= 14UL );
            Assert.False( sn1 < 5UL );

            Assert.True( sn1 > 5UL );
            Assert.True( sn1 >= 14UL );
            Assert.True( sn1 >= 15UL );
            Assert.False( sn1 >= 16UL );
            Assert.False( sn1 > 25UL );
        }

        [Fact]
        public void ComparisonOperatorsPost()
        {
            var sn1 = new SaturatedULong( 15UL, 10UL, 100UL );

            Assert.True( 15UL == sn1 );
            Assert.False( 14UL == sn1 );

            Assert.False( 15UL != sn1 );
            Assert.True( 14UL != sn1 );

            Assert.False( 25UL < sn1 );
            Assert.False( 16UL <= sn1 );
            Assert.True( 15UL <= sn1 );
            Assert.True( 14UL <= sn1 );
            Assert.True( 5UL < sn1 );

            Assert.False( 5UL > sn1 );
            Assert.False( 14UL >= sn1 );
            Assert.True( 15UL >= sn1 );
            Assert.True( 16UL >= sn1 );
            Assert.True( 25UL > sn1 );
        }

        [Fact]
        public void Equality()
        {
            var sn1 = new SaturatedULong( 15UL, 10UL, 100UL );
            var sn2 = new SaturatedULong( 25UL, 10UL, 100UL );
            var sn3 = new SaturatedULong( 15UL, 5UL, 100UL );
            var sn4 = new SaturatedULong( 15UL, 10UL, 150UL );
            var sn5 = new SaturatedULong( 15UL, 10UL, 100UL );

            Assert.True( sn1.Equals( sn1 ) );
            Assert.True( sn1.Equals( sn5 ) );
            Assert.False( sn1.Equals( sn2 ) );
            Assert.False( sn1.Equals( sn3 ) );
            Assert.False( sn1.Equals( sn4 ) );

            Assert.True( sn1.Equals( 15UL ) );
            Assert.False( sn1.Equals( 20UL ) );

            Assert.True( sn1.Equals( (object) 15UL ) );
            Assert.False( sn1.Equals( (object) 20UL ) );

            Assert.True( sn1.Equals( (object) sn5 ) );
            Assert.False( sn1.Equals( (object) sn2 ) );

            Assert.False( sn1.Equals( null ) );

            Assert.False( sn1.Equals( 15.0d ) );

            Assert.Equal( sn1.GetHashCode(), sn5.GetHashCode() );
            Assert.NotEqual( sn1.GetHashCode(), sn2.GetHashCode() );
            Assert.NotEqual( sn1.GetHashCode(), sn3.GetHashCode() );
            Assert.NotEqual( sn1.GetHashCode(), sn4.GetHashCode() );
        }

        [Fact]
        public void CompareTo()
        {
            var sn1 = new SaturatedULong( 15UL, 10UL, 100UL );

            Assert.True( sn1.CompareTo( 14UL ) > 0 );
            Assert.True( sn1.CompareTo( 15UL ) == 0 );
            Assert.True( sn1.CompareTo( 16UL ) < 0 );
        }

        [Fact]
        public void ToStringConversion()
        {
            var sn1 = new SaturatedULong( 15UL, 10UL, 100UL );
            var sn2 = new SaturatedULong( 15000UL );

            Assert.Equal( "15", sn1.ToString() );
            Assert.Equal( "15000", sn2.ToString() );
        }
    }
}