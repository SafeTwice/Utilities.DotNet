/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Xunit;

namespace Utilities.DotNet.Observables.Test
{
    public class ObservableObjectTest
    {
        private class TestClass : ObservableObject
        {
            public int Property1
            {
                get => m_property1;
                set
                {
                    m_property1 = value;
                    OnPropertyChanged();
                }
            }

            public int Property2
            {
                get => m_property2;
                set => SetProperty( ref m_property2, value );
            }

            public TestClass( int property2 = 0 )
            {
                m_property2 = property2;
            }

            private int m_property1;
            private int m_property2;
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

            // Act

            obj.Property1 = 42;

            // Assert

            Assert.Equal( 42, obj.Property1 );
            Assert.Equal( 0, obj.Property2 );
            Assert.True( eventRaised );
        }

        [Fact]
        public void SetProperty_DifferentValue()
        {
            // Arrange

            var obj = new TestClass( 2 );

            var eventRaised = false;

            obj.PropertyChanged += ( sender, args ) =>
            {
                Assert.Equal( nameof( TestClass.Property2 ), args.PropertyName );
                eventRaised = true;
            };

            // Act

            obj.Property2 = 42;

            // Assert

            Assert.Equal( 0, obj.Property1 );
            Assert.Equal( 42, obj.Property2 );
            Assert.True( eventRaised );
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

            // Act

            obj.Property2 = 25;

            // Assert

            Assert.Equal( 0, obj.Property1 );
            Assert.Equal( 25, obj.Property2 );
            Assert.False( eventRaised );
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
    }
}
