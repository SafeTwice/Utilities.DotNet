/// @file
/// @copyright  Copyright (c) 2020-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Xunit;

namespace Utilities.DotNet.Test
{
    public class DoubleUtilitiesTest
    {
        [Theory]
        [InlineData( 15.00d, 15.19d, 0.20d, true )]
        [InlineData( 15.00d, 15.21d, 0.21d, false )]
        [InlineData( 268.0070d, 268.0061d, 0.001d, true )]
        [InlineData( 268.0070d, 268.0059d, 0.001d, false )]
        [InlineData( double.PositiveInfinity, double.PositiveInfinity, 0.000000000001d, true )]
        [InlineData( double.PositiveInfinity, double.NegativeInfinity, 0.000000000001d, false )]
        [InlineData( double.NegativeInfinity, double.PositiveInfinity, 0.000000000001d, false )]
        [InlineData( 234.3d, double.NaN, 0.000000000001d, false )]
        [InlineData( double.NaN, -2355.234d, 0.000000000001d, false )]
        public void EqualsTest( double a, double b, double tolerance, bool result )
        {
            Assert.Equal( result, DoubleUtilities.Equals( a, b, tolerance ) );
        }

        [Theory]
        [InlineData( 5467.245d, 5412.573d, 0.01d, true )]
        [InlineData( 5467.245d, 5412.572d, 0.01d, false )]
        [InlineData( 0.0003416d, 0.000345d, 0.01d, true )]
        [InlineData( 0.0003415d, 0.000345d, 0.01d, false )]
        [InlineData( double.PositiveInfinity, double.PositiveInfinity, 0.000000000001d, true )]
        [InlineData( double.PositiveInfinity, double.NegativeInfinity, 0.000000000001d, false )]
        [InlineData( double.NegativeInfinity, double.PositiveInfinity, 0.000000000001d, false )]
        [InlineData( -234.3d, double.NaN, 0.000000000001d, false )]
        [InlineData( double.NaN, 0.000008787324, 0.000000000001d, false )]
        public void EqualsRelativeTest( double a, double b, double tolerance, bool result )
        {
            Assert.Equal( result, DoubleUtilities.EqualsRelative( a, b, tolerance ) );
        }

        [Theory]
        [InlineData( 15.00d, 15.19d, 0.20d, 0 )]
        [InlineData( 15.00d, 15.21d, 0.21d, -1 )]
        [InlineData( 268.0070d, 268.0061d, 0.001d, 0 )]
        [InlineData( 268.0070d, 268.0059d, 0.001d, 1 )]
        [InlineData( double.PositiveInfinity, double.PositiveInfinity, 0.000000000001d, 0 )]
        [InlineData( double.PositiveInfinity, double.NegativeInfinity, 0.000000000001d, 1 )]
        [InlineData( double.NegativeInfinity, double.PositiveInfinity, 0.000000000001d, -1 )]
        [InlineData( double.NaN, 0.000008787324, 0.000000000001d, -1 )]
        [InlineData( -234.3d, double.NaN, 0.000000000001d, 1 )]
        [InlineData( 234.3d, double.NaN, 0.000000000001d, 1 )]
        public void CompareToTest( double a, double b, double tolerance, int result )
        {
            Assert.Equal( result, DoubleUtilities.CompareTo( a, b, tolerance ) );
        }

        [Theory]
        [InlineData( 5467.245d, 5412.573d, 0.01d, 0 )]
        [InlineData( 5467.245d, 5412.572d, 0.01d, 1 )]
        [InlineData( -0.0003416d, -0.000345d, 0.01d, 0 )]
        [InlineData( -0.0003415d, -0.000345d, 0.01d, 1 )]
        [InlineData( 0.0003415d, 0.000345d, 0.01d, -1 )]
        [InlineData( double.PositiveInfinity, double.PositiveInfinity, 0.000000000001d, 0 )]
        [InlineData( double.PositiveInfinity, double.NegativeInfinity, 0.000000000001d, 1 )]
        [InlineData( double.NegativeInfinity, double.PositiveInfinity, 0.000000000001d, -1 )]
        [InlineData( double.NaN, 0.000008787324, 0.000000000001d, -1 )]
        [InlineData( -234.3d, double.NaN, 0.000000000001d, 1 )]
        [InlineData( 234.3d, double.NaN, 0.000000000001d, 1 )]
        public void CompareRelativeToTest( double a, double b, double tolerance, int result )
        {
            Assert.Equal( result, DoubleUtilities.CompareRelativeTo( a, b, tolerance ) );
        }
    }
}
