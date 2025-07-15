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
    public class ObservableCollectionTest
    {
        private static readonly TestClass ITEM1 = new( "Item1", 10 );
        private static readonly TestClass ITEM2 = new( "Item2", 5 );
        private static readonly TestClass ITEM3 = new( "Item3", 20 );

        [Fact]
        public void Constructor_Default()
        {
            // Act

            var observableCollection = new ObservableCollection<TestClass>();

            // Assert

            Assert.Equal( 0, observableCollection.Count );
            Assert.Equal( new TestClass[] { }, observableCollection );

            Assert.False( ( (ICollection<TestClass>) observableCollection ).IsReadOnly );

            Assert.NotNull( ( (ICollection) observableCollection ).SyncRoot );
            Assert.False( ( (ICollection) observableCollection ).IsSynchronized );

            // The following tests are to ensure interface disambiguation.

            Assert.Equal( 0, ( (IObservableCollection<TestClass>) observableCollection ).Count );
            Assert.Equal( 0, ( (ICollectionEx<TestClass>) observableCollection ).Count );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            // Act

            var observableCollection = new ObservableCollection<TestClass>( new[] { ITEM1, ITEM2, ITEM3 } );

            // Assert

            Assert.Equal( 3, observableCollection.Count );
            Assert.Equal( new[] { ITEM1, ITEM2, ITEM3 }, observableCollection );

            Assert.False( ( (IObservableCollection<TestClass>) observableCollection ).IsReadOnly );

            Assert.NotNull( ( (ICollection) observableCollection ).SyncRoot );
            Assert.False( ( (ICollection) observableCollection ).IsSynchronized );
        }

        [Fact]
        public void Dispose()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableCollection = new ObservableCollection<int>( new[] { 1, 2, 3 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            observableCollection.Dispose();

            // Assert

            Assert.Equal( new int[] { }, observableCollection );
            Assert.Equal( 0, observableCollection.Count );

            Assert.Empty( events );
        }

        public class Add_Class_NonNullable_TestData
            : TheoryData<IEnumerable<TestClass>, TestClass, IEnumerable<TestClass>, IEnumerable<CollectionChangedEventData>>
        {
            public Add_Class_NonNullable_TestData()
            {
                Add( new[] { ITEM1 },
                     ITEM2,
                     new[] { ITEM1, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ) } );
                Add( new[] { ITEM1, ITEM2 },
                     ITEM3,
                     new[] { ITEM1, ITEM2, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 2, -1 ) } );
                Add( new[] { ITEM1, ITEM2 },
                     ITEM1,
                     new[] { ITEM1, ITEM2, ITEM1 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM1 }, null, 2, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( Add_Class_NonNullable_TestData ) )]
        public void Add_Class_NonNullable( IEnumerable<TestClass> initialState, TestClass addedItem, IEnumerable<TestClass> expectedState,
                                           IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        public class Add_Class_Nullable_TestData
            : TheoryData<IEnumerable<TestClass?>, TestClass?, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public Add_Class_Nullable_TestData()
            {
                Add( new[] { ITEM1 },
                     ITEM2,
                     new[] { ITEM1, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ) } );
                Add( new[] { ITEM1, ITEM2 },
                     ITEM3,
                     new[] { ITEM1, ITEM2, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 2, -1 ) } );
                Add( new[] { ITEM1, ITEM2 },
                     null,
                     new[] { ITEM1, ITEM2, null },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 2, -1 ) } );
                Add( new[] { ITEM1, null },
                     ITEM2,
                     new[] { ITEM1, null, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 2, -1 ) } );
                Add( new[] { ITEM1, null, ITEM3 },
                     null,
                     new[] { ITEM1, null, ITEM3, null },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 3, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( Add_Class_Nullable_TestData ) )]
        public void Add_Class_Nullable( IEnumerable<TestClass?> initialState, TestClass? addedItem, IEnumerable<TestClass?> expectedState,
                                        IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass?> observableCollection = new ObservableCollection<TestClass?>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        public class ICollectionEx_Add_Value_NonNullable_TestData
            : TheoryData<IEnumerable<int>, int, IEnumerable<int>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_Add_Value_NonNullable_TestData()
            {
                Add( new int[] { },
                     5,
                     new int[] { 5 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 5 }, null, 0, -1 ) } );
                Add( new[] { 12, 2, 13 },
                     4,
                     new int[] { 12, 2, 13, 4 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 4 }, null, 3, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Add_Value_NonNullable_TestData ) )]
        public void ICollectionEx_Add_Value_NonNullable( IEnumerable<int> initialState, int addedItem, IEnumerable<int> expectedState,
                                                         IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void ICollectionEx_Add_Value_NonNullable_NullValue()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int>() { 33 };
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Add( null ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( new int[] { 33 }, observableCollection );
            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Add_Value_NonNullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int>() { 33 };
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Add( 3.0 ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( new int[] { 33 }, observableCollection );
            Assert.Empty( events );
        }

        public class ICollectionEx_Add_Value_Nullable_TestData
            : TheoryData<IEnumerable<int?>, int?, IEnumerable<int?>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_Add_Value_Nullable_TestData()
            {
                Add( new int?[] { },
                     5,
                     new int?[] { 5 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 5 }, null, 0, -1 ) } );
                Add( new int?[] { 12, 2, null, 13 },
                     4,
                     new int?[] { 12, 2, null, 13, 4 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 4 }, null, 4, -1 ) } );
                Add( new int?[] { 12, 2, null, 13 },
                     null,
                     new int?[] { 12, 2, null, 13, null },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 4, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Add_Value_Nullable_TestData ) )]
        public void ICollectionEx_Add_Value_Nullable( IEnumerable<int?> initialState, int? addedItem, IEnumerable<int?> expectedState,
                                                      IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int?>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void ICollectionEx_Add_Value_Nullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int?>() { 33, null };
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Add( 3.0 ) );

            // Assert

            Assert.Equal( "item", exception.ParamName );
            Assert.Equal( new int?[] { 33, null }, observableCollection );
            Assert.Empty( events );
        }

        public class AddRange_Class_NonNullable_TestData
            : TheoryData<IEnumerable<TestClass>, IEnumerable<TestClass>, IEnumerable<TestClass>, IEnumerable<CollectionChangedEventData>>
        {
            public AddRange_Class_NonNullable_TestData()
            {
                Add( new[] { ITEM1 },
                     new[] { ITEM3, ITEM2 },
                     new[] { ITEM1, ITEM3, ITEM2 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3, ITEM2 }, null, 1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 1, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 2, -1 )
                     } );
