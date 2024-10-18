/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Reflection;
using Xunit;

namespace Utilities.DotNet.Test.General
{
    public class DisposableObjectTest
    {
        private class TestClass : DisposableObject
        {
            protected override void Dispose( bool disposing )
            {
                m_onDispose?.Invoke( disposing );

                base.Dispose( disposing );
            }

            public TestClass( Action<bool> onDisposeAction )
            {
                m_onDispose = onDisposeAction;
            }

            private Action<bool> m_onDispose;
        }

        [Fact]
        public void Dispose()
        {
            // Arrange

            bool disposed = false;
            bool finalized = false;

            var obj = new TestClass( disposing =>
            {
                if( disposing )
                {
                    disposed = true;
                }
                else
                {
                    finalized = true;
                }
            } );

            // Act

            obj.Dispose();

            // Assert

            Assert.True( disposed );
            Assert.False( finalized );
        }

        [Fact]
        public void Finalizer()
        {
            // Arrange

            bool disposed = false;
            bool finalized = false;

            var obj = new TestClass( disposing =>
            {
                if( disposing )
                {
                    disposed = true;
                }
                else
                {
                    finalized = true;
                }
            } );


            // Act

            var finalizer = typeof( TestClass ).GetMethod( "Finalize", BindingFlags.Instance | BindingFlags.NonPublic );
            finalizer!.Invoke( obj, null );

            // Assert

            Assert.False( disposed );
            Assert.True( finalized );
        }
    }
}
