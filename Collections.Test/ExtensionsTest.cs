/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Linq;
using Xunit;

namespace Utilities.DotNet.Collections.Test
{
    public class ExtensionsTest
    {
        [Fact]
        public void ToListEx()
        {
            // Arrange

            var sequence = new int[] { 1, 2, 3 };

            // Act

            var list = sequence.ToListEx();

            // Assert

            Assert.Equal( sequence, list );
        }

        [Fact]
        public void ToHashSetEx()
        {
            // Arrange

            var sequence = new int[] { 1, 2, 3 };

            // Act

            var hashSet = sequence.ToHashSetEx();

            // Assert

            Assert.Equal( sequence, hashSet );
        }

        [Fact]
        public void WrapAsReadOnlyCollection()
        {
            // Arrange

            var collection = new ListEx<int> { 1, 2, 3 };

            // Act

            var readOnlyCollection = collection.WrapAsReadOnlyCollection();

            // Assert

            Assert.Equal( collection, readOnlyCollection );
        }

        [Fact]
        public void WrapAsReadOnlyList()
        {
            // Arrange

            var list = new ListEx<int> { 1, 2, 3 };

            // Act

            var readOnlyList = list.WrapAsReadOnlyList();

            // Assert

            Assert.Equal( list, readOnlyList );
        }

        [Fact]
        public void WrapAsReadOnlySetEx()
        {
            // Arrange

            var set = new HashSetEx<int> { 1, 2, 3 };

            // Act

            var readOnlySet = set.WrapAsReadOnlySetEx();

            // Assert

            Assert.Equal( set.OrderBy( i => i ), readOnlySet.OrderBy( i => i ) );
        }
    }
}