#endif
                Add( new[] { ITEM3, ITEM2 },
                     new[] { ITEM1 },
                     new[] { ITEM3, ITEM2, ITEM1 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM1 }, null, 2, -1 ) } );
                Add( new[] { ITEM3, ITEM2 },
                     new TestClass[] { },
                     new[] { ITEM3, ITEM2 },
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( AddRange_Class_NonNullable_TestData ) )]
        public void AddRange_Class_NonNullable( IEnumerable<TestClass> initialState, IEnumerable<TestClass> addedItems, IEnumerable<TestClass> expectedState,
                                                IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        public class AddRange_Class_Nullable_TestData
            : TheoryData<IEnumerable<TestClass?>, IEnumerable<TestClass?>, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public AddRange_Class_Nullable_TestData()
            {
                Add( new[] { ITEM1 },
                     new[] { ITEM3, ITEM2 },
                     new[] { ITEM1, ITEM3, ITEM2 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object[] { ITEM3, ITEM2 }, null, 1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 1, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 2, -1 )
                     } );
#endif
                Add( new[] { ITEM3, null, ITEM2 },
                     new[] { ITEM1 },
                     new[] { ITEM3, null, ITEM2, ITEM1 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM1 }, null, 3, -1 ) } );
                Add( new[] { ITEM3, ITEM1 },
                     new[] { null, ITEM1, ITEM2 },
                     new[] { ITEM3, ITEM1, null, ITEM1, ITEM2 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null, ITEM1, ITEM2 }, null, 2, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 2, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM1 }, null, 3, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 4, -1 )
                     } );
