/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Utilities.DotNet.Collections;
using Utilities.DotNet.Collections.Observables;
using Xunit;
using Xunit.Abstractions;

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.
#pragma warning disable xUnit2017 // Do not use Contains() to check if a value exists in a collection

namespace Utilities.DotNet.Test.Collections.Observables
{
    public class ObservableListTest
    {
        [Fact]
        public void Constructor_Default()
        {
            // Arrange & Act

            var observableList = new ObservableList<TestClass>();

            // Assert

            Assert.Equal( 0, observableList.Count );

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
        public void Constructor_InitializationList()
        {
            // Arrange

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            // Act

            var observableList = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            // Assert

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
            // Arrange

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

            // Act

            var result = list.Add( item2 );

            // Assert state & result

            Assert.Equal( 1, result );
            Assert.Equal( new[] { item1, item2 }, list );

            // Assert events

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
            // Arrange

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

            // Act

            var result = list.Add( item1 );

            // Assert state & result

            Assert.Equal( -1, result );

            // Assert state

            Assert.Equal( new[] { item2 }, list );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void Insert()
        {
            // Arrange

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

            // Act

            observableList.Insert( 0, item2 );

            // Assert state

            Assert.Equal( new[] { item2, item1 }, observableList );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            // Arrange

            events.Clear();

            // Act

            observableList.Insert( 1, item3 );

            // Assert state

            Assert.Equal( new[] { item2, item3, item1 }, observableList );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            // Arrange

            events.Clear();

            // Act

            observableList.Insert( 3, item4 );

            // Assert state

            Assert.Equal( new[] { item2, item3, item1, item4 }, observableList );

            // Assert events

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
            // Arrange

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

            // Act & Assert

            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Insert( -4657, item3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Insert( -1, item3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Insert( 3, item3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Insert( 1245, item3 ) );

            // Assert state

            Assert.Equal( new[] { item2, item1 }, observableList );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IList_Insert()
        {
            // Arrange

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

            // Act

            list.Insert( 0, item2 );

            // Assert state

            Assert.Equal( new[] { item2, item1 }, list );

            // Assert events

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
            // Arrange

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

            // Act & Assert

            Assert.Throws<InvalidCastException>( () => list.Insert( 0, item1 ) );

            // Assert state

            Assert.Equal( new[] { item2 }, list );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IListExT_InsertRange()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableList<int>( new[] { 5, 8, 2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IListEx list = observableList;

            Assert.Equal( new[] { 5, 8, 2 }, (IList) list );

            // Act

            var result = list.InsertRange( 2, new[] { 66, 1 } );

            // Assert state

            Assert.True( result );
            Assert.Equal( new[] { 5, 8, 66, 1, 2 }, (IList) list );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { 66, 1 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 2, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void IListExT_InsertRange_InvalidObject()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableList = new ObservableList<int>( new[] { 5, 8, 2 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            IListEx list = observableList;

            Assert.Equal( new[] { 5, 8, 2 }, (IList) list );

            // Act

            var result = list.InsertRange( 2, new[] { 66, 1.1 } );

            // Assert state

            Assert.False( result );
            Assert.Equal( new[] { 5, 8, 2 }, (IList) list );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void InsertRange()
        {
            // Arrange

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

            // Act

            observableList.InsertRange( 0, new[] { item2, item3 } );

            // Assert state

            Assert.Equal( new[] { item2, item3, item1 }, observableList );

            // Assert events

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
            // Arrange

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

            // Act

            list.Remove( item1 );

            // Assert state

            Assert.Equal( new[] { item2, item3 }, list );

            // Assert events

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
            // Arrange

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

            // Act

            list.Remove( item1 );

            // Assert state

            Assert.Equal( new[] { item2 }, list );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void RemoveAt()
        {
            // Arrange

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

            // Act

            observableList.RemoveAt( 1 );

            // Assert state

            Assert.Equal( new[] { item1, item3 }, observableList );

            // Assert events

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
            // Arrange

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

            // Act & Assert

            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.RemoveAt( -129 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.RemoveAt( -1 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.RemoveAt( 3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.RemoveAt( 15 ) );

            // Assert state

            Assert.Equal( new[] { item1, item2, item3 }, observableList );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void RemoveRange()
        {
            // Arrange

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

            // Act

            observableList.RemoveRange( 0, 2 );

            // Assert state

            Assert.Equal( new[] { item3 }, observableList );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1, item2 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void ReplaceByIndex()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<double> observableList = new ObservableList<double>( new[] { 5.1, 8.0, 2.9 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = observableList.Replace( 1, 3.5 );

            // Assert state & result

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 3.5, 2.9 }, observableList );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Replace, events[ 0 ].Action );
            Assert.Equal( new[] { 8.0 }, events[ 0 ].OldItems );
            Assert.Equal( new[] { 3.5 }, events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        void ReplaceByIndex_OutOfRange()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<double> observableList = new ObservableList<double>( new[] { 5.1, 8.0, 2.9 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Replace( 3, 3.5 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Replace( -1, 3.5 ) );

            // Assert state

            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, observableList );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IListEx_ReplaceByIndex()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<double> observableList = new ObservableList<double>( new[] { 5.1, 8.0, 2.9 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = ( (IListEx) observableList ).Replace( 1, 3.5 );

            // Assert state & result

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 3.5, 2.9 }, observableList );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Replace, events[ 0 ].Action );
            Assert.Equal( new[] { 8.0 }, events[ 0 ].OldItems );
            Assert.Equal( new[] { 3.5 }, events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void IListEx_ReplaceByIndex_InvalidObject()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<double> observableList = new ObservableList<double>( new[] { 5.1, 8.0, 2.9 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = ( (IListEx) observableList ).Replace( 1, 3.5f );

            // Assert state & result

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, observableList );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void Move()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<double> observableList = new ObservableList<double>( new[] { 5.1, 8.0, 2.9, 44.5 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act: Same position

            var result = observableList.Move( 8.0, 1 );

            // Assert state & result

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, observableList );

            // Assert events

            Assert.Empty( events );

            // Act: Position before current

            result = observableList.Move( 2.9, 0 );

            // Assert state & result

            Assert.True( result );
            Assert.Equal( new[] { 2.9, 5.1, 8.0, 44.5 }, observableList );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { 2.9 }, events[ 0 ].OldItems );
            Assert.Equal( new[] { 2.9 }, events[ 0 ].NewItems );
            Assert.Equal( 2, events[ 0 ].OldStartingIndex );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );

            // Arrange

            events.Clear();

            // Act: Position just after current

            result = observableList.Move( 8.0, 3 );

            // Assert state & result

            Assert.True( result );
            Assert.Equal( new[] { 2.9, 5.1, 8.0, 44.5 }, observableList );

            // Assert events

            Assert.Empty( events );

            // Act: Position at end

            result = observableList.Move( 5.1, 4 );

            // Assert state & result

            Assert.True( result );
            Assert.Equal( new[] { 2.9, 8.0, 44.5, 5.1 }, observableList );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { 5.1 }, events[ 0 ].OldItems );
            Assert.Equal( new[] { 5.1 }, events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( 3, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        void Move_NotExisting()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<double> observableList = new ObservableList<double>( new[] { 5.1, 8.0, 2.9, 44.5 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = observableList.Move( 8.01, 1 );

            // Assert state & result

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, observableList );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        void Move_OutOfRange()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<double> observableList = new ObservableList<double>( new[] { 5.1, 8.0, 2.9, 44.5 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act & Assert

            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Move( 8.0, 5 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Move( 8.0, -1 ) );

            // Assert state

            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, observableList );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        void MoveByIndex()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<double> observableList = new ObservableList<double>( new[] { 5.1, 8.0, 2.9, 44.5 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act: Same position

            var result = observableList.Move( 1, 1 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, observableList );

            // Assert events

            Assert.Empty( events );

            // Act: Position before current

            result = observableList.Move( 2, 0 );

            // Assert state & result

            Assert.True( result );
            Assert.Equal( new[] { 2.9, 5.1, 8.0, 44.5 }, observableList );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { 2.9 }, events[ 0 ].OldItems );
            Assert.Equal( new[] { 2.9 }, events[ 0 ].NewItems );
            Assert.Equal( 2, events[ 0 ].OldStartingIndex );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );

            // Arrange

            events.Clear();

            // Act: Position just after current

            result = observableList.Move( 2, 3 );

            // Assert state & result

            Assert.True( result );
            Assert.Equal( new[] { 2.9, 5.1, 8.0, 44.5 }, observableList );

            // Assert events

            Assert.Empty( events );

            // Act: Position at end

            result = observableList.Move( 5.1, 4 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 2.9, 8.0, 44.5, 5.1 }, observableList );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { 5.1 }, events[ 0 ].OldItems );
            Assert.Equal( new[] { 5.1 }, events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( 3, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        void MoveByIndex_OutOfRange()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<double> observableList = new ObservableList<double>( new[] { 5.1, 8.0, 2.9 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, observableList );

            // Act & Assert

            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Move( 0, 5 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Move( 0, -1 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Move( -2, 3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Move( 10, 2 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList.Move( 10, -2 ) );

            // Assert state

            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, observableList );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IListEX_Move()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<double> observableList = new ObservableList<double>( new[] { 5.1, 8.0, 2.9, 44.5 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = ( (IListEx) observableList ).Move( 44.5, 0 );

            // Assert state & result

            Assert.True( result );
            Assert.Equal( new[] { 44.5, 5.1, 8.0, 2.9 }, observableList );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { 44.5 }, events[ 0 ].OldItems );
            Assert.Equal( new[] { 44.5 }, events[ 0 ].NewItems );
            Assert.Equal( 3, events[ 0 ].OldStartingIndex );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void IListEx_Move_InvalidObject()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<double> observableList = new ObservableList<double>( new[] { 5.1, 8.0, 2.9, 44.5 } );

            observableList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableList, obj );
                events.Add( args );
            };

            // Act

            var result = ( (IListEx) observableList ).Move( 5.1f, 0 );

            // Assert state & result

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9, 44.5 }, observableList );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IList_Clear()
        {
            // Arrange

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

            // Act

            list.Clear();

            // Assert state

            Assert.Empty( list );

            // Assert events

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
            // Arrange

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

            // Act & Assert

            Assert.True( list.Contains( item2 ) );
            Assert.False( list.Contains( item3 ) );
            Assert.False( list.Contains( item4 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void GetRange()
        {
            // Arrange

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

            // Act & Assert

            Assert.Equal( new[] { item3, item2 }, observableList.GetRange( 1, 2 ) );
            Assert.Equal( new[] { item2 }, observableList.GetRange( 0, 1 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IObservableList_GetRange()
        {
            // Arrange

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

            // Act & Assert

            Assert.Equal( new[] { item3, item2 }, observableList.GetRange( 1, 2 ) );
            Assert.Equal( new[] { item2 }, observableList.GetRange( 0, 1 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlyListEx_GetRange()
        {
            // Arrange

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

            // Act & Assert

            Assert.Equal( new[] { item3, item2 }, readOnlyList.GetRange( 1, 2 ) );
            Assert.Equal( new[] { item2 }, readOnlyList.GetRange( 0, 1 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IListExT_GetRange()
        {
            // Arrange

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

            // Act & Assert

            Assert.Equal( new[] { item3, item2 }, list.GetRange( 1, 2 ) );
            Assert.Equal( new[] { item2 }, list.GetRange( 0, 1 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IListEx_GetRange()
        {
            // Arrange

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

            IListEx list = observableList;

            // Act & Assert

            Assert.Equal( new[] { item3, item2 }, (IList) list.GetRange( 1, 2 ) );
            Assert.Equal( new[] { item2 }, (IList) list.GetRange( 0, 1 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void Slice()
        {
            // Arrange

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

            // Act & Assert

            Assert.Equal( new[] { item3, item2 }, observableList.Slice( 1, 2 ) );
            Assert.Equal( new[] { item2 }, observableList.Slice( 0, 1 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IObservableList_Slice()
        {
            // Arrange

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

            // Act & Assert

            Assert.Equal( new[] { item3, item2 }, observableList.Slice( 1, 2 ) );
            Assert.Equal( new[] { item2 }, observableList.Slice( 0, 1 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlyListEx_Slice()
        {
            // Arrange

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

            // Act & Assert

            Assert.Equal( new[] { item3, item2 }, readOnlyList.Slice( 1, 2 ) );
            Assert.Equal( new[] { item2 }, readOnlyList.Slice( 0, 1 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IObservableReadOnlyList_Slice()
        {
            // Arrange

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

            // Act & Assert

            Assert.Equal( new[] { item3, item2 }, readOnlyList.Slice( 1, 2 ) );
            Assert.Equal( new[] { item2 }, readOnlyList.Slice( 0, 1 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IListExT_Slice()
        {
            // Arrange

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

            // Act & Assert

            Assert.Equal( new[] { item3, item2 }, list.Slice( 1, 2 ) );
            Assert.Equal( new[] { item2 }, list.Slice( 0, 1 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IListEx_Slice()
        {
            // Arrange

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

            IListEx list = observableList;

            // Act & Assert

            Assert.Equal( new[] { item3, item2 }, (IList) list.Slice( 1, 2 ) );
            Assert.Equal( new[] { item2 }, (IList) list.Slice( 0, 1 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IndexOf()
        {
            // Arrange

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

            // Act & Assert

            Assert.Equal( -1, observableList.IndexOf( item1 ) );
            Assert.Equal( 0, observableList.IndexOf( item2 ) );
            Assert.Equal( 2, observableList.IndexOf( item2, 1 ) );
            Assert.Equal( -1, observableList.IndexOf( item2, 1, 1 ) );
            Assert.Equal( 1, observableList.IndexOf( item3 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IList_IndexOf()
        {
            // Arrange

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

            // Act & Assert

            Assert.Equal( -1, list.IndexOf( item1 ) );
            Assert.Equal( 0, list.IndexOf( item2 ) );
            Assert.Equal( 1, list.IndexOf( item3 ) );
            Assert.Equal( -1, list.IndexOf( item4 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlyListEx_IndexOf()
        {
            // Arrange

            var list = new ObservableList<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            // Act & Assert

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
        public void IListEx_IndexOf()
        {
            // Arrange

            var list = new ObservableList<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IListEx) list );

            // Act & Assert

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
            // Arrange

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

            // Act & Assert

            Assert.Equal( -1, observableList.LastIndexOf( item1 ) );
            Assert.Equal( 2, observableList.LastIndexOf( item2 ) );
            Assert.Equal( 0, observableList.LastIndexOf( item2, 1 ) );
            Assert.Equal( -1, observableList.LastIndexOf( item2, 1, 1 ) );
            Assert.Equal( 1, observableList.LastIndexOf( item3 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlyListEx_LastIndexOf()
        {
            // Arrange

            var list = new ObservableList<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IReadOnlyListEx<int>) list );

            // Act & Assert

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
        public void IListEx_LastIndexOf()
        {
            // Arrange

            var list = new ObservableList<int>( new[] { 3, 6, 4, 3 } );

            var collection = ( (IListEx) list );

            // Act & Assert

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
            // Arrange

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

            // Act & Assert

            Assert.Equal( item2, observableList[ 0 ] );
            Assert.Equal( item3, observableList[ 1 ] );
            Assert.Equal( item1, observableList[ 2 ] );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void Indexer_Get_InvalidIndex()
        {
            // Arrange

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

            // Act & Assert

            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -654 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -1 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -3 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ 34564 ] );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void Indexer_Set_ValidIndex()
        {
            // Arrange

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

            // Act

            observableList[ 1 ] = item1;

            // Assert state

            Assert.Equal( new[] { item2, item1 }, observableList );

            // Assert events

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
            // Arrange

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

            // Act & Assert

            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -765 ] = item2 );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ -1 ] = item2 );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ 3 ] = item2 );
            Assert.Throws<ArgumentOutOfRangeException>( () => observableList[ 346 ] = item2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Indexer_Get()
        {
            // Arrange

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

            // Act & Assert

            Assert.Equal( item2, list[ 0 ] );
            Assert.Equal( item3, list[ 1 ] );
            Assert.Equal( item1, list[ 2 ] );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IList_Indexer_Set()
        {
            // Arrange

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

            // Act

            list[ 1 ] = item1;

            // Assert state

            Assert.Equal( new[] { item2, item1 }, list );

            // Assert events

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
            // Arrange

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

            // Act & Assert

            Assert.Throws<InvalidCastException>( () => list[ 1 ] = item1 );

            // Assert state

            Assert.Equal( new[] { item2, item3 }, list );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void NoEventListeners()
        {
            // Arrange

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item1 } );

            Assert.Equal( new[] { item1 }, collection );

            // Act

            collection.Add( item2 );

            // Assert

            Assert.Equal( new[] { item1, item2 }, collection );

            // Act

            collection.Insert( 0, item3 );

            // Assert

            Assert.Equal( new[] { item3, item1, item2 }, collection );

            // Act

            collection.Remove( item2 );

            // Assert

            Assert.Equal( new[] { item3, item1 }, collection );

            // Act

            collection.RemoveAt( 0 );

            // Assert

            Assert.Equal( new[] { item1 }, collection );
        }
    }
}
