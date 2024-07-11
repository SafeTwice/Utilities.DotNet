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

#pragma warning disable xUnit2013 // Do not use equality check to check for observableList size.

namespace Utilities.DotNet.Test.Collections.Observables
{
    public class ObservableSortedListTest
    {
        [Fact]
        public void Constructor_Default()
        {
            var observableList = new ObservableSortedList<TestClass>();

            Assert.Equal( 0, observableList.Count );
            Assert.Equal( Comparer<TestClass>.Default, observableList.Comparer );

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
            var customComparer = Comparer<TestClass>.Create( ( x, y ) => 0 );

            var observableList = new ObservableSortedList<TestClass>( customComparer );

            Assert.Equal( 0, observableList.Count );

            Assert.False( ( (IObservableList<TestClass>) observableList ).IsReadOnly );
            Assert.Same( customComparer, observableList.Comparer );

            Assert.False( ( (IList) observableList ).IsReadOnly );
            Assert.False( ( (IList) observableList ).IsFixedSize );
            Assert.NotNull( ( (IList) observableList ).SyncRoot );
            Assert.False( ( (IList) observableList ).IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableSortedList<TestClass>( new[] { item3, item1, item2 } );

            Assert.Equal( new[] { item1, item2, item3 }, observableList );
            Assert.Equal( 3, observableList.Count );
            Assert.Equal( Comparer<TestClass>.Default, observableList.Comparer );

            Assert.False( ( (IObservableList<TestClass>) observableList ).IsReadOnly );

            Assert.False( ( (IList) observableList ).IsReadOnly );
            Assert.False( ( (IList) observableList ).IsFixedSize );
            Assert.NotNull( ( (IList) observableList ).SyncRoot );
            Assert.False( ( (IList) observableList ).IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationListAndComparer()
        {
            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            var customComparer = Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) );

            var observableList = new ObservableSortedList<TestClass>( new[] { item3, item1, item2 }, customComparer );

            Assert.Equal( new[] { item2, item1, item3 }, observableList );
            Assert.Equal( 3, observableList.Count );
            Assert.Same( customComparer, observableList.Comparer );

            Assert.False( ( (IObservableList<TestClass>) observableList ).IsReadOnly );

            Assert.False( ( (IList) observableList ).IsReadOnly );
            Assert.False( ( (IList) observableList ).IsFixedSize );
            Assert.NotNull( ( (IList) observableList ).SyncRoot );
            Assert.False( ( (IList) observableList ).IsSynchronized );
        }

        [Fact]
        public void Add()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2 }, observableList );

            observableList.Add( item1 );

