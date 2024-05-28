/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Utilities.DotNet.Collections;
using Utilities.DotNet.Collections.Observables;
using Xunit;

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.

namespace Utilities.DotNet.Test.Collections.Observables
{
    public class ObservableReadOnlyListTest
    {
        [Fact]
        public void Constructor_Default()
        {
            IObservableList<TestClass> baseList = new ObservableList<TestClass>();

            IObservableReadOnlyList<TestClass> testedList = new ObservableReadOnlyList<TestClass>( baseList );

            Assert.Equal( 0, testedList.Count );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> baseList = new ObservableList<TestClass>( new[] { item1, item2, item3 } );

            IObservableReadOnlyList<TestClass> testedList = new ObservableReadOnlyList<TestClass>( baseList );

            Assert.Equal( new[] { item1, item2, item3 }, testedList );
            Assert.Equal( 3, testedList.Count );
        }

        [Fact]
        public void GetRange()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var baseList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            IObservableReadOnlyList<TestClass> testedList = new ObservableReadOnlyList<TestClass>( baseList );

            testedList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedList, obj );
                events.Add( args );
            };

            var range1 = testedList.GetRange( 1, 2 );

            Assert.Equal( new[] { item3, item2 }, range1 );
            Assert.Empty( events );

            var range2 = testedList.GetRange( 0, 1 );

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

            var baseList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            IObservableReadOnlyList<TestClass> testedList = new ObservableReadOnlyList<TestClass>( baseList );

            testedList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedList, obj );
                events.Add( args );
            };

            IReadOnlyListEx<TestClass> readOnlyList = testedList;

            var range1 = readOnlyList.GetRange( 1, 2 );

            Assert.Equal( new[] { item3, item2 }, range1 );
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

            var baseList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            IObservableReadOnlyList<TestClass> testedList = new ObservableReadOnlyList<TestClass>( baseList );

            testedList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedList, obj );
                events.Add( args );
            };

            var range1 = testedList.Slice( 1, 2 );

            Assert.Equal( new[] { item3, item2 }, range1 );
            Assert.Empty( events );

            var range2 = testedList.Slice( 0, 1 );

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

            var baseList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            IObservableReadOnlyList<TestClass> testedList = new ObservableReadOnlyList<TestClass>( baseList );

            testedList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedList, obj );
                events.Add( args );
            };

            IReadOnlyListEx<TestClass> readOnlyList = testedList;

            var range1 = readOnlyList.Slice( 1, 2 );

            Assert.Equal( new[] { item3, item2 }, range1 );
            Assert.Empty( events );

            var range2 = readOnlyList.Slice( 0, 1 );

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

            IObservableList<TestClass> baseList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            IObservableReadOnlyList<TestClass> testedList = new ObservableReadOnlyList<TestClass>( baseList );

            testedList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedList, obj );
                events.Add( args );
            };

            Assert.Equal( -1, testedList.IndexOf( item1 ) );
            Assert.Equal( 0, testedList.IndexOf( item2 ) );
            Assert.Equal( 2, testedList.IndexOf( item2, 1 ) );
            Assert.Equal( -1, testedList.IndexOf( item2, 1, 1 ) );
            Assert.Equal( 1, testedList.IndexOf( item3 ) );
            Assert.Empty( events );
        }

        [Fact]
        public void LastIndexOf()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> baseList = new ObservableList<TestClass>( new[] { item2, item3, item2 } );

            IObservableReadOnlyList<TestClass> testedList = new ObservableReadOnlyList<TestClass>( baseList );

            testedList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedList, obj );
                events.Add( args );
            };

            Assert.Equal( -1, testedList.LastIndexOf( item1 ) );
            Assert.Equal( 2, testedList.LastIndexOf( item2 ) );
            Assert.Equal( 0, testedList.LastIndexOf( item2, 1 ) );
            Assert.Equal( -1, testedList.LastIndexOf( item2, 1, 1 ) );
            Assert.Equal( 1, testedList.LastIndexOf( item3 ) );
            Assert.Empty( events );
        }

        [Fact]
        public void Indexer_Get_ValidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> baseList = new ObservableList<TestClass>( new[] { item2, item3, item1 } );

            IObservableReadOnlyList<TestClass> testedList = new ObservableReadOnlyList<TestClass>( baseList );

            testedList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedList, obj );
                events.Add( args );
            };

            Assert.Equal( item2, testedList[ 0 ] );
            Assert.Equal( item3, testedList[ 1 ] );
            Assert.Equal( item1, testedList[ 2 ] );
            Assert.Empty( events );
        }

        [Fact]
        public void Indexer_Get_InvalidIndex()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableList<TestClass> baseList = new ObservableList<TestClass>( new[] { item2, item3, item1 } );

            IObservableReadOnlyList<TestClass> testedList = new ObservableReadOnlyList<TestClass>( baseList );

            testedList.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( testedList, obj );
                events.Add( args );
            };

            Assert.Throws<ArgumentOutOfRangeException>( () => testedList[ -654 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => testedList[ -1 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => testedList[ -3 ] );
            Assert.Throws<ArgumentOutOfRangeException>( () => testedList[ 34564 ] );
            Assert.Empty( events );
        }
    }
}
