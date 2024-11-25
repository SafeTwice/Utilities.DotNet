/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Xunit;

namespace Utilities.DotNet.Observables.Test
{
    public class ObservableObjectExTest
    {
        private class TestClass : ObservableObjectEx
        {
            public int Property1
            {
                get => m_property1;
                set
                {
                    var oldValue = m_property1;
                    m_property1 = value;
                    OnPropertyChanged( oldValue, value );
                }
            }

            public int Property2
            {
                get => m_property2;
                set => SetProperty( ref m_property2, value );
            }

            public string Property3
            {
                get => m_property3;
                set
                {
                    m_property3 = value;
#pragma warning disable CS0618 // Type or member is obsolete
                    OnPropertyChanged( nameof( Property3 ) );
#pragma warning restore CS0618 // Type or member is obsolete
                }
            }

            public TestClass( int property2 = 0 )
            {
                m_property2 = property2;
            }

            private int m_property1;
            private int m_property2;
            private string m_property3 = string.Empty;
        }

        [Fact]
        public void PropertyChangedEventRaised()
        {
            // Arrange

            var obj = new TestClass();

            var eventRaised = false;

            obj.PropertyChanged += ( sender, args ) =>
            {
                Assert.Equal( nameof( TestClass.Property1 ), args.PropertyName );
                eventRaised = true;
            };

            var eventExRaised = false;

            obj.PropertyChangedEx += ( sender, args ) =>
            {
                var intArgs = Assert.IsType<PropertyChangedExEventArgs<int>>( args );
                Assert.Equal( nameof( TestClass.Property1 ), intArgs.PropertyName );
                Assert.Equal( 0, intArgs.OldValue );
                Assert.Equal( 42, intArgs.NewValue );
                Assert.Equal( 0, args.OldValue );
                Assert.Equal( 42, args.NewValue );
                eventExRaised = true;
            };

            // Act

            obj.Property1 = 42;

            // Assert

            Assert.Equal( 42, obj.Property1 );
            Assert.Equal( 0, obj.Property2 );
            Assert.True( eventRaised );
            Assert.True( eventExRaised );
        }

        [Fact]
        public void SetProperty_DifferentValue()
        {
            // Arrange

            var obj = new TestClass( 20 );

            var eventRaised = false;

            obj.PropertyChanged += ( sender, args ) =>
            {
                Assert.Equal( nameof( TestClass.Property2 ), args.PropertyName );
                eventRaised = true;
            };

            var eventExRaised = false;

            obj.PropertyChangedEx += ( sender, args ) =>
            {
                var intArgs = Assert.IsType<PropertyChangedExEventArgs<int>>( args );
                Assert.Equal( nameof( TestClass.Property2 ), intArgs.PropertyName );
                Assert.Equal( 20, intArgs.OldValue );
                Assert.Equal( -242, intArgs.NewValue );
                eventExRaised = true;
            };

            // Act

            obj.Property2 = -242;

            // Assert

            Assert.Equal( 0, obj.Property1 );
            Assert.Equal( -242, obj.Property2 );
            Assert.True( eventRaised );
            Assert.True( eventExRaised );
        }

        [Fact]
        public void SetProperty_SameValue()
        {
            // Arrange

            var obj = new TestClass( 25 );

            var eventRaised = false;

            obj.PropertyChanged += ( sender, args ) =>
            {
                Assert.Equal( nameof( TestClass.Property2 ), args.PropertyName );
                eventRaised = true;
            };

            var eventExRaised = false;

            obj.PropertyChangedEx += ( sender, args ) =>
            {
                var intArgs = Assert.IsType<PropertyChangedExEventArgs<int>>( args );
                Assert.Equal( nameof( TestClass.Property2 ), intArgs.PropertyName );
                Assert.Equal( 25, intArgs.OldValue );
                Assert.Equal( 25, intArgs.NewValue );
                eventExRaised = true;
            };

            // Act

            obj.Property2 = 25;

            // Assert

            Assert.Equal( 0, obj.Property1 );
            Assert.Equal( 25, obj.Property2 );
            Assert.False( eventRaised );
            Assert.False( eventExRaised );
        }

        [Fact]
        public void NoListeners()
        {
            // Arrange

            var obj = new TestClass();

            // Act

            obj.Property1 = 42;

            // Assert

            Assert.Equal( 42, obj.Property1 );
            Assert.Equal( 0, obj.Property2 );
        }

        [Fact]
        public void ObsoleteOnPropertyChanged()
        {
            // Arrange

            var obj = new TestClass();

            var eventRaised = false;

            obj.PropertyChanged += ( sender, args ) =>
            {
                Assert.Equal( nameof( TestClass.Property3 ), args.PropertyName );
                eventRaised = true;
            };

            obj.PropertyChangedEx += ( sender, args ) =>
            {
                Assert.Fail( "Unexpected event raised." );
            };

            // Act

            obj.Property3 = "foo";

            // Assert

            Assert.Equal( 0, obj.Property1 );
            Assert.Equal( 0, obj.Property2 );
            Assert.Equal( "foo", obj.Property3 );
            Assert.True( eventRaised );
        }

    }
}
