/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Utilities.DotNet.Collections.Observables;
using Xunit;

#pragma warning disable xUnit2013 // Do not use equality check to check for testedCollection size.

namespace Utilities.DotNet.Test.Collections.Observables
{
    public class ObservableReadOnlyCollectionTest
    {
        [Fact]
        public void Constructor_Default()
        {
            IObservableCollection<int> baseCollection = new ObservableCollection<int>();

            IObservableReadOnlyCollection<int> testedCollection = new ObservableReadOnlyCollection<int>( baseCollection );

            Assert.Equal( 0, testedCollection.Count );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            IObservableCollection<int> baseCollection = new ObservableCollection<int>( new[] { 1, 3, 2, 4 } );

            IObservableReadOnlyCollection<int> testedCollection = new ObservableReadOnlyCollection<int>( baseCollection );

            Assert.Equal( new[] { 1, 3, 2, 4 }, testedCollection );
            Assert.Equal( 4, testedCollection.Count );
        }

        [Fact]
        public void AddToObserverdList()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var baseCollection = new ObservableCollection<TestClass>( new[] { item1 } );

            IObservableReadOnlyCollection<TestClass> testedCollection = new ObservableReadOnlyCollection<TestClass>( baseCollection );

            testedCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1 }, testedCollection );

            baseCollection.Add( item2 );

            Assert.Equal( new[] { item1, item2 }, testedCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            baseCollection.Add( item3 );

            Assert.Equal( new[] { item1, item2, item3 }, testedCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 2, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void RemoveFromObservedList()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var baseCollection = new ObservableCollection<TestClass>( new[] { item2, item1, item3 } );

            IObservableReadOnlyCollection<TestClass> testedCollection = new ObservableReadOnlyCollection<TestClass>( baseCollection );

            testedCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, testedCollection );

            Assert.True( baseCollection.Remove( item1 ) );

            Assert.Equal( new[] { item2, item3 }, testedCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            events.Clear();

            Assert.False( baseCollection.Remove( item1 ) );

            Assert.Equal( new[] { item2, item3 }, testedCollection );
            Assert.Empty( events );
        }

        [Fact]
        public void ClearObservedCollection()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var baseCollection = new ObservableCollection<TestClass>( new[] { item2, item1, item3 } );

            IObservableReadOnlyCollection<TestClass> testedCollection = new ObservableReadOnlyCollection<TestClass>( baseCollection );

            testedCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, testedCollection );

            baseCollection.Clear();

            Assert.Empty( testedCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( new[] { item2, item1, item3 }, events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );

            events.Clear();

            baseCollection.Clear();

            Assert.Empty( events );
        }

        [Fact]
        public void Contains()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<int> baseCollection = new ObservableCollection<int>( new[] { 1, 3, 2, 4 } );

            IObservableReadOnlyCollection<int> testedCollection = new ObservableReadOnlyCollection<int>( baseCollection );

            testedCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedCollection, obj );
                events.Add( args );
            };

            Assert.True( testedCollection.Contains( 1 ) );
            Assert.False( testedCollection.Contains( 5 ) );

            Assert.Empty( events );
        }

        [Fact]
        public void GetEnumerator()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<int> baseCollection = new ObservableCollection<int>( new[] { 6, 9 } );

            IObservableReadOnlyCollection<int> testedCollection = new ObservableReadOnlyCollection<int>( baseCollection );

            testedCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedCollection, obj );
                events.Add( args );
            };

            var enumerator = testedCollection.GetEnumerator();

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

            var baseCollection = new ObservableCollection<int>( new[] { 6, 9 } );

            IObservableReadOnlyCollection<int> testedCollection = new ObservableReadOnlyCollection<int>( baseCollection );

            testedCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedCollection, obj );
                events.Add( args );
            };

            IEnumerable enumerable = testedCollection;

            var enumerator = enumerable.GetEnumerator();

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 6, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 9, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            Assert.Empty( events );
        }

        [Fact]
        public void NoEventListeners()
        {
            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> baseCollection = new ObservableCollection<TestClass>( new[] { item1 } );

            IObservableReadOnlyCollection<TestClass> testedCollection = new ObservableReadOnlyCollection<TestClass>( baseCollection );

            Assert.Equal( new[] { item1 }, testedCollection );

            baseCollection.Add( item2 );

            Assert.Equal( new[] { item1, item2 }, testedCollection );

            baseCollection.Remove( item2 );

            Assert.Equal( new[] { item1 }, testedCollection );
        }
    }
}
