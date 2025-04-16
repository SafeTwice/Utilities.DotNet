/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using Xunit;
using static Utilities.DotNet.ExceptionUtilities;

namespace Utilities.DotNet.Test
{
    public class ExceptionUtilitiesTest
    {
        [Fact]
        public void IgnoreExceptions_Action_NoException()
        {
            // Arrange

            bool canary = false;

            // Act

            var result = BypassExceptions( () =>
            {
                canary = true;
            } );

            // Assert

            Assert.True( canary );
            Assert.True( result );
        }

        [Fact]
        public void IgnoreExceptions_Action_All_Ignored()
        {
            // Arrange

            bool canary = false;

            // Act

            var result = BypassExceptions( () =>
            {
                canary = true;

                throw new InvalidOperationException();
            } );

            // Assert

            Assert.True( canary );
            Assert.False( result );
        }

        [Fact]
        public void IgnoreExceptions_Action_Specific_Ignored()
        {
            // Arrange

            bool canary = false;

            // Act

            var result = BypassExceptions<InvalidOperationException>( () =>
            {
                canary = true;

                throw new InvalidOperationException();
            } );

            // Assert

            Assert.True( canary );
            Assert.False( result );
        }

        [Fact]
        public void IgnoreExceptions_Action_Specific_NotIgnored()
        {
            // Arrange

            bool canary = false;

            // Act

            Assert.Throws<InvalidOperationException>( () =>
            {
                BypassExceptions<ArgumentException>( () =>
                {
                    canary = true;

                    throw new InvalidOperationException();
                } );
            } );

            // Assert

            Assert.True( canary );
        }

        [Fact]
        public void IgnoreExceptions_Function_NoException()
        {
            // Arrange

            bool canary = false;

            // Act

            var result = BypassExceptions( () =>
            {
                canary = true;

                return 0;
            }, -1 );

            // Assert

            Assert.True( canary );
            Assert.Equal( 0, result );
        }

        [Fact]
        public void IgnoreExceptions_Function_All_Ignored()
        {
            // Arrange

            bool canary = false;

            // Act

            var result = BypassExceptions( () =>
            {
                canary = true;

                throw new InvalidOperationException();
            }, -1 );

            // Assert

            Assert.True( canary );
            Assert.Equal( -1, result );
        }

        [Fact]
        public void IgnoreExceptions_Function_Specific_Ignored()
        {
            // Arrange

            bool canary = false;

            // Act

            var result = BypassExceptions<InvalidOperationException, int>( () =>
            {
                canary = true;

                throw new InvalidOperationException();
            }, -33 );

            // Assert

            Assert.True( canary );
            Assert.Equal( -33, result );
        }

        [Fact]
        public void IgnoreExceptions_Function_Specific_NotIgnored()
        {
            // Arrange

            bool canary = false;

            // Act

            Assert.Throws<InvalidOperationException>( () =>
            {
                BypassExceptions<ArgumentException, int>( () =>
                {
                    canary = true;

                    throw new InvalidOperationException();
                }, 333 );
            } );

            // Assert

            Assert.True( canary );
        }
    }
}
