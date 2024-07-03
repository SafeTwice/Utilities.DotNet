/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using Utilities.DotNet.Collections;
using Xunit;

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.

namespace Utilities.DotNet.Test.Collections
{
    public class ListExTest
    {
        [Fact]
        public void Constructor_Default()
        {
            var list = new ListEx<int>();

            Assert.Equal( 0, list.Count );
            Assert.False( ( (ICollection<int>) list ).IsReadOnly );
            Assert.False( ( (IList) list ).IsReadOnly );
            Assert.False( ( (IList) list ).IsFixedSize );
            Assert.NotNull( ( (IList) list ).SyncRoot );
            Assert.False( ( (IList) list ).IsSynchronized );

            // The following tests are to ensure interface disambiguation.

            Assert.Equal( 0, ( (IListEx<int>) list ).Count );
            Assert.Equal( 0, ( (ICollectionEx<int>) list ).Count );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            ListEx<int> list = new ListEx<int>( new[] { 5, 8, 2 } );

            Assert.Equal( new[] { 5, 8, 2 }, list );
            Assert.False( ( (IList) list ).IsReadOnly );
            Assert.False( ( (IList) list ).IsFixedSize );
            Assert.NotNull( ( (IList) list ).SyncRoot );
            Assert.False( ( (IList) list ).IsSynchronized );
        }

        [Fact]
        public void Constructor_Capacity()
        {
            var list = new ListEx<int>( 10 );

            Assert.Equal( 0, list.Count );
            Assert.Equal( 10, list.Capacity );
            Assert.False( ( (ICollection<int>) list ).IsReadOnly );
            Assert.False( ( (IList) list ).IsReadOnly );
            Assert.False( ( (IList) list ).IsFixedSize );
            Assert.NotNull( ( (IList) list ).SyncRoot );
            Assert.False( ( (IList) list ).IsSynchronized );
        }

        [Fact]
        public void GetRange()
        {
            ListEx<int> list = new ListEx<int>( new[] { 5, 8, 2 } );

            var range1 = list.GetRange( 1, 2 );

            Assert.Equal( new[] { 8, 2 }, range1 );

            var range2 = list.GetRange( 0, 1 );

            Assert.Equal( new[] { 5 }, range2 );
        }

