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
#pragma warning disable xUnit2013 // Assert.Equal(0, collection.Count) is used because we want to check the result of the Count property, not the collection itself.
#pragma warning disable xUnit1045
#pragma warning disable CA1825 // Using zero-length array initializers is intentionally to improve test maintainability.
#pragma warning disable CA1859 // Using interface types is intentional to call the interface methods.
#pragma warning disable CA1861 // Using constant arrays is intentionally to improve test maintainability.
#pragma warning restore IDE0079

#pragma warning disable S101 // TestData types are not strictly PascalCase

namespace Utilities.DotNet.Collections.Observables.Test
{
    public class ObservableSortedSetTest
    {
        private static readonly TestClass ITEM1 = new( "Item1", 10 );
        private static readonly TestClass ITEM2 = new( "Item2", 5 );
        private static readonly TestClass ITEM3 = new( "Item3", 20 );

        [Fact]
        public void Constructor_Default()
        {
            // Act

            var observableSet = new ObservableSortedSet<TestClass>();

            // Assert

            Assert.Equal( 0, observableSet.Count );
            Assert.Equal( new TestClass[] { }, observableSet );
            Assert.Same( Comparer<TestClass>.Default, observableSet.Comparer );

            Assert.False( ( (ICollection<TestClass>) observableSet ).IsReadOnly );

            Assert.NotNull( ( (ICollection) observableSet ).SyncRoot );
            Assert.False( ( (ICollection) observableSet ).IsSynchronized );

            // The following tests are to ensure interface disambiguation.

            Assert.Equal( 0, ( (IObservableSet<TestClass>) observableSet ).Count );
            Assert.Equal( 0, ( (ICollectionEx<TestClass>) observableSet ).Count );
        }

