/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

#pragma warning disable IDE0079
#pragma warning disable xUnit2013 // Assert.Equal(0, collection.Count) is used because we want to check the result of the Count property, not the collection itself.
#pragma warning disable xUnit1045
#pragma warning disable CA1825 // Using zero-length array initializers is intentionally to improve test maintainability.
#pragma warning disable CA1859 // Using interface types is intentional to call the interface methods.
#pragma warning disable CA1861 // Using constant arrays is intentionally to improve test maintainability.
#pragma warning restore IDE0079

#pragma warning disable S101

namespace Utilities.DotNet.Collections.Test
{
    public class ListExTest
    {
        private static readonly TestClass ITEM1 = new( "Item1", 10 );
        private static readonly TestClass ITEM2 = new( "Item2", 5 );
        private static readonly TestClass ITEM3 = new( "Item3", 20 );

        [Fact]
        public void Constructor_Default()
        {
            // Act

            var list = new ListEx<int>();

            // Assert

            Assert.Equal( 0, list.Count );
            Assert.Equal( new int[] { }, list );

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
            // Act

            var list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Assert

            Assert.Equal( 3, list.Count );
            Assert.Equal( new[] { 5, 8, 2 }, list );

            Assert.False( ( (ICollection<int>) list ).IsReadOnly );

            Assert.False( ( (IList) list ).IsReadOnly );
            Assert.False( ( (IList) list ).IsFixedSize );
            Assert.NotNull( ( (IList) list ).SyncRoot );
            Assert.False( ( (IList) list ).IsSynchronized );
        }

