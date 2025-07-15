/// @file
/// @copyright  Copyright (c) 2024-2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;
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

namespace Utilities.DotNet.Collections.Observables.Test.WPF
{
    public class ObservableCollectionWpfTest
    {
        [Theory]
        [ClassData( typeof( ObservableCollectionTest.Add_Class_Nullable_TestData ) )]
        public void Add_Nullable( IEnumerable<TestClass?> initialState, TestClass? addedItem, IEnumerable<TestClass?> expectedState,
                                  IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<TestClass?>( initialState );
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
        [ClassData( typeof( ObservableCollectionTest.ICollectionEx_Add_Value_Nullable_TestData ) )]
        public void ICollectionEx_Add_Value_Nullable( IEnumerable<int?> initialState, int? addedItem, IEnumerable<int?> expectedState,
                                                      IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int?>( initialState );
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
        [ClassData( typeof( ObservableCollectionTest.AddRange_Class_Nullable_TestData ) )]
        public void AddRange_Class_Nullable( IEnumerable<TestClass?> initialState, IEnumerable<TestClass?> addedItems, IEnumerable<TestClass?> expectedState,
                                             IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<TestClass?>( initialState );
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
        [ClassData( typeof( ObservableCollectionTest.ICollectionEx_AddRange_Value_Nullable_TestData ) )]
        public void ICollectionEx_AddRange_Value_Nullable( IEnumerable<int?> initialState, IEnumerable<int?> addedItems,
                                                           IEnumerable<int?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int?>( initialState );
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
        [ClassData( typeof( ObservableCollectionTest.Remove_Class_Nullable_TestData ) )]
        public void Remove_Class_Nullable( IEnumerable<TestClass?> initialState, TestClass? removedItem, bool expectedResult,
                                           IEnumerable<TestClass?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<TestClass?>( initialState );
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
        [ClassData( typeof( ObservableCollectionTest.ICollectionEx_Remove_Value_Nullable_TestData ) )]
        public void ICollectionEx_Remove_Value_Nullable( IEnumerable<int?> initialState, object? removedItem, bool expectedResult,
                                                         IEnumerable<int?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int?>( initialState );
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
        [ClassData( typeof( ObservableCollectionTest.RemoveRange_Class_Nullable_TestData ) )]
        public void RemoveRange_Class_Nullable( IEnumerable<TestClass?> initialState, IEnumerable<TestClass?> removedItems,
                                                IEnumerable<TestClass?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<TestClass?>( initialState );
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
        [ClassData( typeof( ObservableCollectionTest.ICollectionEx_RemoveRange_Value_Nullable_TestData ) )]
        public void ICollectionEx_RemoveRange_Value_Nullable( IEnumerable<int?> initialState, IEnumerable removedItems,
                                                              IEnumerable<int?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<int?>( initialState );
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
        [ClassData( typeof( ObservableCollectionTest.Replace_Value_Nullable_TestData ) )]
        public void Replace_Value_Nullable( IEnumerable<double?> initialState, double? oldItem, double? newItem, bool expectedResult,
                                            IEnumerable<double?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<double?>( initialState );
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
        [ClassData( typeof( ObservableCollectionTest.ICollectionEx_Replace_Value_Nullable_TestData ) )]
        public void ICollectionEx_Replace_Value_Nullable( IEnumerable<double?> initialState, object? oldItem, object? newItem, bool expectedResult,
                                                          IEnumerable<double?> expectedState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<double?>( initialState );
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
        [ClassData( typeof( ObservableCollectionTest.Clear_Class_Nullable_TestData ) )]
        public void Clear_Class_Nullable( IEnumerable<TestClass?> initialState, IEnumerable<CollectionChangedEventData> expectedEvents )
        {
            // Arrange

            var events = new List<CollectionChangedEventData>();

            var observableCollection = new ObservableCollection<TestClass?>( initialState );
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
    }
}
