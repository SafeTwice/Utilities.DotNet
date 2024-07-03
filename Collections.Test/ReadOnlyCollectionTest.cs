/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections;
using System.Collections.Generic;
using Utilities.DotNet.Collections;
using Xunit;

#pragma warning disable xUnit2013 // Do not use equality check to check for testedCollection size.

namespace Utilities.DotNet.Test.Collections
{
    public class ReadOnlyCollectionTest
    {
        [Fact]
        public void Constructor_Default()
        {
            var baseCollection = new ListEx<int>();

            IReadOnlyCollection<int> testedCollection = new ReadOnlyCollection<int>( baseCollection );

            Assert.Equal( 0, testedCollection.Count );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            var baseCollection = new ListEx<int>( new[] { 1, 3, 2, 4 } );

            IReadOnlyCollection<int> testedCollection = new ReadOnlyCollection<int>( baseCollection );

            Assert.Equal( new[] { 1, 3, 2, 4 }, testedCollection );
            Assert.Equal( 4, testedCollection.Count );
        }

        [Fact]
        public void AddToLinkedCollection()
        {
            var baseCollection = new ListEx<int>( new[] { 1, 3, 2, 4 } );

            IReadOnlyCollection<int> testedCollection = new ReadOnlyCollection<int>( baseCollection );

            Assert.Equal( new[] { 1, 3, 2, 4 }, testedCollection );

            baseCollection.Add( 9 );

            Assert.Equal( new[] { 1, 3, 2, 4, 9 }, testedCollection );
        }

        [Fact]
        public void RemoveFromLinkedCollection()
        {
            var baseCollection = new ListEx<int>( new[] { 1, 3, 2, 4 } );

            IReadOnlyCollection<int> testedCollection = new ReadOnlyCollection<int>( baseCollection );

            Assert.Equal( new[] { 1, 3, 2, 4 }, testedCollection );

            Assert.True( baseCollection.Remove( 3 ) );

            Assert.Equal( new[] { 1, 2, 4 }, testedCollection );

            Assert.False( baseCollection.Remove( 8 ) );

            Assert.Equal( new[] { 1, 2, 4 }, testedCollection );
        }

        [Fact]
        public void ClearLinkedCollection()
        {
            var baseCollection = new ListEx<int>( new[] { 1, 3, 2, 4 } );

            IReadOnlyCollection<int> testedCollection = new ReadOnlyCollection<int>( baseCollection );

            Assert.Equal( new[] { 1, 3, 2, 4 }, testedCollection );

            baseCollection.Clear();

            Assert.Empty( testedCollection );
        }

        [Fact]
        public void Contains()
        {
            IListEx<int> baseCollection = new ListEx<int>( new[] { 1, 3, 2, 4 } );

            IReadOnlyCollectionEx<int> testedCollection = new ReadOnlyCollection<int>( baseCollection );

            Assert.True( testedCollection.Contains( 1 ) );
            Assert.False( testedCollection.Contains( 5 ) );
        }

        [Fact]
        public void GetEnumerator()
        {
            IListEx<int> baseCollection = new ListEx<int>( new[] { 6, 9 } );

            IReadOnlyCollection<int> testedCollection = new ReadOnlyCollection<int>( baseCollection );

            var enumerator = testedCollection.GetEnumerator();

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 6, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 9, enumerator.Current );

            Assert.False( enumerator.MoveNext() );
        }

        [Fact]
        public void IEnumerable_GetEnumerator()
        {
            var baseCollection = new ListEx<int>( new[] { 6, 9 } );

            IReadOnlyCollection<int> testedCollection = new ReadOnlyCollection<int>( baseCollection );

            IEnumerable enumerable = testedCollection;

            var enumerator = enumerable.GetEnumerator();

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 6, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 9, enumerator.Current );

            Assert.False( enumerator.MoveNext() );
        }
    }
}