        [Fact]
        public void IReadOnlyCollectionEx_GetRange()
        {
            ListEx<int> list = new ListEx<int>( new[] { 5, 8, 2 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            var range1 = collection.GetRange( 1, 2 );

            Assert.Equal( new[] { 8, 2 }, range1 );

            var range2 = collection.GetRange( 0, 1 );

            Assert.Equal( new[] { 5 }, range2 );
        }

        [Fact]
        public void Slice()
        {
            ListEx<int> list = new ListEx<int>( new[] { 5, 8, 2 } );

            var range1 = list.Slice( 1, 2 );

            Assert.Equal( new[] { 8, 2 }, range1 );

            var range2 = list.Slice( 0, 1 );

            Assert.Equal( new[] { 5 }, range2 );
        }

        [Fact]
        public void IReadOnlyCollectionEx_Slice()
        {
            ListEx<int> list = new ListEx<int>( new[] { 5, 8, 2 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            var range1 = collection.Slice( 1, 2 );

            Assert.Equal( new[] { 8, 2 }, range1 );

            var range2 = collection.Slice( 0, 1 );

            Assert.Equal( new[] { 5 }, range2 );
        }

        [Fact]
        public void ICollectionEx_Add()
        {
            ListEx<int> list = new();

            ( (ICollectionEx) list ).Add( 5 );

            Assert.Equal( new[] { 5 }, list );
        }

        [Fact]
        public void ICollectionEx_Add_InvalidObject()
        {
            ListEx<int> list = new();

            Assert.Throws<InvalidCastException>( () => ( (ICollectionEx) list ).Add( 5.0 ) );
        }

        [Fact]
        public void ICollectionEx_AddRange()
        {
            ListEx<int> list = new();

            ( (ICollectionEx) list ).AddRange( new[] { 5, 8, 2 } );

            Assert.Equal( new[] { 5, 8, 2 }, list );
        }

        [Fact]
        public void ICollectionEx_AddRange_InvalidObject()
        {
            ListEx<int> list = new();

            Assert.Throws<InvalidCastException>( () => ( (ICollectionEx) list ).AddRange( new object[] { 5, 8.0, 2 } ) );
        }

        [Fact]
        public void ICollectionEx_Remove()
        {
            ListEx<int> list = new( new[] { 5, 8, 2 } );

            ( (ICollectionEx) list ).Remove( 8 );

            Assert.Equal( new[] { 5, 2 }, list );
        }

        [Fact]
        public void ICollectionEx_Remove_InvalidObject()
        {
            ListEx<int> list = new( new[] { 5, 8, 2 } );

            ( (ICollectionEx) list ).Remove( 8.0 );

            Assert.Equal( new[] { 5, 8, 2 }, list );
        }

        [Fact]
        public void RemoveRange()
        {
            ListEx<int> list = new( new[] { 5, 8, 2, 9 } );

            list.RemoveRange( new[] { 8, 2 } );

            Assert.Equal( new[] { 5, 9 }, list );
        }

        [Fact]
        public void ICollectionEx_RemoveRange()
        {
            ListEx<int> list = new( new[] { 5, 8, 2, 9 } );

            ( (ICollectionEx) list ).RemoveRange( new[] { 8, 2 } );

            Assert.Equal( new[] { 5, 9 }, list );
        }

        [Fact]
        public void ICollectionEx_RemoveRange_InvalidObject()
        {
            ListEx<int> list = new( new[] { 5, 8, 2, 9 } );

            ( (ICollectionEx) list ).RemoveRange( new object[] { 8, 2.0 } );

            Assert.Equal( new[] { 5, 2, 9 }, list );
        }

        [Fact]
        public void ICollectionEx_Contains()
        {
            ListEx<int> list = new( new[] { 5, 8, 2 } );

            Assert.True( ( (ICollectionEx) list ).Contains( 8 ) );
            Assert.False( ( (ICollectionEx) list ).Contains( 9 ) );
            Assert.False( ( (ICollectionEx) list ).Contains( 5.0 ) );
        }

        [Fact]
        public void IReadOnlyCollectionEx_Contains()
        {
            ListEx<int> list = new( new[] { 5, 8, 2 } );

            Assert.True( ( (IReadOnlyCollectionEx<int>) list ).Contains( 8 ) );
            Assert.False( ( (IReadOnlyCollectionEx<int>) list ).Contains( 9 ) );
            Assert.False( ( (IReadOnlyCollectionEx<int>) list ).Contains( 5.0 ) );
        }

        [Fact]
        public void IReadOnlyCollectionEx_IndexOf()
        {
            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            Assert.Equal( 1, collection.IndexOf( 6 ) );
            Assert.Equal( -1, collection.IndexOf( 10 ) );
            Assert.Equal( -1, collection.IndexOf( 10.0 ) );

            Assert.Equal( 3, collection.IndexOf( 3, 1 ) );
            Assert.Equal( -1, collection.IndexOf( 6, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 4.0, 1 ) );

            Assert.Equal( 3, collection.IndexOf( 3, 2, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 6, 2, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 3.0, 2, 2 ) );
        }

        [Fact]
        public void IReadOnlyCollectionEx_LastIndexOf()
        {
            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            Assert.Equal( 1, collection.LastIndexOf( 6 ) );
            Assert.Equal( -1, collection.LastIndexOf( 10 ) );
            Assert.Equal( -1, collection.LastIndexOf( 10.0 ) );

            Assert.Equal( 0, collection.LastIndexOf( 3, 1 ) );
            Assert.Equal( -1, collection.LastIndexOf( 4, 1 ) );
            Assert.Equal( -1, collection.LastIndexOf( 4.0, 1 ) );

            Assert.Equal( 0, collection.LastIndexOf( 3, 1, 2 ) );
            Assert.Equal( -1, collection.LastIndexOf( 3, 2, 2 ) );
            Assert.Equal( -1, collection.LastIndexOf( 3.0, 2, 2 ) );
        }
    }
}
