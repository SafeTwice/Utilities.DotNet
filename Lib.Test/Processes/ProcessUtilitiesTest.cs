/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Linq;
using Utilities.DotNet.Processes;
using Xunit;

namespace Utilities.DotNet.Test.Processes
{
    public static class ProcessUtilitiesTest
    {
        [Fact]
        public static void GetModulesInfo()
        {
            // Act

            var result = ProcessUtilities.GetModulesInfo();

            // Assert

            var testModule = result.Where( mi => ( mi.name == "USER32.dll" ) );

            Assert.True( testModule.Count() == 1 );
            Assert.True( testModule.First().version.Length > 0 );
        }

        [Fact]
        public static void GetAssembliesInfo()
        {
            // Act

            var result = ProcessUtilities.GetAssembliesInfo();

            // Assert

            var testAssembly = result.Where( ai => ( ai.name == "Utilities.DotNet.Test" ) );

            Assert.True( testAssembly.Count() == 1 );
            Assert.True( testAssembly.First().version == "1.0.0.0" );
        }
    }
}
