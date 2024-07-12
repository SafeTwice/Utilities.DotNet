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

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.

namespace Utilities.DotNet.Test.Collections.Observables
{
    public class ObservableSortedSetTest
    {
        [Fact]
        public void Constructor_Default()
        {
            // Act

            var observableSet = new ObservableSortedSet<TestClass>();

            // Assert

            Assert.Equal( 0, observableSet.Count );
            Assert.Equal( Comparer<TestClass>.Default, observableSet.Comparer );

            Assert.False( ( (ICollection<TestClass>) observableSet ).IsReadOnly );

            Assert.NotNull( ( (ICollection) observableSet ).SyncRoot );
            Assert.False( ( (ICollection) observableSet ).IsSynchronized );

            // The following tests are to ensure interface disambiguation.

            Assert.Equal( 0, ( (IObservableSet<TestClass>) observableSet ).Count );
            Assert.Equal( 0, ( (ICollectionEx<TestClass>) observableSet ).Count );
        }

        [Fact]
        public void Constructor_Comparer()
        {
            // Arrange

            var customComparer = Comparer<int>.Create( ( x, y ) => Comparer<int>.Default.Compare( y, x ) );

            // Act

            var observableSet = new ObservableSortedSet<int>( customComparer );

            // Assert

            Assert.Equal( 0, observableSet.Count );
            Assert.Same( customComparer, observableSet.Comparer );

            Assert.False( ( (IObservableCollection<int>) observableSet ).IsReadOnly );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            // Arrange

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            // Act

            var observableSet = new ObservableSortedSet<TestClass>( new[] { item1, item3, item2 } );

            // Assert

            Assert.Equal( new[] { item1, item2, item3 }, observableSet );
            Assert.Equal( 3, observableSet.Count );
            Assert.Equal( Comparer<TestClass>.Default, observableSet.Comparer );

            Assert.False( ( (IObservableSet<TestClass>) observableSet ).IsReadOnly );
        }

        [Fact]
        public void Constructor_InitializationListAndComparer()
        {
            // Arrange

            var customComparer = Comparer<int>.Create( ( x, y ) => Comparer<int>.Default.Compare( y, x ) );

            // Act

            var observableSet = new ObservableSortedSet<int>( new[] { 23, 8, 9, }, customComparer );

            // Assert

            Assert.Equal( new[] { 23, 9, 8 }, observableSet );
            Assert.Equal( 3, observableSet.Count );
            Assert.Same( customComparer, observableSet.Comparer );

            Assert.False( ( (IObservableSet<int>) observableSet ).IsReadOnly );
        }

        [Fact]
        public void Add()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );
            var item4 = new TestClass( "Item3", 25 );

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( new[] { item2 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2 }, observableSet );

            // Act: Added successfully

            var result = observableSet.Add( item3 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { item2, item3 }, observableSet );

            // Assert events
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            // Arrange

            events.Clear();

            // Act: Added successfully

            result = observableSet.Add( item1 );

            // Assert state & results

            Assert.Equal( new[] { item1, item2, item3 }, observableSet );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            // Arrange

            events.Clear();

            // Act: Cannot be added