#endif
                Add( new[] { ITEM3, ITEM2 },
                     new TestClass[] { },
                     new[] { ITEM3, ITEM2 },
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

            IObservableCollection<TestClass?> observableCollection = new ObservableCollection<TestClass?>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        public class ICollectionEx_AddRange_Value_NonNullable_TestData
            : TheoryData<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_AddRange_Value_NonNullable_TestData()
            {
                Add( new int[] { 5 },
                     new int[] { 9, 56 },
                     new int[] { 5, 9, 56 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 9, 56 }, null, 1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 9 }, null, 1, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 56 }, null, 2, -1 )
                     } );
#endif
                Add( new int[] { 7, 45 },
                     new int[] { 3 },
                     new int[] { 7, 45, 3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 3 }, null, 2, -1 ) } );
                Add( new int[] { 5, 76 },
                     new int[] { },
                     new int[] { 5, 76 },
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_AddRange_Value_NonNullable_TestData ) )]
        public void ICollectionEx_AddRange_Value_NonNullable( IEnumerable<int> initialState, IEnumerable<int> addedItems,
                                                              IEnumerable<int> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void ICollectionEx_AddRange_Value_NonNullable_NullValue()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int>() { 33 };
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            Assert.Throws<NullReferenceException>( () => testedCollection.AddRange( new object?[] { 2, null, 5 } ) );

            // Assert

            Assert.Equal( new int[] { 33 }, observableCollection );
            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_AddRange_Value_NonNullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int>() { 33 };
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            Assert.Throws<InvalidCastException>( () => testedCollection.AddRange( new object?[] { 2, 4.0, 5 } ) );

            // Assert

            Assert.Equal( new int[] { 33 }, observableCollection );
            Assert.Empty( events );
        }

        public class ICollectionEx_AddRange_Value_Nullable_TestData
            : TheoryData<IEnumerable<int?>, IEnumerable<int?>, IEnumerable<int?>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_AddRange_Value_Nullable_TestData()
            {
                Add( new int?[] { 5 },
                     new int?[] { 9, 56 },
                     new int?[] { 5, 9, 56 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 9, 56 }, null, 1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 9 }, null, 1, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 56 }, null, 2, -1 )
                     } );
#endif
                Add( new int?[] { 7, null, 45 },
                     new int?[] { 3 },
                     new int?[] { 7, null, 45, 3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 3 }, null, 3, -1 ) } );
                Add( new int?[] { 5, null, 76 },
                     new int?[] { },
                     new int?[] { 5, null, 76 },
                     new CollectionChangedEventData[] { } );
                Add( new int?[] { 5, null, 76 },
                     new int?[] { null },
                     new int?[] { 5, null, 76, null },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 3, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_AddRange_Value_Nullable_TestData ) )]
        public void ICollectionEx_AddRange_Value_Nullable( IEnumerable<int?> initialState, IEnumerable<int?> addedItems,
                                                           IEnumerable<int?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int?>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void ICollectionEx_AddRange_Value_Nullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int?>() { null, 33 };
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            Assert.Throws<InvalidCastException>( () => testedCollection.AddRange( new object?[] { 2, 4.0, 5 } ) );

            // Assert

            Assert.Equal( new int?[] { null, 33 }, observableCollection );
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

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableCollection );
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
                     new TestClass?[] { ITEM1, null, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 2 ) } );
                Add( new TestClass?[] { ITEM1, null, ITEM2, ITEM3 },
                     null,
                     true,
                     new TestClass?[] { ITEM1, ITEM2, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 1 ) } );
                Add( new TestClass?[] { ITEM1, ITEM2 },
                     ITEM3,
                     false,
                     new TestClass?[] { ITEM1, ITEM2 },
                     new CollectionChangedEventData[] { } );
                Add( new TestClass?[] { ITEM1, ITEM2, null },
                     ITEM3,
                     false,
                     new TestClass?[] { ITEM1, ITEM2, null },
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

            IObservableCollection<TestClass?> observableCollection = new ObservableCollection<TestClass?>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableCollection );
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
                     new int[] { 8, 3, 44 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5 }, -1, 3 ) } );
                Add( new int[] { 12, 2, 13 },
                     4,
                     false,
                     new int[] { 12, 2, 13 },
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

            var observableCollection = new ObservableCollection<int>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableCollection );
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
                     new int?[] { 8, null, 44 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5 }, -1, 3 ) } );
                Add( new int?[] { 8, null, 44, 5 },
                     null,
                     true,
                     new int?[] { 8, 44, 5 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 1 ) } );
                Add( new int?[] { 12, 2, 13 },
                     4,
                     false,
                     new int?[] { 12, 2, 13 },
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

            var observableCollection = new ObservableCollection<int?>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableCollection );
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
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM3, ITEM2 }, -1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM3 }, -1, 2 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 1 )
                     } );
