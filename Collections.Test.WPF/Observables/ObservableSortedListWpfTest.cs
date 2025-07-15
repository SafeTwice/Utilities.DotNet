/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;
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

namespace Utilities.DotNet.Collections.Observables.Test.WPF
{
    public class ObservableSortedListWpfTest
    {
        [Theory]
        [ClassData( typeof( ObservableSortedListTest.Add_Class_Nullable_TestData ) )]
        public void Add_Class_Nullable( IEnumerable<TestClass?> initialState, TestClass? addedItem, int expectedResult, IEnumerable<TestClass?> expectedState,
                                        IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<TestClass?> observableList = new ObservableSortedList<TestClass?>( initialState );
            var view = CollectionViewSource.GetDefaultView( observableList );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = observableList.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedListTest.IListEx_Add_Value_Nullable_TestData ) )]
        public void IListEx_Add_Value_Nullable( IEnumerable<int?> initialState, object? addedItem, int expectedResult, IEnumerable<int?> expectedState,
                                                IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<int?>( initialState );
            var testedCollection = (IListEx) observableList;
            var view = CollectionViewSource.GetDefaultView( observableList );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedListTest.IList_Add_Class_Nullable_TestData ) )]
        public void IList_Add_Class_Nullable( IEnumerable<TestClass?> initialState, object? addedItem, int expectedResult, IEnumerable<TestClass?> expectedState,
                                              IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<TestClass?>( initialState );
            var testedCollection = (IList) observableList;
            var view = CollectionViewSource.GetDefaultView( observableList );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            var result = testedCollection.Add( addedItem );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedListTest.Insert_Class_TestData ) )]
        public void Insert_Class( IEnumerable<TestClass?> initialState, int index, TestClass? insertedItem, IEnumerable<TestClass?> expectedState,
                                  IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<TestClass?> observableList = new ObservableSortedList<TestClass?>( initialState );
            var view = CollectionViewSource.GetDefaultView( observableList );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableList.Insert( index, insertedItem );

            // Assert

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedListTest.IList_Insert_Class_TestData ) )]
        public void IList_Insert_Class( IEnumerable<TestClass> initialState, int index, TestClass? insertedItem, IEnumerable<TestClass?> expectedState,
                                        IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<TestClass>( initialState );
            var testedCollection = (IList) observableList;
            var view = CollectionViewSource.GetDefaultView( observableList );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.Insert( index, insertedItem );

            // Assert state

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedListTest.InsertRange_Class_TestData ) )]
        public void InsertRange_Class( IEnumerable<TestClass?> initialState, int index, IEnumerable<TestClass?> insertedItems,
                                       IEnumerable<TestClass?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<TestClass?> observableList = new ObservableSortedList<TestClass?>( initialState );
            var view = CollectionViewSource.GetDefaultView( observableList );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableList.InsertRange( index, insertedItems );

            // Assert

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedListTest.IListEx_InsertRange_Class_TestData ) )]
        public void IListEx_InsertRange_Class( IEnumerable<TestClass> initialState, int index, IEnumerable<TestClass?> insertedItems,
                                               IEnumerable<TestClass?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<TestClass>( initialState );
            var testedCollection = (IListEx) observableList;
            var view = CollectionViewSource.GetDefaultView( observableList );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.InsertRange( index, insertedItems );

            // Assert state

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedListTest.IList_Remove_Value_TestData ) )]
        public void IList_Remove_Value( IEnumerable<int?> initialState, object? removedItem, IEnumerable<int?> expectedState,
                                        IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<int?>( initialState );
            var testedCollection = (IList) observableList;
            var view = CollectionViewSource.GetDefaultView( observableList );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };
            // Act

            testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedListTest.IList_Remove_Value_TestData ) )]
        public void IListEx_Remove_Value( IEnumerable<int?> initialState, object? removedItem, IEnumerable<int?> expectedState,
                                          IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<int?>( initialState );
            var testedCollection = (IListEx) observableList;
            var view = CollectionViewSource.GetDefaultView( observableList );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.Remove( removedItem );

            // Assert

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedListTest.RemoveAt_Value_TestData ) )]
        public void RemoveAt_Value( IEnumerable<int?> initialState, int index, IEnumerable<int?> expectedState,
                                    IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<int?> observableList = new ObservableSortedList<int?>( initialState );
            var view = CollectionViewSource.GetDefaultView( observableList );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableList.RemoveAt( index );

            // Assert state

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedListTest.RemoveRange_Value_TestData ) )]
        public void RemoveRange_Value( IEnumerable<int?> initialState, int index, int count, IEnumerable<int?> expectedState,
                                       IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<int?> observableList = new ObservableSortedList<int?>( initialState );
            var view = CollectionViewSource.GetDefaultView( observableList );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableList.RemoveRange( index, count );

            // Assert state

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedListTest.ReplaceByIndex_Value_TestData ) )]
        public void ReplaceByIndex_Value( IEnumerable<double> initialState, int index, double newItem,
                                          IEnumerable<double> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            IObservableList<double> observableList = new ObservableSortedList<double>( initialState );
            var view = CollectionViewSource.GetDefaultView( observableList );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            observableList.Replace( index, newItem );

            // Assert

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }

        [Theory]
        [ClassData( typeof( ObservableSortedListTest.IListEx_ReplaceByIndex_Value_TestData ) )]
        public void IListEx_ReplaceByIndex_Value( IEnumerable<double> initialState, int index, double newItem,
                                                  IEnumerable<double> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableList = new ObservableSortedList<double>( initialState );
            var testedCollection = (IListEx) observableList;
            var view = CollectionViewSource.GetDefaultView( observableList );

            view.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( view, obj );
                events.Add( new CollectionChangedEventData( args ) );
            };

            // Act

            testedCollection.Replace( index, newItem );

            // Assert state & result

            Assert.Equal( expectedState, observableList );
            Assert.Equal( expectedEvents, events );
        }
    }
}
