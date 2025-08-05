/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Xunit;

namespace Utilities.DotNet.TransferFunctions.Test
{
    public class DecimalLinearInterpolationTest
    {
        [Fact]
        public void Calculate()
        {
            var linear = new DecimalLinearInterpolation( 1.0m, 25.0m, 2.0m, 24.0m );

            Assert.Equal( 10.25m, linear.Calculate( 10.0m ) );
            Assert.Equal( -3.5m, linear.Calculate( -5.0m ) );
            Assert.Equal( 276.1m, linear.Calculate( 300.0m ), 1 );
        }

        [Fact]
        public void Inverse()
        {
            var linear = new DecimalLinearInterpolation( 2.0m, 24.0m, 1.0m, 25.0m );

            Assert.Equal( 10.25m, linear.CalculateInverse( 10.0m ) );
            Assert.Equal( -3.5m, linear.CalculateInverse( -5.0m ) );
            Assert.Equal( 276.1m, linear.CalculateInverse( 300.0m ), 1 );
        }
    }
}
