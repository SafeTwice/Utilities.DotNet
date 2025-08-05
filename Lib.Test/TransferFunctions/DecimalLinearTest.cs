/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Xunit;

namespace Utilities.DotNet.TransferFunctions.Test
{
    public class DecimalLinearTest
    {
        [Fact]
        public void Calculate()
        {
            var linear = new DecimalLinear( 5.0m, 25.0m );

            Assert.Equal( 41.5m, linear.Calculate( 3.3m ) );
            Assert.Equal( 4943896.7m, linear.Calculate( 988774.34m ) );
        }

        [Fact]
        public void Inverse()
        {
            var linear = new DecimalLinear( 0.2m, -5.0m );

            Assert.Equal( 41.5m, linear.CalculateInverse( 3.3m ) );
            Assert.Equal( 4943896.7m, linear.CalculateInverse( 988774.34m ) );
        }
    }
}
