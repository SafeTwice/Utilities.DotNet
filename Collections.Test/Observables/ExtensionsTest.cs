/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Linq;
using Xunit;

namespace Utilities.DotNet.Collections.Observables.Test
{
    public class ExtensionsTest
    {
        [Fact]
        public void ToObservableCollection()
        {
            // Arrange

            var sequence = new int[] { 1, 2, 3 };

            // Act

            var list = sequence.ToObservableCollection();

            // Assert

            Assert.Equal( sequence, list );
        }

        [Fact]
        public void ToObservableList()
        {
            // Arrange

            var sequence = new int[] { 1, 2, 3 };

            // Act

            var hashSet = sequence.ToObservableList();

            // Assert

            Assert.Equal( sequence, hashSet );
        }

        [Fact]
        public void ToObservableSet()
        {
            // Arrange

            var sequence = new int[] { 1, 2, 3 };

            // Act

            var hashSet = sequence.ToObservableSet();

            // Assert

            Assert.Equal( sequence, hashSet );
        }

        [Fact]
        public void ToObservableSortedCollection()
        {
            // Arrange

            var sequence = new int[] { 1, 9, 2, 7, 3 };

            // Act

            var list = sequence.ToObservableSortedCollection();

            // Assert

            Assert.Equal( new int[] { 1, 2, 3, 7, 9 }, list );
        }

        [Fact]
        public void ToObservableSortedList()
        {
            // Arrange

            var sequence = new int[] { 1, 9, 2, 7, 3 };

            // Act

            var list = sequence.ToObservableSortedList();

            // Assert

            Assert.Equal( new int[] { 1, 2, 3, 7, 9 }, list );
        }

        [Fact]
        public void ToObservableSortedSet()
        {
            // Arrange

            var sequence = new int[] { 1, 9, 2, 7, 3 };

            // Act

            var list = sequence.ToObservableSortedSet();

            // Assert

            Assert.Equal( new int[] { 1, 2, 3, 7, 9 }, list );
        }

        [Fact]
        public void WrapAsObservableReadOnlyCollection()
        {
            // Arrange

            var collection = new ObservableCollection<int> { 1, 2, 3 };

            // Act

            var readOnlyCollection = collection.WrapAsObservableReadOnlyCollection();

            // Assert

            Assert.Equal( collection, readOnlyCollection );
        }

        [Fact]
        public void WrapAsObservableReadOnlyList()
        {
            // Arrange

            var collection = new ObservableList<int> { 1, 2, 3 };

            // Act

            var readOnlyList = collection.WrapAsObservableReadOnlyList();

            // Assert

            Assert.Equal( collection, readOnlyList );
        }

        [Fact]
        public void WrapAsObservableReadOnlySet()
        {
            // Arrange

            var collection = new ObservableSet<int> { 1, 2, 3 };

            // Act

            var readOnlySet = collection.WrapAsObservableReadOnlySet();

            // Assert

            Assert.Equal( collection.OrderBy( i => i ), readOnlySet.OrderBy( i => i ) );
        }
    }
}
