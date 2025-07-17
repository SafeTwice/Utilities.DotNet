/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Utilities.DotNet.Collections.Test;
using Xunit;

#pragma warning disable IDE0079
#pragma warning disable xUnit2013 // Do not use equality check to check for observableList size.
#pragma warning disable xUnit1045
#pragma warning disable CA1825 // Using zero-length array initializers is intentionally to improve test maintainability.
#pragma warning disable CA1859 // Using interface types is intentional to call the interface methods.
#pragma warning disable CA1861 // Using constant arrays is intentionally to improve test maintainability.
#pragma warning restore IDE0079

#pragma warning disable S101 // TestData types are not strictly PascalCase

namespace Utilities.DotNet.Collections.Observables.Test
{
    public class ObservableSortedListTest
    {
        private static readonly TestClass ITEM1 = new( "Item1", 10 );
        private static readonly TestClass ITEM2 = new( "Item2", 5 );
        private static readonly TestClass ITEM3 = new( "Item3", 20 );

        [Fact]
        public void Constructor_Default()
        {
            // Act

            var observableList = new ObservableSortedList<TestClass>();

            // Assert

            Assert.Equal( 0, observableList.Count );
            Assert.Equal( new TestClass[] { }, observableList );
            Assert.Same( Comparer<TestClass>.Default, observableList.Comparer );

            Assert.False( ( (IObservableList<TestClass>) observableList ).IsReadOnly );

            Assert.False( ( (IList) observableList ).IsReadOnly );
            Assert.False( ( (IList) observableList ).IsFixedSize );
            Assert.NotNull( ( (IList) observableList ).SyncRoot );
            Assert.False( ( (IList) observableList ).IsSynchronized );

            // The following tests are to ensure interface disambiguation.

            Assert.Equal( 0, ( (IObservableList<TestClass>) observableList ).Count );
            Assert.Equal( 0, ( (ICollectionEx<TestClass>) observableList ).Count );
        }

