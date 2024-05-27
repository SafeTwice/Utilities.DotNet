/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Utilities.DotNet.Collections;
using Xunit;

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.

namespace Utilities.DotNet.Test.Collections
{
    public class SortedObservableListTest
    {
        [Fact]
        public void Constructor_Default()
        {
            IObservableList<TestClass> collection = new SortedObservableList<TestClass>();

            Assert.Equal( 0, collection.Count );
            Assert.False( collection.IsReadOnly );
            Assert.False( collection.IsFixedSize );
            Assert.NotNull( collection.SyncRoot );
            Assert.False( collection.IsSynchronized );
        }

        [Fact]
        public void Constructor_Comparer()
        {
            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( Comparer<TestClass>.Default );

            Assert.Equal( 0, collection.Count );
            Assert.False( collection.IsReadOnly );
            Assert.False( collection.IsFixedSize );
            Assert.NotNull( collection.SyncRoot );
            Assert.False( collection.IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item3, item1, item2 } );

            Assert.Equal( new[] { item1, item2, item3 }, collection );
            Assert.False( collection.IsReadOnly );
            Assert.False( collection.IsFixedSize );
            Assert.NotNull( collection.SyncRoot );
            Assert.False( collection.IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationListAndComparer()
        {
            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item3, item1, item2 },
                Comparer<TestClass>.Create( (x, y) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            Assert.Equal( new[] { item2, item1, item3 }, collection );
            Assert.False( collection.IsReadOnly );
            Assert.False( collection.IsFixedSize );
            Assert.NotNull( collection.SyncRoot );
            Assert.False( collection.IsSynchronized );
        }

        [Fact]
        public void Add()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2 }, collection );

            collection.Add( item1 );

