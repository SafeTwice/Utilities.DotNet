/// @file
/// @copyright  Copyright (c) 2026 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Reflection;
using Xunit;

namespace Utilities.DotNet.Test
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

            private readonly Action<bool> m_onDispose;
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

        private class SimpleTestClass
        {
            public string Value { get; set; }

            public SimpleTestClass( string value )
            {
                Value = value;
            }
        }

        private class ConstructorFailingTestClass : DisposableObject
        {
            public static bool FinalizerCalled { get; set; }

            public ConstructorFailingTestClass( bool fail )
            {
                if( fail )
                {
                    throw new Exception( "Constructor failed." );
                }

                m_object = new SimpleTestClass( "Test" );
            }

            protected override string TraceInfo => $"Object: {m_object.Value} " + base.TraceInfo;

            protected override void Dispose( bool disposing )
            {
                if( !disposing )
                {
                    FinalizerCalled = true;
                }

                base.Dispose( disposing );
            }

            private readonly SimpleTestClass m_object;
        }

#pragma warning disable S1215

        [Fact]
        public void ConstructorFailure()
        {
            // Arrange

            ConstructorFailingTestClass.FinalizerCalled = false;

            // Act

            Assert.Throws<Exception>( () => new ConstructorFailingTestClass( true ) );

            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Assert

            Assert.True( ConstructorFailingTestClass.FinalizerCalled );
        }

#pragma warning restore S1215

    }
}