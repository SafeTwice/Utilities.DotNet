/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

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
            // Arrange & Act

            var list = new ListEx<int>();

            // Assert

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
            // Arrange & Act

            ListEx<int> list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Assert

            Assert.Equal( new[] { 5, 8, 2 }, list );
            Assert.False( ( (IList) list ).IsReadOnly );
            Assert.False( ( (IList) list ).IsFixedSize );
            Assert.NotNull( ( (IList) list ).SyncRoot );
            Assert.False( ( (IList) list ).IsSynchronized );
        }

        [Fact]
        public void Constructor_Capacity()
        {
            // Arrange & Act

            var list = new ListEx<int>( 10 );

            // Assert

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
            // Arrange

            ListEx<int> list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Act

            var range1 = list.GetRange( 1, 2 );
            var range2 = list.GetRange( 0, 1 );

            // Assert

            Assert.Equal( new[] { 8, 2 }, range1 );
            Assert.Equal( new[] { 5 }, range2 );
        }

        [Fact]
        public void IReadOnlyCollectionEx_GetRange()
        {
            // Arrange

            ListEx<int> list = new ListEx<int>( new[] { 5, 8, 2 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            // Act

            var range1 = collection.GetRange( 1, 2 );
            var range2 = collection.GetRange( 0, 1 );

            // Assert

            Assert.Equal( new[] { 8, 2 }, range1 );
            Assert.Equal( new[] { 5 }, range2 );
        }

        [Fact]
        public void Slice()
        {
            // Arrange

            ListEx<int> list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Act

            var range1 = list.Slice( 1, 2 );
            var range2 = list.Slice( 0, 1 );

            // Assert

            Assert.Equal( new[] { 8, 2 }, range1 );
            Assert.Equal( new[] { 5 }, range2 );
        }

        [Fact]
        public void IReadOnlyCollectionEx_Slice()
        {
            // Arrange

            ListEx<int> list = new ListEx<int>( new[] { 5, 8, 2 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            // Act

            var range1 = collection.Slice( 1, 2 );
            var range2 = collection.Slice( 0, 1 );

            // Assert

            Assert.Equal( new[] { 8, 2 }, range1 );
            Assert.Equal( new[] { 5 }, range2 );
        }

        [Fact]
        public void ICollectionExT_TryAdd()
        {
            // Arrange

            ListEx<int> list = new();

            // Act

            var result = ( (ICollectionEx<int>) list ).TryAdd( 5 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5 }, list );
        }

        [Fact]
        public void ICollectionEx_Add()
        {
            // Arrange

            ListEx<int> list = new();

            // Act

            var result = ( (ICollectionEx) list ).Add( 5 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5 }, list );
        }

        [Fact]
        public void ICollectionEx_Add_InvalidObject()
        {
            // Arrange

            ListEx<int> list = new( new[] { 8, 2 } );

            // Act

            var result = ( (ICollectionEx) list ).Add( 5.0 );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 8, 2 }, list );
        }

        [Fact]
        public void ICollectionExT_AddRange()
        {
            // Arrange

            ListEx<int> list = new();

            // Act

            var result = ( (ICollectionEx<int>) list ).AddRange( new[] { 5, 8, 2 } );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5, 8, 2 }, list );
        }

        [Fact]
        public void ICollectionEx_AddRange()
        {
            // Arrange

            ListEx<int> list = new();

            // Act

            var result = ( (ICollectionEx) list ).AddRange( new[] { 5, 8, 2 } );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5, 8, 2 }, list );
        }

        [Fact]
        public void ICollectionEx_AddRange_InvalidObject()
        {
            // Arrange

            ListEx<int> list = new( new [] { 4, 45 } );

            // Act

            var result = ( (ICollectionEx) list ).AddRange( new object[] { 5, 8.0, 2 } );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 4, 45, 5, 2 }, list );
        }

        [Fact]
        public void ICollectionEx_Remove()
        {
            // Arrange

            ListEx<int> list = new( new[] { 5, 8, 2 } );

            // Act

            var result = ( (ICollectionEx) list ).Remove( 8 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5, 2 }, list );
        }

        [Fact]
        public void ICollectionEx_Remove_InvalidObject()
        {
            // Arrange

            ListEx<int> list = new( new[] { 5, 8, 2 } );

            // Act

            var result = ( (ICollectionEx) list ).Remove( 8.0 );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5, 8, 2 }, list );
        }

        [Fact]
        public void RemoveRange_AllRemoved()
        {
            // Arrange

            ListEx<int> list = new( new[] { 5, 8, 2, 9 } );

            // Act

            var result = list.RemoveRange( new[] { 8, 2 } );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5, 9 }, list );
        }

        [Fact]
        public void RemoveRange_SomeNotRemoved()
        {
            // Arrange

            ListEx<int> list = new( new[] { 5, 8, 2, 9 } );

            // Act

            var result = list.RemoveRange( new[] { 8, 2, 55 } );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5, 9 }, list );
        }

        [Fact]
        public void ICollectionEx_RemoveRange()
        {
            // Arrange

            ListEx<int> list = new( new[] { 5, 8, 2, 9 } );

            // Act

            var result = ( (ICollectionEx) list ).RemoveRange( new[] { 8, 2 } );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5, 9 }, list );
        }

        [Fact]
        public void ICollectionEx_RemoveRange_InvalidObject()
        {
            // Arrange

            ListEx<int> list = new( new[] { 5, 8, 2, 9 } );

            // Act

            var result = ( (ICollectionEx) list ).RemoveRange( new object[] { 8, 2.0 } );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5, 2, 9 }, list );
        }

        [Fact]
        public void ICollectionEx_Contains()
        {
            // Arrange

            ListEx<int> list = new( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.True( ( (ICollectionEx) list ).Contains( 8 ) );
            Assert.False( ( (ICollectionEx) list ).Contains( 9 ) );
            Assert.False( ( (ICollectionEx) list ).Contains( 5.0 ) );
        }

        [Fact]
        public void IReadOnlyCollectionEx_Contains()
        {
            // Arrange

            ListEx<int> list = new( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.True( ( (IReadOnlyCollectionEx<int>) list ).Contains( 8 ) );
            Assert.False( ( (IReadOnlyCollectionEx<int>) list ).Contains( 9 ) );
            Assert.False( ( (IReadOnlyCollectionEx<int>) list ).Contains( 5.0 ) );
        }

        [Fact]
        public void IReadOnlyCollectionEx_IndexOf()
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            // Act & Assert

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
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            // Act & Assert

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