#endif
                Add( new[] { ITEM1, ITEM2, ITEM3 },
                     new[] { ITEM2, ITEM3 },
                     new[] { ITEM1 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2, ITEM3 }, -1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM3 }, -1, 1 )
                     } );
#endif
                Add( new[] { ITEM3, ITEM2 },
                     new TestClass[] { },
                     new[] { ITEM3, ITEM2 },
                     new CollectionChangedEventData[] { } );
                Add( new[] { ITEM3, ITEM2 },
                     new[] { ITEM1 },
                     new[] { ITEM3, ITEM2 },
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

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        public class RemoveRange_Class_Nullable_TestData
            : TheoryData<IEnumerable<TestClass?>, IEnumerable<TestClass?>, IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public RemoveRange_Class_Nullable_TestData()
            {
                Add( new[] { ITEM1, ITEM2, null, ITEM3 },
                     new[] { ITEM3, ITEM2 },
                     new[] { ITEM1, null },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM3, ITEM2 }, -1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM3 }, -1, 3 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 1 )
                     } );
#endif
                Add( new[] { ITEM1, ITEM2, null, ITEM3 },
                     new[] { ITEM2, ITEM3, null },
                     new[] { ITEM1 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2, ITEM3, null }, -1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM3 }, -1, 2 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 1 )
                     } );
#endif
                Add( new[] { null, ITEM3, ITEM2 },
                     new TestClass[] { },
                     new[] { null, ITEM3, ITEM2 },
                     new CollectionChangedEventData[] { } );
                Add( new[] { ITEM3, ITEM2, null },
                     new[] { ITEM1 },
                     new[] { ITEM3, ITEM2, null },
                     new CollectionChangedEventData[] { } );
                Add( new[] { ITEM3, ITEM2 },
                     new[] { null, ITEM2 },
                     new[] { ITEM3 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 1 )
                     } );
#endif
            }
        }

        [Theory]
        [ClassData( typeof( RemoveRange_Class_Nullable_TestData ) )]
        public void RemoveRange_Class_Nullable( IEnumerable<TestClass?> initialState, IEnumerable<TestClass?> removedItems,
                                                IEnumerable<TestClass?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass?> observableCollection = new ObservableCollection<TestClass?>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, observableCollection );
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
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5, 3 }, -1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5 }, -1, 3 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3 }, -1, 1 )
                     } );
#endif
                Add( new int[] { 8, 3, 44, 5 },
                     new object?[] { 3, 5 },
                     new int[] { 8, 44 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3, 5 }, -1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3 }, -1, 1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5 }, -1, 2 )
                     } );
#endif
                Add( new int[] { 8, 3, 44, 5 },
                     new object?[] { 44, null, 8.0 },
                     new int[] { 8, 3, 5 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 44 }, -1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 44 }, -1, 2 )
                     } );
