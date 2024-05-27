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
    public class ObservableSortedCollectionTest
    {
        [Fact]
        public void Constructor_Default()
        {
            IObservableCollection<int> collection = new ObservableSortedCollection<int>();

            Assert.Equal( 0, collection.Count );
            Assert.False( collection.IsReadOnly );
            Assert.NotNull( collection.SyncRoot );
            Assert.False( collection.IsSynchronized );
        }

        [Fact]
        public void Constructor_Comparer()
        {
            IObservableCollection<int> collection = new ObservableSortedCollection<int>( Comparer<int>.Default );

            Assert.Equal( 0, collection.Count );
            Assert.False( collection.IsReadOnly );
            Assert.NotNull( collection.SyncRoot );
            Assert.False( collection.IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            IObservableCollection<int> collection = new ObservableSortedCollection<int>( new[] { 1, 3, 2, 4 } );

            Assert.Equal( new[] { 1, 2, 3, 4 }, collection );
            Assert.False( collection.IsReadOnly );
            Assert.NotNull( collection.SyncRoot );
            Assert.False( collection.IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationListAndComparer()
        {
            IObservableCollection<int> collection = new ObservableSortedCollection<int>( new[] { 5, 3, 4, 1 },
                Comparer<int>.Create( (x, y) => Comparer<int>.Default.Compare( y, x ) ) );

            Assert.Equal( new[] { 5, 4, 3, 1 }, collection );
            Assert.False( collection.IsReadOnly );
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

            IObservableCollection<TestClass> collection = new ObservableSortedCollection<TestClass>( new[] { item1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1 }, collection );

            collection.Add( item2 );

            Assert.Equal( new[] { item2, item1 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[0].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            collection.Add( item3 );

            Assert.Equal( new[] { item2, item1, item3 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 2, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void Remove()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> collection = new ObservableSortedCollection<TestClass>( new[] { item1, item2, item3 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, collection );

            Assert.True( collection.Remove( item1 ) );

            Assert.Equal( new[] { item2, item3 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            events.Clear();

            Assert.False( collection.Remove( item1 ) );

            Assert.Equal( new[] { item2, item3 }, collection );
            Assert.Empty( events );
        }

        [Fact]
        public void RemoveRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { item3, item2, item1 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );

            observableCollection.RemoveRange( new[] { item2, item1 } );

            Assert.Equal( new[] { item3 }, observableCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item2, item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void Clear()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> collection = new ObservableSortedCollection<TestClass>( new[] { item1, item2, item3 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, collection );

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

            IObservableCollection<TestClass> collection = new ObservableSortedCollection<TestClass>();

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
        public void AutoReorder_FirstElement()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> collection = new ObservableSortedCollection<TestClass>( new[] { item3, item2, item1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, collection );

            // No reorder necessary

            item2.Value = 9;

            Assert.Equal( new[] { item2, item1, item3 }, collection );
            Assert.Empty( events );

            // No reorder necessary

            item2.Value = 10;

            Assert.Equal( new[] { item2, item1, item3 }, collection );
            Assert.Empty( events );

            // Reorder necessary

            item2.Value = 11;

            Assert.Equal( new[] { item1, item2, item3 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Equal( new[] { item2 }, events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void AutoReorder_MiddleElement()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> collection = new ObservableSortedCollection<TestClass>( new[] { item3, item2, item1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, collection );

            // No reorder necessary

            item1.Value = 6;

            Assert.Equal( new[] { item2, item1, item3 }, collection );
            Assert.Empty( events );

            // No reorder necessary

            item1.Value = 5;

            Assert.Equal( new[] { item2, item1, item3 }, collection );
            Assert.Empty( events );

            // Reorder necessary

            item1.Value = 4;

            Assert.Equal( new[] { item1, item2, item3 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].NewItems );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );

            events.Clear();

            // No reorder necessary

            item2.Value = 19;

            Assert.Equal( new[] { item1, item2, item3 }, collection );
            Assert.Empty( events );

            // No reorder necessary

            item2.Value = 20;

            Assert.Equal( new[] { item1, item2, item3 }, collection );
            Assert.Empty( events );

            // Reorder necessary

            item2.Value = 21;

            Assert.Equal( new[] { item1, item3, item2 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Equal( new[] { item2 }, events[ 0 ].OldItems );
            Assert.Equal( 2, events[ 0 ].NewStartingIndex );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void AutoReorder_LastElement()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> collection = new ObservableSortedCollection<TestClass>( new[] { item3, item2, item1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, collection );

            // No reorder necessary

            item3.Value = 11;

            Assert.Equal( new[] { item2, item1, item3 }, collection );
            Assert.Empty( events );

            // No reorder necessary

            item3.Value = 10;

            Assert.Equal( new[] { item2, item1, item3 }, collection );
            Assert.Empty( events );

            // Reorder necessary

            item3.Value = 9;

            Assert.Equal( new[] { item2, item3, item1 }, collection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].NewItems );
            Assert.Equal( new[] { item3 }, events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( 2, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void UpdateSortOrder_NotExisting()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            var collection = new ObservableSortedCollection<TestClass>( new[] { item3, item1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item3 }, collection );

            Assert.Throws<ArgumentException>( () => collection.UpdateSortOrder( item2 ) );

            Assert.Empty( events );
        }

        [Fact]
        public void Contains()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<int> collection = new ObservableSortedCollection<int>( new[] { 1, 3, 2, 4 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            Assert.True( collection.Contains( 1 ) );
            Assert.False( collection.Contains( 5 ) );

            Assert.Empty( events );
        }

        [Fact]
        public void GetEnumerator()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<int> collection = new ObservableSortedCollection<int>( new[] { 6, 9 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            var enumerator = collection.GetEnumerator();

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 6, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 9, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            Assert.Empty( events );
        }

        [Fact]
        public void IEnumerable_GetEnumerator()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ObservableSortedCollection<int>( new[] { 6, 9 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            IEnumerable enumerable = collection;

            var enumerator = enumerable.GetEnumerator();

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 6, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 9, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            Assert.Empty( events );
        }

        [Fact]
        public void CopyTo()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<int> collection = new ObservableSortedCollection<int>( new[] { 9, 3, 7, 4 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            var array = new int[ 4 ];

            collection.CopyTo( array, 0 );

            Assert.Equal( new[] { 3, 4, 7, 9 }, array );

            Assert.Empty( events );
        }

        [Fact]
        public void ICollection_CopyTo()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var collection = new ObservableSortedCollection<int>( new[] { 9, 3, 7, 4 } );

            collection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( collection, obj );
                events.Add( args );
            };

            var array = new int[ 4 ];

            ( (ICollection) collection ).CopyTo( array, 0 );

            Assert.Equal( new[] { 3, 4, 7, 9 }, array );
            
            Assert.Empty( events );
        }
    }
}
