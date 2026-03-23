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
    public class HashSetExTest
    {
        private static readonly TestClass ITEM1 = new( "Item1", 10 );
        private static readonly TestClass ITEM2 = new( "Item2", 5 );
        private static readonly TestClass ITEM3 = new( "Item3", 20 );

        private class TestComparer : IEqualityComparer<TestClass>
        {
            public bool Equals( TestClass? x, TestClass? y )
            {
                if( ( x is null ) && ( y is null ) )
                {
                    return true;
                }
                else if( ( x is null ) || ( y is null ) )
                {
                    return false;
                }
                else
                {
                    return ( x.Name == y.Name ) && ( x.Value == y.Value );
                }
            }

            public int GetHashCode( TestClass obj )
            {
                return obj.Name.GetHashCode() ^ obj.Value.GetHashCode();
            }
        }

        [Fact]
        public void Constructor_Default()
        {
            // Act

            var set = new HashSetEx<int>();

            // Assert

            Assert.Equal( 0, set.Count );
            Assert.Equal( new int[] { }, set );
            Assert.Equal( EqualityComparer<int>.Default, set.Comparer );

            Assert.False( ( (ICollection<int>) set ).IsReadOnly );

            Assert.NotNull( ( (ICollection) set ).SyncRoot );
            Assert.False( ( (ICollection) set ).IsSynchronized );

            // The following tests are to ensure interface disambiguation.

            Assert.Equal( 0, ( (ISetEx<int>) set ).Count );
            Assert.Equal( 0, ( (ICollectionEx<int>) set ).Count );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            // Act

            var set = new HashSetEx<int>( new[] { 5, 8, 2 } );

            // Assert

            Assert.Equal( 3, set.Count );
            Assert.Equal( new[] { 5, 8, 2 }, set );
            Assert.Equal( EqualityComparer<int>.Default, set.Comparer );

            Assert.False( ( (ICollection<int>) set ).IsReadOnly );

            Assert.NotNull( ( (ICollection) set ).SyncRoot );
            Assert.False( ( (ICollection) set ).IsSynchronized );
        }

        [Fact]
        public void Constructor_Comparer()
        {
            // Arrange

            var comparer = new TestComparer();

            // Act

            var set = new HashSetEx<TestClass>( comparer );

            // Assert

            Assert.Equal( 0, set.Count );
            Assert.Equal( new TestClass[] { }, set );
            Assert.Same( comparer, set.Comparer );

            Assert.False( ( (ICollection<TestClass>) set ).IsReadOnly );

            Assert.NotNull( ( (ICollection) set ).SyncRoot );
            Assert.False( ( (ICollection) set ).IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationList_Comparer()
        {
            // Arrange

            var comparer = new TestComparer();

            // Act

            var set = new HashSetEx<TestClass>( new TestClass[] { ITEM1, ITEM1, ITEM2 }, comparer );

            // Assert

            Assert.Equal( 2, set.Count );
            Assert.Equal( new TestClass[] { ITEM1, ITEM2 }, set );
            Assert.Same( comparer, set.Comparer );

            Assert.False( ( (ICollection<TestClass>) set ).IsReadOnly );

            Assert.NotNull( ( (ICollection) set ).SyncRoot );
            Assert.False( ( (ICollection) set ).IsSynchronized );
        }

        [Fact]
        public void Constructor_Capacity()
        {
            // Act

            var set = new HashSetEx<int>( 10 );

            // Assert

            Assert.Equal( 0, set.Count );
            Assert.Equal( new int[] { }, set );
            Assert.Equal( EqualityComparer<int>.Default, set.Comparer );

            Assert.False( ( (ICollection<int>) set ).IsReadOnly );

            Assert.False( ( (ICollection<int>) set ).IsReadOnly );
            Assert.NotNull( ( (ICollection) set ).SyncRoot );
            Assert.False( ( (ICollection) set ).IsSynchronized );
        }

        [Fact]
        public void Constructor_Capacity_Comparer()
        {
            // Arrange

            var comparer = new TestComparer();

            // Act

            var set = new HashSetEx<TestClass>( 5, comparer );

            // Assert

            Assert.Equal( 0, set.Count );
            Assert.Equal( new TestClass[] { }, set );
            Assert.Same( comparer, set.Comparer );

            Assert.False( ( (ICollection<TestClass>) set ).IsReadOnly );

            Assert.NotNull( ( (ICollection) set ).SyncRoot );
            Assert.False( ( (ICollection) set ).IsSynchronized );
        }

        public class Add_Class_NonNullable_TestData : TheoryData<IEnumerable<TestClass>, TestClass, bool, IEnumerable<TestClass>>
        {
            public Add_Class_NonNullable_TestData()
            {
                Add( new TestClass[] { },
                     ITEM2,
                     true,
                     new TestClass[] { ITEM2 } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     ITEM2,
                     true,
                     new TestClass[] { ITEM1, ITEM3, ITEM2 } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     ITEM1,
                     false,
                     new TestClass[] { ITEM1, ITEM3 } );
            }
        }

        [Theory]
        [ClassData( typeof( Add_Class_NonNullable_TestData ) )]
        public void Add_Class_NonNullable( IEnumerable<TestClass> initialState, TestClass addedItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            ISetEx<TestClass> set = new HashSetEx<TestClass>( initialState );

            // Act

            var result = set.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        public class Add_Class_Nullable_TestData : TheoryData<IEnumerable<TestClass?>, TestClass?, bool, IEnumerable<TestClass?>>
        {
            public Add_Class_Nullable_TestData()
            {
                Add( new TestClass?[] { },
                     ITEM2,
                     true,
                     new TestClass?[] { ITEM2 } );
                Add( new TestClass?[] { ITEM1, null },
                     ITEM2,
                     true,
                     new TestClass?[] { ITEM1, null, ITEM2 } );
                Add( new TestClass?[] { ITEM1, ITEM3 },
                     null,
                     true,
                     new TestClass?[] { ITEM1, ITEM3, null } );
                Add( new TestClass?[] { ITEM1, null, ITEM3 },
                     null,
                     false,
                     new TestClass?[] { ITEM1, null, ITEM3 } );
            }
        }

        [Theory]
        [ClassData( typeof( Add_Class_Nullable_TestData ) )]
        public void Add_Class_Nullable( IEnumerable<TestClass?> initialState, TestClass? addedItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            ISetEx<TestClass?> set = new HashSetEx<TestClass?>( initialState );

            // Act

            var result = set.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( new object[] { },
                     5,
                     true,
                     new object?[] { 5 } )]
        [InlineData( new object[] { 1, 2, 3 },
                     4,
                     true,
                     new object?[] { 1, 2, 3, 4 } )]
        [InlineData( new object[] { 1, 2, 3 },
                     2,
                     false,
                     new object?[] { 1, 2, 3 } )]
        public void Add_Value_NonNullable( IEnumerable initialState, int value, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            ISetEx<int> set = new HashSetEx<int>( initialState.Cast<int>() );

            // Act

            var result = set.Add( value );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( new object?[] { },
                     5,
                     true,
                     new object?[] { 5 } )]
        [InlineData( new object?[] { 1, null, 3 },
                     4,
                     true,
                     new object?[] { 1, null, 3, 4 } )]
        [InlineData( new object?[] { 1, 2, 3 },
                     null,
                     true,
                     new object?[] { 1, 2, 3, null } )]
        [InlineData( new object?[] { 1, 2, 3, null },
                     2,
                     false,
                     new object?[] { 1, 2, 3, null } )]
        [InlineData( new object?[] { 1, null, 3 },
                     null,
                     false,
                     new object?[] { 1, null, 3 } )]
        public void Add_Value_Nullable( IEnumerable initialState, int? value, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            ISetEx<int?> set = new HashSetEx<int?>( initialState.Cast<int?>() );

            // Act

            var result = set.Add( value );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        public class ISetEx_AddClass_TestData : TheoryData<IEnumerable<TestClass?>, TestClass?, bool, IEnumerable<TestClass?>>
        {
            public ISetEx_AddClass_TestData()
            {
                Add( new TestClass[] { },
                     ITEM2,
                     true,
                     new TestClass?[] { ITEM2 } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     ITEM2,
                     true,
                     new TestClass?[] { ITEM1, ITEM3, ITEM2 } );
                Add( new TestClass[] { ITEM1, ITEM3 },
                     null,
                     true,
                     new TestClass?[] { ITEM1, ITEM3, null } );
                Add( new TestClass[] { ITEM1, ITEM2 },
                     ITEM2,
                     false,
                     new TestClass?[] { ITEM1, ITEM2 } );
                Add( new TestClass?[] { ITEM1, null, ITEM3 },
                     null,
                     false,
                     new TestClass?[] { ITEM1, null, ITEM3 } );
            }
        }

        [Theory]
        [ClassData( typeof( ISetEx_AddClass_TestData ) )]
        public void ISetEx_AddClass( IEnumerable<TestClass> initialState, object? addedItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var set = new HashSetEx<TestClass>( initialState );
            var testedCollection = (ISetEx) set;

            // Act

            var result = testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( new object[] { },
                     5,
                     true,
                     new object?[] { 5 } )]
        [InlineData( new object[] { 1, 2, 3 },
                     4,
                     true,
                     new object?[] { 1, 2, 3, 4 } )]
        [InlineData( new object[] { 1, 2, 3 },
                     2,
                     false,
                     new object?[] { 1, 2, 3 } )]
        public void ISetEx_AddValue_NonNullable( IEnumerable initialState, object? value, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var set = new HashSetEx<int>( initialState.Cast<int>() );
            var testedCollection = (ISetEx) set;

            // Act

            var result = testedCollection.Add( value );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( new object[] { 45 },
                     new object?[] { 45 } )]
        [InlineData( new object[] { },
                     new object?[] { } )]
        public void ISetEx_AddValue_NonNullable_NullValue( IEnumerable initialState, IEnumerable expectedState )
        {
            // Arrange

            var set = new HashSetEx<int>( initialState.Cast<int>() );
            var testedCollection = (ISetEx) set;

            // Act & Assert

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Add( null ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( new object?[] { },
                     5,
                     true,
                     new object?[] { 5 } )]
        [InlineData( new object?[] { 1, null, 3 },
                     4,
                     true,
                     new object?[] { 1, null, 3, 4 } )]
        [InlineData( new object?[] { 1, 2, 3 },
                     null,
                     true,
                     new object?[] { 1, 2, 3, null } )]
        [InlineData( new object?[] { 1, 2, 3, null },
                     2,
                     false,
                     new object?[] { 1, 2, 3, null } )]
        [InlineData( new object?[] { 1, null, 3 },
                     null,
                     false,
                     new object?[] { 1, null, 3 } )]
        public void ISetEx_AddValue_Nullable( IEnumerable initialState, object? value, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var set = new HashSetEx<int?>( initialState.Cast<int?>() );
            var testedCollection = (ISetEx) set;

            // Act

            var result = testedCollection.Add( value );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( 23 )]
        [InlineData( 45.0 )]
        public void ISetEx_AddInvalidType( object? addedItem )
        {
            // Arrange

            var set = new HashSetEx<TestClass>() { ITEM1 };
            var testedCollection = (ISetEx) set;

            // Act & Assert

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Add( addedItem ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( new TestClass[] { ITEM1 }, set );
        }

        [Theory]
        [InlineData( new object?[] { },
                     5,
                     new object?[] { 5 } )]
        [InlineData( new object?[] { 1, null, 3 },
                     4,
                     new object?[] { 1, null, 3, 4 } )]
        [InlineData( new object?[] { 1, 2, 3 },
                     null,
                     new object?[] { 1, 2, 3, null } )]
        [InlineData( new object?[] { 1, 2, 3, null },
                     2,
                     new object?[] { 1, 2, 3, null } )]
        [InlineData( new object?[] { 1, null, 3 },
                     null,
                     new object?[] { 1, null, 3 } )]
        public void ICollectionEx_AddValue( IEnumerable initialState, object? value, IEnumerable expectedState )
        {
            // Arrange

            var set = new HashSetEx<int?>( initialState.Cast<int?>() );
            var testedCollection = (ICollectionEx) set;

            // Act

            testedCollection.Add( value );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( new object[] { },
                     new object[] { 5, 8, 2 },
                     new object[] { 5, 8, 2 } )]
        [InlineData( new object[] { 1 },
                     new object[] { 2, 3 },
                     new object[] { 1, 2, 3 } )]
        [InlineData( new object[] { 1, 2 },
                     new object[] { },
                     new object[] { 1, 2 } )]
        [InlineData( new object[] { 1, 2 },
                     new object[] { 2, 3 },
                     new object[] { 1, 2, 3 } )]
        public void AddRange_Value_NonNullable( IEnumerable initialState, IEnumerable addedItems, IEnumerable expectedState )
        {
            // Arrange

            ISetEx<int> set = new HashSetEx<int>( initialState.Cast<int>() );

            // Act

            set.AddRange( addedItems.Cast<int>() );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( new object?[] { 5, 8, 2 },
                     new object?[] { null, 33 },
                     new object?[] { 5, 8, 2, null, 33 } )]
        [InlineData( new object?[] { 1, null },
                     new object?[] { 2, 3 },
                     new object?[] { 1, null, 2, 3 } )]
        [InlineData( new object?[] { 1, 2 },
                     new object?[] { },
                     new object?[] { 1, 2 } )]
        [InlineData( new object?[] { 1, 2 },
                     new object?[] { 2, 3 },
                     new object?[] { 1, 2, 3 } )]
        [InlineData( new object?[] { 1, null },
                     new object?[] { null, 3 },
                     new object?[] { 1, null, 3 } )]
        public void AddRange_Value_Nullable( IEnumerable initialState, IEnumerable addedItems, IEnumerable expectedState )
        {
            // Arrange

            ISetEx<int?> set = new HashSetEx<int?>( initialState.Cast<int?>() );

            // Act

            set.AddRange( addedItems.Cast<int?>() );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) set );
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
                     new TestClass?[] { ITEM1, ITEM3, null } );
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

            var set = new HashSetEx<TestClass>( initialState );
            var testedCollection = (ICollectionEx) set;

            // Act

            testedCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( new object?[] { 5, 8, 2 },
                     new object?[] { 4, 45, 5, 8, 2 } )]
        [InlineData( new object?[] { 45, 4 },
                     new object?[] { 4, 45 } )]
        public void ICollectionEx_AddRange_Value_NonNullable( IEnumerable addedItems, IEnumerable expectedState )
        {
            // Arrange

            var set = new HashSetEx<int>( new[] { 4, 45 } );
            var testedCollection = (ICollectionEx) set;

            // Act

            testedCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Fact]
        public void ICollectionEx_AddRange_Value_NonNullable_NullValue()
        {
            // Arrange

            var set = new HashSetEx<int>( new[] { 45 } );
            var testedCollection = (ICollectionEx) set;

            // Act & Assert

            Assert.Throws<NullReferenceException>( () => testedCollection.AddRange( new object?[] { 2, null, 3 } ) );

            // Assert

            Assert.Equal( new[] { 45 }, set );
        }

        [Theory]
        [InlineData( new object?[] { 5, 8, 2 },
                     new object?[] { 4, null, 45, 5, 8, 2 } )]
        [InlineData( new object?[] { 33, null },
                     new object?[] { 4, null, 45, 33 } )]
        public void ICollectionEx_AddRange_Value_Nullable( IEnumerable addedItems, IEnumerable expectedState )
        {
            // Arrange

            var set = new HashSetEx<int?>( new int?[] { 4, null, 45 } );
            var testedCollection = (ICollectionEx) set;

            // Act

            testedCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Fact]
        public void ICollectionEx_AddRange_InvalidType()
        {
            // Arrange

            var set = new HashSetEx<TestClass>() { ITEM3 };
            var testedCollection = (ICollectionEx) set;

            // Act & Assert

            Assert.Throws<InvalidCastException>( () => testedCollection.AddRange( new[] { 2 } ) );

            // Assert

            Assert.Equal( new[] { ITEM3 }, set );
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

            var set = new HashSetEx<TestClass>( initialState );
            var testedCollection = (ICollectionEx) set;

            // Act

            var result = testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( 8,
                     true,
                     new object[] { 5, 2 } )] // Valid object
        [InlineData( null,
                     false,
                     new object[] { 5, 8, 2 } )] // Null object
        [InlineData( 8.0,
                     false,
                     new object[] { 5, 8, 2 } )] // invalid type
        public void ICollectionEx_Remove_Value_NonNullable( object? removedItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var set = new HashSetEx<int>( new[] { 5, 8, 2 } );
            var testedCollection = (ICollectionEx) set;

            // Act

            var result = testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( 8,
                     true,
                     new object?[] { 5, 2, null } )] // Valid non-null object
        [InlineData( null,
                     true,
                     new object?[] { 5, 8, 2 } )] // Valid null object
        [InlineData( 8.0,
                     false,
                     new object?[] { 5, 8, 2, null } )] // invalid type
        public void ICollectionEx_Remove_Value_Nullable( object? removedItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var set = new HashSetEx<int?>( new int?[] { 5, 8, 2, null } );
            var testedCollection = (ICollectionEx) set;

            // Act

            var result = testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
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

            ISetEx<TestClass?> set = new HashSetEx<TestClass?>( initialState );

            // Act

            set.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( new object[] { 8, 2 },
                     new object[] { 5, 9 } )] // All items removed
        [InlineData( new object[] { 8, 55 },
                     new object[] { 5, 2, 9 } )] // Some items not removed
        public void RemoveRange_Value_NonNullable( IEnumerable removedItems, IEnumerable expectedState )
        {
            // Arrange

            ISetEx<int> set = new HashSetEx<int>( new[] { 5, 8, 2, 9 } );

            // Act

            set.RemoveRange( removedItems.Cast<int>() );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( new object?[] { 8, 2 },
                     new object?[] { 5, null, 9 } )] // All items removed
        [InlineData( new object?[] { 8, 55 },
                     new object?[] { 5, null, 2, 9 } )] // Some items not removed
        [InlineData( new object?[] { 2, null },
                     new object?[] { 5, 8, 9 } )] // Null item removed
        public void RemoveRange_Value_Nullable( IEnumerable removedItems, IEnumerable expectedState )
        {
            // Arrange

            ISetEx<int?> set = new HashSetEx<int?>( new int?[] { 5, null, 8, 2, 9 } );

            // Act

            set.RemoveRange( removedItems.Cast<int?>() );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) set );
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

            var set = new HashSetEx<TestClass>( initialState.Cast<TestClass>() );
            var testedCollection = (ICollectionEx) set;

            // Act

            testedCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( new object?[] { 8, 2 },
                     new object?[] { 5, 9 } )] // All items removed
        [InlineData( new object?[] { 8, 55 },
                     new object?[] { 5, 2, 9 } )] // Some items not removed
        [InlineData( new object?[] { 2, null },
                     new object?[] { 5, 8, 9 } )] // Null item not removed
        [InlineData( new object?[] { 8, 2.0 },
                     new object?[] { 5, 2, 9 } )] // Invalid item not removed
        public void ICollectionEx_RemoveRange_Value_NonNullable( IEnumerable removedItems, IEnumerable expectedState )
        {
            // Arrange

            var set = new HashSetEx<int>( new[] { 5, 8, 2, 9 } );
            var testedCollection = (ICollectionEx) set;

            // Act

            testedCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( new object?[] { 8, 2 },
                     new object?[] { 5, null, 9 } )] // All items removed
        [InlineData( new object?[] { 8, 55 },
                     new object?[] { 5, 2, null, 9 } )] // Some items not removed
        [InlineData( new object?[] { 2, null },
                     new object?[] { 5, 8, 9 } )] // Null item removed
        [InlineData( new object[] { 8, 2.0 },
                     new object?[] { 5, 2, null, 9 } )] // Invalid item not removed
        public void ICollectionEx_RemoveRange_Nullable( IEnumerable removedItems, IEnumerable expectedState )
        {
            // Arrange

            var set = new HashSetEx<int?>( new int?[] { 5, 8, 2, null, 9 } );
            var testedCollection = (ICollectionEx) set;

            // Act

            testedCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) set );
        }

        public class Replace_Class_TestData : TheoryData<IEnumerable<TestClass?>, TestClass?, TestClass?, bool, IEnumerable<TestClass?>>
        {
            public Replace_Class_TestData()
            {
                Add( new TestClass?[] { ITEM2, ITEM3 },
                     ITEM2,
                     ITEM1,
                     true,
                     new TestClass?[] { ITEM1, ITEM3 } ); // Replaced item present, replacement item not present
                Add( new TestClass?[] { },
                     ITEM2,
                     ITEM3,
                     false,
                     new TestClass?[] { } ); // Replaced item not present
                Add( new TestClass?[] { ITEM2, ITEM3 },
                     ITEM2,
                     ITEM3,
                     true,
                     new TestClass?[] { ITEM3 } ); // Replaced item present, replacement item present
                Add( new TestClass?[] { ITEM2, ITEM3 },
                     ITEM2,
                     ITEM2,
                     true,
                     new TestClass?[] { ITEM2, ITEM3 } ); // Same item
            }
        }

        [Theory]
        [ClassData( typeof( Replace_Class_TestData ) )]
        public void Replace_Class( IEnumerable<TestClass?> initialState, TestClass? oldItem, TestClass? newItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            ISetEx<TestClass?> set = new HashSetEx<TestClass?>( initialState );

            // Act

            var result = set.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( 8.0,
                     3.5,
                     true,
                     new object[] { 5.1, 3.5, 2.9 } )] // Valid replacement (new item not existing)
        [InlineData( 8.0,
                     2.9,
                     true,
                     new object[] { 5.1, 2.9 } )] // Valid replacement (new item existing)
        [InlineData( 8.0,
                     8.0,
                     true,
                     new object[] { 5.1, 8.0, 2.9 } )] // Valid replacement (same item)
        [InlineData( 8.01,
                     3.5,
                     false,
                     new object[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (not found)
        public void Replace_Value_NonNullable( double oldItem, double newItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            ISetEx<double> set = new HashSetEx<double>( new[] { 5.1, 8.0, 2.9 } );

            // Act

            var result = set.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( 8.0,
                     3.5,
                     true,
                     new object?[] { 5.1, 3.5, null, 2.9 } )] // Valid replacement (non-null with non-null, new not existing)
        [InlineData( 8.0,
                     5.1,
                     true,
                     new object?[] { 5.1, null, 2.9 } )] // Valid replacement (non-null with non-null, new existing)
        [InlineData( 8.0,
                     8.0,
                     true,
                     new object?[] { 5.1, 8.0, null, 2.9 } )] // Valid replacement (same non-null)
        [InlineData( null,
                     3.5,
                     true,
                     new object?[] { 5.1, 8.0, 3.5, 2.9 } )] // Valid replacement (null with non-null, new not existing)
        [InlineData( null,
                     8.0,
                     true,
                     new object?[] { 5.1, 8.0, 2.9 } )] // Valid replacement (null with non-null, new existing)
        [InlineData( 8.0,
                     null,
                     true,
                     new object?[] { 5.1, null, 2.9 } )] // Valid replacement (non-null with null)
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

            ISetEx<double?> set = new HashSetEx<double?>( new double?[] { 5.1, 8.0, null, 2.9 } );

            // Act

            var result = set.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( 8.0,
                     3.5,
                     true,
                     new[] { 5.1, 3.5, 2.9 } )] // Valid replacement (new item not existing)
        [InlineData( 8.0,
                     2.9,
                     true,
                     new[] { 5.1, 2.9 } )] // Valid replacement (new item existing)
        [InlineData( 8.0,
                     8.0,
                     true,
                     new[] { 5.1, 8.0, 2.9 } )] // Valid replacement (same item)
        [InlineData( null,
                     3.5,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (old item null)
        [InlineData( null,
                     null,
                     false, new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (both items null)
        [InlineData( 8.01,
                     3.5,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (old item not found)
        [InlineData( 8.0f,
                     3.5,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (old item invalid)
        [InlineData( 8.1,
                     3.5f,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (old item valid but not existing, new item invalid)
        [InlineData( 8.1,
                     null,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (old item valid but not existing, new item null)
        [InlineData( 8.0f,
                     3.5f,
                     false,
                     new[] { 5.1, 8.0, 2.9 } )] // Invalid replacement (both items invalid)
        public void ICollectionEx_Replace_Value_NonNullable( object? oldItem, object? newItem, bool expectedResult, IEnumerable expectedState )
        {
            // Arrange

            var set = new HashSetEx<double>( new[] { 5.1, 8.0, 2.9 } );
            var testedCollection = (ICollectionEx) set;

            // Act

            var result = testedCollection.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( 8.0, null )]
        public void ICollectionEx_Replace_Value_NonNullable_NullValue( object? oldItem, object? newItem )
        {
            // Arrange

            var set = new HashSetEx<double>() { 5.1, 8.0, 2.9 };
            var testedCollection = (ICollectionEx) set;

            // Act

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Replace( oldItem, newItem ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, (IEnumerable) set );
        }

        [Theory]
        [InlineData( 8.0, 3.5f )]
        public void ICollectionEx_Replace_Value_NonNullable_InvalidType( object? oldItem, object? newItem )
        {
            // Arrange

            var set = new HashSetEx<double>() { 5.1, 8.0, 2.9 };
            var testedCollection = (ICollectionEx) set;

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Replace( oldItem, newItem ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, (IEnumerable) set );
        }

        [Theory]
        [InlineData( 8.0,
                     3.5,
                     true,
                     new object?[] { 5.1, 3.5, 2.9, null } )] // Valid replacement (non-null with non-null, new not existing)
        [InlineData( 8.0,
                     5.1,
                     true,
                     new object?[] { 5.1, 2.9, null } )] // Valid replacement (non-null with non-null, new existing)
        [InlineData( 8.0,
                     8.0,
                     true,
                     new object?[] { 5.1, 8.0, 2.9, null } )] // Valid replacement (same non-null)
        [InlineData( null,
                     3.5,
                     true,
                     new object?[] { 5.1, 8.0, 2.9, 3.5 } )] // Valid replacement (null with non-null, new not existing)
        [InlineData( null,
                     8.0,
                     true,
                     new object?[] { 5.1, 8.0, 2.9 } )] // Valid replacement (null with non-null, new existing)
        [InlineData( 8.0,
                     null,
                     true,
                     new object?[] { 5.1, 2.9, null } )] // Valid replacement (non-null with null)
        [InlineData( null,
                     null,
                     true, new object?[] { 5.1, 8.0, 2.9, null } )] // Valid replacement (both items null)
        [InlineData( 8.01,
                     3.5,
                     false,
                     new object?[] { 5.1, 8.0, 2.9, null } )] // Invalid replacement (non-null not found)
        [InlineData( 2.91,
                     null,
                     false,
                     new object?[] { 5.1, 8.0, 2.9, null } )] // Invalid replacement (non-null not found)
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

            var set = new HashSetEx<double?>( new double?[] { 5.1, 8.0, 2.9, null } );
            var testedCollection = (ICollectionEx) set;

            // Act

            var result = testedCollection.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) set );
        }

        [Theory]
        [InlineData( 8.0, 3.5f )]
        [InlineData( null, 3.5f )]
        public void ICollectionEx_Replace_Value_Nullable_InvalidType( object? oldItem, object? newItem )
        {
            // Arrange

            var set = new HashSetEx<double?>() { 5.1, 8.0, null, 2.9 };
            var testedCollection = (ICollectionEx) set;

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Replace( oldItem, newItem ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new double?[] { 5.1, 8.0, null, 2.9 }, (IEnumerable) set );
        }

        [Theory]
        [InlineData( 8, true )] // Item exists
        [InlineData( 9, false )] // Item does not exist
        [InlineData( 5.0, false )] // Invalid type
        [InlineData( null, false )] // Null item
        public void ICollectionEx_Contains_NonNullable( object? searchedItem, bool expectedResult )
        {
            // Arrange

            var set = new HashSetEx<int>( new[] { 5, 8, 2 } );
            var testedCollection = (ICollectionEx) set;

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
        public void ICollectionEx_Contains_Nullable( object? searchedItem, bool expectedResult )
        {
            // Arrange

            var set = new HashSetEx<int?>( new int?[] { 5, 8, null, 2 } );
            var testedCollection = (ICollectionEx) set;

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
        public void IReadOnlyCollectionEx_Contains_NonNullable( object? searchedItem, bool expectedResult )
        {
            // Arrange

            var set = new HashSetEx<int>( new[] { 5, 8, 2 } );
            var testedCollection = (IReadOnlyCollectionEx<int>) set;

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
        public void IReadOnlyCollectionEx_Contains_Nullable( object? searchedItem, bool expectedResult )
        {
            // Arrange

            var set = new HashSetEx<int?>( new int?[] { null, 5, 8, 2 } );
            var testedCollection = (IReadOnlyCollectionEx<int?>) set;

            // Act

            var result = testedCollection.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
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