#endif
                Add( new int[] { 12, 2, 13 },
                     new object?[] { 4 },
                     new int[] { 12, 2, 13 },
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

            var observableCollection = new ObservableCollection<int>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, observableCollection );
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
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null, 3 }, -1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 3 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3 }, -1, 1 )
                     } );
#endif
                Add( new int?[] { 8, 3, 44, null },
                     new object?[] { 3, null },
                     new int?[] { 8, 44 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3, null }, -1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3 }, -1, 1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 2 )
                     } );
#endif
                Add( new int?[] { 8, 3, 44, 5 },
                     new object?[] { 44, null, 8.0 },
                     new int?[] { 8, 3, 5 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 44 }, -1, -1 ) } );
#else
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 44 }, -1, 2 )
                     } );
#endif
                Add( new int?[] { 12, 2, null, 13 },
                     new object?[] { 4 },
                     new int?[] { 12, 2, null, 13 },
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

            var observableCollection = new ObservableCollection<int?>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        public class Replace_Value_NonNullable_TestData
            : TheoryData<IEnumerable<double>, double, double, bool, IEnumerable<double>, IEnumerable<CollectionChangedEventData>>
        {
            public Replace_Value_NonNullable_TestData()
            {
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.2,
                     34.0,
                     true,
                     new double[] { 8.0, 34.0, 44.5, 5.2 }, // Replaced item present and different from replacement item
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 34.0 }, new object?[] { 3.2 }, 1, 1 ) } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     5.2,
                     5.2,
                     true,
                     new double[] { 8.0, 3.2, 44.5, 5.2 }, // Replaced item present and equal to replacement item
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 5.2 }, new object?[] { 5.2 }, 3, 3 ) } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.1,
                     34.0,
                     false,
                     new double[] { 8.0, 3.2, 44.5, 5.2 }, // Replaced item not present
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( Replace_Value_NonNullable_TestData ) )]
        public void Replace_Value_NonNullable( IEnumerable<double> initialState, double oldItem, double newItem, bool expectedResult,
                                               IEnumerable<double> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<double> observableCollection = new ObservableCollection<double>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableCollection.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        public class Replace_Value_Nullable_TestData
            : TheoryData<IEnumerable<double?>, double?, double?, bool, IEnumerable<double?>, IEnumerable<CollectionChangedEventData>>
        {
            public Replace_Value_Nullable_TestData()
            {
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     3.2,
                     34.0,
                     true,
                     new double?[] { 8.0, 34.0, null, 44.5, 5.2 }, // Replaced item present and different from replacement item
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 34.0 }, new object?[] { 3.2 }, 1, 1 ) } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     5.2,
                     null,
                     true,
                     new double?[] { 8.0, 3.2, null, 44.5, null }, // Replaced non-null item present and replaced with null
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { null }, new object?[] { 5.2 }, 4, 4 ) } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     null,
                     98.7,
                     true,
                     new double?[] { 8.0, 3.2, 98.7, 44.5, 5.2 }, // Replaced null item present and replaced with non-null item
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 98.7 }, new object?[] { null }, 2, 2 ) } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     null,
                     null,
                     true,
                     new double?[] { 8.0, 3.2, null, 44.5, 5.2 }, // Replaced null item present and replaced with null
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { null }, new object?[] { null }, 2, 2 ) } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     5.2,
                     5.2,
                     true,
                     new double?[] { 8.0, 3.2, null, 44.5, 5.2 }, // Replaced item present and equal to replacement item
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 5.2 }, new object?[] { 5.2 }, 4, 4 ) } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     3.1,
                     34.0,
                     false,
                     new double?[] { 8.0, 3.2, null, 44.5, 5.2 }, // Replaced non-null item not present, replacement item valid non-null
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     3.1,
                     null,
                     false,
                     new double?[] { 8.0, 3.2, null, 44.5, 5.2 }, // Replaced non-null item not present, replacement item valid null
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     null,
                     2.3,
                     false,
                     new double?[] { 8.0, 3.2, 44.5, 5.2 }, // Replaced null item not present, replacement item valid non-null
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     null,
                     null,
                     false,
                     new double?[] { 8.0, 3.2, 44.5, 5.2 }, // Replaced null item not present, replacement item valid null
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

            IObservableCollection<double?> observableCollection = new ObservableCollection<double?>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableCollection.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        public class ICollectionEx_Replace_Value_NonNullable_TestData
            : TheoryData<IEnumerable<double>, object?, object?, bool, IEnumerable<double>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_Replace_Value_NonNullable_TestData()
            {
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.2,
                     34.0,
                     true,
                     new double[] { 8.0, 34.0, 44.5, 5.2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 34.0 }, new object?[] { 3.2 }, 1, 1 ) } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.1,
                     34.0,
                     false,
                     new double[] { 8.0, 3.2, 44.5, 5.2 },
                     new CollectionChangedEventData[] { } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     null,
                     34.0,
                     false,
                     new double[] { 8.0, 3.2, 44.5, 5.2 },
                     new CollectionChangedEventData[] { } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     "3",
                     34.0,
                     false,
                     new double[] { 8.0, 3.2, 44.5, 5.2 },
                     new CollectionChangedEventData[] { } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     null,
                     null,
                     false,
                     new double[] { 8.0, 3.2, 44.5, 5.2 },
                     new CollectionChangedEventData[] { } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     8.1,
                     "foo",
                     false,
                     new double[] { 8.0, 3.2, 44.5, 5.2 },
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

            var observableCollection = new ObservableCollection<double>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void ICollectionEx_Replace_Value_NonNullable_NullValue()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<double>() { 8.0, 3.2, 44.5, 5.2 };
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentNullException>( () => testedCollection.Replace( 3.2, null ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new double[] { 8.0, 3.2, 44.5, 5.2 }, observableCollection );
            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Replace_Value_NonNullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<double>() { 8.0, 3.2, 44.5, 5.2 };
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Replace( 3.2, "foo" ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new double[] { 8.0, 3.2, 44.5, 5.2 }, observableCollection );
            Assert.Empty( events );
        }

        public class ICollectionEx_Replace_Value_Nullable_TestData
            : TheoryData<IEnumerable<double?>, object?, object?, bool, IEnumerable<double?>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_Replace_Value_Nullable_TestData()
            {
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     3.2,
                     34.0,
                     true,
                     new double?[] { 8.0, null, 34.0, 44.5, 5.2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 34.0 }, new object?[] { 3.2 }, 2, 2 ) } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     null,
                     34.0,
                     true,
                     new double?[] { 8.0, 34.0, 3.2, 44.5, 5.2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 34.0 }, new object?[] { null }, 1, 1 ) } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     44.5,
                     null,
                     true,
                     new double?[] { 8.0, null, 3.2, null, 5.2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { null }, new object?[] { 44.5 }, 3, 3 ) } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     null,
                     null,
                     true,
                     new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { null }, new object?[] { null }, 2, 2 ) } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     3.1,
                     34.0,
                     false,
                     new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     "3",
                     34.0,
                     false,
                     new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     null,
                     null,
                     false,
                     new double?[] { 8.0, 3.2, 44.5, 5.2 },
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

            var observableCollection = new ObservableCollection<double?>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Fact]
        public void ICollectionEx_Replace_Value_Nullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<double?>() { 8.0, null, 3.2, 44.5, 5.2 };
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var exception = Assert.Throws<ArgumentException>( () => testedCollection.Replace( 3.2, "foo" ) );

            // Assert

            Assert.Equal( "newItem", exception.ParamName );
            Assert.Equal( new double?[] { 8.0, null, 3.2, 44.5, 5.2 }, observableCollection );
            Assert.Empty( events );
        }

        public class Clear_Class_NonNullable_TestData : TheoryData<IEnumerable<TestClass>, IEnumerable<CollectionChangedEventData>>
        {
            public Clear_Class_NonNullable_TestData()
            {
                Add( new TestClass[] { ITEM1 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM1 }, -1, 0 ) } );
                Add( new TestClass[] { ITEM1, ITEM2 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM1, ITEM2 }, -1, 0 ) } );
