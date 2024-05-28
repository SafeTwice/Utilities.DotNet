/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Utilities.DotNet.Collections.Observables;
using Xunit;

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.

namespace Utilities.DotNet.Test.Collections.Observables
{
    public class ObservableCollectionTest
    {
        [Fact]
        public void Constructor_Default()
        {
            var observableCollection = new ObservableCollection<TestClass>();

            Assert.Equal( 0, observableCollection.Count );
            Assert.False( ( (IObservableCollection<TestClass>) observableCollection ).IsReadOnly );
            Assert.NotNull( ( (IObservableCollection<TestClass>) observableCollection ).SyncRoot );
            Assert.False( ( (IObservableCollection<TestClass>) observableCollection ).IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            var observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2, item3 } );

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );
            Assert.Equal( 3, observableCollection.Count );
            Assert.False( ( (IObservableCollection<TestClass>) observableCollection ).IsReadOnly );
            Assert.NotNull( ( (IObservableCollection<TestClass>) observableCollection ).SyncRoot );
            Assert.False( ( (IObservableCollection<TestClass>) observableCollection ).IsSynchronized );
        }

        [Fact]
        public void Add()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item1 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1 }, observableCollection );

            observableCollection.Add( item2 );

            Assert.Equal( new[] { item1, item2 }, observableCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            observableCollection.Add( item3 );

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 2, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void AddRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item2 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2 }, observableCollection );

            observableCollection.AddRange( new[] { item1, item3 } );

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item1, item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void Remove()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2, item3 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );

            Assert.True( observableCollection.Remove( item1 ) );

            Assert.Equal( new[] { item2, item3 }, observableCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            events.Clear();

            Assert.False( observableCollection.Remove( item1 ) );

            Assert.Equal( new[] { item2, item3 }, observableCollection );
            Assert.Empty( events );
        }

        [Fact]
        public void RemoveRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2, item3 } );

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

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2, item3 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );

            observableCollection.Clear();

            Assert.Empty( observableCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Reset, events[ 0 ].Action );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            observableCollection.Clear();

            Assert.Empty( events );
        }

        [Fact]
        public void Clear_Empty()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>();

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Empty( observableCollection );

            observableCollection.Clear();

            Assert.Empty( observableCollection );
            Assert.Empty( events );
        }

        [Fact]
        public void Contains()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item1, item3 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.True( observableCollection.Contains( item1 ) );
            Assert.False( observableCollection.Contains( item2 ) );

            Assert.Empty( events );
        }

        [Fact]
        public void GetEnumerator()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            var enumerator = observableCollection.GetEnumerator();

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

            var observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            IEnumerable enumerable = observableCollection;

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

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2, item3 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            var array = new TestClass[ 3 ];

            observableCollection.CopyTo( array, 0 );

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

            var observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2, item3 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            ICollection collection = observableCollection;

            var array = new TestClass[ 3 ];

            collection.CopyTo( array, 0 );

            Assert.Equal( new[] { item1, item2, item3 }, array );

            Assert.Empty( events );
        }

        [Fact]
        public void NoEventListeners()
        {
            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item1 } );

            Assert.Equal( new[] { item1 }, observableCollection );

            observableCollection.Add( item2 );

            Assert.Equal( new[] { item1, item2 }, observableCollection );

            observableCollection.Remove( item2 );

            Assert.Equal( new[] { item1 }, observableCollection );
        }
    }
}
