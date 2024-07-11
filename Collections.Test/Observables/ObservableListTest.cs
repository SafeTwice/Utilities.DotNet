/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Utilities.DotNet.Collections;
using Utilities.DotNet.Collections.Observables;
using Xunit;

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.
#pragma warning disable xUnit2017 // Do not use Contains() to check if a value exists in a collection

namespace Utilities.DotNet.Test.Collections.Observables
{
    public class ObservableListTest
    {
        [Fact]
        public void Constructor_Default()
        {
            var observableList = new ObservableList<TestClass>();

            Assert.Equal( 0, observableList.Count );

            Assert.False( ( (IObservableList<TestClass>) observableList ).IsReadOnly );

            Assert.False( ( (IList) observableList ).IsReadOnly );
            Assert.False( ( (IList) observableList ).IsFixedSize );
            Assert.NotNull( ( (IList) observableList ).SyncRoot );
            Assert.False( ( (IList) observableList ).IsSynchronized );

            // The following tests are to ensure interface disambiguation.

            Assert.Equal( 0, ( (IObservableList<TestClass>) observableList ).Count );
            Assert.Equal( 0, ( (ICollectionEx<TestClass>) observableList ).Count );
            //Assert.False( ( (IObservableList<TestClass>) observableList ).IsReadOnly );
            //( (IObservableList<TestClass>) observableList ).Clear();
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            Assert.Equal( new[] { item1, item2, item3 }, observableList );
            Assert.Equal( 3, observableList.Count );

            Assert.False( ( (IObservableList<TestClass>) observableList ).IsReadOnly );

            Assert.False( ( (IList) observableList ).IsReadOnly );
            Assert.False( ( (IList) observableList ).IsFixedSize );
            Assert.NotNull( ( (IList) observableList ).SyncRoot );
            Assert.False( ( (IList) observableList ).IsSynchronized );
        }

        [Fact]
        public void IList_Add()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );

            var observableList = new ObservableList<TestClass>( new[] { item1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item1 }, list );

            Assert.Equal( 1, list.Add( item2 ) );

            Assert.Equal( new[] { item1, item2 }, list );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void IList_Add_InvalidType()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new List<int>();
            var item2 = new TestClass( "Item2", 1 );

            var observableList = new ObservableList<TestClass>( new[] { item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item2 }, list );

            Assert.Equal( -1, list.Add( item1 ) );