#else
                     new[] {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM1 }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 0 )
                     } );
#endif
                Add( new TestClass[] { },
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( Clear_Class_NonNullable_TestData ) )]
        public void Clear_Class_NonNullable( IEnumerable<TestClass> initialState,
                                             IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableCollection.Clear();

            // Assert

            Assert.Empty( observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        public class Clear_Class_Nullable_TestData : TheoryData<IEnumerable<TestClass?>, IEnumerable<CollectionChangedEventData>>
        {
            public Clear_Class_Nullable_TestData()
            {
                Add( new TestClass?[] { ITEM1 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM1 }, -1, 0 ) } );
                Add( new TestClass?[] { null },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ) } );
                Add( new TestClass?[] { ITEM1, null, ITEM2 },
#if BULK_NOTIFY_RANGE_ACTIONS
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM1, null, ITEM2 }, -1, 0 ) } );
#else
                     new[] {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM1 }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 0 )
                     } );
#endif
                Add( new TestClass?[] { },
                     new CollectionChangedEventData[] { } );
            }
        }

        [Theory]
        [ClassData( typeof( Clear_Class_Nullable_TestData ) )]
        public void Clear_Class_Nullable( IEnumerable<TestClass?> initialState,
                                          IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass?> observableCollection = new ObservableCollection<TestClass?>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableCollection.Clear();

            // Assert

            Assert.Empty( observableCollection );
            Assert.Equal( expectedEvents, events );
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

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableCollection.Contains( searchedItem );

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

            IObservableCollection<TestClass?> observableCollection = new ObservableCollection<TestClass?>( initialState );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableCollection.Contains( searchedItem );

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

            var observableCollection = new ObservableCollection<int>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

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

            var observableCollection = new ObservableCollection<int?>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

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

            var observableCollection = new ObservableCollection<int>( initialState );
            var testedCollection = (IReadOnlyCollectionEx<int>) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

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

            var observableCollection = new ObservableCollection<int?>( initialState );
            var testedCollection = (IReadOnlyCollectionEx<int?>) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Fact]
        public void GetEnumerator()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { ITEM1, ITEM2 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var enumerator = observableCollection.GetEnumerator();

            // Assert

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( ITEM1, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( ITEM2, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            Assert.Empty( events );
        }

        [Fact]
        public void IEnumerable_GetEnumerator()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<TestClass>( new[] { ITEM1, ITEM2 } );
            var testedCollection = (IEnumerable) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var enumerator = testedCollection.GetEnumerator();

            // Assert

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( ITEM1, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( ITEM2, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            Assert.Empty( events );
        }

        [Fact]
        public void CopyTo()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { ITEM1, ITEM2, ITEM3 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            var array = new TestClass[ 3 ];

            // Act

            observableCollection.CopyTo( array, 0 );

            // Assert

            Assert.Equal( new[] { ITEM1, ITEM2, ITEM3 }, array );
            Assert.Empty( events );
        }

        [Fact]
        public void ICollection_CopyTo()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<TestClass>( new[] { ITEM1, ITEM2, ITEM3 } );
            var testedCollection = (ICollection) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            var array = new TestClass[ 3 ];

            // Act

            testedCollection.CopyTo( array, 0 );

            // Assert

            Assert.Equal( new[] { ITEM1, ITEM2, ITEM3 }, array );
            Assert.Empty( events );
        }

        [Fact]
        public void NoEventListeners()
        {
            // Arrange

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { ITEM1, ITEM3 } );

            Assert.Equal( new[] { ITEM1, ITEM3 }, observableCollection );

            // Act

            observableCollection.Add( ITEM2 );

            // Assert

            Assert.Equal( new[] { ITEM1, ITEM3, ITEM2 }, observableCollection );

            // Act

            observableCollection.Remove( ITEM3 );

            // Assert

            Assert.Equal( new[] { ITEM1, ITEM2 }, observableCollection );
        }
    }
}