        [Fact]
        public void Constructor_Capacity()
        {
            // Act

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

        public class Add_Class_NonNullable_TestData : TheoryData<IEnumerable<TestClass>, TestClass, int, IEnumerable<TestClass>>
        {
            public Add_Class_NonNullable_TestData()
            {
                Add( new TestClass[] { },
                     ITEM2,
                     0,
                     new TestClass[] { ITEM2 } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     ITEM2,
                     2,
                     new TestClass[] { ITEM1, ITEM3, ITEM2 } );
            }
        }

        [Theory]
        [ClassData( typeof( Add_Class_NonNullable_TestData ) )]
        public void Add_Class_NonNullable( IEnumerable<TestClass> initialState, TestClass addedItem, int expectedResult, IEnumerable expectedState )
        {
            // Arrange

            IListEx<TestClass> list = new ListEx<TestClass>( initialState );

            // Act

            var result = list.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        public class Add_Class_Nullable_TestData : TheoryData<IEnumerable<TestClass?>, TestClass?, int, IEnumerable<TestClass?>>
        {
            public Add_Class_Nullable_TestData()
            {
                Add( new TestClass?[] { },
                     ITEM2,
                     0,
                     new TestClass?[] { ITEM2 } );
                Add( new TestClass?[] { ITEM1, null },
                     ITEM2,
                     2,
                     new TestClass?[] { ITEM1, null, ITEM2 } );
                Add( new TestClass?[] { ITEM1, null, ITEM3 },
                     null,
                     3,
                     new TestClass?[] { ITEM1, null, ITEM3, null } );
            }
        }

        [Theory]
        [ClassData( typeof( Add_Class_Nullable_TestData ) )]
        public void Add_Class_Nullable( IEnumerable<TestClass?> initialState, TestClass? addedItem, int expectedResult, IEnumerable expectedState )
        {
            // Arrange

            IListEx<TestClass?> list = new ListEx<TestClass?>( initialState );

            // Act

            var result = list.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object[] { },
                     5,
                     0,
                     new object?[] { 5 } )]
        [InlineData( new object[] { 1, 2, 3 },
                     4,
                     3,
                     new object?[] { 1, 2, 3, 4 } )]
        public void Add_Value_NonNullable( IEnumerable initialState, int addedItem, int expectedResult, IEnumerable expectedState )
        {
            // Arrange

            IListEx<int> list = new ListEx<int>( initialState.Cast<int>() );

            // Act

            var result = list.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object?[] { },
                     5,
                     0,
                     new object?[] { 5 } )]
        [InlineData( new object?[] { },
                     null,
                     0,
                     new object?[] { null } )]
        [InlineData( new object?[] { 1, null, 3 },
                     4,
                     3,
                     new object?[] { 1, null, 3, 4 } )]
        [InlineData( new object?[] { 1, 2, 3 },
                     null,
                     3,
                     new object?[] { 1, 2, 3, null } )]
        public void Add_Value_Nullable( IEnumerable initialState, int? addedItem, int expectedResult, IEnumerable expectedState )
        {
            // Arrange

            IListEx<int?> list = new ListEx<int?>( initialState.Cast<int?>() );

            // Act

            var result = list.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        public class IListEx_Add_Class_TestData : TheoryData<IEnumerable<TestClass>, TestClass?, int, IEnumerable<TestClass?>>
        {
            public IListEx_Add_Class_TestData()
            {
                Add( new TestClass[] { },
                     ITEM2,
                     0,
                     new TestClass?[] { ITEM2 } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     ITEM2,
                     2,
                     new TestClass?[] { ITEM1, ITEM3, ITEM2 } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     null,
                     2,
                     new TestClass?[] { ITEM1, ITEM3, null } );
            }
        }

        [Theory]
        [ClassData( typeof( IListEx_Add_Class_TestData ) )]
        public void IListEx_Add_Class( IEnumerable<TestClass> initialState, TestClass? addedItem, int expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<TestClass>( initialState );
            var testedCollection = (IListEx) list;

            // Act

            var result = testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object[] { 45 },
                     9,
                     1,
                     new object?[] { 45, 9 } )]
        [InlineData( new object[] { },
                     11,
                     0,
                     new object?[] { 11 } )]
        public void IListEx_Add_Value_NonNullable( IEnumerable initialState, object? addedItem, int expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int>( initialState.Cast<int>() );
            var testedCollection = (IListEx) list;

            // Act

            var result = testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object[] { 45 },
                     new object?[] { 45 } )]
        [InlineData( new object[] { },
                     new object?[] { } )]
        public void IListEx_Add_Value_NonNullable_NullValue( IEnumerable initialState, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int>( initialState.Cast<int>() );
            var testedCollection = (IListEx) list;

            // Act & Assert

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Add( null ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object?[] { 45 },
                     9,
                     1,
                     new object?[] { 45, 9 } )]
        [InlineData( new object?[] { 5, 3 },
                     null,
                     2,
                     new object?[] { 5, 3, null } )]
        public void IListEx_Add_Value_Nullable( IEnumerable initialState, object? addedItem, int expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int?>( initialState.Cast<int?>() );
            var testedCollection = (IListEx) list;

            // Act

            var result = testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 23 )]
        [InlineData( 45.0 )]
        public void IListEx_Add_InvalidType( object? addedItem )
        {
            // Arrange

            var list = new ListEx<TestClass>() { ITEM1 };
            var testedCollection = (IListEx) list;

            // Act & Assert

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Add( addedItem ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( new TestClass[] { ITEM1 }, list );
        }

        [Theory]
        [ClassData( typeof( IListEx_Add_Class_TestData ) )]
        public void ICollectionEx_Add_Class( IEnumerable<TestClass> initialState, object? addedItem, int _, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<TestClass>( initialState );
            var testedCollection = (ICollectionEx) list;

            // Act

            testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object[] { 45 },
                     9,
                     new object[] { 45, 9 } )]
        [InlineData( new object[] { },
                     11,
                     new object[] { 11 } )]
        public void ICollectionEx_Add_Value_NonNullable( IEnumerable initialState, object? addedItem, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int>( initialState.Cast<int>() );
            var testedCollection = (ICollectionEx) list;

            // Act

            testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object[] { 45 },
                     new object?[] { 45 } )]
        [InlineData( new object[] { },
                     new object?[] { } )]
        public void ICollectionEx_Add_Value_NonNullable_NullValue( IEnumerable initialState, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int>( initialState.Cast<int>() );
            var testedCollection = (ICollectionEx) list;

            // Act & Assert

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Add( null ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object?[] { 45 },
                     9,
                     new object?[] { 45, 9 } )]
        [InlineData( new object?[] { 3, 4 },
                     null,
                     new object?[] { 3, 4, null } )]
        public void ICollectionEx_Add_Value_Nullable( IEnumerable initialState, object? addedItem, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int?>( initialState.Cast<int?>() );
            var testedCollection = (ICollectionEx) list;

            // Act

            testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 23 )]
        [InlineData( 45.0 )]
        public void ICollectionEx_Add_InvalidType( object? addedItem )
        {
            // Arrange

            var list = new ListEx<TestClass>() { ITEM1 };
            var testedCollection = (ICollectionEx) list;

            // Act & Assert

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Add( addedItem ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( new TestClass[] { ITEM1 }, list );
        }

        public class ICollectionEx_AddRange_Class_TestData : TheoryData<IEnumerable<TestClass>, IEnumerable<TestClass?>, IEnumerable<TestClass?>>
        {
            public ICollectionEx_AddRange_Class_TestData()
            {
                Add( new TestClass[] { },
                     new TestClass?[] { ITEM2 },
                     new TestClass?[] { ITEM2 } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     new TestClass?[] { null, ITEM3 },
                     new TestClass?[] { ITEM1, ITEM3, null, ITEM3 } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     new TestClass?[] { null },
                     new TestClass?[] { ITEM1, ITEM3, null } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_AddRange_Class_TestData ) )]
        public void ICollectionEx_AddRange_Class( IEnumerable<TestClass> initialState, IEnumerable addedItems, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<TestClass>( initialState );
            var testedCollection = (ICollectionEx) list;

            // Act

            testedCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object?[] { 5, 8, 2 },
                     new object?[] { 4, 45, 5, 8, 2 } )]
        [InlineData( new object?[] { },
                     new object?[] { 4, 45 } )]
        public void ICollectionEx_AddRange_Value_NonNullable( IEnumerable addedItems, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int>() { 4, 45 };
            var testedCollection = (ICollectionEx) list;

            // Act

            testedCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Fact]
        public void ICollectionEx_AddRange_Value_NonNullable_NullValue()
        {
            // Arrange

            var list = new ListEx<int>( new[] { 45 } );
            var testedCollection = (ICollectionEx) list;

            // Act & Assert

            Assert.Throws<NullReferenceException>( () => testedCollection.AddRange( new object?[] { 2, null, 3 } ) );

            // Assert

            Assert.Equal( new[] { 45 }, list );
        }

        [Theory]
        [InlineData( new object?[] { 5, 8, 2 },
                     new object?[] { 4, null, 45, 5, 8, 2 } )]
        [InlineData( new object?[] { 33, null },
                     new object?[] { 4, null, 45, 33, null } )]
        public void ICollectionEx_AddRange_Value_Nullable( IEnumerable addedItems, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int?>() { 4, null, 45 };
            var testedCollection = (ICollectionEx) list;

            // Act

            testedCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Fact]
        public void ICollectionEx_AddRange_InvalidType()
        {
            // Arrange

            var list = new ListEx<TestClass>() { ITEM3 };
            var testedCollection = (ICollectionEx) list;

            // Act & Assert

            Assert.Throws<InvalidCastException>( () => testedCollection.AddRange( new[] { 2 } ) );

            // Assert

            Assert.Equal( new[] { ITEM3 }, list );
        }

        public class IListEx_InsertRange_Class_TestData : TheoryData<IEnumerable<TestClass>, int, IEnumerable<TestClass?>, IEnumerable<TestClass?>>
        {
            public IListEx_InsertRange_Class_TestData()
            {
                Add( new TestClass[] { },
                     0,
                     new TestClass?[] { ITEM2 },
                     new TestClass?[] { ITEM2 } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     1,
                     new TestClass?[] { ITEM2, null },
                     new TestClass?[] { ITEM1, ITEM2, null, ITEM3 } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     0,
                     new TestClass?[] { null },
                     new TestClass?[] { null, ITEM1, ITEM3 } );
            }
        }

        [Theory]
        [ClassData( typeof( IListEx_InsertRange_Class_TestData ) )]
        public void IListEx_InsertRange_Class( IEnumerable<TestClass> initialState, int index, IEnumerable insertedItems, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<TestClass>( initialState );
            var testedCollection = (IListEx) list;

            // Act

            testedCollection.InsertRange( index, insertedItems );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 2,
                     new object?[] { 66, 1 },
                     new object?[] { 5, 8, 66, 1, 2 } )]
        [InlineData( 0,
                     new object?[] { 66, 1 },
                     new object?[] { 66, 1, 5, 8, 2 } )]
        [InlineData( 3,
                     new object?[] { 66, 1 },
                     new object?[] { 5, 8, 2, 66, 1 } )]
        public void IListEx_InsertRange_Value_NonNullable( int index, IEnumerable insertedItems, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int>() { 5, 8, 2 };
            var testedCollection = (IListEx) list;

            // Act

            testedCollection.InsertRange( index, insertedItems );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Fact]
        public void IListEx_InsertRange_Value_NonNullable_NullValue()
        {
            // Arrange

            var list = new ListEx<int>( new[] { 45 } );
            var testedCollection = (IListEx) list;

            // Act & Assert

            Assert.Throws<NullReferenceException>( () => testedCollection.InsertRange( 0, new object?[] { 2, null } ) );

            // Assert

            Assert.Equal( new[] { 45 }, list );
        }

        [Theory]
        [InlineData( 3,
                     new[] { 66, 1 },
                     new object?[] { 5, null, 8, 66, 1, 2 } )]
        [InlineData( 0,
                     new object?[] { null, 1 },
                     new object?[] { null, 1, 5, null, 8, 2 } )]
        public void IListEx_InsertRange_Value_Nullable( int index, IEnumerable insertedItems, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int?>() { 5, null, 8, 2 };
            var testedCollection = (IListEx) list;

            // Act

            testedCollection.InsertRange( index, insertedItems );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Fact]
        public void IListEx_InsertRange_InvalidType()
        {
            // Arrange

            var list = new ListEx<TestClass>() { ITEM3 };
            var testedCollection = (IListEx) list;

            // Act & Assert

            Assert.Throws<InvalidCastException>( () => testedCollection.InsertRange( 0, new[] { 2 } ) );

            // Assert

            Assert.Equal( new[] { ITEM3 }, list );
        }

        [Theory]
        [InlineData( -435 )]
        [InlineData( -1 )]
        [InlineData( 5 )]
        [InlineData( 435 )]
        public void IListEx_InsertRange_OutOfRange( int index )
        {
            // Arrange

            var list = new ListEx<int?>() { 5, null, 8, 2 };
            var testedCollection = (IListEx) list;

            // Act

            var exception = Assert.Throws<ArgumentOutOfRangeException>( () => testedCollection.InsertRange( index, new[] { 66, 1 } ) );

            // Assert

            Assert.Equal( "index", exception.ParamName );
            Assert.Equal( new int?[] { 5, null, 8, 2 }, list );
        }

        public class ICollectionEx_Remove_Class_TestData : TheoryData<IEnumerable<TestClass>, object?, bool, IEnumerable<TestClass?>>
        {
            public ICollectionEx_Remove_Class_TestData()
            {
                Add( new TestClass[] { },
                     ITEM2,
                     false,
                     new TestClass?[] { } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     ITEM3,
                     true,
                     new TestClass?[] { ITEM1 } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     null,
                     false,
                     new TestClass?[] { ITEM1, ITEM3 } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     25,
                     false,
                     new TestClass?[] { ITEM1, ITEM3 } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Remove_Class_TestData ) )]
        public void ICollectionEx_Remove_Class( IEnumerable<TestClass> initialState, object? removedItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<TestClass>( initialState );
            var testedCollection = (ICollectionEx) list;

            // Act

            var result = testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object[] { 5, 8, 2 },
                     8,
                     true,
                     new object?[] { 5, 2 } )]
        [InlineData( new object[] { 5, 8, 2 },
                     9,
                     false,
                     new object?[] { 5, 8, 2 } )]
        [InlineData( new object[] { 5, 8, 2 },
                     null,
                     false,
                     new object?[] { 5, 8, 2 } )]
        [InlineData( new object[] { 5, 8, 2 },
                     8.0,
                     false,
                     new object?[] { 5, 8, 2 } )]
        public void ICollectionEx_Remove_Value_NonNullable( IEnumerable initialState, object? removedItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int>( initialState.Cast<int>() );
            var testedCollection = (ICollectionEx) list;

            // Act

            var result = testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object?[] { 5, 8, 2, null },
                     8,
                     true,
                     new object?[] { 5, 2, null } )]
        [InlineData( new object?[] { 5, 8, 2, null },
                     null,
                     true,
                     new object?[] { 5, 8, 2 } )]
        [InlineData( new object?[] { 5, 8, 2, null },
                     9,
                     false,
                     new object?[] { 5, 8, 2, null } )]
        [InlineData( new object?[] { 5, 8 },
                     null,
                     false,
                     new object?[] { 5, 8 } )]
        [InlineData( new object?[] { 5, 8 },
                     8.0,
                     false,
                     new object?[] { 5, 8 } )]
        public void ICollectionEx_Remove_Value_Nullable( IEnumerable initialState, object? removedItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int?>( initialState.Cast<int?>() );
            var testedCollection = (ICollectionEx) list;

            // Act

            var result = testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        public class RemoveRange_Class_TestData : TheoryData<IEnumerable<TestClass?>, IEnumerable<TestClass?>, IEnumerable<TestClass?>>
        {
            public RemoveRange_Class_TestData()
            {
                Add( new TestClass?[] { },
                     new TestClass?[] { ITEM2 },
                     new TestClass?[] { } );
                Add( new TestClass?[] { ITEM1, ITEM3 },
                     new TestClass?[] { null, ITEM3 },
                     new TestClass?[] { ITEM1 } );
                Add( new TestClass?[] { ITEM1, ITEM3 },
                     new TestClass?[] { null },
                     new TestClass?[] { ITEM1, ITEM3 } );
                Add( new TestClass?[] { ITEM1, null, ITEM3 },
                     new TestClass?[] { null, ITEM2 },
                     new TestClass?[] { ITEM1, ITEM3 } );
                Add( new TestClass?[] { null, ITEM1, ITEM3 },
                     new TestClass?[] { ITEM3 },
                     new TestClass?[] { null, ITEM1 } );
            }
        }

        [Theory]
        [ClassData( typeof( RemoveRange_Class_TestData ) )]
        public void RemoveRange_Class( IEnumerable<TestClass?> initialState, IEnumerable<TestClass?> removedItems, IEnumerable expectedState )
        {
            // Arrange

            IListEx<TestClass?> list = new ListEx<TestClass?>( initialState );

            // Act

            list.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object[] { 5, 8, 2, 9 },
                     new object?[] { 8, 2 },
                     new object?[] { 5, 9 } )] // All items removed
        [InlineData( new object[] { 5, 8, 2, 9 },
                     new object?[] { 8, 55 },
                     new object?[] { 5, 2, 9 } )] // Some items not removed
        public void RemoveRange_Value_NonNullable( IEnumerable initialState, IEnumerable removedItems, IEnumerable expectedState )
        {
            // Arrange

            IListEx<int> list = new ListEx<int>( initialState.Cast<int>() );

            // Act

            list.RemoveRange( removedItems.Cast<int>() );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object?[] { 5, null, 8, 2, 9 },
                     new object?[] { 8, 2 },
                     new object?[] { 5, null, 9 } )] // All items removed
        [InlineData( new object?[] { 5, null, 8, 2, 9 },
                     new object?[] { 8, 55 },
                     new object?[] { 5, null, 2, 9 } )] // Some items not removed
        [InlineData( new object?[] { 5, null, 8, 2, 9 },
                     new object?[] { 2, null },
                     new object?[] { 5, 8, 9 } )] // Null item removed
        [InlineData( new object?[] { 5, 8, 2, 9 },
                     new object?[] { 2, null },
                     new object?[] { 5, 8, 9 } )] // Null item not removed
        public void RemoveRange_Value_Nullable( IEnumerable initialState, IEnumerable removedItems, IEnumerable expectedState )
        {
            // Arrange

            IListEx<int?> list = new ListEx<int?>( initialState.Cast<int?>() );

            // Act

            list.RemoveRange( removedItems.Cast<int?>() );

            // Assert

            Assert.Equal( expectedState, list );
        }

        public class ICollectionEx_RemoveRange_Class_TestData : TheoryData<IEnumerable<TestClass?>, IEnumerable<object?>, IEnumerable<TestClass?>>
        {
            public ICollectionEx_RemoveRange_Class_TestData()
            {
                Add( new TestClass?[] { },
                     new object?[] { ITEM2 },
                     new TestClass?[] { } );
                Add( new TestClass?[] { ITEM1, ITEM3 },
                     new object?[] { null, ITEM3 },
                     new TestClass?[] { ITEM1 } );
                Add( new TestClass?[] { ITEM1, ITEM3 },
                     new object?[] { null },
                     new TestClass?[] { ITEM1, ITEM3 } );
                Add( new TestClass?[] { ITEM1, null, ITEM3 },
                     new object?[] { null, ITEM2 },
                     new TestClass?[] { ITEM1, ITEM3 } );
                Add( new TestClass?[] { null, ITEM1, ITEM3 },
                     new object?[] { ITEM3 },
                     new TestClass?[] { null, ITEM1 } );
                Add( new TestClass?[] { ITEM1, ITEM3, ITEM2 },
                     new object?[] { ITEM3, 29.9, ITEM1 },
                     new TestClass?[] { ITEM2 } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_RemoveRange_Class_TestData ) )]
        public void ICollectionEx_RemoveRange_Class( IEnumerable initialState, IEnumerable removedItems, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<TestClass>( initialState.Cast<TestClass>() );
            var testedCollection = (ICollectionEx) list;

            // Act

            testedCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object?[] { 8, 2 },
                     new object?[] { 5, 9 } )] // All items removed
        [InlineData( new object?[] { 8, 55 },
                     new object?[] { 5, 2, 9 } )] // Some items not removed
        [InlineData( new object?[] { 2, null },
                     new object?[] { 5, 8, 9 } )] // Null item not removed
        [InlineData( new object?[] { 8, 2.0, 5 },
                     new object?[] { 2, 9 } )] // Invalid item not removed
        public void ICollectionEx_RemoveRange_Value_NonNullable( IEnumerable removedItems, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int>() { 5, 8, 2, 9 };
            var testedCollection = (ICollectionEx) list;

            // Act

            testedCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( new object?[] { 8, 2 },
                     new object?[] { 5, null, 9 } )] // All items removed
        [InlineData( new object?[] { 8, 55 },
                     new object?[] { 5, 2, null, 9 } )] // Some items not removed
        [InlineData( new object?[] { 2, null },
                     new object?[] { 5, 8, 9 } )] // Null item removed
        [InlineData( new object?[] { 8, 2.0, null },
                     new object?[] { 5, 2, 9 } )] // Invalid item not removed
        public void ICollectionEx_RemoveRange_Value_Nullable( IEnumerable removedItems, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<int?>() { 5, 8, 2, null, 9 };
            var testedCollection = (ICollectionEx) list;

            // Act

            testedCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, list );
        }

        public class Replace_Class_TestData : TheoryData<IEnumerable<TestClass?>, TestClass?, TestClass?, bool, IEnumerable<TestClass?>>
        {
            public Replace_Class_TestData()
            {
                Add( new TestClass?[] { ITEM2, ITEM3, ITEM2 },
                     ITEM2,
                     ITEM1,
                     true,
                     new TestClass?[] { ITEM1, ITEM3, ITEM2 } );
                Add( new TestClass?[] { },
                     ITEM2,
                     ITEM3,
                     false,
                     new TestClass?[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( Replace_Class_TestData ) )]
        public void Replace_Class( IEnumerable<TestClass?> initialState, TestClass? oldItem, TestClass? newItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            ListEx<TestClass?> list = new ListEx<TestClass?>( initialState );

            // Act

            var result = list.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 8.0,
                     3.5,
                     true,
                     new[] { 5.1, 3.5, 2.9 } )] // Valid replacement
        [InlineData( 8.01,
                     3.5,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (not found)
        public void Replace_Value_NonNullable( double oldItem, double newItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            IListEx<double> list = new ListEx<double>() { 5.1, 8.0, 2.9 };

            // Act

            var result = list.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 8.0,
                     3.5,
                     true,
                     new object?[] { 5.1, 3.5, null, 2.9 } )] // Valid replacement (non-null with non-null)
        [InlineData( null,
                     3.5,
                     true,
                     new object?[] { 5.1, 8.0, 3.5, 2.9 } )] // Valid replacement (null with non-null)
        [InlineData( 8.0,
                     null,
                     true,
                     new object?[] { 5.1, null, null, 2.9 } )] // Valid replacement (non-null with null)
        [InlineData( 8.01,
                     3.5,
                     false,
                     new object?[] { 5.1, 8.0, null, 2.9 } )] // Invalid replacement (non-null not found)
        [InlineData( 2.91,
                     null,
                     false,
                     new object?[] { 5.1, 8.0, null, 2.9 } )] // Invalid replacement (non-null not found)
        public void Replace_Value_Nullable( double? oldItem, double? newItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            IListEx<double?> list = new ListEx<double?>() { 5.1, 8.0, null, 2.9 };

            // Act

            var result = list.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 8.0,
                     3.5,
                     true,
                     new[] { 5.1, 3.5, 2.9 } )] // Valid replacement (both items non-null)
        [InlineData( null,
                     3.5,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (old item null)
        [InlineData( null,
                     null,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (both items null)
        [InlineData( 8.01,
                     3.5,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (old item not found, new item valid)
        [InlineData( 8.01,
                     9.5f,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (old item not found, new item invalid)
        [InlineData( 8.01,
                     null,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (old item not found, new item null)
        [InlineData( 8.0f,
                     3.5,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (old item invalid)
        [InlineData( 8.0f,
                     3.5f,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (both items invalid)
        public void ICollectionEx_Replace_Value_NonNullable( object? oldItem, object? newItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<double>() { 5.1, 8.0, 2.9 };
            var testedCollection = (ICollectionEx) list;

            // Act

            var result = testedCollection.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 8.0, null )]
        public void ICollectionEx_Replace_Value_NonNullable_NullValue( object? oldItem, object? newItem )
        {
            // Arrange

            var list = new ListEx<double>() { 5.1, 8.0, 2.9 };
            var testedCollection = (ICollectionEx) list;

            // Act

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Replace( oldItem, newItem ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, list );
        }

        [Theory]
        [InlineData( 8.0, 3.5f )]
        public void ICollectionEx_Replace_Value_NonNullable_InvalidType( object? oldItem, object? newItem )
        {
            // Arrange

            var list = new ListEx<double>() { 5.1, 8.0, 2.9 };
            var testedCollection = (ICollectionEx) list;

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Replace( oldItem, newItem ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, list );
        }

        [Theory]
        [InlineData( 8.0,
                     3.5,
                     true,
                     new object?[] { 5.1, 3.5, 2.9, null } )] // Valid replacement (both items non-null)
        [InlineData( null,
                     3.5,
                     true,
                     new object?[] { 5.1, 8.0, 2.9, 3.5 } )] // Valid replacement (old item null)
        [InlineData( 8.0,
                     null,
                     true,
                     new object?[] { 5.1, null, 2.9, null } )] // Valid replacement (new item null)
        [InlineData( null,
                     null,
                     true,
                     new object?[] { 5.1, 8.0, 2.9, null } )] // Valid replacement (both items null)
        [InlineData( 8.01,
                     3.5,
                     false,
                     new object?[] { 5.1, 8.0, 2.9, null } )] // Invalid replacement (old item not found)
        [InlineData( 5.09,
                     null,
                     false,
                     new object?[] { 5.1, 8.0, 2.9, null } )] // Invalid replacement (old item not found)
        [InlineData( 8.0f,
                     3.5,
                     false,
                     new object?[] { 5.1, 8.0, 2.9, null } )] // Invalid replacement (old item invalid)
        [InlineData( 8.0f,
                     null,
                     false,
                     new object?[] { 5.1, 8.0, 2.9, null } )] // Invalid replacement (old item invalid)
        public void ICollectionEx_Replace_Value_Nullable( object? oldItem, object? newItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<double?>() { 5.1, 8.0, 2.9, null };
            var testedCollection = (ICollectionEx) list;

            // Act

            var result = testedCollection.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 8.0, 3.5f )]
        [InlineData( null, 3.5f )]
        public void ICollectionEx_Replace_Value_Nullable_InvalidType( object? oldItem, object? newItem )
        {
            // Arrange

            var list = new ListEx<double?>() { 5.1, 8.0, null, 2.9 };
            var testedCollection = (ICollectionEx) list;

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Replace( oldItem, newItem ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new double?[] { 5.1, 8.0, null, 2.9 }, list );
        }

        [Theory]
        [InlineData( 1,
                     3.5,
                     new object[] { 5.1, 3.5, 2.9 } )] // Valid replacement by index
        [InlineData( 0,
                     13.2,
                     new object[] { 13.2, 8.0, 2.9 } )] // Valid replacement by index
        public void ReplaceByIndex_Value_NonNullable( int index, double newValue, IEnumerable expectedState )
        {
            // Arrange

            IListEx<double> list = new ListEx<double>() { 5.1, 8.0, 2.9 };

            // Act

            list.Replace( index, newValue );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 1,
                     3.5,
                     new object?[] { 5.1, 3.5, null, 2.9 } )]
        [InlineData( 0,
                     null,
                     new object?[] { null, 8.0, null, 2.9 } )]

        public void ReplaceByIndex_Value_Nullable( int index, double? newValue, IEnumerable expectedState )
        {
            // Arrange

            IListEx<double?> list = new ListEx<double?>() { 5.1, 8.0, null, 2.9 };

            // Act

            list.Replace( index, newValue );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( -134 )]
        [InlineData( -1 )]
        [InlineData( 3 )]
        [InlineData( 433 )]
        void ReplaceByIndex_Value_OutOfRange( int index )
        {
            // Arrange

            IListEx<double> list = new ListEx<double>( new[] { 5.1, 8.0, 2.9 } );

            // Act & Assert

            var exception = Assert.Throws<ArgumentOutOfRangeException>( () => list.Replace( index, 3.5 ) );

            // Assert state

            Assert.Equal( "index", exception.ParamName );
            Assert.Equal( new[] { 5.1, 8.0, 2.9, }, list );
        }

        [Theory]
        [InlineData( 1,
                     3.5,
                     new[] { 5.1, 3.5, 2.9 } )]
        [InlineData( 0,
                     23.4,
                     new[] { 23.4, 8.0, 2.9 } )]
        public void IListEx_ReplaceByIndex_Value_NonNullable( int index, object? newItem, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<double>() { 5.1, 8.0, 2.9 };
            var testedCollection = (IListEx) list;

            // Act

            testedCollection.Replace( index, newItem );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 0 )]
        [InlineData( 1 )]
        [InlineData( 2 )]
        public void IListEx_ReplaceByIndex_Value_NonNullable_NullValue( int index )
        {
            // Arrange

            var list = new ListEx<double>() { 5.1, 8.0, 2.9 };
            var testedCollection = (IListEx) list;

            // Act

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Replace( index, null ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, list );
        }

        [Theory]
        [InlineData( 1, 3.5f )]
        [InlineData( 0, 234 )]
        public void IListEx_ReplaceByIndex_Value_NonNullable_InvalidType( int index, object? newItem )
        {
            // Arrange

            var list = new ListEx<double>() { 5.1, 8.0, 2.9 };
            var testedCollection = (IListEx) list;

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Replace( index, newItem ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, list );
        }

        [Theory]
        [InlineData( 1,
                     3.5,
                     new object?[] { 5.1, 3.5, 8.0, 2.9 } )] // Valid replacement (null to non-null)
        [InlineData( 2,
                     3.5,
                     new object?[] { 5.1, null, 3.5, 2.9 } )] // Valid replacement (non-null to non-null)
        [InlineData( 3,
                     null,
                     new object?[] { 5.1, null, 8.0, null } )] // Valid replacement (non-null to null)
        [InlineData( 1,
                     null,
                     new object?[] { 5.1, null, 8.0, 2.9 } )] // Valid replacement (null to null)
        public void IListEx_ReplaceByIndex_Value_Nullable( int index, object? newItem, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<double?>() { 5.1, null, 8.0, 2.9 };
            var testedCollection = (IListEx) list;

            // Act

            testedCollection.Replace( index, newItem );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 1, 3.5f )]
        [InlineData( 1, 234 )]
        public void IListEx_ReplaceByIndex_Value_Nullable_InvalidType( int index, object? newItem )
        {
            // Arrange

            var list = new ListEx<double?>() { 5.1, 8.0, null, 2.9 };
            var testedCollection = (IListEx) list;

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Replace( index, newItem ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new double?[] { 5.1, 8.0, null, 2.9 }, list );
        }

        [Theory]
        [InlineData( 8.0,
                     1,
                     true,
                     new[] { 5.1, 8.0, 2.9, 44.5 } )] // Same position
        [InlineData( 8.0,
                     2,
                     true,
                     new[] { 5.1, 8.0, 2.9, 44.5 } )] // Position just after current
        [InlineData( 2.9,
                     0,
                     true,
                     new[] { 2.9, 5.1, 8.0, 44.5 } )] // Position before current
        [InlineData( 5.1,
                     4,
                     true,
                     new[] { 8.0, 2.9, 44.5, 5.1 } )] // Position at end (just past last item)
        [InlineData( 5.1,
                     3,
                     true,
                     new[] { 8.0, 2.9, 5.1, 44.5 } )] // Position at last item
        [InlineData( 8.01,
                     1,
                     false,
                     new[] { 5.1, 8.0, 2.9, 44.5 } )] // Not existing item
        public void Move_Value_NonNullable( double movedItem, int index, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            IListEx<double> list = new ListEx<double>( new[] { 5.1, 8.0, 2.9, 44.5 } );

            // Act

            var result = list.Move( movedItem, index );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 8.0,
                     1,
                     true,
                     new object?[] { 5.1, 8.0, null, 2.9, 44.5 } )] // Same position (non-null)
        [InlineData( null,
                     2,
                     true,
                     new object?[] { 5.1, 8.0, null, 2.9, 44.5 } )] // Same position (null)
        [InlineData( 8.0,
                     2,
                     true,
                     new object?[] { 5.1, 8.0, null, 2.9, 44.5 } )] // Position just after current (non-null)
        [InlineData( null,
                     3,
                     true,
                     new object?[] { 5.1, 8.0, null, 2.9, 44.5 } )] // Position just after current (null)
        [InlineData( 2.9,
                     0,
                     true,
                     new object?[] { 2.9, 5.1, 8.0, null, 44.5 } )] // Position before current (non-null)
        [InlineData( null,
                     0,
                     true,
                     new object?[] { null, 5.1, 8.0, 2.9, 44.5 } )] // Position before current (null)
        [InlineData( 5.1,
                     5,
                     true,
                     new object?[] { 8.0, null, 2.9, 44.5, 5.1 } )] // Position at end (non-null)
        [InlineData( null,
                     5,
                     true,
                     new object?[] { 5.1, 8.0, 2.9, 44.5, null } )] // Position at end (null)
        [InlineData( 8.01,
                     1,
                     false,
                     new object?[] { 5.1, 8.0, null, 2.9, 44.5 } )] // Not existing item
        public void Move_Value_Nullable( double? movedItem, int index, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            IListEx<double?> list = new ListEx<double?>( new double?[] { 5.1, 8.0, null, 2.9, 44.5 } );

            // Act

            var result = list.Move( movedItem, index );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 5.1, -343 )] // Negative index
        [InlineData( 8.0, -1 )] // Negative index
        [InlineData( 8.0, 5 )] // Out of range
        [InlineData( 44.5, 1235 )] // Out of range
        void Move_Index_OutOfRange( double movedItem, int index )
        {
            // Arrange

            IListEx<double> list = new ListEx<double>( new[] { 5.1, 8.0, 2.9, 44.5 } );

            // Act & Assert

            var exception = Assert.Throws<ArgumentOutOfRangeException>( () => list.Move( movedItem, index ) );

            // Assert state

            Assert.Equal( "newIndex", exception.ParamName );
            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, list );
        }

        [Theory]
        [InlineData( 1,
                     1,
                     new[] { 5.1, 8.0, 2.9, 44.5 } )] // Same position
        [InlineData( 2,
                     3,
                     new[] { 5.1, 8.0, 2.9, 44.5 } )] // Position just after current
        [InlineData( 2,
                     0,
                     new[] { 2.9, 5.1, 8.0, 44.5 } )] // Position before current
        [InlineData( 0,
                     4,
                     new[] { 8.0, 2.9, 44.5, 5.1 } )] // Position at end (just past last item)
        [InlineData( 0,
                     3,
                     new[] { 8.0, 2.9, 5.1, 44.5 } )] // Position at last item
        void MoveByIndex_Value_NonNullable( int oldIndex, int newIndex, IEnumerable expectedState )
        {
            // Arrange

            IListEx<double> list = new ListEx<double>( new[] { 5.1, 8.0, 2.9, 44.5 } );

            // Act: Same position

            list.Move( oldIndex, newIndex );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 1,
                     1,
                     new object?[] { 5.1, 8.0, 2.9, 44.5, null } )] // Same position (non-null)
        [InlineData( 4,
                     4,
                     new object?[] { 5.1, 8.0, 2.9, 44.5, null } )] // Same position (null)
        [InlineData( 2,
                     3,
                     new object?[] { 5.1, 8.0, 2.9, 44.5, null } )] // Position just after current
        [InlineData( 2,
                     0,
                     new object?[] { 2.9, 5.1, 8.0, 44.5, null } )] // Position before current
        [InlineData( 0,
                     5,
                     new object?[] { 8.0, 2.9, 44.5, null, 5.1 } )] // Position at end (just past last item)
        [InlineData( 0,
                     4,
                     new object?[] { 8.0, 2.9, 44.5, 5.1, null } )] // Position at last item
        void MoveByIndex_Value_Nullable( int oldIndex, int newIndex, IEnumerable expectedState )
        {
            // Arrange

            IListEx<double?> list = new ListEx<double?>( new double?[] { 5.1, 8.0, 2.9, 44.5, null } );

            // Act: Same position

            list.Move( oldIndex, newIndex );

            // Assert

            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( -12, 5 )]
        [InlineData( -1, 3 )]
        [InlineData( 5, 3 )]
        [InlineData( 10, 2 )]
        [InlineData( 10, -2 )]
        void MoveByIndex_OldIndex_OutOfRange( int oldIndex, int newIndex )
        {
            // Arrange

            IListEx<double> list = new ListEx<double>( new[] { 5.1, 8.0, 2.9, 44.5 } );

            // Act & Assert

            var exception = Assert.Throws<ArgumentOutOfRangeException>( () => list.Move( oldIndex, newIndex ) );

            // Assert state

            Assert.Equal( "oldIndex", exception.ParamName );
            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, list );
        }

        [Theory]
        [InlineData( 0, -5 )]
        [InlineData( 0, -1 )]
        [InlineData( 0, 5 )]
        [InlineData( 0, 24 )]
        void MoveByIndex_NewIndex_OutOfRange( int oldIndex, int newIndex )
        {
            // Arrange

            IListEx<double> list = new ListEx<double>( new[] { 5.1, 8.0, 2.9, 44.5 } );

            // Act & Assert

            var exception = Assert.Throws<ArgumentOutOfRangeException>( () => list.Move( oldIndex, newIndex ) );

            // Assert state

            Assert.Equal( "newIndex", exception.ParamName );
            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, list );
        }

        [Theory]
        [InlineData( 8.0,
                     3,
                     true,
                     new[] { 5.1, 2.9, 8.0, 44.5 } )] // Valid object
        [InlineData( null, 0, false, new[] { 5.1, 8.0, 2.9, 44.5 } )] // Null object
        [InlineData( 8.0f, 3, false, new[] { 5.1, 8.0, 2.9, 44.5 } )] // Invalid type
        void IListEx_Move_Value_NonNullable( object? movedItem, int index, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<double>( new[] { 5.1, 8.0, 2.9, 44.5 } );
            var testedCollection = (IListEx) list;

            // Act

            var result = testedCollection.Move( movedItem, index );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 8.0,
                     3,
                     true,
                     new object?[] { 5.1, 2.9, 8.0, null, 44.5 } )] // Valid object
        [InlineData( null,
                     0,
                     true,
                     new object?[] { null, 5.1, 8.0, 2.9, 44.5 } )] // Null object
        [InlineData( 8.0f,
                     3,
                     false,
                     new object?[] { 5.1, 8.0, 2.9, null, 44.5 } )] // Invalid type
        void IListEx_Move_Value_Nullable( object? movedItem, int index, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var list = new ListEx<double?>( new double?[] { 5.1, 8.0, 2.9, null, 44.5 } );
            var testedCollection = (IListEx) list;

            // Act

            var result = testedCollection.Move( movedItem, index );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
        }

        [Theory]
        [InlineData( 1,
                     2,
                     new[] { 8, 2 } )]
        [InlineData( 0,
                     1,
                     new[] { 5 } )]
        public void GetRange_Value_NonNullable( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            IListEx<int> list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Act

            var result = list.GetRange( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 1,
                     3,
                     new object?[] { null, 8, 2 } )]
        [InlineData( 0,
                     2,
                     new object?[] { 5, null } )]
        public void GetRange_Value_Nullable( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            IListEx<int?> list = new ListEx<int?>( new int?[] { 5, null, 8, 2 } );

            // Act

            var result = list.GetRange( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 1,
                     2,
                     new[] { 8, 2 } )]
        [InlineData( 0,
                     1,
                     new[] { 5 } )]
        public void IListExT_GetRange_Value_NonNullable( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );
            var testedCollection = (IListEx<int>) list;

            // Act

            var result = testedCollection.GetRange( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 1,
                     3,
                     new object?[] { null, 8, 2 } )]
        [InlineData( 0,
                     2,
                     new object?[] { 5, null } )]
        public void IListExT_GetRange_Value_Nullable( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var list = new ListEx<int?>( new int?[] { 5, null, 8, 2 } );
            var testedCollection = (IListEx<int?>) list;

            // Act

            var result = testedCollection.GetRange( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 1,
                     2,
                     new[] { 8, 2 } )]
        [InlineData( 0,
                     1,
                     new[] { 5 } )]
        public void IReadOnlyListEx_GetRange_Value( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );
            var testedCollection = (IReadOnlyListEx<int>) list;

            // Act

            var result = testedCollection.GetRange( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 1,
                     2,
                     new[] { 8, 2 } )]
        [InlineData( 0,
                     1,
                     new[] { 5 } )]
        public void IListEx_GetRange_Value( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );
            var testedCollection = (IListEx) list;

            // Act

            var result = testedCollection.GetRange( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 1,
                     2,
                     new[] { 8, 2 } )]
        [InlineData( 0,
                     1,
                     new[] { 5 } )]
        public void Slice( int start, int length, IEnumerable expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );

            // Act

            var result = list.Slice( start, length );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 1,
                     2,
                     new[] { 8, 2 } )]
        [InlineData( 0,
                     1,
                     new[] { 5 } )]
        public void IListExT_Slice( int start, int length, IEnumerable expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );
            var testedCollection = (IListEx<int>) list;

            // Act

            var result = testedCollection.Slice( start, length );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 1,
                     2,
                     new[] { 8, 2 } )]
        [InlineData( 0,
                     1,
                     new[] { 5 } )]
        public void IReadOnlyListEx_Slice( int start, int length, IEnumerable expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );
            var testedCollection = (IReadOnlyListEx<int>) list;

            // Act

            var result = testedCollection.Slice( start, length );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 1,
                     2,
                     new[] { 8, 2 } )]
        [InlineData( 0,
                     1,
                     new[] { 5 } )]
        public void IListEx_Slice( int start, int length, IEnumerable expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );
            var testedCollection = (IListEx) list;

            // Act

            var result = testedCollection.Slice( start, length );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 8, true )] // Item exists
        [InlineData( 9, false )] // Item does not exist
        [InlineData( 5.0, false )] // Invalid type
        [InlineData( null, false )] // Null item
        public void ICollectionEx_Contains_Value_NonNullable( object? searchedItem, bool expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );
            var testedCollection = (ICollectionEx) list;

            // Act

            var result = testedCollection.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 8, true )] // Item exists
        [InlineData( 9, false )] // Item does not exist
        [InlineData( 5.0, false )] // Invalid type
        [InlineData( null, true )] // Null item
        public void ICollectionEx_Contains_Value_Nullable( object? searchedItem, bool expectedResult )
        {
            // Arrange

            var list = new ListEx<int?>( new int?[] { 5, 8, null, 2 } );
            var testedCollection = (ICollectionEx) list;

            // Act

            var result = testedCollection.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 8, true )] // Item exists
        [InlineData( 9, false )] // Item does not exist
        [InlineData( 5.0, false )] // Invalid type
        [InlineData( null, false )] // Null item
        public void IReadOnlyCollectionEx_Contains_Value_NonNullable( object? searchedItem, bool expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 5, 8, 2 } );
            var testedCollection = (IReadOnlyCollectionEx<int>) list;

            // Act

            var result = testedCollection.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 8, true )] // Item exists
        [InlineData( 9, false )] // Item does not exist
        [InlineData( 5.0, false )] // Invalid type
        [InlineData( null, true )] // Null item
        public void IReadOnlyCollectionEx_Contains_Value_Nullable( object? searchedItem, bool expectedResult )
        {
            // Arrange

            var list = new ListEx<int?>( new int?[] { null, 5, 8, 2 } );
            var testedCollection = (IReadOnlyCollectionEx<int?>) list;

            // Act

            var result = testedCollection.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 6, 1 )] // Item exists
        [InlineData( 4, 2 )] // Item exists
        [InlineData( 10, -1 )] // Item does not exist
        [InlineData( 6.0, -1 )] // Invalid type
        [InlineData( null, -1 )] // Null item
        public void IReadOnlyListEx_IndexOf_Value_NonNullable( object? searchedItem, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );
            var testedCollection = (IReadOnlyListEx<int>) list;

            // Act

            var result = testedCollection.IndexOf( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 3, 1, 3 )] // Item exists in range
        [InlineData( 10, 0, -1 )] // Item does not exist
        [InlineData( 6, 2, -1 )] // Item exists but not in range
        [InlineData( 4.0, 1, -1 )] // Invalid type
        [InlineData( null, 0, -1 )] // Null item
        public void IReadOnlyListEx_IndexOf2_Value_NonNullable( object? searchedItem, int index, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );
            var testedCollection = (IReadOnlyListEx<int>) list;

            // Act

            var result = testedCollection.IndexOf( searchedItem, index );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 3, 2, 2, 3 )] // Item exists in range
        [InlineData( 10, 0, 4, -1 )] // Item does not exist
        [InlineData( 6, 2, 2, -1 )] // Item does not exist in range
        [InlineData( 3.0, 2, 2, -1 )] // Invalid type
        [InlineData( null, 0, 4, -1 )] // Null item
        public void IReadOnlyListEx_IndexOf3_Value_NonNullable( object? searchedItem, int index, int count, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );
            var testedCollection = (IReadOnlyListEx<int>) list;

            // Act

            var result = testedCollection.IndexOf( searchedItem, index, count );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 6, 1 )] // Item exists
        [InlineData( 4, 3 )] // Item exists
        [InlineData( 10, -1 )] // Item does not exist
        [InlineData( 6.0, -1 )] // Invalid type
        [InlineData( null, 2 )] // Null item
        public void IReadOnlyListEx_IndexOf_Value_Nullable( object? searchedItem, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int?>( new int?[] { 3, 6, null, 4, 3 } );
            var testedCollection = (IReadOnlyListEx<int?>) list;

            // Act

            var result = testedCollection.IndexOf( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 3, 1, 4 )] // Item exists in range
        [InlineData( 10, 0, -1 )] // Item does not exist
        [InlineData( 6, 2, -1 )] // Item exists but not in range
        [InlineData( 4.0, 1, -1 )] // Invalid type
        [InlineData( null, 1, 2 )] // Null item in range
        [InlineData( null, 3, -1 )] // Null item out of range
        public void IReadOnlyListEx_IndexOf2_Value_Nullable( object? searchedItem, int index, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int?>( new int?[] { 3, 6, null, 4, 3 } );
            var testedCollection = (IReadOnlyListEx<int?>) list;

            // Act

            var result = testedCollection.IndexOf( searchedItem, index );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 3, 2, 3, 4 )] // Item exists in range
        [InlineData( 10, 0, 4, -1 )] // Item does not exist
        [InlineData( 6, 2, 2, -1 )] // Item does not exist in range
        [InlineData( 3.0, 2, 2, -1 )] // Invalid type
        [InlineData( null, 0, 4, 2 )] // Null item in range
        [InlineData( null, 3, 2, -1 )] // Null item out of range
        public void IReadOnlyListEx_IndexOf3_Value_Nullable( object? searchedItem, int index, int count, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int?>( new int?[] { 3, 6, null, 4, 3 } );
            var testedCollection = (IReadOnlyListEx<int?>) list;

            // Act

            var result = testedCollection.IndexOf( searchedItem, index, count );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 6, 1 )] // Item exists
        [InlineData( 10, -1 )] // Item does not exist
        [InlineData( 6.0, -1 )] // Invalid type
        [InlineData( null, -1 )] // Null item
        public void IListEx_IndexOf( object? searchedItem, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );
            var testedCollection = (IListEx) list;

            // Act & Assert

            Assert.Equal( expectedResult, testedCollection.IndexOf( searchedItem ) );
        }

        [Theory]
        [InlineData( 3, 1, 3 )] // Item exists in range
        [InlineData( 10, 3, -1 )] // Item does not exist
        [InlineData( 4, 3, -1 )] // Item exists but not in range
        [InlineData( 4.0, 3, -1 )] // Invalid type
        [InlineData( null, 3, -1 )] // Null item
        public void IListEx_IndexOf2( object? searchedItem, int index, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );
            var testedCollection = (IListEx) list;

            // Act & Assert

            Assert.Equal( expectedResult, testedCollection.IndexOf( searchedItem, index ) );
        }

        [Theory]
        [InlineData( 6, 1, 2, 1 )] // Item exists in range
        [InlineData( 10, 0, 4, -1 )] // Item does not exist
        [InlineData( 3, 1, 2, -1 )] // Item does not exist in range
        [InlineData( 3.0, 3, 4, -1 )] // Invalid type
        [InlineData( null, 4, 4, -1 )] // Null item
        public void IListEx_IndexOf3( object? searchedItem, int index, int count, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );
            var testedCollection = (IListEx) list;

            // Act & Assert

            Assert.Equal( expectedResult, testedCollection.IndexOf( searchedItem, index, count ) );
        }

        [Theory]
        [InlineData( 6, 1 )] // Item exists
        [InlineData( 10, -1 )] // Item does not exist
        [InlineData( 6.0, -1 )] // Invalid type
        [InlineData( null, -1 )] // Null item
        public void IReadOnlyListEx_LastIndexOf_Value_NonNullable( object? searchedItem, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );
            var testedCollection = (IReadOnlyListEx<int>) list;

            // Act

            var result = testedCollection.LastIndexOf( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 3, 1, 0 )] // Item exists in range
        [InlineData( 10, 3, -1 )] // Item does not exist
        [InlineData( 4, 1, -1 )] // Item exists but not in range
        [InlineData( 4.0, 3, -1 )] // Invalid type
        [InlineData( null, 3, -1 )] // Null item
        public void IReadOnlyListEx_LastIndexOf2_Value_NonNullable( object? searchedItem, int index, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );
            var testedCollection = (IReadOnlyListEx<int>) list;

            // Act

            var result = testedCollection.LastIndexOf( searchedItem, index );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 3, 1, 2, 0 )] // Item exists in range
        [InlineData( 10, 3, 4, -1 )] // Item does not exist
        [InlineData( 3, 2, 2, -1 )] // Item does not exist in range
        [InlineData( 3.0, 3, 4, -1 )] // Invalid type
        [InlineData( null, 4, 4, -1 )] // Null item
        public void IReadOnlyListEx_LastIndexOf3_Value_NonNullable( object? searchedItem, int index, int count, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );
            var testedCollection = (IReadOnlyListEx<int>) list;

            // Act

            var result = testedCollection.LastIndexOf( searchedItem, index, count );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 6, 1 )] // Item exists
        [InlineData( 10, -1 )] // Item does not exist
        [InlineData( 6.0, -1 )] // Invalid type
        [InlineData( null, 2 )] // Null item
        public void IReadOnlyListEx_LastIndexOf_Value_Nullable( object? searchedItem, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int?>( new int?[] { 3, 6, null, 4, 3 } );
            var testedCollection = (IReadOnlyListEx<int?>) list;

            // Act

            var result = testedCollection.LastIndexOf( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 3, 1, 0 )] // Item exists in range
        [InlineData( 10, 4, -1 )] // Item does not exist
        [InlineData( 4, 2, -1 )] // Item exists but not in range
        [InlineData( 4.0, 3, -1 )] // Invalid type
        [InlineData( null, 3, 2 )] // Null item in range
        [InlineData( null, 1, -1 )] // Null item out of range
        public void IReadOnlyListEx_LastIndexOf2_Nullable( object? searchedItem, int index, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int?>( new int?[] { 3, 6, null, 4, 3 } );
            var testedCollection = (IReadOnlyListEx<int?>) list;

            // Act

            var result = testedCollection.LastIndexOf( searchedItem, index );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 3, 2, 3, 0 )] // Item exists in range
        [InlineData( 10, 3, 4, -1 )] // Item does not exist
        [InlineData( 3, 3, 2, -1 )] // Item does not exist in range
        [InlineData( 3.0, 3, 4, -1 )] // Invalid type
        [InlineData( null, 4, 4, 2 )] // Null item in range
        [InlineData( null, 1, 2, -1 )] // Null item out of range
        public void IReadOnlyListEx_LastIndexOf3_Value_Nullable( object? searchedItem, int index, int count, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int?>( new int?[] { 3, 6, null, 4, 3 } );
            var testedCollection = (IReadOnlyListEx<int?>) list;

            // Act

            var result = testedCollection.LastIndexOf( searchedItem, index, count );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 6, 1 )] // Item exists
        [InlineData( 10, -1 )] // Item does not exist
        [InlineData( 6.0, -1 )] // Invalid type
        [InlineData( null, -1 )] // Null item
        public void IListEx_LastIndexOf( object? searchedItem, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );
            var testedCollection = (IListEx) list;

            // Act

            var result = testedCollection.LastIndexOf( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 3, 1, 0 )] // Item exists in range
        [InlineData( 10, 3, -1 )] // Item does not exist
        [InlineData( 4, 1, -1 )] // Item exists but not in range
        [InlineData( 4.0, 3, -1 )] // Invalid type
        [InlineData( null, 3, -1 )] // Null item
        public void IListEx_LastIndexOf2( object? searchedItem, int index, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );
            var testedCollection = (IListEx) list;

            // Act

            var result = testedCollection.LastIndexOf( searchedItem, index );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( 3, 1, 2, 0 )] // Item exists in range
        [InlineData( 10, 3, 4, -1 )] // Item does not exist
        [InlineData( 3, 2, 2, -1 )] // Item does not exist in range
        [InlineData( 3.0, 3, 4, -1 )] // Invalid type
        [InlineData( null, 4, 4, -1 )] // Null item
        public void IListEx_LastIndexOf3( object? searchedItem, int index, int count, int expectedResult )
        {
            // Arrange

            var list = new ListEx<int>( new[] { 3, 6, 4, 3 } );
            var testedCollection = (IListEx) list;

            // Act

            var result = testedCollection.LastIndexOf( searchedItem, index, count );

            // Assert

            Assert.Equal( expectedResult, result );
        }
    }
}