            Assert.Equal( new[] { item2 }, list );
            Assert.Empty( events );
        }

        [Fact]
        public void Insert()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );
            var item4 = new TestClass( "Item4", -12 );

            IObservableList<TestClass> observableList = new ObservableList<TestClass>( new[] { item1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1 }, observableList );

            observableList.Insert( 0, item2 );

            Assert.Equal( new[] { item2, item1 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            observableList.Insert( 1, item3 );

            Assert.Equal( new[] { item2, item3, item1 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            observableList.Insert( 3, item4 );

            Assert.Equal( new[] { item2, item3, item1, item4 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item4 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 3, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void Insert_IndexOutOfRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableList<TestClass>( new[] { item2, item1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1 }, observableList );

            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Insert( -4657, item3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Insert( -1, item3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Insert( 3, item3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Insert( 1245, item3 ) );

            Assert.Equal( new[] { item2, item1 }, observableList );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Insert()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );

            var observableList = new ObservableList<TestClass>( new[] { item1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item1 }, list );

            list.Insert( 0, item2 );

            Assert.Equal( new[] { item2, item1 }, list );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void IList_Insert_InvalidType()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new List<int>();
            var item2 = new TestClass( "Item2", 1 );

            var observableList = new ObservableList<TestClass>( new[] { item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item2 }, list );

            Assert.Throws<InvalidCastException>( () => list.Insert( 0, item1 ) );

            Assert.Equal( new[] { item2 }, list );
            Assert.Empty( events );
        }

        [Fact]
        public void InsertRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableList<TestClass>( new[] { item1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1 }, observableList );

            observableList.InsertRange( 0, new[] { item2, item3 } );

            Assert.Equal( new[] { item2, item3, item1 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item2, item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void IList_Remove()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item1, item2, item3 }, list );

            list.Remove( item1 );

            Assert.Equal( new[] { item2, item3 }, list );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void IList_Remove_InvalidType()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new List<int>();
            var item2 = new TestClass( "Item2", 1 );

            var observableList = new ObservableList<TestClass>( new[] { item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item2 }, list );

            list.Remove( item1 );

            Assert.Equal( new[] { item2 }, list );
            Assert.Empty( events );
        }

        [Fact]
        public void RemoveAt()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, observableList );

            observableList.RemoveAt( 1 );

            Assert.Equal( new[] { item1, item3 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void RemoveAt_OutOfRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, observableList );

            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.RemoveAt( -129 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.RemoveAt( -1 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.RemoveAt( 3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.RemoveAt( 15 ) );

            Assert.Equal( new[] { item1, item2, item3 }, observableList );
            Assert.Empty( events );
        }

        [Fact]
        public void RemoveRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, observableList );

            observableList.RemoveRange( 0, 2 );

            Assert.Equal( new[] { item3 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1, item2 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void IList_Clear()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item1, item2, item3 }, list );

            list.Clear();

            Assert.Empty( list );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Reset, events[ 0 ].Action );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void IList_Contains()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );
            var item4 = new List<int>();

            var observableList = new ObservableList<TestClass>( new[] { item1, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.True( list.Contains( item2 ) );
            Assert.False( list.Contains( item3 ) );
            Assert.False( list.Contains( item4 ) );
            Assert.Empty( events );
        }

        [Fact]
        public void GetRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            var range1 = observableList.GetRange( 1, 2 );

            Assert.Equal( new[] { item3, item2 }, range1 );
            Assert.Empty( events );

            var range2 = observableList.GetRange( 0, 1 );

            Assert.Equal( new[] { item2 }, range2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlyListEx_GetRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IReadOnlyListEx<TestClass> readOnlyList = observableList;

            var range1 = readOnlyList.GetRange( 1, 2 );

            Assert.Equal( new[] { item3, item2 }, range1 );
            Assert.Empty( events );

            var range2 = readOnlyList.GetRange( 0, 1 );

            Assert.Equal( new[] { item2 }, range2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IListEx_GetRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IListEx<TestClass> list = observableList;

            var range1 = list.GetRange( 1, 2 );

            Assert.Equal( new[] { item3, item2 }, range1 );
            Assert.Empty( events );

            var range2 = list.GetRange( 0, 1 );

            Assert.Equal( new[] { item2 }, range2 );
            Assert.Empty( events );
        }

        [Fact]
        public void Slice()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            var range1 = observableList.Slice( 1, 2 );

            Assert.Equal( new[] { item3, item2 }, range1 );
            Assert.Empty( events );

            var range2 = observableList.Slice( 0, 1 );

            Assert.Equal( new[] { item2 }, range2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlyListEx_Slice()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IReadOnlyListEx<TestClass> readOnlyList = observableList;

            var range1 = readOnlyList.Slice( 1, 2 );

            Assert.Equal( new[] { item3, item2 }, range1 );
            Assert.Empty( events );

            var range2 = readOnlyList.Slice( 0, 1 );

            Assert.Equal( new[] { item2 }, range2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IObservableReadOnlyList_Slice()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IObservableReadOnlyList<TestClass> readOnlyList = observableList;

            var range1 = readOnlyList.Slice( 1, 2 );

            Assert.Equal( new[] { item3, item2 }, range1 );
            Assert.Empty( events );

            var range2 = readOnlyList.Slice( 0, 1 );

            Assert.Equal( new[] { item2 }, range2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IListEx_Slice()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IListEx<TestClass> list = observableList;

            var range1 = list.Slice( 1, 2 );

            Assert.Equal( new[] { item3, item2 }, range1 );
            Assert.Empty( events );

            var range2 = list.Slice( 0, 1 );

            Assert.Equal( new[] { item2 }, range2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IndexOf()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( -1, observableList.IndexOf( item1 ) );
            Assert.Equal( 0, observableList.IndexOf( item2 ) );
            Assert.Equal( 2, observableList.IndexOf( item2, 1 ) );
            Assert.Equal( -1, observableList.IndexOf( item2, 1, 1 ) );
            Assert.Equal( 1, observableList.IndexOf( item3 ) );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_IndexOf()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );
            var item4 = new List<int>();

            var observableList = new ObservableList<TestClass>( new[] { item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( -1, list.IndexOf( item1 ) );
            Assert.Equal( 0, list.IndexOf( item2 ) );
            Assert.Equal( 1, list.IndexOf( item3 ) );
            Assert.Equal( -1, list.IndexOf( item4 ) );
            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlyCollectionEx_IndexOf()
        {
            var list = new ObservableList<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            Assert.Equal( 1, collection.IndexOf( 6 ) );
            Assert.Equal( -1, collection.IndexOf( 10 ) );
            Assert.Equal( -1, collection.IndexOf( 10.0 ) );

            Assert.Equal( 3, collection.IndexOf( 3, 1 ) );
            Assert.Equal( -1, collection.IndexOf( 6, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 4.0, 1 ) );

            Assert.Equal( 3, collection.IndexOf( 3, 2, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 6, 2, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 3.0, 2, 2 ) );
        }

        [Fact]
        public void LastIndexOf()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( -1, observableList.LastIndexOf( item1 ) );
            Assert.Equal( 2, observableList.LastIndexOf( item2 ) );
            Assert.Equal( 0, observableList.LastIndexOf( item2, 1 ) );
            Assert.Equal( -1, observableList.LastIndexOf( item2, 1, 1 ) );
            Assert.Equal( 1, observableList.LastIndexOf( item3 ) );
            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlyCollectionEx_LastIndexOf()
        {
            var list = new ObservableList<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            Assert.Equal( 1, collection.LastIndexOf( 6 ) );
            Assert.Equal( -1, collection.LastIndexOf( 10 ) );
            Assert.Equal( -1, collection.LastIndexOf( 10.0 ) );

            Assert.Equal( 0, collection.LastIndexOf( 3, 1 ) );
            Assert.Equal( -1, collection.LastIndexOf( 4, 1 ) );
            Assert.Equal( -1, collection.LastIndexOf( 4.0, 1 ) );

            Assert.Equal( 0, collection.LastIndexOf( 3, 1, 2 ) );
            Assert.Equal( -1, collection.LastIndexOf( 3, 2, 2 ) );
            Assert.Equal( -1, collection.LastIndexOf( 3.0, 2, 2 ) );
        }

        [Fact]
        public void Indexer_Get_ValidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableList<TestClass>( new[] { item2, item3, item1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( item2, observableList[ 0 ] );
            Assert.Equal( item3, observableList[ 1 ] );
            Assert.Equal( item1, observableList[ 2 ] );
            Assert.Empty( events );
        }

        [Fact]
        public void Indexer_Get_InvalidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableList<TestClass>( new[] { item2, item3, item1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -654 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -1 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -3 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ 34564 ] );
            Assert.Empty( events );
        }

        [Fact]
        public void Indexer_Set_ValidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableList<TestClass>( new[] { item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item3 }, observableList );

            observableList[ 1 ] = item1;

            Assert.Equal( new[] { item2, item1 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Replace, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].NewItems );
            Assert.Equal( new[] { item3 }, events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void Indexer_Set_InvalidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableList<TestClass>( new[] { item3, item1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -765 ] = item2 );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -1 ] = item2 );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ 3 ] = item2 );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ 346 ] = item2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Indexer_Get()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableList<TestClass>( new[] { item2, item3, item1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( item2, list[ 0 ] );
            Assert.Equal( item3, list[ 1 ] );
            Assert.Equal( item1, list[ 2 ] );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Indexer_Set()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableList<TestClass>( new[] { item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item2, item3 }, list );

            list[ 1 ] = item1;

            Assert.Equal( new[] { item2, item1 }, list );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Replace, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].NewItems );
            Assert.Equal( new[] { item3 }, events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void IList_Indexer_Set_InvalidType()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new List<int>();
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableList<TestClass>( new[] { item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item2, item3 }, list );

            Assert.Throws<InvalidCastException>( () => list[ 1 ] = item1 );

            Assert.Equal( new[] { item2, item3 }, list );
            Assert.Empty( events );
        }

        [Fact]
        public void NoEventListeners()
        {
            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item1 } );

            Assert.Equal( new[] { item1 }, collection );

            collection.Add( item2 );

            Assert.Equal( new[] { item1, item2 }, collection );

            collection.Insert( 0, item3 );

            Assert.Equal( new[] { item3, item1, item2 }, collection );

            collection.Remove( item2 );

            Assert.Equal( new[] { item3, item1 }, collection );

            collection.RemoveAt( 0 );

            Assert.Equal( new[] { item1 }, collection );
        }
    }
}
