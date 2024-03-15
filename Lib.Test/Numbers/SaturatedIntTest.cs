/// @file
/// @copyright  Copyright (c) 2022 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using Utilities.DotNet.Numbers;
using Xunit;

namespace Utilities.DotNet.Test.Numbers
{
    public class SaturatedIntTest
    {
        [Fact]
        public void ConstructorBetweenBounds()
        {
            var sn1 = new SaturatedInt( 50, -25, 100 );

            Assert.Equal( 50, sn1.Value );
            Assert.Equal( -25, sn1.Minimum );
            Assert.Equal( 100, sn1.Maximum );
        }

        [Fact]
        public void ConstructorLowerThanMinimum()
        {
            var sn1 = new SaturatedInt( -25, -5, 100 );

            Assert.Equal( -5, sn1.Value );
            Assert.Equal( -5, sn1.Minimum );
            Assert.Equal( 100, sn1.Maximum );
        }

        [Fact]
        public void ConstructorGreaterThanMaximum()
        {
            var sn1 = new SaturatedInt( 8000, -125, -100 );

            Assert.Equal( -100, sn1.Value );
            Assert.Equal( -125, sn1.Minimum );
            Assert.Equal( -100, sn1.Maximum );
        }

        [Fact]
        public void ConstructorInvalidBounds()
        {
            Assert.Throws<ArgumentException>( () => new SaturatedInt( 50, -100, -125 ) );
        }

        [Fact]
        public void ConstructorLimitBounds()
        {
            var sn1 = new SaturatedInt( 500 );

            Assert.Equal( 500, sn1.Value );
            Assert.Equal( int.MinValue, sn1.Minimum );
            Assert.Equal( int.MaxValue, sn1.Maximum );
        }

        [Fact]
        public void ConstructorFromOther()
        {
            var sn1 = new SaturatedInt( 18000, -9000, 800000 );
            var sn2 = new SaturatedInt( sn1 );

            Assert.Equal( 18000, sn2.Value );
            Assert.Equal( -9000, sn2.Minimum );
            Assert.Equal( 800000, sn2.Maximum );
        }

        [Fact]
        public void ImplicitConversionToLong()
        {
            var sn1 = new SaturatedInt( 50, -25, 100 );
            long value = sn1;

            Assert.Equal( 50, value );
            Assert.Equal( -25, sn1.Minimum );
            Assert.Equal( 100, sn1.Maximum );
        }

        [Fact]
        public void IncrementNoSaturation()
        {
            var sn1 = new SaturatedInt( 50, -25, 100 );
            sn1++;

            Assert.Equal( 51, sn1.Value );
            Assert.Equal( -25, sn1.Minimum );
            Assert.Equal( 100, sn1.Maximum );
        }

        [Fact]
        public void DecrementNoSaturation()
        {
            var sn1 = new SaturatedInt( 50, -25, 100 );
            sn1--;

            Assert.Equal( 49, sn1.Value );
            Assert.Equal( -25, sn1.Minimum );
            Assert.Equal( 100, sn1.Maximum );
        }

