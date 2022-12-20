using System;
using Utilities.Net.Numbers;
using Xunit;

namespace Utilities.Net.Test.Numbers
{
    public class SaturatedLongTest
    {
        [Fact]
        public void ConstructorBetweenBounds()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );

            Assert.Equal( 50, si1.Value );
            Assert.Equal( 25, si1.Minimum );
            Assert.Equal( 100, si1.Maximum );
        }

        [Fact]
        public void ConstructorLowerThanMinimum()
        {
            var si1 = new SaturatedLong( 5, 25, 100 );

            Assert.Equal( 25, si1.Value );
            Assert.Equal( 25, si1.Minimum );
            Assert.Equal( 100, si1.Maximum );
        }

        [Fact]
        public void ConstructorGreaterThanMaximum()
        {
            var si1 = new SaturatedLong( 8000, 25, 100 );

            Assert.Equal( 100, si1.Value );
            Assert.Equal( 25, si1.Minimum );
            Assert.Equal( 100, si1.Maximum );
        }

        [Fact]
        public void ConstructorInvalidBounds()
        {
            Assert.Throws<ArgumentException>( () => new SaturatedLong( 50, 125, 100 ) );
        }

        [Fact]
        public void ConstructorLimitBounds()
        {
            var si1 = new SaturatedLong( 500 );

            Assert.Equal( 500, si1.Value );
            Assert.Equal( long.MinValue, si1.Minimum );
            Assert.Equal( long.MaxValue, si1.Maximum );
        }

        [Fact]
        public void ConstructorFromOther()
        {
            var si1 = new SaturatedLong( 18000, 9000, 800000 );
            var si2 = new SaturatedLong( si1 );

            Assert.Equal( 18000, si1.Value );
            Assert.Equal( 9000, si1.Minimum );
            Assert.Equal( 800000, si1.Maximum );
        }

        [Fact]
        public void ImplicitConversionToLong()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );
            long value = si1;

            Assert.Equal( 50, value );
            Assert.Equal( 25, si1.Minimum );
            Assert.Equal( 100, si1.Maximum );
        }

        [Fact]
        public void IncrementNoSaturation()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );
            si1++;

            Assert.Equal( 51, si1.Value );
            Assert.Equal( 25, si1.Minimum );
            Assert.Equal( 100, si1.Maximum );
        }

        [Fact]
        public void DecrementNoSaturation()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );
            si1--;

            Assert.Equal( 49, si1.Value );
            Assert.Equal( 25, si1.Minimum );
            Assert.Equal( 100, si1.Maximum );
        }

        [Fact]
        public void AdditionNoSaturationPre()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );
            var si2 = si1 + 25;

            Assert.Equal( 75, si2.Value );
            Assert.Equal( 25, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void AdditionNoSaturationPost()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );
            var si2 = 35 + si1;

            Assert.Equal( 85, si2.Value );
            Assert.Equal( 25, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void SubstractionNoSaturationPre()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );
            var si2 = si1 - 5;

            Assert.Equal( 45, si2.Value );
            Assert.Equal( 25, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void SubstractionNoSaturationPost()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );
            var si2 = 90 - si1;

            Assert.Equal( 40, si2.Value );
            Assert.Equal( 25, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void MultiplicationNoSaturationPre()
        {
            var si1 = new SaturatedLong( 10, 5, 100 );
            var si2 = si1 * 6;

            Assert.Equal( 60, si2.Value );
            Assert.Equal( 5, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void MultiplicationNoSaturationPost()
        {
            var si1 = new SaturatedLong( 15, 10, 100 );
            var si2 = 3 * si1;

            Assert.Equal( 45, si2.Value );
            Assert.Equal( 10, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void DivisionNoSaturationPre()
        {
            var si1 = new SaturatedLong( 80, 15, 100 );
            var si2 = si1 / 4;

            Assert.Equal( 20, si2.Value );
            Assert.Equal( 15, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void DivisionNoSaturationPost()
        {
            var si1 = new SaturatedLong( 3, 0, 100 );
            var si2 = 150 / si1;

            Assert.Equal( 50, si2.Value );
            Assert.Equal( 0, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void IncrementSaturation()
        {
            var si1 = new SaturatedLong( 100, 25, 100 );
            si1++;

            Assert.Equal( 100, si1.Value );
            Assert.Equal( 25, si1.Minimum );
            Assert.Equal( 100, si1.Maximum );
        }

        [Fact]
        public void DecrementSaturation()
        {
            var si1 = new SaturatedLong( 25, 25, 100 );
            si1--;

            Assert.Equal( 25, si1.Value );
            Assert.Equal( 25, si1.Minimum );
            Assert.Equal( 100, si1.Maximum );
        }

        [Fact]
        public void AdditionSaturationPre()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );
            var si2 = si1 + 25000;

            Assert.Equal( 100, si2.Value );
            Assert.Equal( 25, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void AdditionSaturationPost()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );
            var si2 = 35000 + si1;

            Assert.Equal( 100, si2.Value );
            Assert.Equal( 25, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void SubstractionSaturationPre()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );
            var si2 = si1 - 500;

            Assert.Equal( 25, si2.Value );
            Assert.Equal( 25, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void SubstractionSaturationPost()
        {
            var si1 = new SaturatedLong( 80, 25, 100 );
            var si2 = 90 - si1;

            Assert.Equal( 25, si2.Value );
            Assert.Equal( 25, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void MultiplicationSaturationPre()
        {
            var si1 = new SaturatedLong( 10, 5, 100 );
            var si2 = si1 * 60;

            Assert.Equal( 100, si2.Value );
            Assert.Equal( 5, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void MultiplicationSaturationPost()
        {
            var si1 = new SaturatedLong( 15, 10, 100 );
            var si2 = 300 * si1;

            Assert.Equal( 100, si2.Value );
            Assert.Equal( 10, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void DivisionSaturationPre()
        {
            var si1 = new SaturatedLong( 80, 15, 100 );
            var si2 = si1 / 8;

            Assert.Equal( 15, si2.Value );
            Assert.Equal( 15, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void DivisionSaturationPost()
        {
            var si1 = new SaturatedLong( 30, 25, 100 );
            var si2 = 150 / si1;

            Assert.Equal( 25, si2.Value );
            Assert.Equal( 25, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void IncrementOverflow()
        {
            var si1 = new SaturatedLong( long.MaxValue );
            si1++;

            Assert.Equal( long.MaxValue, si1.Value );
            Assert.Equal( long.MinValue, si1.Minimum );
            Assert.Equal( long.MaxValue, si1.Maximum );
        }

        [Fact]
        public void DecrementOverflow()
        {
            var si1 = new SaturatedLong( long.MinValue );
            si1--;

            Assert.Equal( long.MinValue, si1.Value );
            Assert.Equal( long.MinValue, si1.Minimum );
            Assert.Equal( long.MaxValue, si1.Maximum );
        }

        [Fact]
        public void AdditionOverflowPre()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );
            var si2 = si1 + long.MaxValue;

            Assert.Equal( 100, si2.Value );
            Assert.Equal( 25, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void AdditionOverflowPost()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );
            var si2 = long.MaxValue + si1;

            Assert.Equal( 100, si2.Value );
            Assert.Equal( 25, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void SubstractionOverflowPre()
        {
            var si1 = new SaturatedLong( 50, 25, 100 );
            var si2 = si1 - long.MinValue;

            Assert.Equal( 25, si2.Value );
            Assert.Equal( 25, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void SubstractionOverflowPost()
        {
            var si1 = new SaturatedLong( long.MaxValue, 500, long.MaxValue );
            var si2 = -500 - si1;

            Assert.Equal( 500, si2.Value );
            Assert.Equal( 500, si2.Minimum );
            Assert.Equal( long.MaxValue, si2.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPre()
        {
            var si1 = new SaturatedLong( 10, 5, 100 );
            var si2 = si1 * ( long.MaxValue / 2 );

            Assert.Equal( 100, si2.Value );
            Assert.Equal( 5, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void MultiplicationOverflowPost()
        {
            var si1 = new SaturatedLong( 15, 10, 100 );
            var si2 = ( long.MaxValue / 2 ) * si1;

            Assert.Equal( 100, si2.Value );
            Assert.Equal( 10, si2.Minimum );
            Assert.Equal( 100, si2.Maximum );
        }

        [Fact]
        public void ComparisonOperatorsPre()
        {
            var si1 = new SaturatedLong( 15, 10, 100 );

            Assert.True( si1 == 15 );
            Assert.False( si1 == 16 );

            Assert.False( si1 != 15 );
            Assert.True( si1 != 14 );

            Assert.True( si1 < 25 );
            Assert.True( si1 <= 16 );
            Assert.True( si1 <= 15 );
            Assert.False( si1 <= 14 );
            Assert.False( si1 < 5 );

            Assert.True( si1 > 5 );
            Assert.True( si1 >= 14 );
            Assert.True( si1 >= 15 );
            Assert.False( si1 >= 16 );
            Assert.False( si1 > 25 );
        }

        [Fact]
        public void ComparisonOperatorsPost()
        {
            var si1 = new SaturatedLong( 15, 10, 100 );

            Assert.True( 15 == si1 );
            Assert.False( 14 == si1 );

            Assert.False( 15 != si1 );
            Assert.True( 14 != si1 );

            Assert.False( 25 < si1 );
            Assert.False( 16 <= si1 );
            Assert.True( 15 <= si1 );
            Assert.True( 14 <= si1 );
            Assert.True( 5 < si1 );

            Assert.False( 5 > si1 );
            Assert.False( 14 >= si1 );
            Assert.True( 15 >= si1 );
            Assert.True( 16 >= si1 );
            Assert.True( 25 > si1 );
        }

        [Fact]
        public void Equality()
        {
            var si1 = new SaturatedLong( 15, 10, 100 );
            var si2 = new SaturatedLong( 25, 10, 100 );
            var si3 = new SaturatedLong( 15, 5, 100 );
            var si4 = new SaturatedLong( 15, 10, 150 );
            var si5 = new SaturatedLong( 15, 10, 100 );

            Assert.True( si1.Equals( si1 ) );
            Assert.True( si1.Equals( si5 ) );
            Assert.False( si1.Equals( si2 ) );
            Assert.False( si1.Equals( si3 ) );
            Assert.False( si1.Equals( si4 ) );

            Assert.True( si1.Equals( 15 ) );
            Assert.False( si1.Equals( 20 ) );

            Assert.True( si1.Equals( (object) 15l ) );
            Assert.False( si1.Equals( (object) 20l ) );

            Assert.True( si1.Equals( (object) si5 ) );
            Assert.False( si1.Equals( (object) si2 ) );

            Assert.False( si1.Equals( null ) );

            Assert.False( si1.Equals( 15.0 ) );

            Assert.Equal( si1.GetHashCode(), si5.GetHashCode() );
            Assert.NotEqual( si1.GetHashCode(), si2.GetHashCode() );
            Assert.NotEqual( si1.GetHashCode(), si3.GetHashCode() );
            Assert.NotEqual( si1.GetHashCode(), si4.GetHashCode() );
        }

        [Fact]
        public void CompareTo()
        {
            var si1 = new SaturatedLong( 15, 10, 100 );

            Assert.True( si1.CompareTo( 14 ) > 0 );
            Assert.True( si1.CompareTo( 15 ) == 0 );
            Assert.True( si1.CompareTo( 16 ) < 0 );
        }

        [Fact]
        public void ToStringConversion()
        {
            var si1 = new SaturatedLong( 15, 10, 100 );
            var si2 = new SaturatedLong( 15000 );

            Assert.Equal( "15", si1.ToString() );
            Assert.Equal( "15000", si2.ToString() );
        }
    }
}