        [Fact]
        public void Constructor_Comparer()
        {
            // Arrange

#pragma warning disable S2234 // Arguments should be passed in the same order as the method parameters
            var customComparer = Comparer<int>.Create( ( x, y ) => Comparer<int>.Default.Compare( y, x ) );
#pragma warning restore S2234 // Arguments should be passed in the same order as the method parameters

            // Act

            var observableSet = new ObservableSortedSet<int>( customComparer );

            // Assert

            Assert.Equal( 0, observableSet.Count );
            Assert.Equal( new int[] { }, observableSet );
            Assert.Same( customComparer, observableSet.Comparer );

            Assert.False( ( (IObservableCollection<int>) observableSet ).IsReadOnly );

            Assert.NotNull( ( (ICollection) observableSet ).SyncRoot );
            Assert.False( ( (ICollection) observableSet ).IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            // Act

            var observableSet = new ObservableSortedSet<TestClass>( new[] { ITEM1, ITEM3, ITEM2 } );

            // Assert

            Assert.Equal( 3, observableSet.Count );
            Assert.Equal( new[] { ITEM1, ITEM2, ITEM3 }, observableSet );
            Assert.Equal( Comparer<TestClass>.Default, observableSet.Comparer );

            Assert.False( ( (IObservableSet<TestClass>) observableSet ).IsReadOnly );

            Assert.NotNull( ( (ICollection) observableSet ).SyncRoot );
            Assert.False( ( (ICollection) observableSet ).IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationListAndComparer()
        {
            // Arrange

#pragma warning disable S2234 // Arguments should be passed in the same order as the method parameters
            var customComparer = Comparer<int>.Create( ( x, y ) => Comparer<int>.Default.Compare( y, x ) );
#pragma warning restore S2234 // Arguments should be passed in the same order as the method parameters

            // Act

            var observableSet = new ObservableSortedSet<int>( new[] { 9, 8, 23 }, customComparer );

            // Assert

            Assert.Equal( 3, observableSet.Count );
            Assert.Equal( new[] { 23, 9, 8 }, observableSet );
            Assert.Same( customComparer, observableSet.Comparer );

            Assert.False( ( (IObservableSet<int>) observableSet ).IsReadOnly );

            Assert.NotNull( ( (ICollection) observableSet ).SyncRoot );
            Assert.False( ( (ICollection) observableSet ).IsSynchronized );
        }

        [Fact]
        public void Dispose()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableSet = new ObservableSortedSet<int>( new[] { 1, 2, 3 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act

            observableSet.Dispose();

            // Assert

            Assert.Equal( new int[] { }, observableSet );
            Assert.Equal( 0, observableSet.Count );

            Assert.Empty( events );
        }

        public class Add_Class_NonNullable_TestData
            : TheoryData<IEnumerable<TestClass>, TestClass, bool, IEnumerable<TestClass>, IEnumerable<CollectionChangedEventData>>
        {
            public Add_Class_NonNullable_TestData()
            {
                Add( new[] { ITEM1 },
                     ITEM2,
                     true,
                     new[] { ITEM1, ITEM2 }, // Not present
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ) } );
                Add( new[] { ITEM2, ITEM1 },
                     ITEM3,
                     true,
                     new[] { ITEM1, ITEM2, ITEM3 }, // Not present
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 2, -1 ) } );
                Add( new[] { ITEM1, ITEM2 },
                     ITEM1,
                     false,
                     new[] { ITEM1, ITEM2 }, // Already present
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( Add_Class_NonNullable_TestData ) )]
        public void Add_Class_NonNullable( IEnumerable<TestClass> initialState, TestClass addedItem, bool expectedResult, IEnumerable<TestClass> expectedState,
                                           IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( initialState );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableSet.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class Add_Class_Nullable_TestData
            : TheoryData<IEnumerable<TestClass?>, TestClass?, bool, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public Add_Class_Nullable_TestData()
            {
                Add( new[] { ITEM1 },
                     ITEM2,
                     true,
                     new[] { ITEM1, ITEM2 }, // Not present
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ) } );
                Add( new[] { ITEM2, ITEM1 },
                     ITEM3,
                     true,
                     new[] { ITEM1, ITEM2, ITEM3 }, // Not present
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 2, -1 ) } );
                Add( new[] { ITEM1, ITEM2 },
                     null,
                     true,
                     new[] { null, ITEM1, ITEM2 }, // Not present
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
                Add( new[] { ITEM1, null },
                     ITEM2,
                     true,
                     new[] { null, ITEM1, ITEM2 }, // Not present
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 2, -1 ) } );
                Add( new[] { ITEM1, null, ITEM2 },
                     ITEM1,
                     false,
                     new[] { null, ITEM1, ITEM2 }, // Already present
                     new CollectionChangedEventData[] { } );
                Add( new[] { ITEM1, null, ITEM3 },
                     null,
                     false,
                     new[] { null, ITEM1, ITEM3 }, // Not present
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( Add_Class_Nullable_TestData ) )]
        public void Add_Class_Nullable( IEnumerable<TestClass?> initialState, TestClass? addedItem, bool expectedResult, IEnumerable<TestClass?> expectedState,
                                        IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableSet<TestClass?> observableSet = new ObservableSortedSet<TestClass?>( initialState );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableSet.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( Add_Class_Nullable_TestData ) )]
        public void ICollectionExT_Add_Class_Nullable( IEnumerable<TestClass?> initialState, TestClass? addedItem, bool _, IEnumerable<TestClass?> expectedState,
                                                       IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<TestClass?>( initialState );
            var testedCollection = (ICollectionEx<TestClass?>) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class ICollectionEx_Add_Value_NonNullable_TestData
            : TheoryData<IEnumerable<int>, int, IEnumerable<int>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_Add_Value_NonNullable_TestData()
            {
                Add( new int[] { },
                     5,
                     new int[] { 5 }, // Not present
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 5 }, null, 0, -1 ) } );
                Add( new[] { 12, 2, 13 },
                     4,
                     new int[] { 2, 4, 12, 13 }, // Not present
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 4 }, null, 1, -1 ) } );
                Add( new int[] { 5, 23 },
                     5,
                     new int[] { 5, 23 }, // Already present
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Add_Value_NonNullable_TestData ) )]
        public void ICollectionEx_Add_Value_NonNullable( IEnumerable<int> initialState, int addedItem, IEnumerable<int> expectedState,
                                                         IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int>( initialState );
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void ICollectionEx_Add_Value_NonNullable_NullValue()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int>() { 33 };
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Add( null ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( new int[] { 33 }, (IEnumerable) observableSet );
            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Add_Value_NonNullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int>() { 33 };
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Add( 3.0 ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( new int[] { 33 }, (IEnumerable) observableSet );
            Assert.Empty( events );
        }

        public class ICollectionEx_Add_Value_Nullable_TestData
            : TheoryData<IEnumerable<int?>, int?, IEnumerable<int?>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_Add_Value_Nullable_TestData()
            {
                Add( new int?[] { },
                     5,
                     new int?[] { 5 }, // Not present
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 5 }, null, 0, -1 ) } );
                Add( new int?[] { 12, 2, null, 13 },
                     4,
                     new int?[] { null, 2, 4, 12, 13 }, // Not present
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 4 }, null, 2, -1 ) } );
                Add( new int?[] { 12, 2, 13 },
                     null,
                     new int?[] { null, 2, 12, 13 }, // Not present
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
                Add( new int?[] { 12, 2, null, 13 },
                     2,
                     new int?[] { null, 2, 12, 13 }, // Already present
                     new CollectionChangedEventData[] { } );
                Add( new int?[] { 12, 2, null, 13 },
                     null,
                     new int?[] { null, 2, 12, 13 }, // Already present
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Add_Value_Nullable_TestData ) )]
        public void ICollectionEx_Add_Value_Nullable( IEnumerable<int?> initialState, int? addedItem, IEnumerable<int?> expectedState,
                                                      IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int?>( initialState );
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void ICollectionEx_Add_Value_Nullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int?>() { 33, null };
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Add( 3.0 ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( new int?[] { null, 33 }, (IEnumerable) observableSet );
            Assert.Empty( events );
        }

        public class AddRange_Class_NonNullable_TestData
            : TheoryData<IEnumerable<TestClass>, IEnumerable<TestClass>, IEnumerable<TestClass>, IEnumerable<CollectionChangedEventData>>
        {
            public AddRange_Class_NonNullable_TestData()
            {
                Add( new[] { ITEM1 },
                     new[] { ITEM3, ITEM2 },
                     new[] { ITEM1, ITEM2, ITEM3 }, // All not present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 1, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ),
                     } );
                Add( new[] { ITEM3, ITEM2 },
                     new[] { ITEM1 },
                     new[] { ITEM1, ITEM2, ITEM3 }, // Single not present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM1 }, null, 0, -1 ),
                     } );
                Add( new[] { ITEM3, ITEM2 },
                     new TestClass[] { },
                     new[] { ITEM2, ITEM3 },// No items added
                     new CollectionChangedEventData[] { } );
                Add( new[] { ITEM3, ITEM2 },
                     new[] { ITEM1, ITEM2 },
                     new[] { ITEM1, ITEM2, ITEM3 }, // Some present, some not present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM1 }, null, 0, -1 ),
                     } );
            }
        }

        [Theory]
        [ClassData( typeof( AddRange_Class_NonNullable_TestData ) )]
        public void AddRange_Class_NonNullable( IEnumerable<TestClass> initialState, IEnumerable<TestClass> addedItems, IEnumerable<TestClass> expectedState,
                                                IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( initialState );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableSet.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class AddRange_Class_Nullable_TestData
            : TheoryData<IEnumerable<TestClass?>, IEnumerable<TestClass?>, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public AddRange_Class_Nullable_TestData()
            {
                Add( new[] { ITEM1 },
                     new[] { ITEM3, ITEM2 },
                     new[] { ITEM1, ITEM2, ITEM3 }, // All not present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 1, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ),
                     } );
                Add( new[] { ITEM3, null, ITEM2 },
                     new[] { ITEM1 },
                     new[] { null, ITEM1, ITEM2, ITEM3 }, // Single not present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM1 }, null, 1, -1 ),
                     } );
                Add( new[] { ITEM3, ITEM1 },
                     new[] { null, ITEM1, ITEM2 },
                     new[] { null, ITEM1, ITEM2, ITEM3 }, // Some present, some not present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 2, -1 ),
                     } );
                Add( new[] { ITEM3, ITEM2 },
                     new TestClass[] { },
                     new[] { ITEM2, ITEM3 }, // No items added
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( AddRange_Class_Nullable_TestData ) )]
        public void AddRange_Class_Nullable( IEnumerable<TestClass?> initialState, IEnumerable<TestClass?> addedItems, IEnumerable<TestClass?> expectedState,
                                             IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableSet<TestClass?> observableSet = new ObservableSortedSet<TestClass?>( initialState );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableSet.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class ICollectionEx_AddRange_Value_NonNullable_TestData
            : TheoryData<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_AddRange_Value_NonNullable_TestData()
            {
                Add( new int[] { 5 },
                     new int[] { 9, 56 },
                     new int[] { 5, 9, 56 }, // All not present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 9 }, null, 1, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 56 }, null, 2, -1 ),
                     } );
                Add( new int[] { 7, 45 },
                     new int[] { 3 },
                     new int[] { 3, 7, 45 }, // Single not present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 3 }, null, 0, -1 ),
                     } );
                Add( new int[] { 5, 76 },
                     new int[] { },
                     new int[] { 5, 76 }, // No items added
                     new CollectionChangedEventData[] { } );
                Add( new int[] { 7, 45 },
                     new int[] { 45, 3 },
                     new int[] { 3, 7, 45 }, // Some already present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 3 }, null, 0, -1 ),
                     } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_AddRange_Value_NonNullable_TestData ) )]
        public void ICollectionEx_AddRange_Value_NonNullable( IEnumerable<int> initialState, IEnumerable<int> addedItems,
                                                              IEnumerable<int> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int>( initialState );
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void ICollectionEx_AddRange_Value_NonNullable_NullValue()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int>() { 33 };
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            Assert.Throws<NullReferenceException>( () => testedCollection.AddRange( new object?[] { 2, null, 5 } ) );

            // Assert

            Assert.Equal( new int[] { 33 }, observableSet );
            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_AddRange_Value_NonNullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int>() { 33 };
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            Assert.Throws<InvalidCastException>( () => testedCollection.AddRange( new object?[] { 2, 4.0, 5 } ) );

            // Assert

            Assert.Equal( new int[] { 33 }, observableSet );
            Assert.Empty( events );
        }

        public class ICollectionEx_AddRange_Value_Nullable_TestData
            : TheoryData<IEnumerable<int?>, IEnumerable<int?>, IEnumerable<int?>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_AddRange_Value_Nullable_TestData()
            {
                Add( new int?[] { 5 },
                     new int?[] { 9, null },
                     new int?[] { null, 5, 9 }, // All not present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 9 }, null, 1, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ),
                     } );
                Add( new int?[] { 7, null, 45 },
                     new int?[] { 3 },
                     new int?[] { null, 3, 7, 45 }, // Single not present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 3 }, null, 1, -1 ),
                     } );
                Add( new int?[] { 5, null, 76 },
                     new int?[] { },
                     new int?[] { null, 5, 76 }, // No items added
                     new CollectionChangedEventData[] { } );
                Add( new int?[] { 5, null, 76 },
                     new int?[] { null, 45 },
                     new int?[] { null, 5, 45, 76 }, // Some already present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 45 }, null, 2, -1 ),
                     } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_AddRange_Value_Nullable_TestData ) )]
        public void ICollectionEx_AddRange_Value_Nullable( IEnumerable<int?> initialState, IEnumerable<int?> addedItems,
                                                           IEnumerable<int?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int?>( initialState );
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void ICollectionEx_AddRange_Value_Nullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int?>() { null, 33 };
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            Assert.Throws<InvalidCastException>( () => testedCollection.AddRange( new object?[] { 2, 4.0, 5 } ) );

            // Assert

            Assert.Equal( new int?[] { null, 33 }, observableSet );
            Assert.Empty( events );
        }

        public class Remove_Class_NonNullable_TestData
            : TheoryData<IEnumerable<TestClass>, TestClass, bool, IEnumerable<TestClass>, IEnumerable<CollectionChangedEventData>>
        {
            public Remove_Class_NonNullable_TestData()
            {
                Add( new TestClass[] { ITEM1, ITEM2, ITEM3 },
                     ITEM2,
                     true,
                     new TestClass[] { ITEM1, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 1 ) } );
                Add( new TestClass[] { ITEM1, ITEM2 },
                     ITEM3,
                     false,
                     new TestClass[] { ITEM1, ITEM2 },
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( Remove_Class_NonNullable_TestData ) )]
        public void Remove_Class_NonNullable( IEnumerable<TestClass> initialState, TestClass removedItem, bool expectedResult,
                                              IEnumerable<TestClass> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( initialState );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableSet.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class Remove_Class_Nullable_TestData
            : TheoryData<IEnumerable<TestClass?>, TestClass?, bool, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public Remove_Class_Nullable_TestData()
            {
                Add( new TestClass?[] { ITEM1, ITEM2, ITEM3 },
                     ITEM2,
                     true,
                     new TestClass?[] { ITEM1, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 1 ) } );
                Add( new TestClass?[] { ITEM1, null, ITEM2, ITEM3 },
                     ITEM2,
                     true,
                     new TestClass?[] { null, ITEM1, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 2 ) } );
                Add( new TestClass?[] { ITEM1, null, ITEM2, ITEM3 },
                     null,
                     true,
                     new TestClass?[] { ITEM1, ITEM2, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ) } );
                Add( new TestClass?[] { ITEM1, ITEM2 },
                     ITEM3,
                     false,
                     new TestClass?[] { ITEM1, ITEM2 },
                     new CollectionChangedEventData[] { } );
                Add( new TestClass?[] { ITEM1, ITEM2, null },
                     ITEM3,
                     false,
                     new TestClass?[] { null, ITEM1, ITEM2 },
                     new CollectionChangedEventData[] { } );
                Add( new TestClass?[] { ITEM1, ITEM2 },
                     null,
                     false,
                     new TestClass?[] { ITEM1, ITEM2 },
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( Remove_Class_Nullable_TestData ) )]
        public void Remove_Class_Nullable( IEnumerable<TestClass?> initialState, TestClass? removedItem, bool expectedResult,
                                           IEnumerable<TestClass?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableSet<TestClass?> observableSet = new ObservableSortedSet<TestClass?>( initialState );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableSet.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class ICollectionEx_Remove_Value_NonNullable_TestData
            : TheoryData<IEnumerable<int>, object?, bool, IEnumerable<int>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_Remove_Value_NonNullable_TestData()
            {
                Add( new int[] { 8, 3, 44, 5 },
                     5,
                     true,
                     new int[] { 3, 8, 44 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5 }, -1, 1 ) } );
                Add( new int[] { 12, 2, 13 },
                     4,
                     false,
                     new int[] { 2, 12, 13 },
                     new CollectionChangedEventData[] { } );
                Add( new int[] { },
                     5,
                     false,
                     new int[] { },
                     new CollectionChangedEventData[] { } );
                Add( new int[] { 33 },
                     null,
                     false,
                     new int[] { 33 },
                     new CollectionChangedEventData[] { } );
                Add( new int[] { 98 },
                     3.0,
                     false,
                     new int[] { 98 },
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Remove_Value_NonNullable_TestData ) )]
        public void ICollectionEx_Remove_Value_NonNullable( IEnumerable<int> initialState, object? removedItem, bool expectedResult, IEnumerable<int> expectedState,
                                                            IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int>( initialState );
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class ICollectionEx_Remove_Value_Nullable_TestData
            : TheoryData<IEnumerable<int?>, object?, bool, IEnumerable<int?>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_Remove_Value_Nullable_TestData()
            {
                Add( new int?[] { 8, null, 44, 5 },
                     5,
                     true,
                     new int?[] { null, 8, 44 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5 }, -1, 1 ) } );
                Add( new int?[] { 8, null, 44, 5 },
                     null,
                     true,
                     new int?[] { 5, 8, 44 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ) } );
                Add( new int?[] { 12, 2, 13 },
                     4,
                     false,
                     new int?[] { 2, 12, 13 },
                     new CollectionChangedEventData[] { } );
                Add( new int?[] { },
                     5,
                     false,
                     new int?[] { },
                     new CollectionChangedEventData[] { } );
                Add( new int?[] { 33 },
                     null,
                     false,
                     new int?[] { 33 },
                     new CollectionChangedEventData[] { } );
                Add( new int?[] { 98 },
                     3.0,
                     false,
                     new int?[] { 98 },
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Remove_Value_Nullable_TestData ) )]
        public void ICollectionEx_Remove_Value_Nullable( IEnumerable<int?> initialState, object? removedItem, bool expectedResult, IEnumerable<int?> expectedState,
                                                         IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int?>( initialState );
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class RemoveRange_Class_NonNullable_TestData
            : TheoryData<IEnumerable<TestClass>, IEnumerable<TestClass>, IEnumerable<TestClass>, IEnumerable<CollectionChangedEventData>>
        {
            public RemoveRange_Class_NonNullable_TestData()
            {
                Add( new[] { ITEM1, ITEM2, ITEM3 },
                     new[] { ITEM3, ITEM2 },
                     new[] { ITEM1 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM3 }, -1, 2 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 1 ),
                     } );
                Add( new[] { ITEM1, ITEM2, ITEM3 },
                     new[] { ITEM2, ITEM3 },
                     new[] { ITEM1 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM3 }, -1, 1 ),
                     } );
                Add( new[] { ITEM3, ITEM2 },
                     new TestClass[] { },
                     new[] { ITEM2, ITEM3 },
                     new CollectionChangedEventData[] { } );
                Add( new[] { ITEM3, ITEM2 },
                     new[] { ITEM1 },
                     new[] { ITEM2, ITEM3 },
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( RemoveRange_Class_NonNullable_TestData ) )]
        public void RemoveRange_Class_NonNullable( IEnumerable<TestClass> initialState, IEnumerable<TestClass> removedItems,
                                                   IEnumerable<TestClass> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( initialState );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableSet.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class RemoveRange_Class_Nullable_TestData
            : TheoryData<IEnumerable<TestClass?>, IEnumerable<TestClass?>, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public RemoveRange_Class_Nullable_TestData()
            {
                Add( new[] { ITEM1, ITEM2, null, ITEM3 },
                     new[] { ITEM3, ITEM2 },
                     new[] { null, ITEM1 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM3 }, -1, 3 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 2 ),
                     } );
                Add( new[] { ITEM1, ITEM2, null, ITEM3 },
                     new[] { ITEM2, ITEM3, null },
                     new[] { ITEM1 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 2 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM3 }, -1, 2 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ),
                     } );
                Add( new[] { null, ITEM3, ITEM2 },
                     new TestClass[] { },
                     new[] { null, ITEM2, ITEM3 },
                     new CollectionChangedEventData[] { } );
                Add( new[] { ITEM3, ITEM2, null },
                     new[] { ITEM1 },
                     new[] { null, ITEM2, ITEM3 },
                     new CollectionChangedEventData[] { } );
                Add( new[] { ITEM3, ITEM2 },
                     new[] { null, ITEM2 },
                     new[] { ITEM3 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 0 ),
                     } );
            }
        }

        [Theory]
        [ClassData( typeof( RemoveRange_Class_Nullable_TestData ) )]
        public void RemoveRange_Class_Nullable( IEnumerable<TestClass?> initialState, IEnumerable<TestClass?> removedItems,
                                                IEnumerable<TestClass?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableSet<TestClass?> observableSet = new ObservableSortedSet<TestClass?>( initialState );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableSet.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class ICollectionEx_RemoveRange_Value_NonNullable_TestData
            : TheoryData<IEnumerable<int>, IEnumerable, IEnumerable<int>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_RemoveRange_Value_NonNullable_TestData()
            {
                Add( new int[] { 8, 3, 44, 5 },
                     new object?[] { 5, 3 },
                     new int[] { 8, 44 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5 }, -1, 1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3 }, -1, 0 ),
                     } );
                Add( new int[] { 8, 3, 44, 5 },
                     new object?[] { 3, 5 },
                     new int[] { 8, 44 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3 }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5 }, -1, 0 ),
                     } );
                Add( new int[] { 8, 3, 44, 5 },
                     new object?[] { 44, null, 8.0 },
                     new int[] { 3, 5, 8 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 44 }, -1, 3 ),
                     } );
                Add( new int[] { 12, 2, 13 },
                     new object?[] { 4 },
                     new int[] { 2, 12, 13 },
                     new CollectionChangedEventData[] { } );
                Add( new int[] { },
                     new object?[] { 5 },
                     new int[] { },
                     new CollectionChangedEventData[] { } );
                Add( new int[] { 33 },
                     new object?[] { null },
                     new int[] { 33 },
                     new CollectionChangedEventData[] { } );
                Add( new int[] { 98 },
                     new object?[] { 3.0 },
                     new int[] { 98 },
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_RemoveRange_Value_NonNullable_TestData ) )]
        public void ICollectionEx_RemoveRange_Value_NonNullable( IEnumerable<int> initialState, IEnumerable removedItems,
                                                                 IEnumerable<int> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int>( initialState );
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class ICollectionEx_RemoveRange_Value_Nullable_TestData
            : TheoryData<IEnumerable<int?>, IEnumerable, IEnumerable<int?>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_RemoveRange_Value_Nullable_TestData()
            {
                Add( new int?[] { 8, 3, 44, null },
                     new object?[] { null, 3 },
                     new int?[] { 8, 44 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3 }, -1, 0 ),
                     } );
                Add( new int?[] { 8, 3, 44, null },
                     new object?[] { 3, null },
                     new int?[] { 8, 44 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3 }, -1, 1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ),
                     } );
                Add( new int?[] { 8, 3, 44, 5 },
                     new object?[] { 44, null, 8.0 },
                     new int?[] { 3, 5, 8 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 44 }, -1, 3 ),
                     } );
                Add( new int?[] { 12, 2, null, 13 },
                     new object?[] { 4 },
                     new int?[] { null, 2, 12, 13 },
                     new CollectionChangedEventData[] { } );
                Add( new int?[] { },
                     new object?[] { 5 },
                     new int?[] { },
                     new CollectionChangedEventData[] { } );
                Add( new int?[] { 33 },
                     new object?[] { null },
                     new int?[] { 33 },
                     new CollectionChangedEventData[] { } );
                Add( new int?[] { 98 },
                     new object?[] { 3.0 },
                     new int?[] { 98 },
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_RemoveRange_Value_Nullable_TestData ) )]
        public void ICollectionEx_RemoveRange_Value_Nullable( IEnumerable<int?> initialState, IEnumerable removedItems,
                                                              IEnumerable<int?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int?>( initialState );
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class Replace_Value_NonNullable_TestData
            : TheoryData<IEnumerable<double>, double, double, bool, IEnumerable<double>, IEnumerable<CollectionChangedEventData>>
        {
            public Replace_Value_NonNullable_TestData()
            {
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.2,
                     3.4,
                     true,
                     new double[] { 3.4, 5.2, 8.0, 44.5 }, // Replaced item present, replacement item not present, does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 3.4 }, new object?[] { 3.2 }, 0, 0 ),
                     } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.2,
                     34.0,
                     true,
                     new double[] { 5.2, 8.0, 34.0, 44.5 }, // Replaced item present, replacement item not present, needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3.2 }, -1, -0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 34.0 }, null, 2, -1 ),
                     } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     8.0,
                     8.0,
                     true,
                     new double[] { 3.2, 5.2, 8.0, 44.5 }, // Same item
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 8.0 }, new object?[] { 8.0 }, 2, 2 ),
                     } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.1,
                     34.0,
                     false,
                     new double[] { 3.2, 5.2, 8.0, 44.5 }, // Replaced item not present
                     new CollectionChangedEventData[] { } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.2,
                     8.0,
                     true,
                     new double[] { 5.2, 8.0, 44.5 }, // Replaced item present, replacement item present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3.2 }, -1, 0 ),
                     } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.2,
                     3.2,
                     true,
                     new double[] { 3.2, 5.2, 8.0, 44.5 }, // Same item
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 3.2 }, new object?[] { 3.2 }, 0, 0 ),
                     } );
            }
        }

        [Theory]
        [ClassData( typeof( Replace_Value_NonNullable_TestData ) )]
        public void Replace_Value_NonNullable( IEnumerable<double> initialState, double oldItem, double newItem, bool expectedResult,
                                               IEnumerable<double> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableSet<double> observableSet = new ObservableSortedSet<double>( initialState );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableSet.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class Replace_Value_Nullable_TestData
            : TheoryData<IEnumerable<double?>, double?, double?, bool, IEnumerable<double?>, IEnumerable<CollectionChangedEventData>>
        {
            public Replace_Value_Nullable_TestData()
            {
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     3.2,
                     3.4,
                     true,
                     new double?[] { null, 3.4, 5.2, 8.0, 44.5 }, // Replaced item present, replacement item not present, does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 3.4 }, new object?[] { 3.2 }, 1, 1 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     3.2,
                     34.0,
                     true,
                     new double?[] { null, 5.2, 8.0, 34.0, 44.5 }, // Replaced item present, replacement item not present, needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3.2 }, -1, 1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 34.0 }, null, 3, -1 ),
                     } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     5.2,
                     null,
                     true,
                     new double?[] { null, 3.2, 8.0, 44.5 }, // Replaced item present, replacement item not present, needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5.2 }, -1, 1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     null,
                     98.7,
                     true,
                     new double?[] { 3.2, 5.2, 8.0, 44.5, 98.7 }, // Replaced item present, replacement item not present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 98.7 }, null, 4, -1 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     44.5,
                     44.5,
                     true,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 }, // Same non-null item
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 44.5 }, new object?[] { 44.5 }, 4, 4 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     null,
                     null,
                     true,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 }, // Same null item
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { null }, new object?[] { null }, 0, 0 ),
                     } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     3.2,
                     8.0,
                     true,
                     new double?[] { 5.2, 8.0, 44.5 }, // Replaced item present, replacement item present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3.2 }, -1, 0 ),
                     } );
                Add( new double?[] { null, 8.0, 3.2, 44.5, 5.2 },
                     3.2,
                     null,
                     true,
                     new double?[] { null, 5.2, 8.0, 44.5 }, // Replaced item present, replacement item present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3.2 }, -1, 1 ),
                     } );
                Add( new double?[] { null, 8.0, 3.2, 44.5, 5.2 },
                     null,
                     3.2,
                     true,
                     new double?[] { 3.2, 5.2, 8.0, 44.5 }, // Replaced item present, replacement item present
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     3.1,
                     34.0,
                     false,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 }, // Replaced item not present
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     3.1,
                     null,
                     false,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 }, // Replaced item not present
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     null,
                     2.3,
                     false,
                     new double?[] { 3.2, 5.2, 8.0, 44.5 }, // Replaced item not present
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( Replace_Value_Nullable_TestData ) )]
        public void Replace_Value_Nullable( IEnumerable<double?> initialState, double? oldItem, double? newItem, bool expectedResult,
                                            IEnumerable<double?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableSet<double?> observableSet = new ObservableSortedSet<double?>( initialState );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableSet.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        public class ICollectionEx_Replace_Value_NonNullable_TestData
            : TheoryData<IEnumerable<double>, object?, object?, bool, IEnumerable<double>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_Replace_Value_NonNullable_TestData()
            {
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.2,
                     3.4,
                     true,
                     new double[] { 3.4, 5.2, 8.0, 44.5 }, // Replaced item present, replacement item not present, does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 3.4 }, new object?[] { 3.2 }, 0, 0 ),
                     } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.2,
                     34.0,
                     true,
                     new double[] { 5.2, 8.0, 34.0, 44.5 }, // Replaced item present, replacement item not present, needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3.2 }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 34.0 }, null, 2, -1 ),
                     } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.2,
                     44.5,
                     true,
                     new double[] { 5.2, 8.0, 44.5 }, // Replaced item present, replacement item present
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3.2 }, -1, 0 ) } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     5.2,
                     5.2,
                     true,
                     new double[] { 3.2, 5.2, 8.0, 44.5 }, // Same item
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 5.2 }, new object?[] { 5.2 }, 1, 1 ) } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.1,
                     34.0,
                     false,
                     new double[] { 3.2, 5.2, 8.0, 44.5 }, // Replaced item not present (valid)
                     new CollectionChangedEventData[] { } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     null,
                     34.0,
                     false,
                     new double[] { 3.2, 5.2, 8.0, 44.5 },
                     new CollectionChangedEventData[] { } ); // Replaced item not present (null)
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     "3",
                     34.0,
                     false,
                     new double[] { 3.2, 5.2, 8.0, 44.5 }, // Replaced item not present (invalid type)
                     new CollectionChangedEventData[] { } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     null,
                     null,
                     false,
                     new double[] { 3.2, 5.2, 8.0, 44.5 }, // Replaced item not present (null), replacement item invalid (null)
                     new CollectionChangedEventData[] { } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     8.1,
                     "foo",
                     false,
                     new double[] { 3.2, 5.2, 8.0, 44.5 }, // Replaced item not present (valid), replacement item invalid (type)
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Replace_Value_NonNullable_TestData ) )]
        public void ICollectionEx_Replace_Value_NonNullable( IEnumerable<double> initialState, object? oldItem, object? newItem, bool expectedResult,
                                                             IEnumerable<double> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<double>( initialState );
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void ICollectionEx_Replace_Value_NonNullable_NullValue()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<double>() { 8.0, 3.2, 44.5, 5.2 };
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Replace( 3.2, null ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new double[] { 3.2, 5.2, 8.0, 44.5 }, observableSet );
            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Replace_Value_NonNullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<double>() { 8.0, 3.2, 44.5, 5.2 };
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Replace( 3.2, "foo" ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new double[] { 3.2, 5.2, 8.0, 44.5 }, observableSet );
            Assert.Empty( events );
        }

        public class ICollectionEx_Replace_Value_Nullable_TestData
            : TheoryData<IEnumerable<double?>, object?, object?, bool, IEnumerable<double?>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_Replace_Value_Nullable_TestData()
            {
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     3.2,
                     3.4,
                     true,
                     new double?[] { null, 3.4, 5.2, 8.0, 44.5 }, // Replaced item present (non-null), replacement item not present (non-null), does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 3.4 }, new object?[] { 3.2 }, 1, 1 ),
                     } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     3.2,
                     34.0,
                     true,
                     new double?[] { null, 5.2, 8.0, 34.0, 44.5 }, // Replaced item present (non-null), replacement item not present (non-null), needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3.2 }, -1, 1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 34.0 }, null, 3, -1 ),
                     } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     null,
                     2.0,
                     true,
                     new double?[] { 2.0, 3.2, 5.2, 8.0, 44.5 }, // Replaced item present (null), replacement item not present (non-null), does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 2.0 }, new object?[] { null }, 0, 0 ),
                     } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     null,
                     34.0,
                     true,
                     new double?[] { 3.2, 5.2, 8.0, 34.0, 44.5 }, // Replaced item present (null), replacement item not present (non-null), needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 34.0 }, null, 3, -1 ),
                     } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     44.5,
                     null,
                     true,
                     new double?[] { null, 3.2, 5.2, 8.0 }, // Replaced item present (non-null), replacement item not present (null), needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 44.5 }, -1, 3 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ),
                     } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     null,
                     8.0,
                     true,
                     new double?[] { 3.2, 5.2, 8.0, 44.5 }, // Replaced item present (null), replacement item present (non-null)
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ),
                     } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     44.5,
                     null,
                     true,
                     new double?[] { null, 3.2, 5.2, 8.0 }, // Replaced item present (non-null), replacement item present (null)
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 44.5 }, -1, 4 ) } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     3.2,
                     3.2,
                     true,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 }, // Same item (non-null)
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 3.2 }, new object?[] { 3.2 }, 1, 1 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     null,
                     null,
                     true,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 }, // Same item (null)
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { null }, new object?[] { null }, 0, 0 ),
                     } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     3.1,
                     34.0,
                     false,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 }, // Replaced item not present (valid)
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     "3",
                     34.0,
                     false,
                     new double?[] { 3.2, 5.2, 8.0, 44.5 }, // Replaced item not present (invalid type)
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     null,
                     null,
                     false,
                     new double?[] { 3.2, 5.2, 8.0, 44.5 }, // Replaced item not present (null)
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     8.1,
                     "foo",
                     false,
                     new double?[] { 3.2, 5.2, 8.0, 44.5 }, // Replaced item not present (valid), replacement item invalid (type)
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Replace_Value_Nullable_TestData ) )]
        public void ICollectionEx_Replace_Value_Nullable( IEnumerable<double?> initialState, object? oldItem, object? newItem, bool expectedResult,
                                                          IEnumerable<double?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<double?>( initialState );
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, (IEnumerable) observableSet );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void ICollectionEx_Replace_Value_Nullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<double?>() { 8.0, null, 3.2, 44.5, 5.2 };
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Replace( 3.2, "foo" ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new double?[] { null, 3.2, 5.2, 8.0, 44.5 }, observableSet );
            Assert.Empty( events );
        }

        public class Contains_Class_NonNullable_TestData : TheoryData<IEnumerable<TestClass>, TestClass, bool>
        {
            public Contains_Class_NonNullable_TestData()
            {
                Add( new TestClass[] { ITEM1, ITEM2 }, ITEM1, true );
                Add( new TestClass[] { ITEM1, ITEM2 }, ITEM2, true );
                Add( new TestClass[] { ITEM1, ITEM3 }, ITEM2, false );
            }
        }

        [Theory]
        [ClassData( typeof( Contains_Class_NonNullable_TestData ) )]
        public void Contains_Class_NonNullable( IEnumerable<TestClass> initialState, TestClass searchedItem, bool expectedResult )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( initialState );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableSet.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        public class Contains_Class_Nullable_TestData : TheoryData<IEnumerable<TestClass?>, TestClass?, bool>
        {
            public Contains_Class_Nullable_TestData()
            {
                Add( new TestClass?[] { ITEM1, ITEM2 }, ITEM1, true );
                Add( new TestClass?[] { ITEM1, ITEM2 }, ITEM2, true );
                Add( new TestClass?[] { ITEM1, ITEM2, null }, ITEM1, true );
                Add( new TestClass?[] { ITEM1, ITEM2, null }, null, true );
                Add( new TestClass?[] { ITEM1, ITEM3 }, ITEM2, false );
                Add( new TestClass?[] { ITEM1, ITEM3 }, null, false );
            }
        }

        [Theory]
        [ClassData( typeof( Contains_Class_Nullable_TestData ) )]
        public void Contains_Class_Nullable( IEnumerable<TestClass> initialState, TestClass? searchedItem, bool expectedResult )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableSet<TestClass?> observableSet = new ObservableSortedSet<TestClass?>( initialState );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableSet.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        public class ICollectionEx_Contains_Value_NonNullable_TestData : TheoryData<IEnumerable<int>, object?, bool>
        {
            public ICollectionEx_Contains_Value_NonNullable_TestData()
            {
                Add( new int[] { 8, 9 }, 8, true );
                Add( new int[] { 8, 9 }, 9, true );
                Add( new int[] { 8, 9 }, 3, false );
                Add( new int[] { 8, 9 }, null, false );
                Add( new int[] { 8, 9 }, 8.0, false );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Contains_Value_NonNullable_TestData ) )]
        public void ICollectionEx_Contains_Value_NonNullable( IEnumerable<int> initialState, object? searchedItem, bool expectedResult )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int>( initialState );
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act & Assert results

            var result = testedCollection.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        public class ICollectionEx_Contains_Value_Nullable_TestData : TheoryData<IEnumerable<int?>, object?, bool>
        {
            public ICollectionEx_Contains_Value_Nullable_TestData()
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
        [ClassData( typeof( ICollectionEx_Contains_Value_Nullable_TestData ) )]
        public void ICollectionEx_Contains_Value_Nullable( IEnumerable<int?> initialState, object? searchedItem, bool expectedResult )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int?>( initialState );
            var testedCollection = (ICollectionEx) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act & Assert results

            var result = testedCollection.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Contains_Value_NonNullable_TestData ) )]
        public void IReadOnlyCollectionEx_Contains_Value_NonNullable( IEnumerable<int> initialState, object? searchedItem, bool expectedResult )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int>( initialState );
            var testedCollection = (IReadOnlyCollectionEx<int>) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act & Assert results

            var result = testedCollection.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Contains_Value_Nullable_TestData ) )]
        public void IReadOnlyCollectionEx_Contains_Value_Nullable( IEnumerable<int?> initialState, object? searchedItem, bool expectedResult )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableSet = new ObservableSortedSet<int?>( initialState );
            var testedCollection = (IReadOnlyCollectionEx<int?>) observableSet;

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act & Assert results

            var result = testedCollection.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Fact]
        public void GetEnumerator()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( new[] { ITEM1, ITEM2 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act & Assert

            var enumerator = observableSet.GetEnumerator();

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( ITEM1, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( ITEM2, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IEnumerable_GetEnumerator()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableSet = new ObservableSortedSet<TestClass>( new[] { ITEM1, ITEM2 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act & Assert

            var enumerator = ( (IEnumerable) observableSet ).GetEnumerator();

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( ITEM1, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( ITEM2, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void CopyTo()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( new[] { ITEM1, ITEM2, ITEM3 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            var array = new TestClass[ 3 ];

            // Act

            observableSet.CopyTo( array, 0 );

            // Assert result

            Assert.Equal( new[] { ITEM1, ITEM2, ITEM3 }, array );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void ICollection_CopyTo()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableSet = new ObservableSortedSet<TestClass>( new[] { ITEM1, ITEM2, ITEM3 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            var array = new TestClass[ 3 ];

            // Act

            ( (ICollection) observableSet ).CopyTo( array, 0 );

            // Assert result

            Assert.Equal( new[] { ITEM1, ITEM2, ITEM3 }, array );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void UnionWith()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<int> observableSet = new ObservableSortedSet<int>( new[] { 8, 2, 23 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

            // Act

            observableSet.UnionWith( new[] { 1, 23, 9 } );

            // Assert state & results

            Assert.Equal( new[] { 1, 2, 8, 9, 23 }, observableSet );

            // Assert events

            Assert.Equal( 2, events.Count );

            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { 1 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 1 ].Action );
            Assert.Equal( new[] { 9 }, events[ 1 ].NewItems );
            Assert.Null( events[ 1 ].OldItems );
            Assert.Equal( 3, events[ 1 ].NewStartingIndex );
            Assert.Equal( -1, events[ 1 ].OldStartingIndex );
        }

        [Fact]
        public void IntersectWith()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<int> observableSet = new ObservableSortedSet<int>( new[] { 8, 2, 23 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

            // Act

            observableSet.IntersectWith( new[] { 1, 23, 9, 8 } );

            // Assert state & results

            Assert.Equal( new[] { 8, 23 }, observableSet );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { 2 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ExceptWith()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<int> observableSet = new ObservableSortedSet<int>( new[] { 8, 2, 23 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

            // Act

            observableSet.ExceptWith( new[] { 1, 23, 9 } );

            // Assert state & results

            Assert.Equal( new[] { 2, 8 }, observableSet );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { 23 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( 2, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void SymmetricExceptWith()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<int> observableSet = new ObservableSortedSet<int>( new[] { 8, 2, 23 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

            // Act

            observableSet.SymmetricExceptWith( new[] { 1, 23, 9 } );

            // Assert state & results

            Assert.Equal( new[] { 1, 2, 8, 9 }, observableSet );

            // Assert events

            Assert.Equal( 3, events.Count );

            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { 23 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( 2, events[ 0 ].OldStartingIndex );

            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 1 ].Action );
            Assert.Equal( new[] { 1 }, events[ 1 ].NewItems );
            Assert.Null( events[ 1 ].OldItems );
            Assert.Equal( 0, events[ 1 ].NewStartingIndex );
            Assert.Equal( -1, events[ 1 ].OldStartingIndex );

            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 2 ].Action );
            Assert.Equal( new[] { 9 }, events[ 2 ].NewItems );
            Assert.Null( events[ 2 ].OldItems );
            Assert.Equal( 3, events[ 2 ].NewStartingIndex );
            Assert.Equal( -1, events[ 2 ].OldStartingIndex );
        }

        [Fact]
        public void SetComparisons()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<int> observableSet = new ObservableSortedSet<int>( new[] { 8, 2, 23 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

            // Act & Assert

            Assert.True( observableSet.IsSubsetOf( new[] { 23, 2, 8, 55, 12 } ) );
            Assert.False( observableSet.IsSubsetOf( new[] { 22, 2, 8, 55, 12 } ) );

            Assert.True( observableSet.IsProperSubsetOf( new[] { 23, 2, 8, 55, 12 } ) );
            Assert.False( observableSet.IsProperSubsetOf( new[] { 22, 2, 8, 12 } ) );
            Assert.False( observableSet.IsProperSubsetOf( new[] { 22, 2, 8 } ) );

            Assert.True( observableSet.IsSupersetOf( new[] { 8, 23 } ) );
            Assert.False( observableSet.IsSupersetOf( new[] { 8, 23, 1 } ) );

            Assert.True( observableSet.IsProperSupersetOf( new[] { 8, 23 } ) );
            Assert.False( observableSet.IsProperSupersetOf( new[] { 8, 23, 1 } ) );
            Assert.False( observableSet.IsProperSupersetOf( new[] { 8, 23, 2 } ) );

            Assert.True( observableSet.Overlaps( new[] { 12, 2, 55 } ) );
            Assert.False( observableSet.Overlaps( new[] { 12, 1, 55 } ) );

            Assert.True( observableSet.SetEquals( new[] { 8, 23, 2 } ) );
            Assert.False( observableSet.SetEquals( new[] { 8, 23, 1 } ) );

            // Assert unmodified state

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlySetEx_SetComparisons()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<int> observableSet = new ObservableSortedSet<int>( new[] { 8, 2, 23 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            var readonlySet = (IReadOnlySetEx<int>) observableSet;

            Assert.Equal( new[] { 2, 8, 23 }, readonlySet );

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

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void NoEventListeners()
        {
            // Arrange

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( new[] { ITEM1 } );

            Assert.Equal( new[] { ITEM1 }, observableSet );

            // Act

            observableSet.Add( ITEM2 );

            // Assert

            Assert.Equal( new[] { ITEM1, ITEM2 }, observableSet );

            // Act

            observableSet.Remove( ITEM2 );

            // Assert

            Assert.Equal( new[] { ITEM1 }, observableSet );
        }
    }
}
