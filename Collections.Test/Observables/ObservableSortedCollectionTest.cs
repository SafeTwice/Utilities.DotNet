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
#pragma warning disable xUnit2013 // Do not use equality check to check for observableCollection size.
#pragma warning disable xUnit1045
#pragma warning disable CA1825 // Using zero-length array initializers is intentionally to improve test maintainability.
#pragma warning disable CA1859 // Using interface types is intentional to call the interface methods.
#pragma warning disable CA1861 // Using constant arrays is intentionally to improve test maintainability.
#pragma warning restore IDE0079

#pragma warning disable S101 // TestData types are not strictly PascalCase

namespace Utilities.DotNet.Collections.Observables.Test
{
    public class ObservableSortedCollectionTest
    {
        private static readonly TestClass ITEM1 = new( "Item1", 10 );
        private static readonly TestClass ITEM2 = new( "Item2", 5 );
        private static readonly TestClass ITEM3 = new( "Item3", 20 );

        [Fact]
        public void Constructor_Default()
        {
            // Act

            var observableCollection = new ObservableSortedCollection<int>();

            // Assert

            Assert.Equal( 0, observableCollection.Count );
            Assert.Equal( new int[] { }, observableCollection );
            Assert.Same( Comparer<int>.Default, observableCollection.Comparer );

            Assert.False( ( (ICollection<int>) observableCollection ).IsReadOnly );

            Assert.NotNull( ( (ICollection) observableCollection ).SyncRoot );
            Assert.False( ( (ICollection) observableCollection ).IsSynchronized );

            // The following tests are to ensure interface disambiguation.

            Assert.Equal( 0, ( (IObservableCollection<int>) observableCollection ).Count );
            Assert.Equal( 0, ( (ICollectionEx<int>) observableCollection ).Count );
        }

