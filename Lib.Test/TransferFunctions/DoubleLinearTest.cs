/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Utilities.Net.TransferFunctions;
using Xunit;

namespace Utilities.Net.Test.TransferFunctions
{
    public class DoubleLinearTest
    {
        [Fact]
        public void Calculate()
        {
            var linear = new DoubleLinear( 5.0, 25.0 );

            Assert.Equal( 41.5, linear.Calculate( 3.3 ), 1 );
            Assert.Equal( 4943896.7, linear.Calculate( 988774.34 ), 1 );
        }

        [Fact]
        public void Inverse()
        {
            var linear = new DoubleLinear( 0.2, -5.0 );

            Assert.Equal( 41.5, linear.CalculateInverse( 3.3 ), 1 );
            Assert.Equal( 4943896.7, linear.CalculateInverse( 988774.34 ), 1 );
        }
    }
}
