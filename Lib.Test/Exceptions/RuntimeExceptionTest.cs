/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using Utilities.Net.Exceptions;
using Xunit;

namespace Utilities.Net.Test.Exceptions
{
    public class RuntimeExceptionTest
    {
        [Fact]
        public void New()
        {
            var message = "foo67";

            var exception = new RuntimeException( message );

            Assert.Equal( message, exception.Message );
        }

        [Fact]
        public void New_InnerException()
        {
            var message = "foo67";
            var innerException = new Exception( "chazz" );

            var exception = new RuntimeException( message, innerException );

            Assert.Equal( message, exception.Message );
            Assert.Equal( innerException, exception.InnerException );
        }
    }
}