        [Fact]
        public void Constructor_Comparer()
        {
            // Arrange

            var customComparer = Comparer<TestClass>.Create( ( x, y ) => 0 );

            // Act

            var observableList = new ObservableSortedList<TestClass>( customComparer );

            // Assert

            Assert.Equal( 0, observableList.Count );
            Assert.Equal( new TestClass[] { }, observableList );
            Assert.Same( customComparer, observableList.Comparer );

            Assert.False( ( (IObservableList<TestClass>) observableList ).IsReadOnly );

            Assert.False( ( (IList) observableList ).IsReadOnly );
            Assert.False( ( (IList) observableList ).IsFixedSize );
            Assert.NotNull( ( (IList) observableList ).SyncRoot );
            Assert.False( ( (IList) observableList ).IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            // Act

            var observableList = new ObservableSortedList<TestClass>( new[] { ITEM3, ITEM1, ITEM2 } );

            // Assert

            Assert.Equal( 3, observableList.Count );
            Assert.Equal( new[] { ITEM1, ITEM2, ITEM3 }, observableList );
            Assert.Same( Comparer<TestClass>.Default, observableList.Comparer );

            Assert.False( ( (IObservableList<TestClass>) observableList ).IsReadOnly );

            Assert.False( ( (IList) observableList ).IsReadOnly );
            Assert.False( ( (IList) observableList ).IsFixedSize );
            Assert.NotNull( ( (IList) observableList ).SyncRoot );
            Assert.False( ( (IList) observableList ).IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationListAndComparer()
        {
            // Arrange

            var customComparer = Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) );

            // Act

            var observableList = new ObservableSortedList<TestClass>( new[] { ITEM3, ITEM1, ITEM2 }, customComparer );

            // Assert

            Assert.Equal( 3, observableList.Count );
            Assert.Equal( new[] { ITEM2, ITEM1, ITEM3 }, observableList );
            Assert.Same( customComparer, observableList.Comparer );

            Assert.False( ( (IObservableList<TestClass>) observableList ).IsReadOnly );

            Assert.False( ( (IList) observableList ).IsReadOnly );
            Assert.False( ( (IList) observableList ).IsFixedSize );
            Assert.NotNull( ( (IList) observableList ).SyncRoot );
            Assert.False( ( (IList) observableList ).IsSynchronized );
        }

        public class Add_Class_NonNullable_TestData
            : TheoryData<IEnumerable<TestClass>, TestClass, int, IEnumerable<TestClass>, IEnumerable<CollectionChangedEventData>>
        {
            public Add_Class_NonNullable_TestData()
            {
                Add( new TestClass[] { ITEM1 },
                     ITEM2,
                     1,
                     new TestClass[] { ITEM1, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ) } );
                Add( new TestClass[] { ITEM1, ITEM2 },
                     ITEM3,
                     2,
                     new TestClass[] { ITEM1, ITEM2, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 2, -1 ) } );
                Add( new TestClass[] { ITEM3, ITEM1 },
                     ITEM2,
                     1,
                     new TestClass[] { ITEM1, ITEM2, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( Add_Class_NonNullable_TestData ) )]
        public void Add_Class_NonNullable( IEnumerable<TestClass> initialState, TestClass addedItem, int expectedResult, IEnumerable<TestClass> expectedState,
                                           IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<TestClass> list = new ObservableSortedList<TestClass>( initialState );

            list.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( list, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = list.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, list );
            Assert.Equal( expectedEvents, events );
        }

        public class Add_Class_Nullable_TestData
            : TheoryData<IEnumerable<TestClass?>, TestClass?, int, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public Add_Class_Nullable_TestData()
            {
                Add( new TestClass?[] { ITEM1 },
                     ITEM2,
                     1,
                     new TestClass?[] { ITEM1, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ) } );
                Add( new TestClass?[] { ITEM1, ITEM2 },
                     ITEM3,
                     2,
                     new TestClass?[] { ITEM1, ITEM2, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 2, -1 ) } );
                Add( new TestClass[] { ITEM3, ITEM1 },
                     ITEM2,
                     1,
                     new TestClass[] { ITEM1, ITEM2, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ) } );
                Add( new TestClass?[] { ITEM1, ITEM2 },
                     null,
                     0,
                     new TestClass?[] { null, ITEM1, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
                Add( new TestClass?[] { ITEM1, null },
                     ITEM2,
                     2,
                     new TestClass?[] { null, ITEM1, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 2, -1 ) } );
                Add( new TestClass?[] { ITEM1, null, ITEM3 },
                     null,
                     0,
                     new TestClass?[] { null, null, ITEM1, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( Add_Class_Nullable_TestData ) )]
        public void Add_Class_Nullable( IEnumerable<TestClass?> initialState, TestClass? addedItem, int expectedResult, IEnumerable<TestClass?> expectedState,
                                        IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<TestClass?> observableList = new ObservableSortedList<TestClass?>( initialState );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableList.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        public class IListEx_Add_Value_NonNullable_TestData
            : TheoryData<IEnumerable<int>, int, int, IEnumerable<int>, IEnumerable<CollectionChangedEventData>>
        {
            public IListEx_Add_Value_NonNullable_TestData()
            {
                Add( new int[] { },
                     5,
                     0,
                     new int[] { 5 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 5 }, null, 0, -1 ) } );
                Add( new int[] { 12, 2, 13 },
                     4,
                     1,
                     new int[] { 2, 4, 12, 13 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 4 }, null, 1, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( IListEx_Add_Value_NonNullable_TestData ) )]
        public void IListEx_Add_Value_NonNullable( IEnumerable<int> initialState, object? addedItem, int expectedResult, IEnumerable<int> expectedState,
                                                   IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<int>( initialState );
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void IListEx_Add_Value_NonNullable_NullValue()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<int>() { 33 };
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Add( null ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( new int[] { 33 }, observableList );
            Assert.Empty( events );
        }

        [Fact]
        public void IListEx_Add_Value_NonNullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<int>() { 33 };
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Add( 3.0 ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( new int[] { 33 }, observableList );
            Assert.Empty( events );
        }

        public class IListEx_Add_Value_Nullable_TestData
            : TheoryData<IEnumerable<int?>, int?, int, IEnumerable<int?>, IEnumerable<CollectionChangedEventData>>
        {
            public IListEx_Add_Value_Nullable_TestData()
            {
                Add( new int?[] { },
                     5,
                     0,
                     new int?[] { 5 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 5 }, null, 0, -1 ) } );
                Add( new int?[] { 12, null, 13 },
                     4,
                     1,
                     new int?[] { null, 4, 12, 13 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 4 }, null, 1, -1 ) } );
                Add( new int?[] { 12, 2, null, 13 },
                     null,
                     0,
                     new int?[] { null, null, 2, 12, 13 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( IListEx_Add_Value_Nullable_TestData ) )]
        public void IListEx_Add_Value_Nullable( IEnumerable<int?> initialState, object? addedItem, int expectedResult, IEnumerable<int?> expectedState,
                                                IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<int?>( initialState );
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void IListEx_Add_Value_Nullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<int?>() { 33, null };
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Add( 3.0 ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( new int?[] { null, 33 }, observableList );
            Assert.Empty( events );
        }

        public class IList_Add_Class_NonNullable_TestData
            : TheoryData<IEnumerable<TestClass>, TestClass?, int, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public IList_Add_Class_NonNullable_TestData()
            {
                Add( new TestClass[] { },
                     ITEM2,
                     0,
                     new TestClass?[] { ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 0, -1 ) } );
                Add( new TestClass[] { ITEM2, ITEM1 },
                     ITEM2,
                     1,
                     new TestClass?[] { ITEM1, ITEM2, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ) } );
                Add( new TestClass[] { ITEM2 },
                     null,
                     0,
                     new TestClass?[] { null, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( IList_Add_Class_NonNullable_TestData ) )]
        public void IList_Add_Class_NonNullable( IEnumerable<TestClass> initialState, object? addedItem, int expectedResult, IEnumerable<TestClass?> expectedState,
                                                 IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<TestClass>( initialState );
            var testedCollection = (IList) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        public class IList_Add_Class_Nullable_TestData
            : TheoryData<IEnumerable<TestClass?>, TestClass?, int, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public IList_Add_Class_Nullable_TestData()
            {
                Add( new TestClass?[] { },
                     ITEM2,
                     0,
                     new TestClass?[] { ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 0, -1 ) } );
                Add( new TestClass?[] { ITEM2, null, ITEM1 },
                     ITEM2,
                     2,
                     new TestClass?[] { null, ITEM1, ITEM2, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 2, -1 ) } );
                Add( new TestClass?[] { ITEM2 },
                     null,
                     0,
                     new TestClass?[] { null, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
                Add( new TestClass?[] { ITEM2, null, ITEM1 },
                     null,
                     0,
                     new TestClass?[] { null, null, ITEM1, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( IList_Add_Class_Nullable_TestData ) )]
        public void IList_Add_Class_Nullable( IEnumerable<TestClass?> initialState, object? addedItem, int expectedResult, IEnumerable<TestClass?> expectedState,
                                              IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<TestClass?>( initialState );
            var testedCollection = (IList) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void IList_Add_Class_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<TestClass>( new[] { ITEM2 } );
            var testedCollection = (IList) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Add( 3.0 ) );

            // Assert

            Assert.Equal( "value", exception.ParamName );
            Assert.Equal( new[] { ITEM2 }, testedCollection );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Add_Value_InvalidNull()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<int>( new[] { 22, 44 } );
            var testedCollection = (IList) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Add( null ) );

            // Assert

            Assert.Equal( "value", exception.ParamName );
            Assert.Equal( new[] { 22, 44 }, testedCollection );
            Assert.Empty( events );
        }

