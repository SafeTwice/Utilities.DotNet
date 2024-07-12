/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using Utilities.DotNet.Collections;
using Xunit;

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.

namespace Utilities.DotNet.Test.Collections
{
    public class ReadOnlyListExTest
    {
        [Fact]
        public void Constructor_Default()
        {
            var baseList = new ListEx<int>();

            IReadOnlyList<int> testedList = new ReadOnlyListEx<int>( baseList );

            Assert.Equal( 0, testedList.Count );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            var baseList = new ListEx<int>( new[] { 8, 5, 3, 2 } );

            IReadOnlyList<int> testedList = new ReadOnlyListEx<int>( baseList );

            Assert.Equal( new[] { 8, 5, 3, 2 }, testedList );
            Assert.Equal( 4, testedList.Count );
        }

        [Fact]
        public void GetRange()
        {
            var baseList = new ListEx<int>( new[] { 8, 5, 3, 2 } );

            IReadOnlyListEx<int> testedList = new ReadOnlyListEx<int>( baseList );

            var range1 = testedList.GetRange( 1, 2 );

            Assert.Equal( new[] { 5, 3 }, range1 );

            var range2 = testedList.GetRange( 0, 1 );

            Assert.Equal( new[] { 8 }, range2 );
        }

        [Fact]
        public void Slice()
        {
            var baseList = new ListEx<int>( new[] { 8, 5, 3, 2 } );

            IReadOnlyListEx<int> testedList = new ReadOnlyListEx<int>( baseList );

            var range1 = testedList.Slice( 1, 2 );

            Assert.Equal( new[] { 5, 3 }, range1 );

            var range2 = testedList.Slice( 0, 1 );

            Assert.Equal( new[] { 8 }, range2 );
        }

        [Fact]
        public void IndexOf()
        {
            IListEx<int> baseList = new ListEx<int>( new[] { 8, 2, 5, 3, 2, 2, 8 } );

            IReadOnlyListEx<int> testedList = new ReadOnlyListEx<int>( baseList );

            Assert.Equal( -1, testedList.IndexOf( 99 ) );
            Assert.Equal( 0, testedList.IndexOf( 8 ) );
            Assert.Equal( 6, testedList.IndexOf( 8, 1 ) );
            Assert.Equal( -1, testedList.IndexOf( 2, 2, 2 ) );
            Assert.Equal( 1, testedList.IndexOf( 2 ) );
        }

        [Fact]
        public void LastIndexOf()
        {
            IListEx<int> baseList = new ListEx<int>( new[] { 8, 2, 5, 3, 2, 2, 8 } );

            IReadOnlyListEx<int> testedList = new ReadOnlyListEx<int>( baseList );

            Assert.Equal( -1, testedList.LastIndexOf( 9 ) );
            Assert.Equal( 5, testedList.LastIndexOf( 2 ) );
            Assert.Equal( 0, testedList.LastIndexOf( 8, 5 ) );
            Assert.Equal( -1, testedList.LastIndexOf( 8, 5, 3 ) );
            Assert.Equal( 6, testedList.LastIndexOf( 8 ) );
        }

        [Fact]
        public void Indexer_Get_ValidIndex()
        {
            IListEx<int> baseList = new ListEx<int>( new[] { 8, 5, 3, 2 } );

            IReadOnlyList<int> testedList = new ReadOnlyListEx<int>( baseList );

            Assert.Equal( 8, testedList[ 0 ] );
            Assert.Equal( 5, testedList[ 1 ] );
            Assert.Equal( 3, testedList[ 2 ] );
        }

        [Fact]
        public void Indexer_Get_InvalidIndex()
        {
            IListEx<int> baseList = new ListEx<int>( new[] { 8, 5, 3, 2 } );

            IReadOnlyList<int> testedList = new ReadOnlyListEx<int>( baseList );

            Assert.Throws<ArgumentOutOfRangeException>( () => testedList[ -654 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => testedList[ -1 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => testedList[ -3 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => testedList[ 34564 ] );
        }
    }
}
