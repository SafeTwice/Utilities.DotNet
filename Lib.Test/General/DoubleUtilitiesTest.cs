/// @file
/// @copyright  Copyright (c) 2020 SafeTwice S.L. All rights reserved.
/// @license    MIT (https://opensource.org/licenses/MIT)

using Xunit;

namespace Utilities.DotNet.Test
{
    public class DoubleUtilitiesTest
    {
        [Fact]
        public void EqualsTest()
        {
            Assert.True( DoubleUtilities.Equals( 15.0, 15.2, 0.21 ) );
            Assert.False( DoubleUtilities.Equals( 268.006, 268.0071, 0.001 ) );
        }
    }
}