        public class Insert_Class_TestData
            : TheoryData<IEnumerable<TestClass?>, int, TestClass?, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public Insert_Class_TestData()
            {
                Add( new TestClass?[] { },
                     5,
                     ITEM2,
                     new TestClass?[] { ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 0, -1 ) } );
                Add( new TestClass?[] { ITEM2, null, ITEM1 },
                     0,
                     ITEM3,
                     new TestClass?[] { null, ITEM1, ITEM2, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 3, -1 ) } );
                Add( new TestClass?[] { ITEM2 },
                     -1,
                     null,
                     new TestClass?[] { null, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
                Add( new TestClass?[] { null, ITEM2, ITEM1 },
                     2,
                     null,
                     new TestClass?[] { null, null, ITEM1, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( Insert_Class_TestData ) )]
        public void Insert_Class( IEnumerable<TestClass?> initialState, int index, TestClass? insertedItem, IEnumerable<TestClass?> expectedState,
                                  IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<TestClass?> observableList = new ObservableSortedList<TestClass?>( initialState );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableList.Insert( index, insertedItem );

            // Assert

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        public class IList_Insert_Class_TestData
            : TheoryData<IEnumerable<TestClass>, int, TestClass?, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public IList_Insert_Class_TestData()
            {
                Add( new TestClass[] { },
                     10,
                     ITEM2,
                     new TestClass?[] { ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 0, -1 ) } );
                Add( new TestClass[] { ITEM2, ITEM1 },
                     0,
                     ITEM3,
                     new TestClass?[] { ITEM1, ITEM2, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 2, -1 ) } );
                Add( new TestClass[] { ITEM2 },
                     -1,
                     null,
                     new TestClass?[] { null, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( IList_Insert_Class_TestData ) )]
        public void IList_Insert_Class( IEnumerable<TestClass> initialState, int index, TestClass? insertedItem, IEnumerable<TestClass?> expectedState,
                                        IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<TestClass>( initialState );
            var testedCollection = (IList) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.Insert( index, insertedItem );

            // Assert state

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void IList_Insert_Value_InvalidNull()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<int>() { 23 };
            var testedCollection = (IList) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Insert( 0, null ) );

            // Assert

            Assert.Equal( "value", exception.ParamName );
            Assert.Equal( new[] { 23 }, testedCollection );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Insert_Value_InvalidType()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass>() { ITEM2 };
            var testedCollection = (IList) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Insert( 0, 3.0 ) );

            // Assert

            Assert.Equal( "value", exception.ParamName );
            Assert.Equal( new[] { ITEM2 }, testedCollection );
            Assert.Empty( events );
        }

        public class InsertRange_Class_TestData
            : TheoryData<IEnumerable<TestClass?>, int, IEnumerable<TestClass?>, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public InsertRange_Class_TestData()
            {
                Add( new TestClass?[] { },
                     -1,
                     new TestClass?[] { ITEM2 },
                     new TestClass?[] { ITEM2 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 0, -1 ),
                     } );
                Add( new TestClass?[] { ITEM2, null, ITEM1 },
                     0,
                     new TestClass?[] { ITEM3, ITEM1 },
                     new TestClass?[] { null, ITEM1, ITEM1, ITEM2, ITEM3 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 3, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM1 }, null, 1, -1 ),
                     } );
                Add( new TestClass?[] { ITEM2 },
                     1,
                     new TestClass?[] { null },
                     new TestClass?[] { null, ITEM2 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ),
                     } );
                Add( new TestClass?[] { null, ITEM2, ITEM1 },
                     2,
                     new TestClass?[] { null, ITEM3 },
                     new TestClass?[] { null, null, ITEM1, ITEM2, ITEM3 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 4, -1 ),
                     } );
            }
        }

        [Theory]
        [ClassData( typeof( InsertRange_Class_TestData ) )]
        public void InsertRange_Class( IEnumerable<TestClass?> initialState, int index, IEnumerable<TestClass?> insertedItems, IEnumerable<TestClass?> expectedState,
                                       IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<TestClass?> observableList = new ObservableSortedList<TestClass?>( initialState );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableList.InsertRange( index, insertedItems );

            // Assert

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        public class IListEx_InsertRange_Class_TestData
            : TheoryData<IEnumerable<TestClass>, int, IEnumerable<TestClass?>, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public IListEx_InsertRange_Class_TestData()
            {
                Add( new TestClass[] { },
                     0,
                     new TestClass?[] { ITEM2 },
                     new TestClass?[] { ITEM2 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 0, -1 ),
                     } );
                Add( new TestClass[] { ITEM2, ITEM1 },
                     0,
                     new TestClass?[] { ITEM3 },
                     new TestClass?[] { ITEM1, ITEM2, ITEM3 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 2, -1 ),
                     } );
                Add( new TestClass[] { ITEM2 },
                     10,
                     new TestClass?[] { ITEM1, null },
                     new TestClass?[] { null, ITEM1, ITEM2 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM1 }, null, 0, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ),
                     } );
            }
        }

        [Theory]
        [ClassData( typeof( IListEx_InsertRange_Class_TestData ) )]
        public void IListEx_InsertRange_Class( IEnumerable<TestClass> initialState, int index, IEnumerable<TestClass?> insertedItems, IEnumerable<TestClass?> expectedState,
                                               IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<TestClass>( initialState );
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.InsertRange( index, insertedItems );

            // Assert state

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void IListEx_InsertRange_Value_InvalidNull()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<int>( new[] { 5, 8, 2 } );
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            Assert.Throws<NullReferenceException>( () => testedCollection.InsertRange( 2, new object?[] { 66, null, 32 } ) );

            // Assert

            Assert.Equal( new[] { 2, 5, 8 }, (IList) testedCollection );
            Assert.Empty( events );
        }

        [Fact]
        public void IListEx_InsertRange_Value_InvalidType()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<int>( new[] { 5, 8, 2 } );
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            Assert.Throws<InvalidCastException>( () => testedCollection.InsertRange( 2, new[] { 66, 1.1, 55 } ) );

            // Assert

            Assert.Equal( new[] { 2, 5, 8 }, (IList) testedCollection );
            Assert.Empty( events );
        }

        public class IList_Remove_Value_TestData
            : TheoryData<IEnumerable<int?>, object?, IEnumerable<int?>, IEnumerable<CollectionChangedEventData>>
        {
            public IList_Remove_Value_TestData()
            {
                Add( new int?[] { 8, null, 44, 5 },
                     5,
                     new int?[] { null, 8, 44 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5 }, -1, 1 ) } );
                Add( new int?[] { 8, null, 44, 5 },
                     null,
                     new int?[] { 5, 8, 44 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ) } );
                Add( new int?[] { 12, 2, 13 },
                     4,
                     new int?[] { 2, 12, 13 },
                     new CollectionChangedEventData[] { } );
                Add( new int?[] { },
                     5,
                     new int?[] { },
                     new CollectionChangedEventData[] { } );
                Add( new int?[] { 33 },
                     null,
                     new int?[] { 33 },
                     new CollectionChangedEventData[] { } );
                Add( new int?[] { 98 },
                     3.0,
                     new int?[] { 98 },
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( IList_Remove_Value_TestData ) )]
        public void IList_Remove_Value( IEnumerable<int?> initialState, object? removedItem, IEnumerable<int?> expectedState,
                                        IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<int?>( initialState );
            var testedCollection = (IList) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( IList_Remove_Value_TestData ) )]
        public void IListEx_Remove_Value( IEnumerable<int?> initialState, object? removedItem, IEnumerable<int?> expectedState,
                                          IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<int?>( initialState );
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        public class RemoveAt_Value_TestData
            : TheoryData<IEnumerable<int?>, int, IEnumerable<int?>, IEnumerable<CollectionChangedEventData>>
        {
            public RemoveAt_Value_TestData()
            {
                Add( new int?[] { 8, null, 44, 5 },
                     3,
                     new int?[] { null, 5, 8 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 44 }, -1, 3 ) } );
                Add( new int?[] { 8, null, 44, 5 },
                     1,
                     new int?[] { null, 8, 44 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5 }, -1, 1 ) } );
                Add( new int?[] { 8, null, 44, 5 },
                     0,
                     new int?[] { 5, 8, 44 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( RemoveAt_Value_TestData ) )]
        public void RemoveAt_Value( IEnumerable<int?> initialState, int index, IEnumerable<int?> expectedState,
                                    IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<int?> observableList = new ObservableSortedList<int?>( initialState );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableList.RemoveAt( index );

            // Assert state

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [InlineData( -129 )]
        [InlineData( -1 )]
        [InlineData( 3 )]
        [InlineData( 15 )]
        public void RemoveAt_OutOfRange( int index )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { ITEM1, ITEM2, ITEM3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            var exception = Assert.Throws<ArgumentOutOfRangeException>( () => observableList.RemoveAt( index ) );

            // Assert

            Assert.Equal( "index", exception.ParamName );
            Assert.Equal( new[] { ITEM1, ITEM2, ITEM3 }, observableList );
            Assert.Empty( events );
        }

        public class RemoveRange_Value_TestData
            : TheoryData<IEnumerable<int?>, int, int, IEnumerable<int?>, IEnumerable<CollectionChangedEventData>>
        {
            public RemoveRange_Value_TestData()
            {
                Add( new int?[] { 8, null, 44, 5 },
                     3,
                     1,
                     new int?[] { null, 5, 8 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 44 }, -1, 3 ),
                     } );
                Add( new int?[] { 8, null, 44, 5 },
                     1,
                     2,
                     new int?[] { null, 44 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5 }, -1, 1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 8 }, -1, 1 ),
                     } );
                Add( new int?[] { 8, 44, 5 },
                     1,
                     0,
                     new int?[] { 5, 8, 44 },
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( RemoveRange_Value_TestData ) )]
        public void RemoveRange_Value( IEnumerable<int?> initialState, int index, int count, IEnumerable<int?> expectedState,
                                       IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<int?> observableList = new ObservableSortedList<int?>( initialState );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableList.RemoveRange( index, count );

            // Assert state

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [InlineData( -129, 1 )]
        [InlineData( -1, 1 )]
        public void RemoveRange_Index_OutOfRange( int index, int count )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { ITEM1, ITEM2, ITEM3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            var exception = Assert.Throws<ArgumentOutOfRangeException>( () => observableList.RemoveRange( index, count ) );

            // Assert

            Assert.Equal( "index", exception.ParamName );
            Assert.Equal( new[] { ITEM1, ITEM2, ITEM3 }, observableList );
            Assert.Empty( events );
        }

        [Theory]
        [InlineData( 2, -1 )]
        [InlineData( 0, -34 )]
        public void RemoveRange_Count_OutOfRange( int index, int count )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { ITEM1, ITEM2, ITEM3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            var exception = Assert.Throws<ArgumentOutOfRangeException>( () => observableList.RemoveRange( index, count ) );

            // Assert

            Assert.Equal( "count", exception.ParamName );
            Assert.Equal( new[] { ITEM1, ITEM2, ITEM3 }, observableList );
            Assert.Empty( events );
        }

        [Theory]
        [InlineData( 3, 0 )]
        [InlineData( 3, 2 )]
        [InlineData( 15, 1 )]
        [InlineData( 2, 2 )]
        [InlineData( 0, 4 )]
        public void RemoveRange_Combination_OutOfRange( int index, int count )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { ITEM1, ITEM2, ITEM3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            Assert.Throws<ArgumentException>( () => observableList.RemoveRange( index, count ) );

            // Assert

            Assert.Equal( new[] { ITEM1, ITEM2, ITEM3 }, observableList );
            Assert.Empty( events );
        }

        public class ReplaceByIndex_Value_TestData
            : TheoryData<IEnumerable<double>, int, double, IEnumerable<double>, IEnumerable<CollectionChangedEventData>>
        {
            public ReplaceByIndex_Value_TestData()
            {
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     1,
                     34.0,
                     new double[] { 3.2, 8.0, 34.0, 44.5 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 34.0 }, new object?[] { 5.2 }, -1, -1 ) } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3,
                     34.0,
                     new double[] { 3.2, 5.2, 8.0, 34.0 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 34.0 }, new object?[] { 44.5 }, -1, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( ReplaceByIndex_Value_TestData ) )]
        public void ReplaceByIndex_Value( IEnumerable<double> initialState, int index, double newItem,
                                          IEnumerable<double> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<double> observableList = new ObservableSortedList<double>( initialState );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableList.Replace( index, newItem );

            // Assert

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [InlineData( -8129 )]
        [InlineData( -1 )]
        [InlineData( 3 )]
        [InlineData( 152 )]
        void ReplaceByIndex_OutOfRange( int index )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<double> observableList = new ObservableSortedList<double>( new[] { 5.1, 8.0, 2.9 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            var exception = Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Replace( index, 3.5 ) );

            // Assert

            Assert.Equal( "index", exception.ParamName );
            Assert.Equal( new[] { 2.9, 5.1, 8.0 }, observableList );
            Assert.Empty( events );
        }

        public class IListEx_ReplaceByIndex_Value_TestData
            : TheoryData<IEnumerable<double>, int, double, IEnumerable<double>, IEnumerable<CollectionChangedEventData>>
        {
            public IListEx_ReplaceByIndex_Value_TestData()
            {
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     1,
                     34.0,
                     new double[] { 3.2, 8.0, 34.0, 44.5, },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 34.0 }, new object?[] { 5.2 }, -1, -1 ) } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3,
                     34.0,
                     new double[] { 3.2, 5.2, 8.0, 34.0 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 34.0 }, new object?[] { 44.5 }, -1, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( IListEx_ReplaceByIndex_Value_TestData ) )]
        public void IListEx_ReplaceByIndex_Value( IEnumerable<double> initialState, int index, double newItem,
                                                  IEnumerable<double> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<double>( initialState );
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.Replace( index, newItem );

            // Assert state & result

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void IListEx_ReplaceByIndex_InvalidNull()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<double>( new[] { 5.1, 8.0, 2.9 } );
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Replace( 1, null ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new[] { 2.9, 5.1, 8.0 }, observableList );
            Assert.Empty( events );
        }

        [Fact]
        public void IListEx_ReplaceByIndex_InvalidType()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<double>( new[] { 5.1, 8.0, 2.9 } );
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Replace( 1, 3.5f ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new[] { 2.9, 5.1, 8.0 }, observableList );
            Assert.Empty( events );
        }

        [Theory]
        [InlineData( -8129 )]
        [InlineData( -1 )]
        [InlineData( 3 )]
        [InlineData( 152 )]
        void IListEx_ReplaceByIndex_OutOfRange( int index )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<double>( new[] { 5.1, 8.0, 2.9 } );
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            var exception = Assert.Throws<ArgumentOutOfRangeException>( () => testedCollection.Replace( index, 3.5 ) );

            // Assert

            Assert.Equal( "index", exception.ParamName );
            Assert.Equal( new[] { 2.9, 5.1, 8.0 }, observableList );
            Assert.Empty( events );
        }

        public class Move_Value_TestData
            : TheoryData<double?, int, bool>
        {
            public Move_Value_TestData()
            {
                Add( 8.0,
                     3,
                     true ); // Same position (non-null)
                Add( null,
                     2,
                     true ); // Same position (null)
                Add( 8.0,
                     5,
                     true ); // Different position (non-null)
                Add( null,
                     44,
                     true ); // Different position (null)
                Add( 8.01,
                     1,
                     false ); // Item not present
            }
        }

        [Theory]
        [ClassData( typeof( Move_Value_TestData ) )]
        public void Move_Value( double? movedItem, int index, bool expectedResult )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<double?> observableList = new ObservableSortedList<double?>() { 5.1, 8.0, null, 2.9, 44.5 };

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableList.Move( movedItem, index );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( new double?[] { null, 2.9, 5.1, 8.0, 44.5 }, observableList );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( Move_Value_TestData ) )]
        public void IListEx_Move_Value( double? movedItem, int index, bool expectedResult )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<double?> observableList = new ObservableSortedList<double?>() { 5.1, 8.0, null, 2.9, 44.5 };
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Move( movedItem, index );

            // Assert state & result

            Assert.Equal( expectedResult, result );
            Assert.Equal( new double?[] { null, 2.9, 5.1, 8.0, 44.5 }, observableList );
            Assert.Empty( events );
        }

        [Fact]
        public void IListEx_Move_InvalidObject()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<double> observableList = new ObservableSortedList<double>( new[] { 5.1, 8.0, 2.9, 44.5 } );
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = testedCollection.Move( 5.1f, 0 );

            // Assert state & result

            Assert.False( result );
            Assert.Equal( new[] { 2.9, 5.1, 8.0, 44.5 }, observableList );
            Assert.Empty( events );
        }

        [Theory]
        [InlineData( 0, 0 )]
        [InlineData( -1, 0 )]
        [InlineData( 0, -1 )]
        [InlineData( 4, 0 )]
        [InlineData( 5, 0 )]
        [InlineData( 15, 0 )]
        [InlineData( 0, 4 )]
        [InlineData( 0, 5 )]
        [InlineData( 0, 14 )]

        void MoveByIndex_Value( int oldIndex, int newIndex )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<double?> observableList = new ObservableSortedList<double?>() { 5.1, 8.0, null, 2.9, 44.5 };

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableList.Move( oldIndex, newIndex );

            // Assert

            Assert.Equal( new double?[] { null, 2.9, 5.1, 8.0, 44.5 }, observableList );
            Assert.Empty( events );
        }

        public class GetRange_TestData : TheoryData<int, int, IEnumerable<TestClass?>>
        {
            public GetRange_TestData()
            {
                Add( 1, 2, new TestClass?[] { ITEM1, ITEM2 } );
                Add( 0, 4, new TestClass?[] { null, ITEM1, ITEM2, ITEM2 } );
            }
        }

        [Theory]
        [ClassData( typeof( GetRange_TestData ) )]
        public void GetRange( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass?>() { ITEM2, ITEM3, null, ITEM2, ITEM1 };

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = observableList.GetRange( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( GetRange_TestData ) )]
        public void IObservableList_GetRange( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass?>() { ITEM2, ITEM3, null, ITEM2, ITEM1 };
            var testedCollection = (IObservableList<TestClass>) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = testedCollection.GetRange( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( GetRange_TestData ) )]
        public void IReadOnlyListEx_GetRange( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass?>() { ITEM2, ITEM3, null, ITEM2, ITEM1 };
            var testedCollection = (IReadOnlyListEx<TestClass>) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = testedCollection.GetRange( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( GetRange_TestData ) )]
        public void IObservableReadOnlyList_GetRange( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass?>() { ITEM2, ITEM3, null, ITEM2, ITEM1 };
            var testedCollection = (IObservableReadOnlyList<TestClass>) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = testedCollection.GetRange( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( GetRange_TestData ) )]
        public void IListExT_GetRange( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass?>() { ITEM2, ITEM3, null, ITEM2, ITEM1 };
            var testedCollection = (IListEx<TestClass>) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = testedCollection.GetRange( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( GetRange_TestData ) )]
        public void IListEx_GetRange( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass?>() { ITEM2, ITEM3, null, ITEM2, ITEM1 };
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = testedCollection.GetRange( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( GetRange_TestData ) )]
        public void Slice( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass?>() { ITEM2, ITEM3, null, ITEM2, ITEM1 };

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = observableList.Slice( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( GetRange_TestData ) )]
        public void IObservableList_Slice( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass?>() { ITEM2, ITEM3, null, ITEM2, ITEM1 };
            var testedCollection = (IObservableList<TestClass>) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = testedCollection.Slice( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( GetRange_TestData ) )]
        public void IReadOnlyListEx_Slice( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass?>() { ITEM2, ITEM3, null, ITEM2, ITEM1 };
            var testedCollection = (IReadOnlyListEx<TestClass>) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = testedCollection.Slice( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( GetRange_TestData ) )]
        public void IObservableReadOnlyList_Slice( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass?>() { ITEM2, ITEM3, null, ITEM2, ITEM1 };
            var testedCollection = (IObservableReadOnlyList<TestClass>) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = testedCollection.Slice( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( GetRange_TestData ) )]
        public void IListExT_Slice( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass?>() { ITEM2, ITEM3, null, ITEM2, ITEM1 };
            var testedCollection = (IListEx<TestClass>) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = testedCollection.Slice( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( GetRange_TestData ) )]
        public void IListEx_Slice( int index, int count, IEnumerable expectedResult )
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass?>() { ITEM2, ITEM3, null, ITEM2, ITEM1 };
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = testedCollection.Slice( index, count );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        public class IList_Contains_Value_Nullable_TestData : TheoryData<IEnumerable<int?>, object?, bool>
        {
            public IList_Contains_Value_Nullable_TestData()
            {
                Add( new int?[] { 8, 9 }, 8, true );
                Add( new int?[] { 8, 9, null }, 9, true );
                Add( new int?[] { 8, 9, null }, null, true );
                Add( new int?[] { 8, 9 }, 3, false );
                Add( new int?[] { 8, 9 }, null, false );
                Add( new int?[] { 8, 9, null }, "8", false );
            }
        }

        [Theory]
        [ClassData( typeof( IList_Contains_Value_Nullable_TestData ) )]
        public void IList_Contains_Value_Nullable( IEnumerable<int?> initialState, object? searchedItem, bool expectedResult )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<int?>( initialState );
            var testedCollection = (IList) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act & Assert results

            var result = testedCollection.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( IList_Contains_Value_Nullable_TestData ) )]
        public void IListEx_Contains_Value_Nullable( IEnumerable<int?> initialState, object? searchedItem, bool expectedResult )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<int?>( initialState );
            var testedCollection = (IListEx) observableList;

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act & Assert results

            var result = testedCollection.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Fact]
        public void IndexOf()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { ITEM2, ITEM3, ITEM2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            Assert.Equal( -1, observableList.IndexOf( ITEM1 ) );
            Assert.Equal( 0, observableList.IndexOf( ITEM2 ) );
            Assert.Equal( 1, observableList.IndexOf( ITEM2, 1 ) );
            Assert.Equal( -1, observableList.IndexOf( ITEM2, 2, 1 ) );
            Assert.Equal( 2, observableList.IndexOf( ITEM3 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IList_IndexOf()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass>( new[] { ITEM2, ITEM3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            // Act & Assert

            Assert.Equal( -1, list.IndexOf( ITEM1 ) );
            Assert.Equal( 0, list.IndexOf( ITEM2 ) );
            Assert.Equal( 1, list.IndexOf( ITEM3 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlyListEx_IndexOf()
        {
            // Arrange

            var list = new ObservableSortedList<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            // Act & Assert

            Assert.Equal( 3, collection.IndexOf( 6 ) );
            Assert.Equal( -1, collection.IndexOf( 10 ) );
            Assert.Equal( -1, collection.IndexOf( 10.0 ) );

            Assert.Equal( 1, collection.IndexOf( 3, 1 ) );
            Assert.Equal( -1, collection.IndexOf( 3, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 4.0, 1 ) );

            Assert.Equal( 1, collection.IndexOf( 3, 1, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 6, 1, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 3.0, 2, 2 ) );
        }

        [Fact]
        public void IListEx_IndexOf()
        {
            // Arrange

            var list = new ObservableSortedList<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IListEx) list );

            // Act & Assert

            Assert.Equal( 3, collection.IndexOf( 6 ) );
            Assert.Equal( -1, collection.IndexOf( 10 ) );
            Assert.Equal( -1, collection.IndexOf( 10.0 ) );

            Assert.Equal( 1, collection.IndexOf( 3, 1 ) );
            Assert.Equal( -1, collection.IndexOf( 3, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 4.0, 1 ) );

            Assert.Equal( 1, collection.IndexOf( 3, 1, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 6, 1, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 3.0, 2, 2 ) );
        }

        [Fact]
        public void LastIndexOf()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { ITEM2, ITEM3, ITEM2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            Assert.Equal( -1, observableList.LastIndexOf( ITEM1 ) );
            Assert.Equal( 1, observableList.LastIndexOf( ITEM2 ) );
            Assert.Equal( 0, observableList.LastIndexOf( ITEM2, 0 ) );
            Assert.Equal( -1, observableList.LastIndexOf( ITEM2, 2, 1 ) );
            Assert.Equal( 2, observableList.LastIndexOf( ITEM3 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlyListEx_LastIndexOf()
        {
            // Arrange

            var list = new ObservableSortedList<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            // Act & Assert

            Assert.Equal( 3, collection.LastIndexOf( 6 ) );
            Assert.Equal( -1, collection.LastIndexOf( 10 ) );
            Assert.Equal( -1, collection.LastIndexOf( 10.0 ) );

            Assert.Equal( 1, collection.LastIndexOf( 3, 1 ) );
            Assert.Equal( -1, collection.LastIndexOf( 4, 1 ) );
            Assert.Equal( -1, collection.LastIndexOf( 4.0, 1 ) );

            Assert.Equal( 1, collection.LastIndexOf( 3, 1, 2 ) );
            Assert.Equal( -1, collection.LastIndexOf( 3, 3, 2 ) );
            Assert.Equal( -1, collection.LastIndexOf( 3.0, 2, 2 ) );
        }

        [Fact]
        public void IListEx_LastIndexOf()
        {
            // Arrange

            var list = new ObservableSortedList<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IListEx) list );

            // Act & Assert

            Assert.Equal( 3, collection.LastIndexOf( 6 ) );
            Assert.Equal( -1, collection.LastIndexOf( 10 ) );
            Assert.Equal( -1, collection.LastIndexOf( 10.0 ) );

            Assert.Equal( 1, collection.LastIndexOf( 3, 1 ) );
            Assert.Equal( -1, collection.LastIndexOf( 4, 1 ) );
            Assert.Equal( -1, collection.LastIndexOf( 4.0, 1 ) );

            Assert.Equal( 1, collection.LastIndexOf( 3, 1, 2 ) );
            Assert.Equal( -1, collection.LastIndexOf( 3, 3, 2 ) );
            Assert.Equal( -1, collection.LastIndexOf( 3.0, 2, 2 ) );
        }

        [Fact]
        public void Indexer_Get_ValidIndex()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { ITEM2, ITEM3, ITEM1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            Assert.Equal( ITEM1, observableList[ 0 ] );
            Assert.Equal( ITEM2, observableList[ 1 ] );
            Assert.Equal( ITEM3, observableList[ 2 ] );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void Indexer_Get_InvalidIndex()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { ITEM2, ITEM3, ITEM1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -654 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -1 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ 3 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ 34564 ] );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void Indexer_Set_ValidIndex()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { ITEM2, ITEM3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { ITEM2, ITEM3 }, observableList );

            // Act

            observableList[ 1 ] = ITEM1;

            // Assert state

            Assert.Equal( new[] { ITEM1, ITEM2 }, observableList );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Replace, events[ 0 ].Action );
            Assert.Equal( new[] { ITEM1 }, events[ 0 ].NewItems );
            Assert.Equal( new[] { ITEM3 }, events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void Indexer_Set_InvalidIndex()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { ITEM3, ITEM1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -765 ] = ITEM2 );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -1 ] = ITEM2 );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ 3 ] = ITEM2 );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ 346 ] = ITEM2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Indexer_Get()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass>( new[] { ITEM2, ITEM3, ITEM1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            // Act & Assert

            Assert.Equal( ITEM1, list[ 0 ] );
            Assert.Equal( ITEM2, list[ 1 ] );
            Assert.Equal( ITEM3, list[ 2 ] );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IList_Indexer_Set()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass>( new[] { ITEM2, ITEM3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { ITEM2, ITEM3 }, list );

            // Act

            list[ 1 ] = ITEM1;

            // Assert state

            Assert.Equal( new[] { ITEM1, ITEM2 }, list );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Replace, events[ 0 ].Action );
            Assert.Equal( new[] { ITEM1 }, events[ 0 ].NewItems );
            Assert.Equal( new[] { ITEM3 }, events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void IList_Indexer_Set_InvalidType()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableSortedList<TestClass>( new[] { ITEM2, ITEM3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { ITEM2, ITEM3 }, list );

            // Act & Assert

            Assert.Throws<ArgumentException>( () => list[ 1 ] = 3.0 );

            // Assert state

            Assert.Equal( new[] { ITEM2, ITEM3 }, list );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void NoEventListeners()
        {
            // Arrange

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { ITEM2 } );

            Assert.Equal( new[] { ITEM2 }, observableList );

            // Act

            observableList.Add( ITEM1 );

            Assert.Equal( new[] { ITEM1, ITEM2 }, observableList );

            // Act

            observableList.Insert( 0, ITEM3 );

            Assert.Equal( new[] { ITEM1, ITEM2, ITEM3 }, observableList );

            // Act

            observableList.Remove( ITEM2 );

            Assert.Equal( new[] { ITEM1, ITEM3 }, observableList );

            // Act

            observableList.RemoveAt( 0 );

            Assert.Equal( new[] { ITEM3 }, observableList );
        }
    }
}
