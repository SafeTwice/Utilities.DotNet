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
            var observableCollection = new ObservableSortedCollection<int>();

            Assert.Equal( 0, observableCollection.Count );
            Assert.Equal( Comparer<int>.Default, observableCollection.Comparer );
            Assert.False( ( (IObservableCollection<int>) observableCollection ).IsReadOnly );
            Assert.NotNull( ( (IObservableCollection<int>) observableCollection ).SyncRoot );
            Assert.False( ( (IObservableCollection<int>) observableCollection ).IsSynchronized );
        }

        [Fact]
        public void Constructor_Comparer()
        {
            var observableCollection = new ObservableSortedCollection<int>( Comparer<int>.Default );

            Assert.Equal( 0, observableCollection.Count );
            Assert.Equal( Comparer<int>.Default, observableCollection.Comparer );
            Assert.False( ( (IObservableCollection<int>) observableCollection ).IsReadOnly );
            Assert.NotNull( ( (IObservableCollection<int>) observableCollection ).SyncRoot );
            Assert.False( ( (IObservableCollection<int>) observableCollection ).IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            var observableCollection = new ObservableSortedCollection<int>( new[] { 1, 3, 2, 4 } );

            Assert.Equal( new[] { 1, 2, 3, 4 }, observableCollection );
            Assert.Equal( 4, observableCollection.Count );
            Assert.Equal( Comparer<int>.Default, observableCollection.Comparer );
            Assert.False( ( (IObservableCollection<int>) observableCollection ).IsReadOnly );
            Assert.NotNull( ( (IObservableCollection<int>) observableCollection ).SyncRoot );
            Assert.False( ( (IObservableCollection<int>) observableCollection ).IsSynchronized );
        }

        [Fact]
        public void Constructor_InitializationListAndComparer()
        {
            Comparer<int> comparer = Comparer<int>.Create( ( x, y ) => Comparer<int>.Default.Compare( y, x ) );

            var observableCollection = new ObservableSortedCollection<int>( new[] { 5, 3, 4, 1 },
                comparer );

            Assert.Equal( new[] { 5, 4, 3, 1 }, observableCollection );
            Assert.Equal( 4, observableCollection.Count );
            Assert.Equal( comparer, observableCollection.Comparer );
            Assert.False( ( (IObservableCollection<int>) observableCollection ).IsReadOnly );
            Assert.NotNull( ( (IObservableCollection<int>) observableCollection ).SyncRoot );
            Assert.False( ( (IObservableCollection<int>) observableCollection ).IsSynchronized );
        }

        [Fact]
        public void Add()
        {
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

            observableCollection.Add( item2 );

            Assert.Equal( new[] { item2, item1 }, observableCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            events.Clear();

            observableCollection.Add( item3 );

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 2, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ICollectionEx_Add()
        {
            ObservableSortedCollection<int> collection = new();

            ( (ICollectionEx) collection ).Add( 5 );

            Assert.Equal( new[] { 5 }, collection );
        }

        [Fact]
        public void ICollectionEx_Add_InvalidObject()
        {
            ObservableSortedCollection<int> collection = new();

            Assert.Throws<InvalidCastException>( () => ( (ICollectionEx) collection ).Add( 5.0 ) );
        }

        [Fact]
        public void AddRange()
        {
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

            observableCollection.AddRange( new[] { item1, item3 } );

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item1, item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ICollectionEx_AddRange()
        {
            ObservableSortedCollection<int> collection = new();

            ( (ICollectionEx) collection ).AddRange( new[] { 5, 8, 2 } );

            Assert.Equal( new[] { 2, 5, 8 }, collection );
        }

        [Fact]
        public void ICollectionEx_AddRange_InvalidObject()
        {
            ObservableSortedCollection<int> collection = new();

            Assert.Throws<InvalidCastException>( () => ( (ICollectionEx) collection ).AddRange( new object[] { 5, 8.0, 2 } ) );
        }

        [Fact]
        public void Remove()
        {
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

            Assert.True( observableCollection.Remove( item1 ) );

            Assert.Equal( new[] { item2, item3 }, observableCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );

            events.Clear();

            Assert.False( observableCollection.Remove( item1 ) );

            Assert.Equal( new[] { item2, item3 }, observableCollection );
            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Remove()
        {
            ObservableSortedCollection<int> collection = new( new[] { 5, 8, 2 } );

            ( (ICollectionEx) collection ).Remove( 8 );

            Assert.Equal( new[] { 2, 5 }, collection );
        }

        [Fact]
        public void ICollectionEx_Remove_InvalidObject()
        {
            ObservableSortedCollection<int> collection = new( new[] { 5, 8, 2 } );

            ( (ICollectionEx) collection ).Remove( 8.0 );

            Assert.Equal( new[] { 2, 5, 8 }, collection );
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
        public void ICollectionEx_RemoveRange()
        {
            ObservableSortedCollection<int> collection = new( new[] { 5, 8, 2, 9 } );

            ( (ICollectionEx) collection ).RemoveRange( new[] { 8, 2 } );

            Assert.Equal( new[] { 5, 9 }, collection );
        }

        [Fact]
        public void Clear()
        {
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

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>();

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
        public void AutoReorder_FirstElement()
        {
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

            // No reorder necessary

            item2.Value = 9;

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );
            Assert.Empty( events );

            // No reorder necessary

            item2.Value = 10;

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );
            Assert.Empty( events );

            // Reorder necessary

            item2.Value = 11;

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );
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

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { item3, item2, item1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // No reorder necessary

            item1.Value = 6;

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );
            Assert.Empty( events );

            // No reorder necessary

            item1.Value = 5;

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );
            Assert.Empty( events );

            // Reorder necessary

            item1.Value = 4;

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );
            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Move, events[ 0 ].Action );
            Assert.Equal( new[] { item1 }, events[ 0 ].NewItems );
            Assert.Equal( new[] { item1 }, events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );

            events.Clear();

            // No reorder necessary

            item2.Value = 19;

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );
            Assert.Empty( events );

            // No reorder necessary

            item2.Value = 20;

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );
            Assert.Empty( events );

            // Reorder necessary

            item2.Value = 21;

            Assert.Equal( new[] { item1, item3, item2 }, observableCollection );
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

            IObservableCollection<TestClass> observableCollection = new ObservableSortedCollection<TestClass>( new[] { item3, item2, item1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // No reorder necessary

            item3.Value = 11;

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );
            Assert.Empty( events );

            // No reorder necessary

            item3.Value = 10;

            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );
            Assert.Empty( events );

            // Reorder necessary

            item3.Value = 9;

            Assert.Equal( new[] { item2, item3, item1 }, observableCollection );
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

            var observableCollection = new ObservableSortedCollection<TestClass>( new[] { item3, item1 },
                Comparer<TestClass>.Create( ( x, y ) => Comparer<int>.Default.Compare( x.Value, y.Value ) ) );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item3 }, observableCollection );

            Assert.Throws<ArgumentException>( () => observableCollection.UpdateSortOrder( item2 ) );

            Assert.Empty( events );
        }

        [Fact]
        public void Contains()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<int> observableCollection = new ObservableSortedCollection<int>( new[] { 1, 3, 2, 4 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.True( observableCollection.Contains( 1 ) );
            Assert.False( observableCollection.Contains( 5 ) );

            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Contains()
        {
            ObservableSortedCollection<int> collection = new( new[] { 5, 8, 2 } );

            Assert.True( ( (ICollectionEx) collection ).Contains( 8 ) );
            Assert.False( ( (ICollectionEx) collection ).Contains( 9 ) );
            Assert.False( ( (ICollectionEx) collection ).Contains( 5.0 ) );
        }

        [Fact]
        public void IReadOnlyCollectionEx_Contains()
        {
            ObservableSortedCollection<int> collection = new( new[] { 5, 8, 2 } );

            Assert.True( ( (IReadOnlyCollectionEx<int>) collection ).Contains( 8 ) );
            Assert.False( ( (IReadOnlyCollectionEx<int>) collection ).Contains( 9 ) );
            Assert.False( ( (IReadOnlyCollectionEx<int>) collection ).Contains( 5.0 ) );
        }

        [Fact]
        public void GetEnumerator()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<int> observableCollection = new ObservableSortedCollection<int>( new[] { 6, 9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            var enumerator = observableCollection.GetEnumerator();

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

            var observableCollection = new ObservableSortedCollection<int>( new[] { 6, 9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            IEnumerable enumerable = observableCollection;

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

            IObservableCollection<int> observableCollection = new ObservableSortedCollection<int>( new[] { 9, 3, 7, 4 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            var array = new int[ 4 ];

            observableCollection.CopyTo( array, 0 );

            Assert.Equal( new[] { 3, 4, 7, 9 }, array );

            Assert.Empty( events );
        }

        [Fact]
        public void ICollection_CopyTo()
        {
            var events = new List<NotifyCollectionChangedEventArgs>();

            var observableCollection = new ObservableSortedCollection<int>( new[] { 9, 3, 7, 4 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            var array = new int[ 4 ];

            ( (ICollection) observableCollection ).CopyTo( array, 0 );

            Assert.Equal( new[] { 3, 4, 7, 9 }, array );

            Assert.Empty( events );
        }
    }
}
