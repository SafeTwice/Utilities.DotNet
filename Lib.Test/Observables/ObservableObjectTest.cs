/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Reflection;
using Utilities.DotNet.Observables;
using Xunit;

namespace Utilities.DotNet.Test.General
{
    public class ObservableObjectTest
    {
        private class TestClass : ObservableObject
        {
            public int Property
            {
                get => m_property;
                set
                {
                    m_property = value;
                    OnPropertyChanged();
                }
            }

            private int m_property;
        }

        [Fact]
        public void PropertyChangedEventRaised()
        {
            // Arrange

            var obj = new TestClass();

            var eventRaised = false;

            obj.PropertyChanged += ( sender, args ) =>
            {
                if( args.PropertyName == nameof( TestClass.Property ) )
                {
                    eventRaised = true;
                }
            };

            // Act

            obj.Property = 42;

            // Assert

            Assert.Equal( 42, obj.Property );
            Assert.True( eventRaised );
        }

        [Fact]
        public void NoListeners()
        {
            // Arrange

            var obj = new TestClass();

            // Act

            obj.Property = 42;

            // Assert

            Assert.Equal( 42, obj.Property );
        }
    }
}