        [Fact]
        public void AdditionNoSaturationPre()
        {
            var sn1 = new SaturatedInt( 50, -25, 100 );
            var sn2 = sn1 + 25;

            Assert.Equal( 75, sn2.Value );
            Assert.Equal( -25, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void AdditionNoSaturationPost()
        {
            var sn1 = new SaturatedInt( 50, -25, 100 );
            var sn2 = 35 + sn1;

            Assert.Equal( 85, sn2.Value );
            Assert.Equal( -25, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void SubtractionNoSaturationPre()
        {
            var sn1 = new SaturatedInt( 50, -25, 100 );
            var sn2 = sn1 - 5;

            Assert.Equal( 45, sn2.Value );
            Assert.Equal( -25, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void SubtractionNoSaturationPost()
        {
            var sn1 = new SaturatedInt( 50, -25, 100 );
            var sn2 = 30 - sn1;

            Assert.Equal( -20, sn2.Value );
            Assert.Equal( -25, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationNoSaturationPre()
        {
            var sn1 = new SaturatedInt( 10, -5, 100 );
            var sn2 = sn1 * 6;

            Assert.Equal( 60, sn2.Value );
            Assert.Equal( -5, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationNoSaturationPost()
        {
            var sn1 = new SaturatedInt( 15, -10, 100 );
            var sn2 = 3 * sn1;

            Assert.Equal( 45, sn2.Value );
            Assert.Equal( -10, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void DivisionNoSaturationPre()
        {
            var sn1 = new SaturatedInt( 80, 15, 100 );
            var sn2 = sn1 / 4;

            Assert.Equal( 20, sn2.Value );
            Assert.Equal( 15, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void DivisionNoSaturationPost()
        {
            var sn1 = new SaturatedInt( 3, 0, 100 );
            var sn2 = 150 / sn1;

            Assert.Equal( 50, sn2.Value );
            Assert.Equal( 0, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void IncrementSaturation()
        {
            var sn1 = new SaturatedInt( 100, -25, 100 );
            sn1++;

            Assert.Equal( 100, sn1.Value );
            Assert.Equal( -25, sn1.Minimum );
            Assert.Equal( 100, sn1.Maximum );
        }

        [Fact]
        public void DecrementSaturation()
        {
            var sn1 = new SaturatedInt( -25, -25, 100 );
            sn1--;

            Assert.Equal( -25, sn1.Value );
            Assert.Equal( -25, sn1.Minimum );
            Assert.Equal( 100, sn1.Maximum );
        }

        [Fact]
        public void AdditionSaturationPrePositive()
        {
            var sn1 = new SaturatedInt( 50, -25, 100 );
            var sn2 = sn1 + 25000;

            Assert.Equal( 100, sn2.Value );
            Assert.Equal( -25, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void AdditionSaturationPreNegative()
        {
            var bn = -25000;
            var sn1 = new SaturatedInt( 50, -25, 100 );
            var sn2 = sn1 + bn;

            Assert.Equal( -25, sn2.Value );
            Assert.Equal( -25, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void AdditionSaturationPostPositive()
        {
            var sn1 = new SaturatedInt( 50, 25, 100 );
            var sn2 = 35000 + sn1;

            Assert.Equal( 100, sn2.Value );
            Assert.Equal( 25, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void AdditionSaturationPostNegative()
        {
            var sn1 = new SaturatedInt( -50, -100, 100 );
            var sn2 = -60 + sn1;

            Assert.Equal( -100, sn2.Value );
            Assert.Equal( -100, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void SubtractionSaturationPre()
        {
            var sn1 = new SaturatedInt( 50, 25, 100 );
            var sn2 = sn1 - 500;

            Assert.Equal( 25, sn2.Value );
            Assert.Equal( 25, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void SubtractionSaturationPost()
        {
            var sn1 = new SaturatedInt( 80, 25, 100 );
            var sn2 = 90 - sn1;

            Assert.Equal( 25, sn2.Value );
            Assert.Equal( 25, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationSaturationPre()
        {
            var sn1 = new SaturatedInt( 10, 5, 100 );
            var sn2 = sn1 * 60;

            Assert.Equal( 100, sn2.Value );
            Assert.Equal( 5, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationSaturationPost()
        {
            var sn1 = new SaturatedInt( 15, 10, 100 );
            var sn2 = 300 * sn1;

            Assert.Equal( 100, sn2.Value );
            Assert.Equal( 10, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void DivisionSaturationPre()
        {
            var sn1 = new SaturatedInt( 80, 15, 100 );
            var sn2 = sn1 / 8;

            Assert.Equal( 15, sn2.Value );
            Assert.Equal( 15, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void DivisionSaturationPost()
        {
            var sn1 = new SaturatedInt( 30, 25, 100 );
            var sn2 = 150 / sn1;

            Assert.Equal( 25, sn2.Value );
            Assert.Equal( 25, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void IncrementOverflow()
        {
            var sn1 = new SaturatedInt( int.MaxValue );
            sn1++;

            Assert.Equal( int.MaxValue, sn1.Value );
            Assert.Equal( int.MinValue, sn1.Minimum );
            Assert.Equal( int.MaxValue, sn1.Maximum );
        }

        [Fact]
        public void DecrementOverflow()
        {
            var sn1 = new SaturatedInt( int.MinValue );
            sn1--;

            Assert.Equal( int.MinValue, sn1.Value );
            Assert.Equal( int.MinValue, sn1.Minimum );
            Assert.Equal( int.MaxValue, sn1.Maximum );
        }

        [Fact]
        public void AdditionOverflowPrePositive()
        {
            var sn1 = new SaturatedInt( 50, -125, 100 );
            var sn2 = sn1 + int.MaxValue;

            Assert.Equal( 100, sn2.Value );
            Assert.Equal( -125, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void AdditionOverflowPreNegative()
        {
            var sn1 = new SaturatedInt( -50, -100, 100 );
            var sn2 = sn1 + int.MinValue;

            Assert.Equal( -100, sn2.Value );
            Assert.Equal( -100, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void AdditionOverflowPostPositive()
        {
            var sn1 = new SaturatedInt( 50, -25, 100 );
            var sn2 = int.MaxValue + sn1;

            Assert.Equal( 100, sn2.Value );
            Assert.Equal( -25, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void AdditionOverflowPostNegative()
        {
            var sn1 = new SaturatedInt( -50, -125, 100 );
            var sn2 = int.MinValue + sn1;

            Assert.Equal( -125, sn2.Value );
            Assert.Equal( -125, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void SubtractionOverflowPrePositive()
        {
            var sn1 = new SaturatedInt( 50, -125, 100 );
            var sn2 = sn1 - int.MinValue;

            Assert.Equal( 100, sn2.Value );
            Assert.Equal( -125, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void SubtractionOverflowPreNegative()
        {
            var sn1 = new SaturatedInt( -50, -125, 100 );
            var sn2 = sn1 - int.MaxValue;

            Assert.Equal( -125, sn2.Value );
            Assert.Equal( -125, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );
        }

        [Fact]
        public void SubtractionOverflowPostPositive()
        {
            var sn1 = new SaturatedInt( int.MinValue, int.MinValue, -899 );
            var sn2 = 500 - sn1;

            Assert.Equal( -899, sn2.Value );
            Assert.Equal( int.MinValue, sn2.Minimum );
            Assert.Equal( -899, sn2.Maximum );
        }

        [Fact]
        public void SubtractionOverflowPostNegative()
        {
            var sn1 = new SaturatedInt( int.MaxValue, 500, int.MaxValue );
            var sn2 = -500 - sn1;

            Assert.Equal( 500, sn2.Value );
            Assert.Equal( 500, sn2.Minimum );
            Assert.Equal( int.MaxValue, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPrePositive()
        {
            var sn1 = new SaturatedInt( 10, -5, 100 );
            var sn2 = sn1 * ( int.MaxValue / 2 );

            var sn3 = new SaturatedInt( -10, -50, 100 );
            var sn4 = sn3 * ( int.MinValue / 2 );

            Assert.Equal( 100, sn2.Value );
            Assert.Equal( -5, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );

            Assert.Equal( 100, sn4.Value );
            Assert.Equal( -50, sn4.Minimum );
            Assert.Equal( 100, sn4.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPreNegative()
        {
            var sn1 = new SaturatedInt( -10, -50, 100 );
            var sn2 = sn1 * ( int.MaxValue / 2 );

            var sn3 = new SaturatedInt( 10, -50, 100 );
            var sn4 = sn3 * ( int.MinValue / 2 );

            Assert.Equal( -50, sn2.Value );
            Assert.Equal( -50, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );

            Assert.Equal( -50, sn4.Value );
            Assert.Equal( -50, sn4.Minimum );
            Assert.Equal( 100, sn4.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPostPositive()
        {
            var sn1 = new SaturatedInt( 15, -100, 100 );
            var sn2 = ( int.MaxValue / 2 ) * sn1;

            var sn3 = new SaturatedInt( -15, -100, 100 );
            var sn4 = ( int.MinValue / 2 ) * sn3;

            Assert.Equal( 100, sn2.Value );
            Assert.Equal( -100, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );

            Assert.Equal( 100, sn4.Value );
            Assert.Equal( -100, sn4.Minimum );
            Assert.Equal( 100, sn4.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPostNegative()
        {
            var sn1 = new SaturatedInt( 15, -100, 100 );
            var sn2 = ( int.MinValue / 2 ) * sn1;

            var sn3 = new SaturatedInt( -15, -100, 100 );
            var sn4 = ( int.MaxValue / 2 ) * sn3;

            Assert.Equal( -100, sn2.Value );
            Assert.Equal( -100, sn2.Minimum );
            Assert.Equal( 100, sn2.Maximum );

            Assert.Equal( -100, sn4.Value );
            Assert.Equal( -100, sn4.Minimum );
            Assert.Equal( 100, sn4.Maximum );
        }

        [Fact]
        public void ComparisonOperatorsPre()
        {
            var sn1 = new SaturatedInt( 15, 10, 100 );

            Assert.True( sn1 == 15 );
            Assert.False( sn1 == 16 );

            Assert.False( sn1 != 15 );
            Assert.True( sn1 != 14 );

            Assert.True( sn1 < 25 );
            Assert.True( sn1 <= 16 );
            Assert.True( sn1 <= 15 );
            Assert.False( sn1 <= 14 );
            Assert.False( sn1 < 5 );

            Assert.True( sn1 > 5 );
            Assert.True( sn1 >= 14 );
            Assert.True( sn1 >= 15 );
            Assert.False( sn1 >= 16 );
            Assert.False( sn1 > 25 );
        }

        [Fact]
        public void ComparisonOperatorsPost()
        {
            var sn1 = new SaturatedInt( 15, 10, 100 );

            Assert.True( 15 == sn1 );
            Assert.False( 14 == sn1 );

            Assert.False( 15 != sn1 );
            Assert.True( 14 != sn1 );

            Assert.False( 25 < sn1 );
            Assert.False( 16 <= sn1 );
            Assert.True( 15 <= sn1 );
            Assert.True( 14 <= sn1 );
            Assert.True( 5 < sn1 );

            Assert.False( 5 > sn1 );
            Assert.False( 14 >= sn1 );
            Assert.True( 15 >= sn1 );
            Assert.True( 16 >= sn1 );
            Assert.True( 25 > sn1 );
        }

        [Fact]
        public void Equality()
        {
            var sn1 = new SaturatedInt( 15, 10, 100 );
            var sn2 = new SaturatedInt( 25, 10, 100 );
            var sn3 = new SaturatedInt( 15, 5, 100 );
            var sn4 = new SaturatedInt( 15, 10, 150 );
            var sn5 = new SaturatedInt( 15, 10, 100 );

            Assert.True( sn1.Equals( sn1 ) );
            Assert.True( sn1.Equals( sn5 ) );
            Assert.False( sn1.Equals( sn2 ) );
            Assert.False( sn1.Equals( sn3 ) );
            Assert.False( sn1.Equals( sn4 ) );

            Assert.True( sn1.Equals( 15 ) );
            Assert.False( sn1.Equals( 20 ) );

            Assert.True( sn1.Equals( (object) 15 ) );
            Assert.False( sn1.Equals( (object) 20 ) );

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
            var sn1 = new SaturatedInt( 15, 10, 100 );

            Assert.True( sn1.CompareTo( 14 ) > 0 );
            Assert.True( sn1.CompareTo( 15 ) == 0 );
            Assert.True( sn1.CompareTo( 16 ) < 0 );
        }

        [Fact]
        public void ToStringConversion()
        {
            var sn1 = new SaturatedInt( 15, 10, 100 );
            var sn2 = new SaturatedInt( 15000 );

            Assert.Equal( "15", sn1.ToString() );
            Assert.Equal( "15000", sn2.ToString() );
        }
    }
}