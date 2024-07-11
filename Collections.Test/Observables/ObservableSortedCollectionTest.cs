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

#pragma warning disable xUnit2013 // Do not use equality check to check for observableCollection size.

namespace Utilities.DotNet.Test.Collections.Observables
{
    public class ObservableSortedCollectionTest
    {
        [Fact]
        public void Constructor_Default()
        {
            // Arrange & Act

            var observableCollection = new ObservableSortedCollection<int>();

            // Assert

            Assert.Equal( 0, observableCollection.Count );
            Assert.Equal( Comparer<int>.Default, observableCollection.Comparer );

            Assert.False( ( (IObservableCollection<int>) observableCollection ).IsReadOnly );

            // The following tests are to ensure interface disambiguation.

            Assert.Equal( 0, ( (IObservableCollection<int>) observableCollection ).Count );
            Assert.Equal( 0, ( (ICollectionEx<int>) observableCollection ).Count );
        }

        [Fact]
        public void Constructor_Comparer()
        {
            // Arrange & Act

            var customComparer = Comparer<int>.Create( ( x, y ) => Comparer<int>.Default.Compare( y, x ) );

            var observableCollection = new ObservableSortedCollection<int>( customComparer );

            // Assert

            Assert.Equal( 0, observableCollection.Count );
            Assert.Same( customComparer, observableCollection.Comparer );

            Assert.False( ( (IObservableCollection<int>) observableCollection ).IsReadOnly );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            // Arrange & Act

            var observableCollection = new ObservableSortedCollection<int>( new[] { 1, 3, 2, 4 } );

            // Assert

            Assert.Equal( new[] { 1, 2, 3, 4 }, observableCollection );
            Assert.Equal( 4, observableCollection.Count );
            Assert.Equal( Comparer<int>.Default, observableCollection.Comparer );

            Assert.False( ( (IObservableCollection<int>) observableCollection ).IsReadOnly );
        }

        [Fact]
        public void Constructor_InitializationListAndComparer()
        {
            // Arrange & Act

            var customComparer = Comparer<int>.Create( ( x, y ) => Comparer<int>.Default.Compare( y, x ) );

            var observableCollection = new ObservableSortedCollection<int>( new[] { 5, 3, 4, 1 }, customComparer );

            // Assert

            Assert.Equal( new[] { 5, 4, 3, 1 }, observableCollection );
            Assert.Equal( 4, observableCollection.Count );
            Assert.Same( customComparer, observableCollection.Comparer );

            Assert.False( ( (IObservableCollection<int>) observableCollection ).IsReadOnly );
        }

        [Fact]
        public void Add()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { item1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1 }, observableCollection );

            // Act

            observableCollection.Add( item2 );

            // Assert state & results

