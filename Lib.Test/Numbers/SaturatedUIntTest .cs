/// @file
/// @copyright  Copyright (c) 2022U SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using Utilities.DotNet.Numbers;
using Xunit;

namespace Utilities.DotNet.Test.Numbers
{
    public class SaturatedUIntTest
    {
        [Fact]
        public void ConstructorBetweenBounds()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );

            Assert.Equal( 50U, sn1.Value );
            Assert.Equal( 25U, sn1.Minimum );
            Assert.Equal( 100U, sn1.Maximum );
        }

        [Fact]
        public void ConstructorLowerThanMinimum()
        {
            var sn1 = new SaturatedUInt( 5U, 25U, 100U );

            Assert.Equal( 25U, sn1.Value );
            Assert.Equal( 25U, sn1.Minimum );
            Assert.Equal( 100U, sn1.Maximum );
        }

        [Fact]
        public void ConstructorGreaterThanMaximum()
        {
            var sn1 = new SaturatedUInt( 8000U, 25U, 100U );

            Assert.Equal( 100U, sn1.Value );
            Assert.Equal( 25U, sn1.Minimum );
            Assert.Equal( 100U, sn1.Maximum );
        }

        [Fact]
        public void ConstructorInvalidBounds()
        {
            Assert.Throws<ArgumentException>( () => new SaturatedUInt( 50U, 125U, 100U ) );
        }

        [Fact]
        public void ConstructorLimitBounds()
        {
            var sn1 = new SaturatedUInt( 500U );

            Assert.Equal( 500U, sn1.Value );
            Assert.Equal( uint.MinValue, sn1.Minimum );
            Assert.Equal( uint.MaxValue, sn1.Maximum );
        }

        [Fact]
        public void ConstructorFromOther()
        {
            var sn1 = new SaturatedUInt( 18000U, 9000U, 800000U );
            var sn2 = new SaturatedUInt( sn1 );

            Assert.Equal( 18000U, sn1.Value );
            Assert.Equal( 9000U, sn1.Minimum );
            Assert.Equal( 800000U, sn1.Maximum );
        }

        [Fact]
        public void ImplicitConversionToInt()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );
            uint value = sn1;

            Assert.Equal( 50U, value );
            Assert.Equal( 25U, sn1.Minimum );
            Assert.Equal( 100U, sn1.Maximum );
        }

        [Fact]
        public void IncrementNoSaturation()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );
            sn1++;

            Assert.Equal( 51U, sn1.Value );
            Assert.Equal( 25U, sn1.Minimum );
            Assert.Equal( 100U, sn1.Maximum );
        }

        [Fact]
        public void DecrementNoSaturation()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );
            sn1--;

            Assert.Equal( 49U, sn1.Value );
            Assert.Equal( 25U, sn1.Minimum );
            Assert.Equal( 100U, sn1.Maximum );
        }

        [Fact]
        public void AdditionNoSaturationPre()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );
            var sn2 = sn1 + 25U;

            Assert.Equal( 75U, sn2.Value );
            Assert.Equal( 25U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void AdditionNoSaturationPost()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );
            var sn2 = 35U + sn1;

            Assert.Equal( 85U, sn2.Value );
            Assert.Equal( 25U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void SubtractionNoSaturationPre()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );
            var sn2 = sn1 - 5U;

            Assert.Equal( 45U, sn2.Value );
            Assert.Equal( 25U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void SubtractionNoSaturationPost()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );
            var sn2 = 90U - sn1;

            Assert.Equal( 40U, sn2.Value );
            Assert.Equal( 25U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationNoSaturationPre()
        {
            var sn1 = new SaturatedUInt( 10U, 5U, 100U );
            var sn2 = sn1 * 6U;

            Assert.Equal( 60U, sn2.Value );
            Assert.Equal( 5U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationNoSaturationPost()
        {
            var sn1 = new SaturatedUInt( 15U, 10U, 100U );
            var sn2 = 3U * sn1;

            Assert.Equal( 45U, sn2.Value );
            Assert.Equal( 10U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void DivisionNoSaturationPre()
        {
            var sn1 = new SaturatedUInt( 80U, 15U, 100U );
            var sn2 = sn1 / 4U;

            Assert.Equal( 20U, sn2.Value );
            Assert.Equal( 15U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void DivisionNoSaturationPost()
        {
            var sn1 = new SaturatedUInt( 3U, 0U, 100U );
            var sn2 = 150U / sn1;

            Assert.Equal( 50U, sn2.Value );
            Assert.Equal( 0U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void IncrementSaturation()
        {
            var sn1 = new SaturatedUInt( 100U, 25U, 100U );
            sn1++;

            Assert.Equal( 100U, sn1.Value );
            Assert.Equal( 25U, sn1.Minimum );
            Assert.Equal( 100U, sn1.Maximum );
        }

        [Fact]
        public void DecrementSaturation()
        {
            var sn1 = new SaturatedUInt( 25U, 25U, 100U );
            sn1--;

            Assert.Equal( 25U, sn1.Value );
            Assert.Equal( 25U, sn1.Minimum );
            Assert.Equal( 100U, sn1.Maximum );
        }

        [Fact]
        public void AdditionSaturationPre()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );
            var sn2 = sn1 + 25000U;

            Assert.Equal( 100U, sn2.Value );
            Assert.Equal( 25U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void AdditionSaturationPost()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );
            var sn2 = 35000U + sn1;

            Assert.Equal( 100U, sn2.Value );
            Assert.Equal( 25U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void SubtractionSaturationPre()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );
            var sn2 = sn1 - 500U;

            Assert.Equal( 25U, sn2.Value );
            Assert.Equal( 25U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void SubtractionSaturationPost()
        {
            var sn1 = new SaturatedUInt( 80U, 25U, 100U );
            var sn2 = 90U - sn1;

            Assert.Equal( 25U, sn2.Value );
            Assert.Equal( 25U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationSaturationPre()
        {
            var sn1 = new SaturatedUInt( 10U, 5U, 100U );
            var sn2 = sn1 * 60U;

            Assert.Equal( 100U, sn2.Value );
            Assert.Equal( 5U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationSaturationPost()
        {
            var sn1 = new SaturatedUInt( 15U, 10U, 100U );
            var sn2 = 300U * sn1;

            Assert.Equal( 100U, sn2.Value );
            Assert.Equal( 10U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void DivisionSaturationPre()
        {
            var sn1 = new SaturatedUInt( 80U, 15U, 100U );
            var sn2 = sn1 / 8U;

            Assert.Equal( 15U, sn2.Value );
            Assert.Equal( 15U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void DivisionSaturationPost()
        {
            var sn1 = new SaturatedUInt( 30U, 25U, 100U );
            var sn2 = 150U / sn1;

            Assert.Equal( 25U, sn2.Value );
            Assert.Equal( 25U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void IncrementOverflow()
        {
            var sn1 = new SaturatedUInt( uint.MaxValue );
            sn1++;

            Assert.Equal( uint.MaxValue, sn1.Value );
            Assert.Equal( uint.MinValue, sn1.Minimum );
            Assert.Equal( uint.MaxValue, sn1.Maximum );
        }

        [Fact]
        public void DecrementOverflow()
        {
            var sn1 = new SaturatedUInt( uint.MinValue );
            sn1--;

            Assert.Equal( uint.MinValue, sn1.Value );
            Assert.Equal( uint.MinValue, sn1.Minimum );
            Assert.Equal( uint.MaxValue, sn1.Maximum );
        }

        [Fact]
        public void AdditionOverflowPre()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );
            var sn2 = sn1 + uint.MaxValue;

            Assert.Equal( 100U, sn2.Value );
            Assert.Equal( 25U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void AdditionOverflowPost()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );
            var sn2 = uint.MaxValue + sn1;

            Assert.Equal( 100U, sn2.Value );
            Assert.Equal( 25U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void SubtractionOverflowPre()
        {
            var sn1 = new SaturatedUInt( 50U, 25U, 100U );
            var sn2 = sn1 - uint.MaxValue;

            Assert.Equal( 25U, sn2.Value );
            Assert.Equal( 25U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void SubtractionOverflowPost()
        {
            var sn1 = new SaturatedUInt( uint.MaxValue, 500U, uint.MaxValue );
            var sn2 = 500U - sn1;

            Assert.Equal( 500U, sn2.Value );
            Assert.Equal( 500U, sn2.Minimum );
            Assert.Equal( uint.MaxValue, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPre()
        {
            var sn1 = new SaturatedUInt( 10U, 5U, 100U );
            var sn2 = sn1 * ( uint.MaxValue / 2U );

            Assert.Equal( 100U, sn2.Value );
            Assert.Equal( 5U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPost()
        {
            var sn1 = new SaturatedUInt( 15U, 10U, 100U );
            var sn2 = ( uint.MaxValue / 2U ) * sn1;

            Assert.Equal( 100U, sn2.Value );
            Assert.Equal( 10U, sn2.Minimum );
            Assert.Equal( 100U, sn2.Maximum );
        }

        [Fact]
        public void ComparisonOperatorsPre()
        {
            var sn1 = new SaturatedUInt( 15U, 10U, 100U );

            Assert.True( sn1 == 15U );
            Assert.False( sn1 == 16U );

            Assert.False( sn1 != 15U );
            Assert.True( sn1 != 14U );

            Assert.True( sn1 < 25U );
            Assert.True( sn1 <= 16U );
            Assert.True( sn1 <= 15U );
            Assert.False( sn1 <= 14U );
            Assert.False( sn1 < 5U );

            Assert.True( sn1 > 5U );
            Assert.True( sn1 >= 14U );
            Assert.True( sn1 >= 15U );
            Assert.False( sn1 >= 16U );
            Assert.False( sn1 > 25U );
        }

        [Fact]
        public void ComparisonOperatorsPost()
        {
            var sn1 = new SaturatedUInt( 15U, 10U, 100U );

            Assert.True( 15U == sn1 );
            Assert.False( 14U == sn1 );

            Assert.False( 15U != sn1 );
            Assert.True( 14U != sn1 );

            Assert.False( 25U < sn1 );
            Assert.False( 16U <= sn1 );
            Assert.True( 15U <= sn1 );
            Assert.True( 14U <= sn1 );
            Assert.True( 5U < sn1 );

            Assert.False( 5U > sn1 );
            Assert.False( 14U >= sn1 );
            Assert.True( 15U >= sn1 );
            Assert.True( 16U >= sn1 );
            Assert.True( 25U > sn1 );
        }

        [Fact]
        public void Equality()
        {
            var sn1 = new SaturatedUInt( 15U, 10U, 100U );
            var sn2 = new SaturatedUInt( 25U, 10U, 100U );
            var sn3 = new SaturatedUInt( 15U, 5U, 100U );
            var sn4 = new SaturatedUInt( 15U, 10U, 150U );
            var sn5 = new SaturatedUInt( 15U, 10U, 100U );

            Assert.True( sn1.Equals( sn1 ) );
            Assert.True( sn1.Equals( sn5 ) );
            Assert.False( sn1.Equals( sn2 ) );
            Assert.False( sn1.Equals( sn3 ) );
            Assert.False( sn1.Equals( sn4 ) );

            Assert.True( sn1.Equals( 15U ) );
            Assert.False( sn1.Equals( 20U ) );

            Assert.True( sn1.Equals( (object) 15U ) );
            Assert.False( sn1.Equals( (object) 20U ) );

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
            var sn1 = new SaturatedUInt( 15U, 10U, 100U );

            Assert.True( sn1.CompareTo( 14U ) > 0 );
            Assert.True( sn1.CompareTo( 15U ) == 0 );
            Assert.True( sn1.CompareTo( 16U ) < 0 );
        }

        [Fact]
        public void ToStringConversion()
        {
            var sn1 = new SaturatedUInt( 15U, 10U, 100U );
            var sn2 = new SaturatedUInt( 15000U );

            Assert.Equal( "15", sn1.ToString() );
            Assert.Equal( "15000", sn2.ToString() );
        }
    }
}