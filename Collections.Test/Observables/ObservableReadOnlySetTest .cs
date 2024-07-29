/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;
using System.Collections.Specialized;
using Utilities.DotNet.Collections.Observables;
using Xunit;

#pragma warning disable xUnit2013 // Do not use equality check to check for collection size.

namespace Utilities.DotNet.Test.Collections.Observables
{
    public class ObservableReadOnlySetTest
    {
        [Fact]
        public void Constructor_Default()
        {
            IObservableSet<TestClass> baseList = new ObservableSet<TestClass>();

            IObservableReadOnlySet<TestClass> testedList = new ObservableReadOnlySet<TestClass>( baseList );

            Assert.Equal( 0, testedList.Count );
        }

        [Fact]
        public void Constructor_InitializationList()
        {
            var item1 = new TestClass( "Item1", 10 );
            var item2 = new TestClass( "Item2", 5 );
            var item3 = new TestClass( "Item3", 20 );

            IObservableSet<TestClass> baseList = new ObservableSet<TestClass>( new[] { item1, item2, item3 } );

            IObservableReadOnlySet<TestClass> testedList = new ObservableReadOnlySet<TestClass>( baseList );

            Assert.Equal( new[] { item1, item2, item3 }, testedList );
            Assert.Equal( 3, testedList.Count );
        }

        [Fact]
        public void SetComparisons()
        {
            // Arrange

            var events = new List<NotifyCollectionChangedEventArgs>();

            IObservableSet<int> observableSet = new ObservableSet<int>( new[] { 8, 2, 23 } );

            var readonlySet = new ObservableReadOnlySet<int>( observableSet );

            readonlySet.CollectionChanged += ( obj, args ) =>
            {
                Assert.Same( observableSet, obj );
                events.Add( args );
            };

            Assert.Equal( new[] { 8, 2, 23 }, readonlySet );

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

            Assert.Equal( new[] { 8, 2, 23 }, observableSet );

            // Assert events

            Assert.Empty( events );
        }
    }
}
