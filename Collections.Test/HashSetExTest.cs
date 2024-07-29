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
    public class HashSetExTest
    {
        [Fact]
        public void Constructor_Default()
        {
            // Arrange & Act

            var set = new HashSetEx<int>();

            // Assert

            Assert.Equal( 0, set.Count );
            Assert.False( ( (ICollection<int>) set ).IsReadOnly );
            Assert.NotNull( ( (ICollection) set ).SyncRoot );
            Assert.False( ( (ICollection) set ).IsSynchronized );

            // The following tests are to ensure interface disambiguation.

            Assert.Equal( 0, ( (ISet<int>) set ).Count );
            Assert.Equal( 0, ( (ICollectionEx<int>) set ).Count );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            // Arrange & Act

            HashSetEx<int> set = new HashSetEx<int>( new[] { 5, 8, 2 } );

            // Assert

            Assert.Equal( new[] { 5, 8, 2 }, set );
            Assert.NotNull( ( (ICollection) set ).SyncRoot );
            Assert.False( ( (ICollection) set ).IsSynchronized );
        }

        [Fact]
        public void Constructor_Capacity()
        {
            // Arrange & Act

            var set = new HashSetEx<int>( 10 );

            // Assert

            Assert.Equal( 0, set.Count );
            Assert.False( ( (ICollection<int>) set ).IsReadOnly );
            Assert.NotNull( ( (ICollection) set ).SyncRoot );
            Assert.False( ( (ICollection) set ).IsSynchronized );
        }

        [Fact]
        public void Add()
        {
            // Arrange

            HashSetEx<int> set = new();

            // Act

            var result = set.Add( 5 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5 }, set );
        }

        [Fact]
        public void ICollectionT_Add()
        {
            // Arrange

            HashSetEx<int> set = new();

            // Act

            ( (ICollection<int>) set ).Add( 5 );

            // Assert

            Assert.Equal( new[] { 5 }, set );
        }

        [Fact]
        public void ICollectionEx_Add()
        {
            // Arrange

            HashSetEx<int> set = new();

            // Act

            var result = ( (ICollectionEx) set ).Add( 5 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5 }, set );
        }

        [Fact]
        public void ICollectionEx_Add_InvalidObject()
        {
            // Arrange

            HashSetEx<int> set = new( new[] { 8, 2 } );

            // Act

            var result = ( (ICollectionEx) set ).Add( 5.0 );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 8, 2 }, set );
        }

        [Fact]
        public void AddRange_AllValid()
        {
            // Arrange

            HashSetEx<int> set = new();

            // Act

            var result = set.AddRange( new[] { 5, 8, 2 } );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5, 8, 2 }, set );
        }

        [Fact]
        public void AddRange_SomeAlreadyExist()
        {
            // Arrange

            HashSetEx<int> set = new( new[] { 5 } );

            // Act

            var result = set.AddRange( new[] { 5, 8, 2 } );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5, 8, 2 }, set );
        }

        [Fact]
        public void ICollectionEx_AddRange()
        {
            // Arrange

            HashSetEx<int> set = new();

            // Act

            var result = ( (ICollectionEx) set ).AddRange( new[] { 5, 8, 2 } );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5, 8, 2 }, set );
        }

        [Fact]
        public void ICollectionEx_AddRange_InvalidObject()
        {
            // Arrange

            HashSetEx<int> set = new( new[] { 4, 45 } );

            // Act

            var result = ( (ICollectionEx) set ).AddRange( new object[] { 5, 8.0, 2 } );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 4, 45 }, set );
        }

        [Fact]
        public void ICollectionEx_Remove()
        {
            // Arrange

            HashSetEx<int> set = new( new[] { 5, 8, 2 } );

            // Act

            var result = ( (ICollectionEx) set ).Remove( 8 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5, 2 }, set );
        }

        [Fact]
        public void ICollectionEx_Remove_InvalidObject()
        {
            // Arrange

            HashSetEx<int> set = new( new[] { 5, 8, 2 } );

            // Act

            var result = ( (ICollectionEx) set ).Remove( 8.0 );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5, 8, 2 }, set );
        }

        [Fact]
        public void RemoveRange_AllRemoved()
        {
            // Arrange

            HashSetEx<int> set = new( new[] { 5, 8, 2, 9 } );

            // Act

            var result = set.RemoveRange( new[] { 8, 2 } );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5, 9 }, set );
        }

        [Fact]
        public void RemoveRange_SomeNotRemoved()
        {
            // Arrange

            HashSetEx<int> set = new( new[] { 5, 8, 2, 9 } );

            // Act

            var result = set.RemoveRange( new[] { 8, 2, 55 } );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5, 9 }, set );
        }

        [Fact]
        public void ICollectionEx_RemoveRange()
        {
            // Arrange

            HashSetEx<int> set = new( new[] { 5, 8, 2, 9 } );

            // Act

            var result = ( (ICollectionEx) set ).RemoveRange( new[] { 8, 2 } );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5, 9 }, set );
        }

        [Fact]
        public void ICollectionEx_RemoveRange_InvalidObject()
        {
            // Arrange

            HashSetEx<int> set = new( new[] { 5, 8, 2, 9 } );

            // Act

            var result = ( (ICollectionEx) set ).RemoveRange( new object[] { 8, 2.0 } );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5, 2, 9 }, set );
        }

        [Fact]
        public void Replace()
        {
            // Arrange

            HashSetEx<double> set = new( new[] { 5.1, 8.0, 2.9 } );

            // Act

            var result = set.Replace( 8.0, 3.5 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 3.5, 2.9 }, set );
        }

        [Fact]
        public void Replace_OldNotExisting()
        {
            // Arrange

            HashSetEx<double> set = new( new[] { 5.1, 8.0, 2.9 } );

            // Act

            var result = set.Replace( 8.01, 3.5 );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, set );
        }

        [Fact]
        public void Replace_NewExisting()
        {
            // Arrange

            HashSetEx<double> set = new( new[] { 5.1, 8.0, 2.9 } );

            // Act

            var result = set.Replace( 8.0, 5.1 );

            // Assert

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, set );
        }

        [Fact]
        public void Replace_Same()
        {
            // Arrange

            HashSetEx<double> set = new( new[] { 5.1, 8.0, 2.9 } );

            // Act

            var result = set.Replace( 8.0, 8.0 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, set );
        }

        [Fact]
        public void ICollectionEx_Replace()
        {
            // Arrange

            HashSetEx<double> list = new( new[] { 5.1, 8.0, 2.9 } );

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

            HashSetEx<double> list = new( new[] { 5.1, 8.0, 2.9 } );

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
        public void ICollectionEx_Contains()
        {
            // Arrange

            HashSetEx<int> set = new( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.True( ( (ICollectionEx) set ).Contains( 8 ) );
            Assert.False( ( (ICollectionEx) set ).Contains( 9 ) );
            Assert.False( ( (ICollectionEx) set ).Contains( 5.0 ) );
        }

        [Fact]
        public void IReadOnlyCollectionEx_Contains()
        {
            // Arrange

            HashSetEx<int> set = new( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.True( ( (IReadOnlyCollectionEx<int>) set ).Contains( 8 ) );
            Assert.False( ( (IReadOnlyCollectionEx<int>) set ).Contains( 9 ) );
            Assert.False( ( (IReadOnlyCollectionEx<int>) set ).Contains( 5.0 ) );
        }

        [Fact]
        public void IReadOnlySetEx_SetComparisons()
        {
            // Arrange

            var set = new HashSetEx<int>( new[] { 8, 2, 23 } );

            var readonlySet = (IReadOnlySetEx<int>) set;

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

        [Fact]
        public void ICollection_CopyTo()
        {
            // Arrange

            var set = new HashSetEx<int>( new[] { 23, 34, 2 } );

            var array = new int[ 3 ];

            // Act

            ( (ICollection) set ).CopyTo( array, 0 );

            // Assert result

            Assert.Equal( new[] { 23, 34, 2 }, array );
        }
    }
}
