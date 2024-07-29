/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Utilities.DotNet.Collections;
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
            // Arrange & Act

            var observableCollection = new ObservableCollection<TestClass>();

            // Assert

            Assert.Equal( 0, observableCollection.Count );

            Assert.False( ( (ICollection<TestClass>) observableCollection ).IsReadOnly );

            Assert.NotNull( ( (ICollection) observableCollection ).SyncRoot );
            Assert.False( ( (ICollection) observableCollection ).IsSynchronized );

            // The following tests are to ensure interface disambiguation.

            Assert.Equal( 0, ( (IObservableCollection<TestClass>) observableCollection ).Count );
            Assert.Equal( 0, ( (ICollectionEx<TestClass>) observableCollection ).Count );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            // Arrange

            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            // Act

            var observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2, item3 } );

            // Assert

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );
            Assert.Equal( 3, observableCollection.Count );

            Assert.False( ( (IObservableCollection<TestClass>) observableCollection ).IsReadOnly );
        }

        [Fact]
        public void Add()
        {
            // Arrange

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

            // Act

            var result = observableCollection.Add( item2 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { item1, item2 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );

            // Arrange

            events.Clear();

            // Act

            result = observableCollection.Add( item3 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 2, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ICollectionT_Add()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableCollection<int> observableCollection = new();

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            ( (ICollection<int>) observableCollection ).Add( 5 );

            // Assert state & results

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

            ObservableCollection<int> observableCollection = new( new[] { 13, 12 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).Add( 5 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 13, 12, 5 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { 5 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 2, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ICollectionEx_Add_InvalidObject()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableCollection<int> observableCollection = new( new[] { 8, 5 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).Add( 5.0 );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 8, 5 }, observableCollection );

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

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item2 } );

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
            Assert.Equal( new[] { item2, item1, item3 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { item1, item3 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ICollectionExT_AddRange()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableCollection<int> observableCollection = new();

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx<int>) observableCollection ).AddRange( new[] { 5, 8, 2 } );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 5, 8, 2 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { 5, 8, 2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 0, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ICollectionEx_AddRange()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableCollection<int> observableCollection = new( new[] { 83 } );

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
            Assert.Equal( new[] { 83, 5, 8, 2 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { 5, 8, 2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
        }

        [Fact]
        public void ICollectionEx_AddRange_InvalidObject()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableCollection<int> observableCollection = new( new[] { 33, 22 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).AddRange( new object[] { 5, 8.0, 2 } );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 33, 22, 5, 2 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Add, events[ 0 ].Action );
            Assert.Equal( new[] { 5, 2 }, events[ 0 ].NewItems );
            Assert.Null( events[ 0 ].OldItems );
            Assert.Equal( 2, events[ 0 ].NewStartingIndex );
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

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2, item3 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );

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
            Assert.Equal( 0, events[ 0 ].OldStartingIndex );
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

            ObservableCollection<int> observableCollection = new( new[] { 5, 8, 2 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).Remove( 8 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 5, 2 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { 8 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void ICollectionEx_Remove_InvalidObject()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableCollection<int> observableCollection = new( new[] { 5, 8, 2 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).Remove( 8.0 );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 5, 8, 2 }, observableCollection );

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

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2, item3 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { item1, item2, item3 }, observableCollection );

            // Act

            var result = observableCollection.RemoveRange( new[] { item2, item1 } );

            // Assert state & results

            Assert.True( result );
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

            ObservableCollection<int> observableCollection = new( new[] { 5, 8, 2, 9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).RemoveRange( new[] { 8, 2 } );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 5, 9 }, observableCollection );

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

            ObservableCollection<int> observableCollection = new( new[] { 5, 8, 2, 9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).RemoveRange( new object[] { 8, 2.0 } );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 5, 2, 9 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Remove, events[ 0 ].Action );
            Assert.Equal( new[] { 8 }, events[ 0 ].OldItems );
            Assert.Null( events[ 0 ].NewItems );
            Assert.Equal( -1, events[ 0 ].OldStartingIndex );
            Assert.Equal( -1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void Replace()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<double> observableCollection = new ObservableCollection<double>( new[] { 5.1, 8.0, 2.9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = observableCollection.Replace( 8.0, 3.5 );

            // Assert state & results

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 3.5, 2.9 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Replace, events[ 0 ].Action );
            Assert.Equal( new[] { 8.0 }, events[ 0 ].OldItems );
            Assert.Equal( new[] { 3.5 }, events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void Replace_OldNotExisting()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<double> observableCollection = new ObservableCollection<double>( new[] { 5.1, 8.0, 2.9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = observableCollection.Replace( 8.01, 3.5 );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, observableCollection );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Replace()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<double> observableCollection = new ObservableCollection<double>( new[] { 5.1, 8.0, 2.9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).Replace( 8.0, 3.5 );

            // Assert

            Assert.True( result );
            Assert.Equal( new[] { 5.1, 3.5, 2.9 }, observableCollection );

            // Assert events

            Assert.Equal( 1, events.Count );
            Assert.Equal( NotifyCollectionChangedAction.Replace, events[ 0 ].Action );
            Assert.Equal( new[] { 8.0 }, events[ 0 ].OldItems );
            Assert.Equal( new[] { 3.5 }, events[ 0 ].NewItems );
            Assert.Equal( 1, events[ 0 ].OldStartingIndex );
            Assert.Equal( 1, events[ 0 ].NewStartingIndex );
        }

        [Fact]
        public void ICollectionEx_Replace_InvalidObject()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableCollection<double> observableCollection = new ObservableCollection<double>( new[] { 5.1, 8.0, 2.9 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act

            var result = ( (ICollectionEx) observableCollection ).Replace( 8.0, 3.5f );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, observableCollection );

            // Act

            result = ( (ICollectionEx) observableCollection ).Replace( 8.0f, 3.5 );

            // Assert state & results

            Assert.False( result );
            Assert.Equal( new[] { 5.1, 8.0, 2.9 }, observableCollection );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void Clear()
        {
            // Arrange

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

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>();

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
        public void Contains()
        {
            // Arrange

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

            // Act & Assert results

            Assert.True( observableCollection.Contains( item1 ) );
            Assert.False( observableCollection.Contains( item2 ) );

            // Assert events

            Assert.Empty( events );
        }

        [Fact]
        public void ICollectionEx_Contains()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            ObservableCollection<int> observableCollection = new( new[] { 5, 8, 2 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act & Assert results

            Assert.True( ( (ICollectionEx) observableCollection ).Contains( 8 ) );
            Assert.False( ( (ICollectionEx) observableCollection ).Contains( 9 ) );
            Assert.False( ( (ICollectionEx) observableCollection ).Contains( 5.0 ) );

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

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act & Assert

            var enumerator = observableCollection.GetEnumerator();

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

            var observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            // Act & Assert

            var enumerator = ( (IEnumerable) observableCollection ).GetEnumerator();

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

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2, item3 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            var array = new TestClass[ 3 ];

            // Act

            observableCollection.CopyTo( array, 0 );

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

            var observableCollection = new ObservableCollection<TestClass>( new[] { item1, item2, item3 } );

            observableCollection.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableCollection, obj );
                events.Add( args );
            };

            var array = new TestClass[ 3 ];

            // Act

            ( (ICollection) observableCollection ).CopyTo( array, 0 );

            // Assert result

            Assert.Equal( new[] { item1, item2, item3 }, array );

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

            IObservableCollection<TestClass> observableCollection = new ObservableCollection<TestClass>( new[] { item1 } );

            Assert.Equal( new[] { item1 }, observableCollection );

            // Act

            observableCollection.Add( item2 );

            // Assert

            Assert.Equal( new[] { item1, item2 }, observableCollection );

            // Act

            observableCollection.Remove( item2 );

            // Assert

            Assert.Equal( new[] { item1 }, observableCollection );
        }
    }
}
