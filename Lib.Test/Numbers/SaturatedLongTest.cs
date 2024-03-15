/// @file
/// @copyright  Copyright (c) 2022 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using Utilities.DotNet.Numbers;
using Xunit;

namespace Utilities.DotNet.Test.Numbers
{
    public class SaturatedLongTest
    {
        [Fact]
        public void ConstructorBetweenBounds()
        {
            var sn1 = new SaturatedLong( 50L, -25L, 100L );

            Assert.Equal( 50L, sn1.Value );
            Assert.Equal( -25L, sn1.Minimum );
            Assert.Equal( 100L, sn1.Maximum );
        }

        [Fact]
        public void ConstructorLowerThanMinimum()
        {
            var sn1 = new SaturatedLong( -25L, -5L, 100L );

            Assert.Equal( -5L, sn1.Value );
            Assert.Equal( -5L, sn1.Minimum );
            Assert.Equal( 100L, sn1.Maximum );
        }

        [Fact]
        public void ConstructorGreaterThanMaximum()
        {
            var sn1 = new SaturatedLong( 8000L, -125L, -100L );

            Assert.Equal( -100L, sn1.Value );
            Assert.Equal( -125L, sn1.Minimum );
            Assert.Equal( -100L, sn1.Maximum );
        }

        [Fact]
        public void ConstructorInvalidBounds()
        {
            Assert.Throws<ArgumentException>( () => new SaturatedLong( 50L, -100L, -125L ) );
        }

        [Fact]
        public void ConstructorLimitBounds()
        {
            var sn1 = new SaturatedLong( 500L );

            Assert.Equal( 500L, sn1.Value );
            Assert.Equal( long.MinValue, sn1.Minimum );
            Assert.Equal( long.MaxValue, sn1.Maximum );
        }

        [Fact]
        public void ConstructorFromOther()
        {
            var sn1 = new SaturatedLong( 18000L, -9000L, 800000L );
            var sn2 = new SaturatedLong( sn1 );

            Assert.Equal( 18000L, sn2.Value );
            Assert.Equal( -9000L, sn2.Minimum );
            Assert.Equal( 800000L, sn2.Maximum );
        }

        [Fact]
        public void ImplicitConversionToLong()
        {
            var sn1 = new SaturatedLong( 50L, -25L, 100L );
            long value = sn1;

            Assert.Equal( 50L, value );
            Assert.Equal( -25L, sn1.Minimum );
            Assert.Equal( 100L, sn1.Maximum );
        }

        [Fact]
        public void IncrementNoSaturation()
        {
            var sn1 = new SaturatedLong( 50L, -25L, 100L );
            sn1++;

            Assert.Equal( 51L, sn1.Value );
            Assert.Equal( -25L, sn1.Minimum );
            Assert.Equal( 100L, sn1.Maximum );
        }

        [Fact]
        public void DecrementNoSaturation()
        {
            var sn1 = new SaturatedLong( 50L, -25L, 100L );
            sn1--;

            Assert.Equal( 49L, sn1.Value );
            Assert.Equal( -25L, sn1.Minimum );
            Assert.Equal( 100L, sn1.Maximum );
        }

