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
        public void Add()
        {
            // Arrange

            ListEx<int> list = new();

            // Act

            var result = list.Add( 5 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5 }, list );
        }

        [Fact]
        public void ICollectionT_Add()
        {
            // Arrange

            ListEx<int> list = new();

            // Act

            ( (ICollection<int>) list ).Add( 5 );

            // Assert

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

            ListEx<int> list = new( new[] { 4, 45 } );

            // Act

            var result = ( (ICollectionEx) list ).AddRange( new object[] { 5, 8.0, 2 } );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 4, 45 }, list );
        }

        [Fact]
        public void IListExT_InsertRange()
        {
            // Arrange

            ListEx<int> list = new( new[] { 5, 8, 2 } );

            // Act

            var result = ( (IListEx) list ).InsertRange( 2, new[] { 66, 1 } );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5, 8, 66, 1 , 2 }, list );
        }

        [Fact]
        public void IListExT_InsertRange_InvalidObject()
        {
            // Arrange

            ListEx<int> list = new( new[] { 5, 8, 2 } );

            // Act

            var result = ( (IListEx) list ).InsertRange( 2, new[] { 66, 1.1 } );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5, 8, 2 }, list );
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
        public void Replace()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9 } );

            // Act

            var result = list.Replace( 8.0, 3.5 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 3.5, 2.9 }, list );
        }

        [Fact]
        public void Replace_OldNotExisting()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9 } );

            // Act

            var result = list.Replace( 8.01, 3.5 );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, list );
        }

        [Fact]
        public void ICollectionEx_Replace()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9 } );

            // Act

            var result = ( (ICollectionEx) list ).Replace( 8.0, 3.5 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 3.5, 2.9 }, list );
        }

        [Fact]
        public void ICollectionEx_Replace_InvalidObject()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9 } );

            // Act

            var result = ( (ICollectionEx) list ).Replace( 8.0, 3.5f );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, list );

            // Act

            result = ( (ICollectionEx) list ).Replace( 8.0f, 3.5 );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, list );
        }

        [Fact]
        public void ReplaceByIndex()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9 } );

            // Act

            var result = list.Replace( 1, 3.5 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 3.5, 2.9 }, list );
        }

        [Fact]
        void ReplaceByIndex_OutOfRange()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9 } );

            // Act & Assert

            Assert.Throws<ArgumentOutOfRangeException>( () => list.Replace( 3, 3.5 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => list.Replace( -1, 3.5 ) );

            // Assert state

            Assert.Equal( new[] { 5.1, 8.0, 2.9, }, list );
        }

        [Fact]
        public void IListEx_ReplaceByIndex()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9 } );

            // Act

            var result = ( (IListEx) list ).Replace( 1, 3.5 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 3.5, 2.9 }, list );
        }

        [Fact]
        public void IListEx_ReplaceByIndex_InvalidObject()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9 } );

            // Act

            var result = ( (IListEx) list ).Replace( 1, 3.5f );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, list );
        }

        [Fact]
        public void Move()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9, 44.5 } );

            // Act: Same position

            var result = list.Move( 8.0, 1 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, list );

            // Act: Position before current

            result = list.Move( 2.9, 0 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 2.9, 5.1, 8.0, 44.5 }, list );

            // Act: Position just after current

            result = list.Move( 8.0, 3 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 2.9, 5.1, 8.0, 44.5 }, list );

            // Act: Position at end

            result = list.Move( 5.1, 4 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 2.9, 8.0, 44.5, 5.1 }, list );
        }

        [Fact]
        void Move_NotExisting()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9, 44.5 } );

            // Act

            var result = list.Move( 8.01, 1 );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, list );
        }

        [Fact]
        void Move_OutOfRange()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9, 44.5 } );

            // Act & Assert

            Assert.Throws<ArgumentOutOfRangeException>( () => list.Move( 8.0, 5 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => list.Move( 8.0, -1 ) );

            // Assert state

            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, list );
        }

        [Fact]
        void MoveByIndex()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9, 44.5 } );

            // Act: Same position

            var result = list.Move( 1, 1 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, list );

            // Act: Position before current

            result = list.Move( 2, 0 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 2.9, 5.1, 8.0, 44.5 }, list );

            // Act: Position just after current

            result = list.Move( 2, 3 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 2.9, 5.1, 8.0, 44.5 }, list );

            // Act: Position at end

            result = list.Move( 5.1, 4 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 2.9, 8.0, 44.5, 5.1 }, list );
        }

        [Fact]
        void MoveByIndex_OutOfRange()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9, 44.5 } );

            // Act & Assert

            Assert.Throws<ArgumentOutOfRangeException>( () => list.Move( 0, 5 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => list.Move( 0, -1 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => list.Move( -2, 3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => list.Move( 10, 2 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => list.Move( 10, -2 ) );

            // Assert state

            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, list );
        }

        [Fact]
        void IListEx_Move()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9, 44.5 } );

            // Act

            var result = ( (IListEx) list ).Move( 8.0, 3 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 2.9, 8.0, 44.5 }, list );
        }

        [Fact]
        void IListEx_Move_InvalidObject()
        {
            // Arrange

            ListEx<double> list = new( new[] { 5.1, 8.0, 2.9, 44.5 } );

            // Act

            var result = ( (IListEx) list ).Move( 8.0f, 3 );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, list );
        }

        [Fact]
        public void GetRange()
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.Equal( new[] { 8, 2 }, list.GetRange( 1, 2 ) );
            Assert.Equal( new[] { 5 }, list.GetRange( 0, 1 ) );
        }

        [Fact]
        public void IListExT_GetRange()
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.Equal( new[] { 8, 2 }, ( (IListEx<int>) list ).GetRange( 1, 2 ) );
            Assert.Equal( new[] { 5 }, ( (IListEx<int>) list ).GetRange( 0, 1 ) );
        }

        [Fact]
        public void IReadOnlyListEx_GetRange()
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.Equal( new[] { 8, 2 }, ( (IReadOnlyListEx<int>) list ).GetRange( 1, 2 ) );
            Assert.Equal( new[] { 5 }, ( (IReadOnlyListEx<int>) list ).GetRange( 0, 1 ) );
        }

        [Fact]
        public void IListEx_GetRange()
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.Equal( new[] { 8, 2 }, (IList) ( (IListEx) list ).GetRange( 1, 2 ) );
            Assert.Equal( new[] { 5 }, (IList) ( (IListEx) list ).GetRange( 0, 1 ) );
        }

        [Fact]
        public void Slice()
        {
            // Arrange

            ListEx<int> list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.Equal( new[] { 8, 2 }, list.Slice( 1, 2 ) );
            Assert.Equal( new[] { 5 }, list.Slice( 0, 1 ) );
        }

        [Fact]
        public void IListExT_Slice()
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.Equal( new[] { 8, 2 }, ( (IListEx<int>) list ).Slice( 1, 2 ) );
            Assert.Equal( new[] { 5 }, ( (IListEx<int>) list ).Slice( 0, 1 ) );
        }

        [Fact]
        public void IReadOnlyListEx_Slice()
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.Equal( new[] { 8, 2 }, ( (IReadOnlyListEx<int>) list ).Slice( 1, 2 ) );
            Assert.Equal( new[] { 5 }, ( (IReadOnlyListEx<int>) list ).Slice( 0, 1 ) );
        }

        [Fact]
        public void IListEx_Slice()
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.Equal( new[] { 8, 2 }, (IList) ( (IListEx) list ).Slice( 1, 2 ) );
            Assert.Equal( new[] { 5 }, (IList) ( (IListEx) list ).Slice( 0, 1 ) );
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
        public void IReadOnlyListEx_IndexOf()
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
        public void IListEx_IndexOf()
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IListEx) list );

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
        public void IReadOnlyListEx_LastIndexOf()
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

        [Fact]
        public void IListEx_LastIndexOf()
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IListEx) list );

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