        [Fact]
        public void Constructor_Comparer()
        {
            // Arrange

#pragma warning disable S2234 // Arguments should be passed in the same order as the method parameters
            var customComparer = Comparer<int>.Create( ( x, y ) => Comparer<int>.Default.Compare( y, x ) );
#pragma warning restore S2234 // Arguments should be passed in the same order as the method parameters

            // Act

            var observableCollection = new ObservableSortedCollection<int>( customComparer );

            // Assert

            Assert.Equal( 0, observableCollection.Count );
            Assert.Equal( new int[] { }, observableCollection );
            Assert.Same( customComparer, observableCollection.Comparer );

            Assert.False( ( (ICollection<int>) observableCollection ).IsReadOnly );

            Assert.NotNull( ( (ICollection) observableCollection ).SyncRoot );
            Assert.False( ( (ICollection) observableCollection ).IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            // Act

            var observableCollection = new ObservableSortedCollection<int>( new[] { 1, 3, 2, 4 } );

            // Assert

            Assert.Equal( 4, observableCollection.Count );
            Assert.Equal( new[] { 1, 2, 3, 4 }, observableCollection );
            Assert.Same( Comparer<int>.Default, observableCollection.Comparer );

            Assert.False( ( (ICollection<int>) observableCollection ).IsReadOnly );

            Assert.NotNull( ( (ICollection) observableCollection ).SyncRoot );
            Assert.False( ( (ICollection) observableCollection ).IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationListAndComparer()
        {
            // Arrange

#pragma warning disable S2234 // Arguments should be passed in the same order as the method parameters
            var customComparer = Comparer<int>.Create( ( x, y ) => Comparer<int>.Default.Compare( y, x ) );
#pragma warning restore S2234 // Arguments should be passed in the same order as the method parameters

            // Act

            var observableCollection = new ObservableSortedCollection<int>( new[] { 5, 3, 4, 1 }, customComparer );

            // Assert

            Assert.Equal( 4, observableCollection.Count );
            Assert.Equal( new[] { 5, 4, 3, 1 }, observableCollection );
            Assert.Same( customComparer, observableCollection.Comparer );

            Assert.False( ( (ICollection<int>) observableCollection ).IsReadOnly );

            Assert.NotNull( ( (ICollection) observableCollection ).SyncRoot );
            Assert.False( ( (ICollection) observableCollection ).IsSynchronized );
        }

        [Fact]
        public void Dispose()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var testedCollection = new ObservableSortedCollection<int>( new[] { 1, 2, 3 } );

            testedCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedCollection, obj );
                events.Add( args );
            };

            // Act

            testedCollection.Dispose();

            // Assert

            Assert.Equal( new int[] { }, testedCollection );
            Assert.Equal( 0, testedCollection.Count );

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
                Add( new[] { ITEM1, ITEM3 },
                     ITEM2,
                     new[] { ITEM1, ITEM2, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ) } );
                Add( new[] { ITEM1, ITEM2 },
                     ITEM1,
                     new[] { ITEM1, ITEM1, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM1 }, null, 0, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( Add_Class_NonNullable_TestData ) )]
        public void Add_Class_NonNullable( IEnumerable<TestClass> initialState, TestClass addedItem, IEnumerable<TestClass> expectedState,
                                           IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( initialState );

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
                Add( new[] { ITEM1, ITEM3 },
                     ITEM2,
                     new[] { ITEM1, ITEM2, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ) } );
                Add( new[] { ITEM1, ITEM2 },
                     null,
                     new[] { null, ITEM1, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
                Add( new[] { ITEM1, null },
                     ITEM2,
                     new[] { null, ITEM1, ITEM2 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 2, -1 ) } );
                Add( new[] { ITEM1, null, ITEM3 },
                     null,
                     new[] { null, null, ITEM1, ITEM3 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( Add_Class_Nullable_TestData ) )]
        public void Add_Class_Nullable( IEnumerable<TestClass?> initialState, TestClass? addedItem, IEnumerable<TestClass?> expectedState,
                                        IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass?> observableCollection = new ObservableSortedCollection<TestClass?>( initialState );

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
                     new int[] { 2, 4, 12, 13 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 4 }, null, 1, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Add_Value_NonNullable_TestData ) )]
        public void ICollectionEx_Add_Value_NonNullable( IEnumerable<int> initialState, int addedItem, IEnumerable<int> expectedState,
                                                         IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableSortedCollection<int>( initialState );
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

            var observableCollection = new ObservableSortedCollection<int>() { 33 };
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

            var observableCollection = new ObservableSortedCollection<int>() { 33 };
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
                     new int?[] { null, 2, 4, 12, 13 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 4 }, null, 2, -1 ) } );
                Add( new int?[] { 12, 2, null, 13 },
                     null,
                     new int?[] { null, null, 2, 12, 13 },
                     new[] { new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ) } );
            }
        }

        [Theory]
        [ClassData( typeof( ICollectionEx_Add_Value_Nullable_TestData ) )]
        public void ICollectionEx_Add_Value_Nullable( IEnumerable<int?> initialState, int? addedItem, IEnumerable<int?> expectedState,
                                                      IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableSortedCollection<int?>( initialState );
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

            var observableCollection = new ObservableSortedCollection<int?>() { 33, null };
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
            Assert.Equal( new int?[] { null, 33 }, observableCollection );
            Assert.Empty( events );
        }

        public class AddRange_Class_NonNullable_TestData
            : TheoryData<IEnumerable<TestClass>, IEnumerable<TestClass>, IEnumerable<TestClass>, IEnumerable<CollectionChangedEventData>>
        {
            public AddRange_Class_NonNullable_TestData()
            {
                Add( new[] { ITEM1 },
                     new[] { ITEM3, ITEM2 },
                     new[] { ITEM1, ITEM2, ITEM3 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 1, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ),
                     } );
                Add( new[] { ITEM3, ITEM2 },
                     new[] { ITEM1 },
                     new[] { ITEM1, ITEM2, ITEM3 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM1 }, null, 0, -1 ),
                     } );
                Add( new[] { ITEM3, ITEM2 },
                     new TestClass[] { },
                     new[] { ITEM2, ITEM3 },
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

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( initialState );

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
                     new[] { ITEM1, ITEM2, ITEM3 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM3 }, null, 1, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 1, -1 ),
                     } );
                Add( new[] { ITEM3, null, ITEM2 },
                     new[] { ITEM1 },
                     new[] { null, ITEM1, ITEM2, ITEM3 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM1 }, null, 1, -1 ),
                     } );
                Add( new[] { ITEM3, ITEM1 },
                     new[] { null, ITEM1, ITEM2 },
                     new[] { null, ITEM1, ITEM1, ITEM2, ITEM3 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM1 }, null, 1, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { ITEM2 }, null, 3, -1 ),
                     } );
                Add( new[] { ITEM3, ITEM2 },
                     new TestClass[] { },
                     new[] { ITEM2, ITEM3 },
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

            IObservableCollection<TestClass?> observableCollection = new ObservableSortedCollection<TestClass?>( initialState );

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
                     new int[] { 56, 9 },
                     new int[] { 5, 9, 56 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 56 }, null, 1, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 9 }, null, 1, -1 ),
                     } );
                Add( new int[] { 7, 45 },
                     new int[] { 3 },
                     new int[] { 3, 7, 45 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 3 }, null, 0, -1 ),
                     } );
                Add( new int[] { 7, 45 },
                     new int[] { 99 },
                     new int[] { 7, 45, 99 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 99 }, null, 2, -1 ),
                     } );
                Add( new int[] { 76, 5 },
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

            var observableCollection = new ObservableSortedCollection<int>( initialState );
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

            var observableCollection = new ObservableSortedCollection<int>() { 33 };
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

            var observableCollection = new ObservableSortedCollection<int>() { 33 };
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
                Add( new int?[] { 9 },
                     new int?[] { 5, 56 },
                     new int?[] { 5, 9, 56 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 5 }, null, 0, -1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 56 }, null, 2, -1 ),
                     } );
                Add( new int?[] { 7, null, 45 },
                     new int?[] { 3 },
                     new int?[] { null, 3, 7, 45 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 3 }, null, 1, -1 ),
                     } );
                Add( new int?[] { 5, null, 76 },
                     new int?[] { },
                     new int?[] { null, 5, 76 },
                     new CollectionChangedEventData[] { } );
                Add( new int?[] { 5, null, 76 },
                     new int?[] { null },
                     new int?[] { null, null, 5, 76 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ),
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

            var observableCollection = new ObservableSortedCollection<int?>( initialState );
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

            var observableCollection = new ObservableSortedCollection<int?>() { null, 33 };
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

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( initialState );

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

            IObservableCollection<TestClass?> observableCollection = new ObservableSortedCollection<TestClass?>( initialState );

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

            var observableCollection = new ObservableSortedCollection<int>( initialState );
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

            var observableCollection = new ObservableSortedCollection<int?>( initialState );
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

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( initialState );

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

            IObservableCollection<TestClass?> observableCollection = new ObservableSortedCollection<TestClass?>( initialState );

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

            var observableCollection = new ObservableSortedCollection<int>( initialState );
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

            var observableCollection = new ObservableSortedCollection<int?>( initialState );
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
                     2.0,
                     true,
                     new double[] { 2.0, 5.2, 8.0, 44.5 }, // Different value, does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 2.0 }, new object?[] { 3.2 }, 0, 0 ),
                     } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.2,
                     34.0,
                     true,
                     new double[] { 5.2, 8.0, 34.0, 44.5 }, // Different value, needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3.2 }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 34.0 }, null, 2, -1 ),
                     } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     5.2,
                     5.2,
                     true,
                     new double[] { 3.2, 5.2, 8.0, 44.5 }, // Same value, does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 5.2 }, new object?[] { 5.2 }, 1, 1 ),
                     } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.1,
                     34.0,
                     false,
                     new double[] { 3.2, 5.2, 8.0, 44.5 },
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

            IObservableCollection<double> observableCollection = new ObservableSortedCollection<double>( initialState );

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
                     3.3,
                     true,
                     new double?[] { null, 3.3, 5.2, 8.0, 44.5 }, // Different value, does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 3.3 }, new object?[] { 3.2 }, 1, 1 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     3.2,
                     34.0,
                     true,
                     new double?[] { null, 5.2, 8.0, 34.0, 44.5 }, // Different value, needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3.2 }, -1, 1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 34.0 }, null, 3, -1 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     5.2,
                     null,
                     true,
                     new double?[] { null, null, 3.2, 8.0, 44.5 }, // Different value, needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 5.2 }, -1, 2 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     null,
                     98.7,
                     true,
                     new double?[] { 3.2, 5.2, 8.0, 44.5, 98.7 }, // Different value, needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 98.7 }, null, 4, -1 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     44.5,
                     44.5,
                     true,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 }, // Same value, does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 44.5 }, new object?[] { 44.5 }, 4, 4 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     null,
                     null,
                     true,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 }, // Same value, does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { null }, new object?[] { null }, 0, 0 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     3.1,
                     34.0,
                     false,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 },
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     3.1,
                     null,
                     false,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 },
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     null,
                     2.3,
                     false,
                     new double?[] { 3.2, 5.2, 8.0, 44.5 },
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

            IObservableCollection<double?> observableCollection = new ObservableSortedCollection<double?>( initialState );

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
                     4.5,
                     true,
                     new double[] { 4.5, 5.2, 8.0, 44.5 }, // Different item, does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 4.5 }, new object?[] { 3.2 }, 0, 0 ),
                     } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.2,
                     34.0,
                     true,
                     new double[] { 5.2, 8.0, 34.0, 44.5 }, // Different item, needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3.2 }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 34.0 }, null, 2, -1 ),
                     } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     5.2,
                     5.2,
                     true,
                     new double[] { 3.2, 5.2, 8.0, 44.5 }, // Same item
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 5.2 }, new object?[] { 5.2 }, 1, 1 ),
                     } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     3.1,
                     34.0,
                     false,
                     new double[] { 3.2, 5.2, 8.0, 44.5 },
                     new CollectionChangedEventData[] { } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     null,
                     34.0,
                     false,
                     new double[] { 3.2, 5.2, 8.0, 44.5 },
                     new CollectionChangedEventData[] { } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     "3",
                     34.0,
                     false,
                     new double[] { 3.2, 5.2, 8.0, 44.5 },
                     new CollectionChangedEventData[] { } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     null,
                     null,
                     false,
                     new double[] { 3.2, 5.2, 8.0, 44.5 },
                     new CollectionChangedEventData[] { } );
                Add( new double[] { 8.0, 3.2, 44.5, 5.2 },
                     8.1,
                     "foo",
                     false,
                     new double[] { 3.2, 5.2, 8.0, 44.5 },
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

            var observableCollection = new ObservableSortedCollection<double>( initialState );
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

            var observableCollection = new ObservableSortedCollection<double>() { 8.0, 3.2, 44.5, 5.2 };
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
            Assert.Equal( new double[] { 3.2, 5.2, 8.0, 44.5 }, observableCollection );
            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Replace_Value_NonNullable_InvalidType()
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableSortedCollection<double>() { 8.0, 3.2, 44.5, 5.2 };
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
            Assert.Equal( new double[] { 3.2, 5.2, 8.0, 44.5 }, observableCollection );
            Assert.Empty( events );
        }

        public class ICollectionEx_Replace_Value_Nullable_TestData
            : TheoryData<IEnumerable<double?>, object?, object?, bool, IEnumerable<double?>, IEnumerable<CollectionChangedEventData>>
        {
            public ICollectionEx_Replace_Value_Nullable_TestData()
            {
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     3.2,
                     4.5,
                     true,
                     new double?[] { null, 4.5, 5.2, 8.0, 44.5 }, // Different item, does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 4.5 }, new object?[] { 3.2 }, 1, 1 ),
                     } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     3.2,
                     34.0,
                     true,
                     new double?[] { null, 5.2, 8.0, 34.0, 44.5 }, // Different item, needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 3.2 }, -1, 1 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 34.0 }, null, 3, -1 ),
                     } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     null,
                     2.0,
                     true,
                     new double?[] { 2.0, 3.2, 5.2, 8.0, 44.5 }, // Different item, does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 2.0 }, new object?[] { null }, 0, 0 ),
                     } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     null,
                     34.0,
                     true,
                     new double?[] { 3.2, 5.2, 8.0, 34.0, 44.5 }, // Different item, needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { 34.0 }, null, 3, -1 ),
                     } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     44.5,
                     null,
                     true,
                     new double?[] { null, null, 3.2, 5.2, 8.0 }, // Different item, needs reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { 44.5 }, -1, 4 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Add, new object?[] { null }, null, 0, -1 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     5.2,
                     5.2,
                     true,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 }, // Same item, does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { 5.2 }, new object?[] { 5.2 }, 2, 2 ),
                     } );
                Add( new double?[] { 8.0, 3.2, null, 44.5, 5.2 },
                     null,
                     null,
                     true,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 }, // Same item, does not need reordering
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Replace, new object?[] { null }, new object?[] { null }, 0, 0 ),
                     } );
                Add( new double?[] { 8.0, null, 3.2, 44.5, 5.2 },
                     3.1,
                     34.0,
                     false,
                     new double?[] { null, 3.2, 5.2, 8.0, 44.5 },
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     "3",
                     34.0,
                     false,
                     new double?[] { 3.2, 5.2, 8.0, 44.5 },
                     new CollectionChangedEventData[] { } );
                Add( new double?[] { 8.0, 3.2, 44.5, 5.2 },
                     null,
                     null,
                     false,
                     new double?[] { 3.2, 5.2, 8.0, 44.5 },
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

            var observableCollection = new ObservableSortedCollection<double?>( initialState );
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

            var observableCollection = new ObservableSortedCollection<double?>() { 8.0, null, 3.2, 44.5, 5.2 };
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
            Assert.Equal( new double?[] { null, 3.2, 5.2, 8.0, 44.5 }, observableCollection );
            Assert.Empty( events );
        }

        public class Clear_Class_NonNullable_TestData : TheoryData<IEnumerable<TestClass>, IEnumerable<CollectionChangedEventData>>
        {
            public Clear_Class_NonNullable_TestData()
            {
                Add( new TestClass[] { ITEM1 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM1 }, -1, 0 ),
                     } );
                Add( new TestClass[] { ITEM2, ITEM1 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM1 }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 0 ),
                     } );
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

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( initialState );

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
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM1 }, -1, 0 ),
                     } );
                Add( new TestClass?[] { null },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ),
                     } );
                Add( new TestClass?[] { ITEM1, null, ITEM2 },
                     new[]
                     {
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { null }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM1 }, -1, 0 ),
                         new CollectionChangedEventData( NotifyCollectionChangedAction.Remove, null, new object?[] { ITEM2 }, -1, 0 ),
                     } );
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

            IObservableCollection<TestClass?> observableCollection = new ObservableSortedCollection<TestClass?>( initialState );

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

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( initialState );

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

            IObservableCollection<TestClass?> observableCollection = new ObservableSortedCollection<TestClass?>( initialState );

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

            var observableCollection = new ObservableSortedCollection<int>( initialState );
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

            var observableCollection = new ObservableSortedCollection<int?>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
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

            var observableCollection = new ObservableSortedCollection<int>( initialState );
            var testedCollection = (IReadOnlyCollectionEx<int>) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
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

            var observableCollection = new ObservableSortedCollection<int?>( initialState );
            var testedCollection = (IReadOnlyCollectionEx<int?>) observableCollection;

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act & Assert results

            var result = testedCollection.Contains( searchedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Empty( events );
        }

        [Fact]
        public void AutoReorder_FirstElement()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var itemA = new TestClass( "ItemA", 10 );
            var itemB = new TestClass( "ItemB", 5 );
            var itemC = new TestClass( "ItemC", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { itemC, itemB, itemA },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { itemB, itemA, itemC }, observableCollection );

            // Act: No reorder necessary

            itemB.Value = 9;

            // Assert state

            Assert.Equal( new[] { itemB, itemA, itemC }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: No reorder necessary

            itemB.Value = 10;

            // Assert state

            Assert.Equal( new[] { itemB, itemA, itemC }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: Reorder necessary

            itemB.Value = 11;

            // Assert state

            Assert.Equal( new[] { itemA, itemB, itemC }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { itemB }, events[ 0 ].NewItems );
            Assert.Equal( new[] { itemB }, events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void AutoReorder_MiddleElement()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var itemA = new TestClass( "ItemA", 10 );
            var itemB = new TestClass( "ItemB", 5 );
            var itemC = new TestClass( "ItemC", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { itemC, itemB, itemA },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { itemB, itemA, itemC }, observableCollection );

            // Act: No reorder necessary

            itemA.Value = 6;

            // Assert state

            Assert.Equal( new[] { itemB, itemA, itemC }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: No reorder necessary

            itemA.Value = 5;

            // Assert state

            Assert.Equal( new[] { itemB, itemA, itemC }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: Reorder necessary

            itemA.Value = 4;

            // Assert state

            Assert.Equal( new[] { itemA, itemB, itemC }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { itemA }, events[ 0 ].NewItems );
            Assert.Equal( new[] { itemA }, events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );

            // Arrange

            events.Clear();

            // Act: No reorder necessary

            itemB.Value = 19;

            // Assert state

            Assert.Equal( new[] { itemA, itemB, itemC }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: No reorder necessary

            itemB.Value = 20;

            // Assert state

            Assert.Equal( new[] { itemA, itemB, itemC }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: Reorder necessary

            itemB.Value = 21;

            // Assert state

            Assert.Equal( new[] { itemA, itemC, itemB }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { itemB }, events[ 0 ].NewItems );
            Assert.Equal( new[] { itemB }, events[ 0 ].OldItems );
            Assert.Equal( 2, events[ 0 ].NewStartingIndex );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void AutoReorder_LastElement()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var itemA = new TestClass( "ItemA", 10 );
            var itemB = new TestClass( "ItemB", 5 );
            var itemC = new TestClass( "ItemC", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { itemC, itemB, itemA },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { itemB, itemA, itemC }, observableCollection );

            // Act: No reorder necessary

            itemC.Value = 11;

            // Assert state

            Assert.Equal( new[] { itemB, itemA, itemC }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: No reorder necessary

            itemC.Value = 10;

            // Assert state

            Assert.Equal( new[] { itemB, itemA, itemC }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: Reorder necessary

            itemC.Value = 9;

            // Assert state

            Assert.Equal( new[] { itemB, itemC, itemA }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { itemC }, events[ 0 ].NewItems );
            Assert.Equal( new[] { itemC }, events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( 2, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void UpdateSortOrder_NotExisting()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableCollection = new ObservableSortedCollection<TestClass>( new[] { ITEM3, ITEM1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { ITEM1, ITEM3 }, observableCollection );

            // Act

            var exception = Assert.Throws<ArgumentException>( () => observableCollection.UpdateSortOrder( ITEM2 ) );

            // Assert events

            Assert.Equal( "item", exception.ParamName );
            Assert.Empty( events );
        }

        [Fact]
        public void Contains()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<int> observableCollection = new ObservableSortedCollection<int>( new[] { 1, 3, 2, 4 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act & Assert

            Assert.True( observableCollection.Contains( 1 ) );
            Assert.False( observableCollection.Contains( 5 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void GetEnumerator()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<int> observableCollection = new ObservableSortedCollection<int>( new[] { 6, 9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var enumerator = observableCollection.GetEnumerator();

            // Assert

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 6, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 9, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IEnumerable_GetEnumerator()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableCollection = new ObservableSortedCollection<int>( new[] { 6, 9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            IEnumerable enumerable = observableCollection;

            // Act

            var enumerator = enumerable.GetEnumerator();

            // Assert

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 6, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 9, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void CopyTo()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<int> observableCollection = new ObservableSortedCollection<int>( new[] { 9, 3, 7, 4 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            var array = new int[ 4 ];

            // Act

            observableCollection.CopyTo( array, 0 );

            // Assert

            Assert.Equal( new[] { 3, 4, 7, 9 }, array );

            Assert.Empty( events );
        }

        [Fact]
        public void ICollection_CopyTo()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableCollection = new ObservableSortedCollection<int>( new[] { 9, 3, 7, 4 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            var array = new int[ 4 ];

            // Act

            ( (ICollection) observableCollection ).CopyTo( array, 0 );

            // Assert

            Assert.Equal( new[] { 3, 4, 7, 9 }, array );

            Assert.Empty( events );
        }
    }
}
