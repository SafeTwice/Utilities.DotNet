/// @file
/// @copyright  Copyright (c) 2023-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Linq;
using Xunit;

namespace Utilities.DotNet.System.Processes.Test
{
    public static class ProcessUtilitiesTest
    {
        [Fact]
        public static void GetModulesInfo()
        {
            // Act

            var result = ProcessUtilities.GetModulesInfo();

            // Assert

            var testModules = result.Where( mi => ( mi.name == "USER32.dll" ) );

            Assert.Single( testModules );
            Assert.NotEmpty( testModules.First().version );
        }

        [Fact]
        public static void GetAssembliesInfo()
        {
            // Act

            var result = ProcessUtilities.GetAssembliesInfo();

            // Assert

            var testAssemblies = result.Where( ai => ( ai.name == "Utilities.DotNet.System.Test" ) );

            Assert.Single( testAssemblies );
            Assert.Equal( "1.0.0.0", testAssemblies.First().version );
        }
    }
}