            Assert.Equal( new[] { item1, item2 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            observableList.Add( item3 );

            Assert.Equal( new[] { item1, item2, item3 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 2, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void IList_Add()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableSortedList<TestClass>( new[] { item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item2 }, observableList );

            Assert.Equal( 0, list.Add( item1 ) );

            Assert.Equal( new[] { item1, item2 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void IList_Add_InvalidType()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new List<int>();
            var item2 = new TestClass( "Item2", 1 );

            var observableList = new ObservableSortedList<TestClass>( new[] { item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item2 }, observableList );

            Assert.Equal( -1, list.Add( item1 ) );

            Assert.Equal( new[] { item2 }, observableList );
            Assert.Empty( events );
        }

        [Fact]
        public void AddRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2 }, observableList );

            observableList.AddRange( new[] { item3, item1 } );

            Assert.Equal( new[] { item1, item2, item3 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3, item1 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void Insert()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2 }, observableList );

            observableList.Insert( 1, item1 );

            Assert.Equal( new[] { item1, item2 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            observableList.Insert( 0, item3 );

            Assert.Equal( new[] { item1, item2, item3 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 2, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void IList_Insert()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableSortedList<TestClass>( new[] { item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item2 }, observableList );

            list.Insert( 55, item1 );

            Assert.Equal( new[] { item1, item2 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].NewItems );
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

            var observableList = new ObservableSortedList<TestClass>( new[] { item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item2 }, observableList );

            Assert.Throws<InvalidCastException>( () => list.Insert( 0, item1 ) );

            Assert.Equal( new[] { item2 }, observableList );
            Assert.Empty( events );
        }

        [Fact]
        public void InsertRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableSortedList<TestClass>( new[] { item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2 }, observableList );

            observableList.InsertRange( 0, new[] { item3, item1 } );

            Assert.Equal( new[] { item1, item2, item3 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3, item1 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void Remove()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item1, item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, observableList );

            observableList.Remove( item1 );

            Assert.Equal( new[] { item2, item3 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            events.Clear();

            observableList.Remove( item1 );

            Assert.Equal( new[] { item2, item3 }, observableList );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Remove()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableSortedList<TestClass>( new[] { item1, item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item1, item2, item3 }, observableList );

            list.Remove( item1 );

            Assert.Equal( new[] { item2, item3 }, observableList );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            events.Clear();

            list.Remove( item1 );

            Assert.Equal( new[] { item2, item3 }, observableList );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Remove_InvalidType()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new List<int>();
            var item2 = new TestClass( "Item2", 1 );

            var observableList = new ObservableSortedList<TestClass>( new[] { item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item2 }, observableList );

            list.Remove( item1 );

            Assert.Equal( new[] { item2 }, observableList );
            Assert.Empty( events );
        }

        [Fact]
        public void RemoveAt()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item2, item3, item1 } );

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

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item2, item3, item1 } );

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

            var observableList = new ObservableSortedList<TestClass>( new[] { item2, item1, item3 } );

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
        public void Contains()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item1, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.True( observableList.Contains( item2 ) );
            Assert.False( observableList.Contains( item3 ) );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Contains()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );
            var item4 = new List<int>();

            var observableList = new ObservableSortedList<TestClass>( new[] { item1, item2 } );

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

            var observableList = new ObservableSortedList<TestClass>( new[] { item2, item3, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            var range1 = observableList.GetRange( 1, 2 );

            Assert.Equal( new[] { item2, item3 }, range1 );
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

            var observableList = new ObservableSortedList<TestClass>( new[] { item2, item3, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IReadOnlyListEx<TestClass> readOnlyList = observableList;

            var range1 = readOnlyList.GetRange( 1, 2 );

            Assert.Equal( new[] { item2, item3 }, range1 );
            Assert.Empty( events );

            var range2 = readOnlyList.GetRange( 0, 1 );

            Assert.Equal( new[] { item2 }, range2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IObservableReadOnlyList_GetRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableSortedList<TestClass>( new[] { item2, item3, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IObservableReadOnlyList<TestClass> readOnlyList = observableList;

            var range1 = readOnlyList.GetRange( 1, 2 );

            Assert.Equal( new[] { item2, item3 }, range1 );
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

            var observableList = new ObservableSortedList<TestClass>( new[] { item2, item3, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IListEx<TestClass> readOnlyList = observableList;

            var range1 = readOnlyList.GetRange( 1, 2 );

            Assert.Equal( new[] { item2, item3 }, range1 );
            Assert.Empty( events );

            var range2 = readOnlyList.GetRange( 0, 1 );

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

            var observableList = new ObservableSortedList<TestClass>( new[] { item2, item3, item1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            var range1 = observableList.Slice( 0, 2 );

            Assert.Equal( new[] { item1, item2 }, range1 );
            Assert.Empty( events );

            var range2 = observableList.Slice( 2, 1 );

            Assert.Equal( new[] { item3 }, range2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlyListEx_Slice()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableSortedList<TestClass>( new[] { item1, item3, item1, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IReadOnlyListEx<TestClass> readOnlyList = observableList;

            var range1 = readOnlyList.Slice( 1, 2 );

            Assert.Equal( new[] { item1, item2 }, range1 );
            Assert.Empty( events );

            var range2 = readOnlyList.Slice( 0, 2 );

            Assert.Equal( new[] { item1, item1 }, range2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IObservableReadOnlyList_Slice()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableSortedList<TestClass>( new[] { item1, item3, item1, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IObservableReadOnlyList<TestClass> readOnlyList = observableList;

            var range1 = readOnlyList.Slice( 1, 2 );

            Assert.Equal( new[] { item1, item2 }, range1 );
            Assert.Empty( events );

            var range2 = readOnlyList.Slice( 0, 2 );

            Assert.Equal( new[] { item1, item1 }, range2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IListEx_Slice()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableSortedList<TestClass>( new[] { item1, item3, item1, item2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IListEx<TestClass> readOnlyList = observableList;

            var range1 = readOnlyList.Slice( 1, 2 );

            Assert.Equal( new[] { item1, item2 }, range1 );
            Assert.Empty( events );

            var range2 = readOnlyList.Slice( 0, 2 );

            Assert.Equal( new[] { item1, item1 }, range2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IndexOf()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item3, item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( -1, observableList.IndexOf( item1 ) );
            Assert.Equal( 0, observableList.IndexOf( item2 ) );
            Assert.Equal( -1, observableList.IndexOf( item2, 1 ) );
            Assert.Equal( 1, observableList.IndexOf( item3 ) );
            Assert.Equal( 2, observableList.IndexOf( item3, 2 ) );
            Assert.Equal( -1, observableList.IndexOf( item3, 0, 1 ) );
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

            var observableList = new ObservableSortedList<TestClass>( new[] { item2, item3 } );

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
            var list = new ObservableSortedList<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            Assert.Equal( 3, collection.IndexOf( 6 ) );
            Assert.Equal( -1, collection.IndexOf( 10 ) );
            Assert.Equal( -1, collection.IndexOf( 10.0 ) );

            Assert.Equal( 1, collection.IndexOf( 3, 1 ) );
            Assert.Equal( -1, collection.IndexOf( 3, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 4.0, 1 ) );

            Assert.Equal( 3, collection.IndexOf( 6, 2, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 6, 1, 2 ) );
            Assert.Equal( -1, collection.IndexOf( 3.0, 2, 2 ) );
        }

        [Fact]
        public void LastIndexOf()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item3, item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( -1, observableList.LastIndexOf( item1 ) );
            Assert.Equal( 0, observableList.LastIndexOf( item2 ) );
            Assert.Equal( -1, observableList.LastIndexOf( item2, 2, 2 ) );
            Assert.Equal( 2, observableList.LastIndexOf( item3 ) );
            Assert.Equal( 1, observableList.LastIndexOf( item3, 1 ) );
            Assert.Equal( -1, observableList.LastIndexOf( item3, 0, 1 ) );
            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlyCollectionEx_LastIndexOf()
        {
            var list = new ObservableSortedList<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            Assert.Equal( 3, collection.LastIndexOf( 6 ) );
            Assert.Equal( -1, collection.LastIndexOf( 10 ) );
            Assert.Equal( -1, collection.LastIndexOf( 10.0 ) );

            Assert.Equal( 1, collection.LastIndexOf( 3, 2 ) );
            Assert.Equal( -1, collection.LastIndexOf( 6, 2 ) );
            Assert.Equal( -1, collection.LastIndexOf( 4.0, 1 ) );

            Assert.Equal( 2, collection.LastIndexOf( 4, 2, 2 ) );
            Assert.Equal( -1, collection.LastIndexOf( 4, 1, 2 ) );
            Assert.Equal( -1, collection.LastIndexOf( 3.0, 2, 2 ) );
        }

        [Fact]
        public void Indexer_Get_ValidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item2, item3, item1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( item1, observableList[ 0 ] );
            Assert.Equal( item2, observableList[ 1 ] );
            Assert.Equal( item3, observableList[ 2 ] );
            Assert.Empty( events );
        }

        [Fact]
        public void Indexer_Get_InvalidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item2, item3, item1 } );

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

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item3 }, observableList );

            observableList[ 1 ] = item1;

            Assert.Equal( new[] { item1, item2 }, observableList );
            Assert.Equal( 2, events.Count );

            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 1 ].Action );
            Assert.Equal( new[] { item1 }, events[ 1 ].NewItems );
            Assert.Null( events[ 1 ].OldItems );
            Assert.Equal( 0, events[ 1 ].NewStartingIndex );
            Assert.Equal( -1, events[ 1 ].OldStartingIndex );
        }

        [Fact]
        public void Indexer_Set_InvalidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item3, item1 } );

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

            var observableList = new ObservableSortedList<TestClass>( new[] { item2, item3, item1 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( item1, list[ 0 ] );
            Assert.Equal( item2, list[ 1 ] );
            Assert.Equal( item3, list[ 2 ] );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Indexer_Set()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableSortedList<TestClass>( new[] { item2, item3 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IList list = observableList;

            Assert.Equal( new[] { item2, item3 }, list );

            list[ 1 ] = item1;

            Assert.Equal( new[] { item1, item2 }, list );
            Assert.Equal( 2, events.Count );

            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 1 ].Action );
            Assert.Equal( new[] { item1 }, events[ 1 ].NewItems );
            Assert.Null( events[ 1 ].OldItems );
            Assert.Equal( 0, events[ 1 ].NewStartingIndex );
            Assert.Equal( -1, events[ 1 ].OldStartingIndex );
        }

        [Fact]
        public void IList_Indexer_Set_InvalidType()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new List<int>();
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableList = new ObservableSortedList<TestClass>( new[] { item2, item3 } );

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

            IObservableList<TestClass> observableList = new ObservableSortedList<TestClass>( new[] { item2 } );

            Assert.Equal( new[] { item2 }, observableList );

            observableList.Add( item1 );

            Assert.Equal( new[] { item1, item2 }, observableList );

            observableList.Insert( 0, item3 );

            Assert.Equal( new[] { item1, item2, item3 }, observableList );

            observableList.Remove( item2 );

            Assert.Equal( new[] { item1, item3 }, observableList );

            observableList.RemoveAt( 0 );

            Assert.Equal( new[] { item3 }, observableList );
        }
    }
}