            Assert.Equal( new[] { item2, item1 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            // Arrange

            events.Clear();

            observableCollection.Add( item3 );

            // Assert state & results

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 2, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ICollectionExT_TryAdd()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedCollection<int> observableCollection = new();

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx<int>) observableCollection ).TryAdd( 5 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 5 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { 5 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ICollectionEx_Add()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedCollection<int> observableCollection = new( new[] { 13, 12 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).Add( 5 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 5, 12, 13 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { 5 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ICollectionEx_Add_InvalidObject()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedCollection<int> observableCollection = new( new[] { 8, 5 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).Add( 5.0 );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 5, 8 }, observableCollection );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void AddRange()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { item2 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2 }, observableCollection );

            // Act

            var result = observableCollection.AddRange( new[] { item1, item3 } );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item1, item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ICollectionExT_AddRange()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedCollection<int> observableCollection = new();

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx<int>) observableCollection ).AddRange( new[] { 5, 8, 2 } );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 2, 5, 8 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { 5, 8, 2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ICollectionEx_AddRange()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedCollection<int> observableCollection = new( new[] { 83 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 83 }, observableCollection );

            // Act

            var result = ( (ICollectionEx) observableCollection ).AddRange( new[] { 5, 8, 2 } );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 2, 5, 8, 83 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { 5, 8, 2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ICollectionEx_AddRange_InvalidObject()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedCollection<int> observableCollection = new( new[] { 33, 22 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).AddRange( new object[] { 5, 8.0, 2 } );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 2, 5, 22, 33 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { 5, 2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void Remove()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { item1, item2, item3 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // Act

            var result = observableCollection.Remove( item1 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { item2, item3 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            // Arrange

            events.Clear();

            // Act

            result = observableCollection.Remove( item1 );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { item2, item3 }, observableCollection );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Remove()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedCollection<int> observableCollection = new( new[] { 5, 8, 2 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).Remove( 8 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 2, 5 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { 8 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 2, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void ICollectionEx_Remove_InvalidObject()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedCollection<int> observableCollection = new( new[] { 5, 8, 2 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).Remove( 8.0 );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 2, 5, 8 }, observableCollection );

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

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { item3, item2, item1 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );

            // Act

            observableCollection.RemoveRange( new[] { item2, item1 } );

            // Assert state & results

            Assert.Equal( new[] { item3 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item2, item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void ICollectionEx_RemoveRange()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedCollection<int> observableCollection = new( new[] { 5, 8, 2, 9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).RemoveRange( new[] { 2, 8 } );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 5, 9 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { 2, 8 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            // Arrange

            events.Clear();

            // Act

            result = ( (ICollectionEx) observableCollection ).RemoveRange( new[] { 5, 12 } );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 9 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { 5 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void ICollectionEx_RemoveRange_InvalidObject()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedCollection<int> observableCollection = new( new[] { 5, 8, 2, 9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).RemoveRange( new object[] { 8, 2.0 } );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 2, 5, 9 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { 8 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void Clear()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { item1, item2, item3 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // Act

            observableCollection.Clear();

            // Assert state & results

            Assert.Empty( observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Reset, events[ 0 ].Action );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            // Arrange

            events.Clear();

            // Act

            observableCollection.Clear();

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void Clear_Empty()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>();

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Empty( observableCollection );

            // Act

            observableCollection.Clear();

            // Assert state & results

            Assert.Empty( observableCollection );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void AutoReorder_FirstElement()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { item3, item2, item1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // Act: No reorder necessary

            item2.Value = 9;

            // Assert state

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: No reorder necessary

            item2.Value = 10;

            // Assert state

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: Reorder necessary

            item2.Value = 11;

            // Assert state

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );

            // Assert events

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
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { item3, item2, item1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // Act: No reorder necessary

            item1.Value = 6;

            // Assert state

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: No reorder necessary

            item1.Value = 5;

            // Assert state

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: Reorder necessary

            item1.Value = 4;

            // Assert state

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].NewItems );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );

            // Arrange

            events.Clear();

            // Act: No reorder necessary

            item2.Value = 19;

            // Assert state

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: No reorder necessary

            item2.Value = 20;

            // Assert state

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: Reorder necessary

            item2.Value = 21;

            // Assert state

            Assert.Equal( new[] { item1, item3, item2 }, observableCollection );

            // Assert events

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
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { item3, item2, item1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // Act: No reorder necessary

            item3.Value = 11;

            // Assert state

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: No reorder necessary

            item3.Value = 10;

            // Assert state

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // Assert events

            Assert.Empty( events );

            // Act: Reorder necessary

            item3.Value = 9;

            // Assert state

            Assert.Equal( new[] { item2, item3, item1 }, observableCollection );

            // Assert events

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
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            var observableCollection = new ObservableSortedCollection<TestClass>( new[] { item3, item1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item3 }, observableCollection );

            // Act

            Assert.Throws<ArgumentException>( () => observableCollection.UpdateSortOrder( item2 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void Contains()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<int> observableCollection = new ObservableSortedCollection<int>( new[] { 1, 3, 2, 4 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act & Assert

            Assert.True( observableCollection.Contains( 1 ) );
            Assert.False( observableCollection.Contains( 5 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Contains()
        {
            // Arrange & Act

            ObservableSortedCollection<int> collection = new( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.True( ( (ICollectionEx) collection ).Contains( 8 ) );
            Assert.False( ( (ICollectionEx) collection ).Contains( 9 ) );
            Assert.False( ( (ICollectionEx) collection ).Contains( 5.0 ) );
        }

        [Fact]
        public void IReadOnlyCollectionEx_Contains()
        {
            // Arrange & Act

            ObservableSortedCollection<int> collection = new( new[] { 5, 8, 2 } );

            // Act & Assert

            Assert.True( ( (IReadOnlyCollectionEx<int>) collection ).Contains( 8 ) );
            Assert.False( ( (IReadOnlyCollectionEx<int>) collection ).Contains( 9 ) );
            Assert.False( ( (IReadOnlyCollectionEx<int>) collection ).Contains( 5.0 ) );
        }

        [Fact]
        public void GetEnumerator()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<int> observableCollection = new ObservableSortedCollection<int>( new[] { 6, 9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act & Assert

            var enumerator = observableCollection.GetEnumerator();

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 6, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 9, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IEnumerable_GetEnumerator()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableCollection = new ObservableSortedCollection<int>( new[] { 6, 9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            IEnumerable enumerable = observableCollection;

            // Act & Assert

            var enumerator = enumerable.GetEnumerator();

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 6, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( 9, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void CopyTo()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<int> observableCollection = new ObservableSortedCollection<int>( new[] { 9, 3, 7, 4 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            var array = new int[ 4 ];

            // Act

            observableCollection.CopyTo( array, 0 );

            // Assert

            Assert.Equal( new[] { 3, 4, 7, 9 }, array );

            Assert.Empty( events );
        }

        [Fact]
        public void ICollection_CopyTo()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableCollection = new ObservableSortedCollection<int>( new[] { 9, 3, 7, 4 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            var array = new int[ 4 ];

            // Act

            ( (ICollection) observableCollection ).CopyTo( array, 0 );

            // Assert

            Assert.Equal( new[] { 3, 4, 7, 9 }, array );

            Assert.Empty( events );
        }
    }
}
