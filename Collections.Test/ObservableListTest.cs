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
#pragma warning disable xUnit2017 // Do not use Contains() to check if a value exists in a collection

namespace Utilities.DotNet.Test.Collections
{
    public class ObservableListTest
    {
        [Fact]
        public void Constructor_Default()
        {
            IObservableList<TestClass> collection = new ObservableList<TestClass>();

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

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            Assert.Equal( new[] { item1, item2, item3 }, collection );
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

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1 }, collection );

            collection.Add( item2 );

            Assert.Equal( new[] { item1, item2 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
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

            var collection = new ObservableList<TestClass>( new[] { item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

            Assert.Equal( new[] { item1 }, collection );

            Assert.Equal( 1, list.Add( item2 ) );

            Assert.Equal( new[] { item1, item2 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[0].Action );
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

            var collection = new ObservableList<TestClass>( new[] { item2 } );

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
        public void Insert()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );
            var item4 = new TestClass( "Item4", -12 );

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1 }, collection );

            collection.Insert( 0, item2 );

            Assert.Equal( new[] { item2, item1 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            collection.Insert( 1, item3 );

            Assert.Equal( new[] { item2, item3, item1 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            collection.Insert( 3, item4 );

            Assert.Equal( new[] { item2, item3, item1, item4 }, collection );
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

            var collection = new ObservableList<TestClass>( new[] { item2, item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1 }, collection );

            Assert.Throws<ArgumentOutOfRangeException>( () => collection.Insert( -4657, item3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => collection.Insert( -1, item3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => collection.Insert( 3, item3 ) );
            Assert.Throws<ArgumentOutOfRangeException>( () => collection.Insert( 1245, item3 ) );

            Assert.Equal( new[] { item2, item1 }, collection );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Insert()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );

            var collection = new ObservableList<TestClass>( new[] { item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

            Assert.Equal( new[] { item1 }, collection );

            list.Insert( 0, item2 );

            Assert.Equal( new[] { item2, item1 }, collection );
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

            var collection = new ObservableList<TestClass>( new[] { item2 } );

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
        public void Remove()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, collection );

            Assert.True( collection.Remove( item1 ) );

            Assert.Equal( new[] { item2, item3 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            events.Clear();

            Assert.False( collection.Remove( item1 ) );

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

            var collection = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

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
        }

        [Fact]
        public void IList_Remove_InvalidType()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new List<int>();
            var item2 = new TestClass( "Item2", 1 );

            var collection = new ObservableList<TestClass>( new[] { item2 } );

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

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

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

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

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
        public void Clear()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, collection );

            collection.Clear();

            Assert.Empty( collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Reset, events[ 0 ].Action );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            Assert.False( collection.Remove( item1 ) );

            Assert.Empty( events );
        }

        [Fact]
        public void Clear_Empty()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableList<TestClass> collection = new ObservableList<TestClass>();

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Empty( collection );

            collection.Clear();

            Assert.Empty( collection );
            Assert.Empty( events );
        }

        [Fact]
        public void IList_Clear()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var collection = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

            Assert.Equal( new[] { item1, item2, item3 }, collection );

            list.Clear();

            Assert.Empty( collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Reset, events[ 0 ].Action );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void Contains()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item1, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.True( collection.Contains( item1 ) );
            Assert.False( collection.Contains( item2 ) );

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

            var collection = new ObservableList<TestClass>( new[] { item1, item2 } );

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
        public void IndexOf()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );
            var item4 = new List<int>();

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( -1, collection.IndexOf( item1 ) );
            Assert.Equal( 0, collection.IndexOf( item2 ) );
            Assert.Equal( 1, collection.IndexOf( item3 ) );
            Assert.Equal( -1, collection.IndexOf( item4 ) );
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

            var collection = new ObservableList<TestClass>( new[] { item2, item3 } );

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
        public void Indexer_Get_ValidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item2, item3, item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( item2, collection[ 0 ] );
            Assert.Equal( item3, collection[ 1 ] );
            Assert.Equal( item1, collection[ 2 ] );
            Assert.Empty( events );
        }

        [Fact]
        public void Indexer_Get_InvalidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item2, item3, item1 } );

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

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item3 }, collection );

            collection[ 1 ] = item1;

            Assert.Equal( new[] { item2, item1 }, collection );
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

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item3, item1 } );

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

            var collection = new ObservableList<TestClass>( new[] { item2, item3, item1 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

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

            var collection = new ObservableList<TestClass>( new[] { item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IList list = collection;

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

            var collection = new ObservableList<TestClass>( new[] { item2, item3 } );

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
        public void GetEnumerator()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item1, item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            var enumerator = collection.GetEnumerator();

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( item1, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( item2, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            Assert.Empty( events );
        }

        [Fact]
        public void IEnumerable_GetEnumerator()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );

            var collection = new ObservableList<TestClass>( new[] { item1, item2 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IEnumerable enumerable = collection;

            var enumerator = enumerable.GetEnumerator();

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( item1, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( item2, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            Assert.Empty( events );
        }

        [Fact]
        public void CopyTo()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> collection = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            var array = new TestClass[ 3 ];

            collection.CopyTo( array, 0 );

            Assert.Equal( new[] { item1, item2, item3 }, array );

            Assert.Empty( events );
        }

        [Fact]
        public void ICollection_CopyTo()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var collection = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            var array = new TestClass[ 3 ];

            ( (ICollection) collection ).CopyTo( array, 0 );

            Assert.Equal( new[] { item1, item2, item3 }, array );
            
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
