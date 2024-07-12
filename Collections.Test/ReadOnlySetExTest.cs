/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Utilities.DotNet.Collections;
using Xunit;

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.

namespace Utilities.DotNet.Test.Collections
{
    public class ReadOnlySetExTest
    {
        [Fact]
        public void Constructor_Default()
        {
            var baseSet = new HashSetEx<int>();

            IReadOnlySetEx<int> testedList = new ReadOnlySetEx<int>( baseSet );

            Assert.Equal( 0, testedList.Count );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            var baseSet = new HashSetEx<int>( new[] { 8, 5, 3, 2 } );

            IReadOnlySetEx<int> testedList = new ReadOnlySetEx<int>( baseSet );

            Assert.Equal( new[] { 8, 5, 3, 2 }, testedList );
            Assert.Equal( 4, testedList.Count );
        }

        [Fact]
        public void SetComparisons()
        {
            // Arrange

            var set = new HashSetEx<int>( new[] { 8, 2, 23 } );

            var readonlySet = new ReadOnlySetEx<int>( set );

            Assert.Equal( new[] { 8, 2, 23 }, readonlySet );

            // Act & Assert

            Assert.True( readonlySet.IsSubsetOf( new object[] { 23, 2, 8, 55, 12.0 } ) );
            Assert.False( readonlySet.IsSubsetOf( new object[] { 23, 2.0, 8, 55, 12 } ) );
            Assert.False( readonlySet.IsSubsetOf( new object[] { 22, 2, 8, 55, 12 } ) );

            Assert.True( readonlySet.IsProperSubsetOf( new object[] { 23, 2, 8, 55, 12.0 } ) );
            Assert.False( readonlySet.IsProperSubsetOf( new object[] { 23, 2.0, 8, 55, 12 } ) );
            Assert.False( readonlySet.IsProperSubsetOf( new object[] { 22, 2, 8, 12.0 } ) );
            Assert.False( readonlySet.IsProperSubsetOf( new object[] { 23, 2, 8 } ) );

            Assert.True( readonlySet.IsSupersetOf( new object[] { 8, 23 } ) );
            Assert.False( readonlySet.IsSupersetOf( new object[] { 8, 23.0 } ) );
            Assert.False( readonlySet.IsSupersetOf( new object[] { 8, 23, 1 } ) );

            Assert.True( readonlySet.IsProperSupersetOf( new object[] { 8, 23 } ) );
            Assert.False( readonlySet.IsProperSupersetOf( new object[] { 8.0, 23 } ) );
            Assert.False( readonlySet.IsProperSupersetOf( new object[] { 8, 23, 1 } ) );
            Assert.False( readonlySet.IsProperSupersetOf( new object[] { 8, 23, 2 } ) );

            Assert.True( readonlySet.Overlaps( new object[] { 12, 2, 55 } ) );
            Assert.False( readonlySet.Overlaps( new object[] { 12, 2.0, 55 } ) );
            Assert.False( readonlySet.Overlaps( new object[] { 12, 1, 55 } ) );

            Assert.True( readonlySet.SetEquals( new object[] { 8, 23, 2 } ) );
            Assert.False( readonlySet.SetEquals( new object[] { 8, 23.0, 2 } ) );
            Assert.False( readonlySet.SetEquals( new object[] { 8, 23, 1 } ) );

            // Assert unmodified state

            Assert.Equal( new[] { 8, 2, 23 }, set );
        }
    }
}