            Assert.Equal( new[] { item1, item2 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            collection.Add( item3 );

            Assert.Equal( new[] { item1, item2, item3 }, collection );
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

            var collection = new SortedObservableList<TestClass>( new[] { item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

            Assert.Equal( new[] { item2 }, collection );

            Assert.Equal( 0, list.Add( item1 ) );

            Assert.Equal( new[] { item1, item2 }, collection );
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

            var collection = new SortedObservableList<TestClass>( new[] { item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

            Assert.Equal( new[] { item2 }, collection );

            Assert.Equal( -1, list.Add( item1 ) );

            Assert.Equal( new[] { item2 }, collection );
            Assert.Empty( events );
        }

        [Fact]
        public void AddRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2 }, collection );

            collection.AddRange( new[] { item3, item1 } );

            Assert.Equal( new[] { item1, item2, item3 }, collection );
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

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2 }, collection );

            collection.Insert( 1, item1 );

            Assert.Equal( new[] { item1, item2 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            collection.Insert( 0, item3 );

            Assert.Equal( new[] { item1, item2, item3 }, collection );
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

            var collection = new SortedObservableList<TestClass>( new[] { item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

            Assert.Equal( new[] { item2 }, collection );

            list.Insert( 55, item1 );

            Assert.Equal( new[] { item1, item2 }, collection );
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

            var collection = new SortedObservableList<TestClass>( new[] { item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

            Assert.Equal( new[] { item2 }, collection );

            Assert.Throws<InvalidCastException>( () => list.Insert( 0, item1 ) );

            Assert.Equal( new[] { item2 }, collection );
            Assert.Empty( events );
        }

        [Fact]
        public void InsertRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var collection = new SortedObservableList<TestClass>( new[] { item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2 }, collection );

            collection.InsertRange( 0, new[] { item3, item1 } );

            Assert.Equal( new[] { item1, item2, item3 }, collection );
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

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item1, item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, collection );

            collection.Remove( item1 );

            Assert.Equal( new[] { item2, item3 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            events.Clear();

            collection.Remove( item1 );

            Assert.Equal( new[] { item2, item3 }, collection );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Remove()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var collection = new SortedObservableList<TestClass>( new[] { item1, item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

            Assert.Equal( new[] { item1, item2, item3 }, collection );

            list.Remove( item1 );

            Assert.Equal( new[] { item2, item3 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            events.Clear();

            list.Remove( item1 );

            Assert.Equal( new[] { item2, item3 }, collection );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Remove_InvalidType()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new List<int>();
            var item2 = new TestClass( "Item2", 1 );

            var collection = new SortedObservableList<TestClass>( new[] { item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

            Assert.Equal( new[] { item2 }, collection );

            list.Remove( item1 );

            Assert.Equal( new[] { item2 }, collection );
            Assert.Empty( events );
        }

        [Fact]
        public void RemoveAt()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item2, item3, item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, collection );

            collection.RemoveAt( 1 );

            Assert.Equal( new[] { item1, item3 }, collection );
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

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item2, item3, item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, collection );

            Assert.Throws<ArgumentOutOfRangeException>( () => collection.RemoveAt( -129 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => collection.RemoveAt( -1 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => collection.RemoveAt( 3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => collection.RemoveAt( 15 ) );

            Assert.Equal( new[] { item1, item2, item3 }, collection );
            Assert.Empty( events );
        }

        [Fact]
        public void RemoveRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var collection = new SortedObservableList<TestClass>( new[] { item2, item1, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, collection );

            collection.RemoveRange( 0, 2 );

            Assert.Equal( new[] { item3 }, collection );
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

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item1, item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.True( collection.Contains( item2 ) );
            Assert.False( collection.Contains( item3 ) );
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

            var collection = new SortedObservableList<TestClass>( new[] { item1, item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

            Assert.True( list.Contains( item2 ) );
            Assert.False( list.Contains( item3 ) );
            Assert.False( list.Contains( item4 ) );
            Assert.Empty( events );
        }

        [Fact]
        public void IListEx_GetRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item2, item3, item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            var range1 = collection.GetRange( 1, 2 );

            Assert.Equal( new[] { item2, item3 }, range1 );

            var range2 = collection.GetRange( 0, 1 );

            Assert.Equal( new[] { item2 }, range2 );
        }

        [Fact]
        public void Slice()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var collection = new SortedObservableList<TestClass>( new[] { item2, item3, item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            var range1 = collection.Slice( 0, 2 );

            Assert.Equal( new[] { item1, item2 }, range1 );

            var range2 = collection.Slice( 2, 1 );

            Assert.Equal( new[] { item3 }, range2 );
        }

        [Fact]
        public void IListEx_Slice()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item1, item3, item1, item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            var range1 = collection.Slice( 1, 2 );

            Assert.Equal( new[] { item1, item2 }, range1 );

            var range2 = collection.Slice( 0, 2 );

            Assert.Equal( new[] { item1, item1 }, range2 );
        }

        [Fact]
        public void IndexOf()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item3, item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( -1, collection.IndexOf( item1 ) );
            Assert.Equal( 0, collection.IndexOf( item2 ) );
            Assert.Equal( -1, collection.IndexOf( item2, 1 ) );
            Assert.Equal( 1, collection.IndexOf( item3 ) );
            Assert.Equal( 2, collection.IndexOf( item3, 2 ) );
            Assert.Equal( -1, collection.IndexOf( item3, 0, 1 ) );
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

            var collection = new SortedObservableList<TestClass>( new[] { item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

            Assert.Equal( -1, list.IndexOf( item1 ) );
            Assert.Equal( 0, list.IndexOf( item2 ) );
            Assert.Equal( 1, list.IndexOf( item3 ) );
            Assert.Equal( -1, list.IndexOf( item4 ) );
            Assert.Empty( events );
        }

        [Fact]
        public void LastIndexOf()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item3, item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( -1, collection.LastIndexOf( item1 ) );
            Assert.Equal( 0, collection.LastIndexOf( item2 ) );
            Assert.Equal( -1, collection.LastIndexOf( item2, 2, 2 ) );
            Assert.Equal( 2, collection.LastIndexOf( item3 ) );
            Assert.Equal( 1, collection.LastIndexOf( item3, 1 ) );
            Assert.Equal( -1, collection.LastIndexOf( item3, 0, 1 ) );
            Assert.Empty( events );
        }

        [Fact]
        public void Indexer_Get_ValidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item2, item3, item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( item1, collection[ 0 ] );
            Assert.Equal( item2, collection[ 1 ] );
            Assert.Equal( item3, collection[ 2 ] );
            Assert.Empty( events );
        }

        [Fact]
        public void Indexer_Get_InvalidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item2, item3, item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Throws<ArgumentOutOfRangeException>( () => collection[ -654 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => collection[ -1 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => collection[ -3 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => collection[ 34564 ] );
            Assert.Empty( events );
        }

        [Fact]
        public void Indexer_Set_ValidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item3 }, collection );

            collection[ 1 ] = item1;

            Assert.Equal( new[] { item1, item2 }, collection );
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

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item3, item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Throws<ArgumentOutOfRangeException>( () => collection[ -765 ] = item2 );
            Assert.Throws<ArgumentOutOfRangeException>( () => collection[ -1 ] = item2 );
            Assert.Throws<ArgumentOutOfRangeException>( () => collection[ 3 ] = item2 );
            Assert.Throws<ArgumentOutOfRangeException>( () => collection[ 346 ] = item2 );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Indexer_Get()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var collection = new SortedObservableList<TestClass>( new[] { item2, item3, item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

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

            var collection = new SortedObservableList<TestClass>( new[] { item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

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

            var collection = new SortedObservableList<TestClass>( new[] { item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

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

            IObservableList<TestClass> collection = new SortedObservableList<TestClass>( new[] { item2 } );

            Assert.Equal( new[] { item2 }, collection );

            collection.Add( item1 );

            Assert.Equal( new[] { item1, item2 }, collection );

            collection.Insert( 0, item3 );

            Assert.Equal( new[] { item1, item2, item3 }, collection );

            collection.Remove( item2 );

            Assert.Equal( new[] { item1, item3 }, collection );

            collection.RemoveAt( 0 );

            Assert.Equal( new[] { item3 }, collection );
        }
    }
}