            result = observableSet.Add( item4 );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { item1, item2, item3 }, observableSet );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionT_Add()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedSet<int> observableSet = new();

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act: Added successfully

            ( (ICollection<int>) observableSet ).Add( 5 );

            // Assert state & results

            Assert.Equal( new[] { 5 }, observableSet );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { 5 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            // Arrange

            events.Clear();

            // Act: Cannot be added

            ( (ICollectionEx<int>) observableSet ).Add( 5 );

            // Assert state & results

            Assert.Equal( new[] { 5 }, observableSet );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionT_Add_AlreadyExists()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedSet<int> observableSet = new( new[] { 5 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act & Assert

            Assert.Throws<InvalidOperationException>( () => ( (ICollection<int>) observableSet ).Add( 5 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Add()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedSet<int> observableSet = new( new[] { 13, 12 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableSet ).Add( 5 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 5, 12, 13 }, observableSet );

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

            ObservableSortedSet<int> observableSet = new( new[] { 8, 5 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableSet ).Add( 5.0 );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 5, 8 }, observableSet );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void AddRange_AllValid()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( new[] { item2 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2 }, observableSet );

            // Act

            var result = observableSet.AddRange( new[] { item1, item3 } );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { item1, item2, item3 }, observableSet );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item1, item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void AddRange_SomeAlreadyExist()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( new[] { item2 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2 }, observableSet );

            // Act

            var result = observableSet.AddRange( new[] { item1, item2, item3 } );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { item1, item2, item3 }, observableSet );

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

            ObservableSortedSet<int> observableSet = new();

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx<int>) observableSet ).AddRange( new[] { 5, 8, 2 } );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 2, 5, 8 }, observableSet );

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

            ObservableSortedSet<int> observableSet = new( new[] { 83 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 83 }, observableSet );

            // Act

            var result = ( (ICollectionEx) observableSet ).AddRange( new[] { 5, 8, 2 } );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 2, 5, 8, 83 }, observableSet );

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

            ObservableSortedSet<int> observableSet = new( new[] { 33, 22 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableSet ).AddRange( new object[] { 5, 8.0, 2 } );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 2, 5, 22, 33 }, observableSet );

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

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( new[] { item1, item2, item3 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, observableSet );

            // Act

            var result = observableSet.Remove( item1 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { item2, item3 }, observableSet );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            // Arrange

            events.Clear();

            // Act

            result = observableSet.Remove( item1 );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { item2, item3 }, observableSet );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Remove()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedSet<int> observableSet = new( new[] { 5, 8, 2 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableSet ).Remove( 8 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 2, 5 }, observableSet );

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

            ObservableSortedSet<int> observableSet = new( new[] { 5, 8, 2 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableSet ).Remove( 8.0 );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 2, 5, 8 }, observableSet );

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

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( new[] { item1, item2, item3 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, observableSet );

            // Act

            var result = observableSet.RemoveRange( new[] { item2, item1 } );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { item3 }, observableSet );

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

            ObservableSortedSet<int> observableSet = new( new[] { 5, 8, 2, 9 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableSet ).RemoveRange( new[] { 8, 2 } );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 5, 9 }, observableSet );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { 8, 2 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            // Arrange

            events.Clear();

            // Act

            result = ( (ICollectionEx) observableSet ).RemoveRange( new[] { 5, 12 } );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 9 }, observableSet );

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

            ObservableSortedSet<int> observableSet = new( new[] { 5, 8, 2, 9 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableSet ).RemoveRange( new object[] { 8, 2.0 } );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 2, 5, 9 }, observableSet );

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

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( new[] { item1, item2, item3 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, observableSet );

            // Act

            observableSet.Clear();

            // Assert state & results

            Assert.Empty( observableSet );

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

            observableSet.Clear();

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void Clear_Empty()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>();

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Empty( observableSet );

            // Act

            observableSet.Clear();

            // Assert state & results

            Assert.Empty( observableSet );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void Contains()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( new[] { item1, item3 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act & Assert results

            Assert.True( observableSet.Contains( item1 ) );
            Assert.False( observableSet.Contains( item2 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Contains()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedSet<int> observableSet = new( new[] { 5, 8, 2 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act & Assert results

            Assert.True( ( (ICollectionEx) observableSet ).Contains( 8 ) );
            Assert.False( ( (ICollectionEx) observableSet ).Contains( 9 ) );
            Assert.False( ( (ICollectionEx) observableSet ).Contains( 5.0 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlyCollectionEx_Contains()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableSortedSet<int> observableSet = new( new[] { 5, 8, 2 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act & Assert results

            Assert.True( ( (IReadOnlyCollectionEx<int>) observableSet ).Contains( 8 ) );
            Assert.False( ( (IReadOnlyCollectionEx<int>) observableSet ).Contains( 9 ) );
            Assert.False( ( (IReadOnlyCollectionEx<int>) observableSet ).Contains( 5.0 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void GetEnumerator()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( new[] { item1, item2 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act & Assert

            var enumerator = observableSet.GetEnumerator();

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( item1, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( item2, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IEnumerable_GetEnumerator()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );

            var observableSet = new ObservableSortedSet<TestClass>( new[] { item1, item2 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            // Act & Assert

            var enumerator = ( (IEnumerable) observableSet ).GetEnumerator();

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( item1, enumerator.Current );

            Assert.True( enumerator.MoveNext() );
            Assert.Equal( item2, enumerator.Current );

            Assert.False( enumerator.MoveNext() );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void CopyTo()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( new[] { item1, item2, item3 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            var array = new TestClass[ 3 ];

            // Act

            observableSet.CopyTo( array, 0 );

            // Assert result

            Assert.Equal( new[] { item1, item2, item3 }, array );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void ICollection_CopyTo()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 1 );
            var item3 = new TestClass( "Item3", 20 );

            var observableSet = new ObservableSortedSet<TestClass>( new[] { item1, item2, item3 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            var array = new TestClass[ 3 ];

            // Act

            ( (ICollection) observableSet ).CopyTo( array, 0 );

            // Assert result

            Assert.Equal( new[] { item1, item2, item3 }, array );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void UnionWith()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<int> observableSet = new ObservableSortedSet<int>( new[] { 8, 2, 23 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

            // Act

            observableSet.UnionWith( new[] { 1, 23, 9 } );

            // Assert state & results

            Assert.Equal( new[] { 1, 2, 8, 9, 23 }, observableSet );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { 1, 9 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void IntersectWith()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<int> observableSet = new ObservableSortedSet<int>( new[] { 8, 2, 23 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

            // Act

            observableSet.IntersectWith( new[] { 1, 23, 9, 8 } );

            // Assert state & results

            Assert.Equal( new[] { 8, 23 }, observableSet );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { 2 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ExceptWith()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<int> observableSet = new ObservableSortedSet<int>( new[] { 8, 2, 23 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

            // Act

            observableSet.ExceptWith( new[] { 1, 23, 9 } );

            // Assert state & results

            Assert.Equal( new[] { 2, 8 }, observableSet );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { 23 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void SymmetricExceptWith()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<int> observableSet = new ObservableSortedSet<int>( new[] { 8, 2, 23 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

            // Act

            observableSet.SymmetricExceptWith( new[] { 1, 23, 9 } );

            // Assert state & results

            Assert.Equal( new[] { 1, 2, 8, 9 }, observableSet );

            // Assert events

            Assert.Equal( 2, events.Count );

            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { 23 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 1 ].Action );
            Assert.Equal( new[] { 1, 9 }, events[ 1 ].NewItems );
            Assert.Null( events[ 1 ].OldItems );
            Assert.Equal( -1, events[ 1 ].NewStartingIndex );
            Assert.Equal( -1, events[ 1 ].OldStartingIndex );
        }

        [Fact]
        public void SetComparisons()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<int> observableSet = new ObservableSortedSet<int>( new[] { 8, 2, 23 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

            // Act & Assert

            Assert.True( observableSet.IsSubsetOf( new[] { 23, 2, 8, 55, 12 } ) );
            Assert.False( observableSet.IsSubsetOf( new[] { 22, 2, 8, 55, 12 } ) );

            Assert.True( observableSet.IsProperSubsetOf( new[] { 23, 2, 8, 55, 12 } ) );
            Assert.False( observableSet.IsProperSubsetOf( new[] { 22, 2, 8, 12 } ) );
            Assert.False( observableSet.IsProperSubsetOf( new[] { 22, 2, 8 } ) );

            Assert.True( observableSet.IsSupersetOf( new[] { 8, 23 } ) );
            Assert.False( observableSet.IsSupersetOf( new[] { 8, 23, 1 } ) );

            Assert.True( observableSet.IsProperSupersetOf( new[] { 8, 23 } ) );
            Assert.False( observableSet.IsProperSupersetOf( new[] { 8, 23, 1 } ) );
            Assert.False( observableSet.IsProperSupersetOf( new[] { 8, 23, 2 } ) );

            Assert.True( observableSet.Overlaps( new[] { 12, 2, 55 } ) );
            Assert.False( observableSet.Overlaps( new[] { 12, 1, 55 } ) );

            Assert.True( observableSet.SetEquals( new[] { 8, 23, 2 } ) );
            Assert.False( observableSet.SetEquals( new[] { 8, 23, 1 } ) );

            // Assert unmodified state

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void IReadOnlySetEx_SetComparisons()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<int> observableSet = new ObservableSortedSet<int>( new[] { 8, 2, 23 } );

            observableSet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            var readonlySet = (IReadOnlySetEx<int>) observableSet;

            Assert.Equal( new[] { 2, 8, 23 }, readonlySet );

            // Act & Assert

            Assert.True( readonlySet.IsSubsetOf( new object[] { 23, 2, 8, 55, 12.0 } ) );
            Assert.False( readonlySet.IsSubsetOf( new object[] { 23, 2.0, 8, 55, 12 } ) );
            Assert.False( readonlySet.IsSubsetOf( new object[] { 22, 2, 8, 55, 12 } ) );

            Assert.True( readonlySet.IsProperSubsetOf( new object[] { 23, 2, 8, 55, 12.0 } ) );
            Assert.False( readonlySet.IsProperSubsetOf( new object[] { 23, 2.0, 8, 55, 12 } ) );
            Assert.False( readonlySet.IsProperSubsetOf( new object[] { 22, 2, 8, 12.0 } ) );
            Assert.False( readonlySet.IsProperSubsetOf( new object[] { 23, 2, 8 } ) );

            Assert.True( readonlySet.IsSupersetOf( new object[] { 8, 23 } ) );
            Assert.False( readonlySet.IsSupersetOf( new object[] { 8, 23.0 } ) );
            Assert.False( readonlySet.IsSupersetOf( new object[] { 8, 23, 1 } ) );

            Assert.True( readonlySet.IsProperSupersetOf( new object[] { 8, 23 } ) );
            Assert.False( readonlySet.IsProperSupersetOf( new object[] { 8.0, 23 } ) );
            Assert.False( readonlySet.IsProperSupersetOf( new object[] { 8, 23, 1 } ) );
            Assert.False( readonlySet.IsProperSupersetOf( new object[] { 8, 23, 2 } ) );

            Assert.True( readonlySet.Overlaps( new object[] { 12, 2, 55 } ) );
            Assert.False( readonlySet.Overlaps( new object[] { 12, 2.0, 55 } ) );
            Assert.False( readonlySet.Overlaps( new object[] { 12, 1, 55 } ) );

            Assert.True( readonlySet.SetEquals( new object[] { 8, 23, 2 } ) );
            Assert.False( readonlySet.SetEquals( new object[] { 8, 23.0, 2 } ) );
            Assert.False( readonlySet.SetEquals( new object[] { 8, 23, 1 } ) );

            // Assert unmodified state

            Assert.Equal( new[] { 2, 8, 23 }, observableSet );

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

            IObservableSet<TestClass> observableSet = new ObservableSortedSet<TestClass>( new[] { item1 } );

            Assert.Equal( new[] { item1 }, observableSet );

            // Act

            observableSet.Add( item2 );

            // Assert

            Assert.Equal( new[] { item1, item2 }, observableSet );

            // Act

            observableSet.Remove( item2 );

            // Assert

            Assert.Equal( new[] { item1 }, observableSet );
        }
    }
}
