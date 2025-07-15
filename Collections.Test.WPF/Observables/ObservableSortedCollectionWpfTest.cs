/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Data;
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

namespace Utilities.DotNet.Collections.Observables.Test.WPF
{
    public class ObservableSortedCollectionWpfTest
    {
        [Theory]
        [ClassData( typeof( ObservableSortedCollectionTest.Add_Class_Nullable_TestData ) )]
        public void Add_Class_Nullable( IEnumerable<TestClass?> initialState, TestClass? addedItem, IEnumerable<TestClass?> expectedState,
                                        IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass?> observableCollection = new ObservableSortedCollection<TestClass?>( initialState );
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedCollectionTest.ICollectionEx_Add_Value_Nullable_TestData ) )]
        public void ICollectionEx_Add_Value_Nullable( IEnumerable<int?> initialState, int? addedItem, IEnumerable<int?> expectedState,
                                                      IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableSortedCollection<int?>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedCollectionTest.AddRange_Class_Nullable_TestData ) )]
        public void AddRange_Class_Nullable( IEnumerable<TestClass?> initialState, IEnumerable<TestClass?> addedItems, IEnumerable<TestClass?> expectedState,
                                             IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass?> observableCollection = new ObservableSortedCollection<TestClass?>( initialState );
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedCollectionTest.ICollectionEx_AddRange_Value_Nullable_TestData ) )]
        public void ICollectionEx_AddRange_Value_Nullable( IEnumerable<int?> initialState, IEnumerable<int?> addedItems,
                                                           IEnumerable<int?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableSortedCollection<int?>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.AddRange( addedItems );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedCollectionTest.Remove_Class_Nullable_TestData ) )]
        public void Remove_Class_Nullable( IEnumerable<TestClass?> initialState, TestClass? removedItem, bool expectedResult,
                                           IEnumerable<TestClass?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass?> observableCollection = new ObservableSortedCollection<TestClass?>( initialState );
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedCollectionTest.ICollectionEx_Remove_Value_Nullable_TestData ) )]
        public void ICollectionEx_Remove_Value_Nullable( IEnumerable<int?> initialState, object? removedItem, bool expectedResult, IEnumerable<int?> expectedState,
                                                         IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableSortedCollection<int?>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedCollectionTest.RemoveRange_Class_Nullable_TestData ) )]
        public void RemoveRange_Class_Nullable( IEnumerable<TestClass?> initialState, IEnumerable<TestClass?> removedItems,
                                                IEnumerable<TestClass?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass?> observableCollection = new ObservableSortedCollection<TestClass?>( initialState );
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedCollectionTest.ICollectionEx_RemoveRange_Value_Nullable_TestData ) )]
        public void ICollectionEx_RemoveRange_Value_Nullable( IEnumerable<int?> initialState, IEnumerable removedItems,
                                                              IEnumerable<int?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableSortedCollection<int?>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.RemoveRange( removedItems );

            // Assert

            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedCollectionTest.Replace_Value_Nullable_TestData ) )]
        public void Replace_Value_Nullable( IEnumerable<double?> initialState, double? oldItem, double? newItem, bool expectedResult,
                                            IEnumerable<double?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<double?> observableCollection = new ObservableSortedCollection<double?>( initialState );
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableCollection.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedCollectionTest.ICollectionEx_Replace_Value_Nullable_TestData ) )]
        public void ICollectionEx_Replace_Value_Nullable( IEnumerable<double?> initialState, object? oldItem, object? newItem, bool expectedResult,
                                                          IEnumerable<double?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableSortedCollection<double?>( initialState );
            var testedCollection = (ICollectionEx) observableCollection;
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Replace( oldItem, newItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableCollection );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedCollectionTest.Clear_Class_Nullable_TestData ) )]
        public void Clear_Class_Nullable( IEnumerable<TestClass?> initialState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableCollection<TestClass?> observableCollection = new ObservableSortedCollection<TestClass?>( initialState );
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableCollection.Clear();

            // Assert

            Assert.Empty( observableCollection );
            Assert.Equal( expectedEvents, events );
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
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
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
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
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
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
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

            var itemA = new TestClass( "ItemA", 10 );
            var itemB = new TestClass( "ItemB", 5 );
            var itemC = new TestClass( "ItemC", 20 );

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableCollection = new ObservableSortedCollection<TestClass>( new[] { itemC, itemA },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );
            var view = CollectionViewSource.GetDefaultView( observableCollection );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { itemA, itemC }, observableCollection );

            // Act

            var exception = Assert.Throws<ArgumentException>( () => observableCollection.UpdateSortOrder( itemB ) );

            // Assert events

            Assert.Equal( "item", exception.ParamName );
            Assert.Empty( events );
        }
    }
}
