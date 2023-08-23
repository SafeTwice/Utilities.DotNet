/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Utilities.DotNet.TransferFunctions;
using Xunit;

namespace Utilities.DotNet.Test.TransferFunctions
{
    public class DoubleLinearInterpolationTest
    {
        [Fact]
        public void Calculate()
        {
            var linear = new DoubleLinearInterpolation( 1.0, 25.0, 2.0, 24.0 );

            Assert.Equal( 10.2, linear.Calculate( 10.0 ), 1 );
            Assert.Equal( -3.5, linear.Calculate( -5.0 ), 1 );
            Assert.Equal( 276.1, linear.Calculate( 300.0 ), 1 );
        }

        [Fact]
        public void Inverse()
        {
            var linear = new DoubleLinearInterpolation( 2.0, 24.0, 1.0, 25.0 );

            Assert.Equal( 10.2, linear.CalculateInverse( 10.0 ), 1 );
            Assert.Equal( -3.5, linear.CalculateInverse( -5.0 ), 1 );
            Assert.Equal( 276.1, linear.CalculateInverse( 300.0 ), 1 );
        }
    }
}