        [Fact]
        public void AdditionNoSaturationPre()
        {
            var sn1 = new SaturatedLong( 50L, -25L, 100L );
            var sn2 = sn1 + 25L;

            Assert.Equal( 75L, sn2.Value );
            Assert.Equal( -25L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void AdditionNoSaturationPost()
        {
            var sn1 = new SaturatedLong( 50L, -25L, 100L );
            var sn2 = 35L + sn1;

            Assert.Equal( 85L, sn2.Value );
            Assert.Equal( -25L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void SubtractionNoSaturationPre()
        {
            var sn1 = new SaturatedLong( 50L, -25L, 100L );
            var sn2 = sn1 - 5L;

            Assert.Equal( 45L, sn2.Value );
            Assert.Equal( -25L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void SubtractionNoSaturationPost()
        {
            var sn1 = new SaturatedLong( 50L, -25L, 100L );
            var sn2 = 30L - sn1;

            Assert.Equal( -20L, sn2.Value );
            Assert.Equal( -25L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationNoSaturationPre()
        {
            var sn1 = new SaturatedLong( 10L, -5L, 100L );
            var sn2 = sn1 * 6L;

            Assert.Equal( 60L, sn2.Value );
            Assert.Equal( -5L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationNoSaturationPost()
        {
            var sn1 = new SaturatedLong( 15L, -10L, 100L );
            var sn2 = 3L * sn1;

            Assert.Equal( 45L, sn2.Value );
            Assert.Equal( -10L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void DivisionNoSaturationPre()
        {
            var sn1 = new SaturatedLong( 80L, 15L, 100L );
            var sn2 = sn1 / 4L;

            Assert.Equal( 20L, sn2.Value );
            Assert.Equal( 15L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void DivisionNoSaturationPost()
        {
            var sn1 = new SaturatedLong( 3L, 0L, 100L );
            var sn2 = 150L / sn1;

            Assert.Equal( 50L, sn2.Value );
            Assert.Equal( 0L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void IncrementSaturation()
        {
            var sn1 = new SaturatedLong( 100L, -25L, 100L );
            sn1++;

            Assert.Equal( 100L, sn1.Value );
            Assert.Equal( -25L, sn1.Minimum );
            Assert.Equal( 100L, sn1.Maximum );
        }

        [Fact]
        public void DecrementSaturation()
        {
            var sn1 = new SaturatedLong( -25L, -25L, 100L );
            sn1--;

            Assert.Equal( -25L, sn1.Value );
            Assert.Equal( -25L, sn1.Minimum );
            Assert.Equal( 100L, sn1.Maximum );
        }

        [Fact]
        public void AdditionSaturationPrePositive()
        {
            var sn1 = new SaturatedLong( 50L, -25L, 100L );
            var sn2 = sn1 + 25000L;

            Assert.Equal( 100L, sn2.Value );
            Assert.Equal( -25L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void AdditionSaturationPreNegative()
        {
            var bn = -25000L;
            var sn1 = new SaturatedLong( 50L, -25L, 100L );
            var sn2 = sn1 + bn;

            Assert.Equal( -25L, sn2.Value );
            Assert.Equal( -25L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void AdditionSaturationPostPositive()
        {
            var sn1 = new SaturatedLong( 50L, 25L, 100L );
            var sn2 = 35000L + sn1;

            Assert.Equal( 100L, sn2.Value );
            Assert.Equal( 25L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void AdditionSaturationPostNegative()
        {
            var sn1 = new SaturatedLong( -50L, -100L, 100L );
            var sn2 = -60L + sn1;

            Assert.Equal( -100L, sn2.Value );
            Assert.Equal( -100L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void SubtractionSaturationPre()
        {
            var sn1 = new SaturatedLong( 50L, 25L, 100L );
            var sn2 = sn1 - 500L;

            Assert.Equal( 25L, sn2.Value );
            Assert.Equal( 25L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void SubtractionSaturationPost()
        {
            var sn1 = new SaturatedLong( 80L, 25L, 100L );
            var sn2 = 90L - sn1;

            Assert.Equal( 25L, sn2.Value );
            Assert.Equal( 25L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationSaturationPre()
        {
            var sn1 = new SaturatedLong( 10L, 5L, 100L );
            var sn2 = sn1 * 60L;

            Assert.Equal( 100L, sn2.Value );
            Assert.Equal( 5L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationSaturationPost()
        {
            var sn1 = new SaturatedLong( 15L, 10L, 100L );
            var sn2 = 300L * sn1;

            Assert.Equal( 100L, sn2.Value );
            Assert.Equal( 10L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void DivisionSaturationPre()
        {
            var sn1 = new SaturatedLong( 80L, 15L, 100L );
            var sn2 = sn1 / 8L;

            Assert.Equal( 15L, sn2.Value );
            Assert.Equal( 15L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void DivisionSaturationPost()
        {
            var sn1 = new SaturatedLong( 30L, 25L, 100L );
            var sn2 = 150L / sn1;

            Assert.Equal( 25L, sn2.Value );
            Assert.Equal( 25L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void IncrementOverflow()
        {
            var sn1 = new SaturatedLong( long.MaxValue );
            sn1++;

            Assert.Equal( long.MaxValue, sn1.Value );
            Assert.Equal( long.MinValue, sn1.Minimum );
            Assert.Equal( long.MaxValue, sn1.Maximum );
        }

        [Fact]
        public void DecrementOverflow()
        {
            var sn1 = new SaturatedLong( long.MinValue );
            sn1--;

            Assert.Equal( long.MinValue, sn1.Value );
            Assert.Equal( long.MinValue, sn1.Minimum );
            Assert.Equal( long.MaxValue, sn1.Maximum );
        }

        [Fact]
        public void AdditionOverflowPrePositive()
        {
            var sn1 = new SaturatedLong( 50L, -125L, 100L );
            var sn2 = sn1 + long.MaxValue;

            Assert.Equal( 100L, sn2.Value );
            Assert.Equal( -125L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void AdditionOverflowPreNegative()
        {
            var sn1 = new SaturatedLong( -50L, -100L, 100L );
            var sn2 = sn1 + long.MinValue;

            Assert.Equal( -100L, sn2.Value );
            Assert.Equal( -100L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void AdditionOverflowPostPositive()
        {
            var sn1 = new SaturatedLong( 50L, -25L, 100L );
            var sn2 = long.MaxValue + sn1;

            Assert.Equal( 100L, sn2.Value );
            Assert.Equal( -25L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void AdditionOverflowPostNegative()
        {
            var sn1 = new SaturatedLong( -50L, -125L, 100L );
            var sn2 = long.MinValue + sn1;

            Assert.Equal( -125L, sn2.Value );
            Assert.Equal( -125L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void SubtractionOverflowPrePositive()
        {
            var sn1 = new SaturatedLong( 50L, -125L, 100L );
            var sn2 = sn1 - long.MinValue;

            Assert.Equal( 100L, sn2.Value );
            Assert.Equal( -125L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void SubtractionOverflowPreNegative()
        {
            var sn1 = new SaturatedLong( -50L, -125L, 100L );
            var sn2 = sn1 - long.MaxValue;

            Assert.Equal( -125L, sn2.Value );
            Assert.Equal( -125L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );
        }

        [Fact]
        public void SubtractionOverflowPostPositive()
        {
            var sn1 = new SaturatedLong( long.MinValue, long.MinValue, -899L );
            var sn2 = 500L - sn1;

            Assert.Equal( -899L, sn2.Value );
            Assert.Equal( long.MinValue, sn2.Minimum );
            Assert.Equal( -899L, sn2.Maximum );
        }

        [Fact]
        public void SubtractionOverflowPostNegative()
        {
            var sn1 = new SaturatedLong( long.MaxValue, 500L, long.MaxValue );
            var sn2 = -500L - sn1;

            Assert.Equal( 500L, sn2.Value );
            Assert.Equal( 500L, sn2.Minimum );
            Assert.Equal( long.MaxValue, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPrePositive()
        {
            var sn1 = new SaturatedLong( 10L, -5L, 100L );
            var sn2 = sn1 * ( long.MaxValue / 2L );

            var sn3 = new SaturatedLong( -10L, -50L, 100L );
            var sn4 = sn3 * ( long.MinValue / 2L );

            Assert.Equal( 100L, sn2.Value );
            Assert.Equal( -5L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );

            Assert.Equal( 100L, sn4.Value );
            Assert.Equal( -50L, sn4.Minimum );
            Assert.Equal( 100L, sn4.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPreNegative()
        {
            var sn1 = new SaturatedLong( -10L, -50L, 100L );
            var sn2 = sn1 * ( long.MaxValue / 2L );

            var sn3 = new SaturatedLong( 10L, -50L, 100L );
            var sn4 = sn3 * ( long.MinValue / 2L );

            Assert.Equal( -50L, sn2.Value );
            Assert.Equal( -50L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );

            Assert.Equal( -50L, sn4.Value );
            Assert.Equal( -50L, sn4.Minimum );
            Assert.Equal( 100L, sn4.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPostPositive()
        {
            var sn1 = new SaturatedLong( 15L, -100L, 100L );
            var sn2 = ( long.MaxValue / 2L ) * sn1;

            var sn3 = new SaturatedLong( -15L, -100L, 100L );
            var sn4 = ( long.MinValue / 2L ) * sn3;

            Assert.Equal( 100L, sn2.Value );
            Assert.Equal( -100L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );

            Assert.Equal( 100L, sn4.Value );
            Assert.Equal( -100L, sn4.Minimum );
            Assert.Equal( 100L, sn4.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPostNegative()
        {
            var sn1 = new SaturatedLong( 15L, -100L, 100L );
            var sn2 = ( long.MinValue / 2L ) * sn1;

            var sn3 = new SaturatedLong( -15L, -100L, 100L );
            var sn4 = ( long.MaxValue / 2L ) * sn3;

            Assert.Equal( -100L, sn2.Value );
            Assert.Equal( -100L, sn2.Minimum );
            Assert.Equal( 100L, sn2.Maximum );

            Assert.Equal( -100L, sn4.Value );
            Assert.Equal( -100L, sn4.Minimum );
            Assert.Equal( 100L, sn4.Maximum );
        }

        [Fact]
        public void ComparisonOperatorsPre()
        {
            var sn1 = new SaturatedLong( 15L, 10L, 100L );

            Assert.True( sn1 == 15L );
            Assert.False( sn1 == 16L );

            Assert.False( sn1 != 15L );
            Assert.True( sn1 != 14L );

            Assert.True( sn1 < 25L );
            Assert.True( sn1 <= 16L );
            Assert.True( sn1 <= 15L );
            Assert.False( sn1 <= 14L );
            Assert.False( sn1 < 5L );

            Assert.True( sn1 > 5L );
            Assert.True( sn1 >= 14L );
            Assert.True( sn1 >= 15L );
            Assert.False( sn1 >= 16L );
            Assert.False( sn1 > 25L );
        }

        [Fact]
        public void ComparisonOperatorsPost()
        {
            var sn1 = new SaturatedLong( 15L, 10L, 100L );

            Assert.True( 15L == sn1 );
            Assert.False( 14L == sn1 );

            Assert.False( 15L != sn1 );
            Assert.True( 14L != sn1 );

            Assert.False( 25L < sn1 );
            Assert.False( 16L <= sn1 );
            Assert.True( 15L <= sn1 );
            Assert.True( 14L <= sn1 );
            Assert.True( 5L < sn1 );

            Assert.False( 5L > sn1 );
            Assert.False( 14L >= sn1 );
            Assert.True( 15L >= sn1 );
            Assert.True( 16L >= sn1 );
            Assert.True( 25L > sn1 );
        }

        [Fact]
        public void Equality()
        {
            var sn1 = new SaturatedLong( 15L, 10L, 100L );
            var sn2 = new SaturatedLong( 25L, 10L, 100L );
            var sn3 = new SaturatedLong( 15L, 5L, 100L );
            var sn4 = new SaturatedLong( 15L, 10L, 150L );
            var sn5 = new SaturatedLong( 15L, 10L, 100L );

            Assert.True( sn1.Equals( sn1 ) );
            Assert.True( sn1.Equals( sn5 ) );
            Assert.False( sn1.Equals( sn2 ) );
            Assert.False( sn1.Equals( sn3 ) );
            Assert.False( sn1.Equals( sn4 ) );

            Assert.True( sn1.Equals( 15L ) );
            Assert.False( sn1.Equals( 20L ) );

            Assert.True( sn1.Equals( (object) 15L ) );
            Assert.False( sn1.Equals( (object) 20L ) );

            Assert.True( sn1.Equals( (object) sn5 ) );
            Assert.False( sn1.Equals( (object) sn2 ) );

            Assert.False( sn1.Equals( null ) );

            Assert.False( sn1.Equals( 15.0 ) );

            Assert.Equal( sn1.GetHashCode(), sn5.GetHashCode() );
            Assert.NotEqual( sn1.GetHashCode(), sn2.GetHashCode() );
            Assert.NotEqual( sn1.GetHashCode(), sn3.GetHashCode() );
            Assert.NotEqual( sn1.GetHashCode(), sn4.GetHashCode() );
        }

        [Fact]
        public void CompareTo()
        {
            var sn1 = new SaturatedLong( 15L, 10L, 100L );

            Assert.True( sn1.CompareTo( 14L ) > 0 );
            Assert.True( sn1.CompareTo( 15L ) == 0 );
            Assert.True( sn1.CompareTo( 16L ) < 0 );
        }

        [Fact]
        public void ToStringConversion()
        {
            var sn1 = new SaturatedLong( 15L, 10L, 100L );
            var sn2 = new SaturatedLong( 15000L );

            Assert.Equal( "15", sn1.ToString() );
            Assert.Equal( "15000", sn2.ToString() );
        }
    }
}