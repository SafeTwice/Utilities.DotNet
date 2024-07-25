/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using Utilities.DotNet.Exceptions;
using Xunit;

namespace Utilities.DotNet.Test.Exceptions
{
    public class InvalidCastExceptionExTest
    {
        [Fact]
        public void New()
        {
            string obj = "foo";

            var exception = new InvalidCastExceptionEx( obj, typeof( Exception ) );

            Assert.Equal( "Cannot cast object 'foo' of type System.String to type System.Exception.", exception.Message );
            Assert.Same( obj, exception.Object );
            Assert.Equal( typeof( Exception ), exception.TargetType );
        }
    }
}
