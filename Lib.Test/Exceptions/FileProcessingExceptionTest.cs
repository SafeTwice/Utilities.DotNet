/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using Utilities.Net.Exceptions;
using Xunit;

namespace Utilities.Net.Test.Exceptions
{
    public class FileProcessingExceptionTest
    {
        [Fact]
        public void New()
        {
            var exception = new FileProcessingException( "foo", "bar", 44 );

            Assert.Equal( "foo", exception.ShortMessage );
            Assert.Equal( "bar", exception.Filename );
            Assert.Equal( 44, exception.Line );
        }

        [Fact]
        public void New_InnerException()
        {
            var innerException = new Exception( "chazz" );
            var exception = new FileProcessingException( "foo", "bar", 55, innerException );

            Assert.Equal( "foo", exception.ShortMessage );
            Assert.Equal( "bar", exception.Filename );
            Assert.Equal( 55, exception.Line );
            Assert.Equal( innerException, exception.InnerException );
        }

        [Fact]
        public void Message_FilenameAndLine()
        {
            var exception = new FileProcessingException( "foo", "bar", 234 );

            Assert.Equal( "[bar @ Line 234] foo", exception.Message );
        }

        [Fact]
        public void Message_FilenameAndNoLine()
        {
            var exception = new FileProcessingException( "foo", "bar", 0 );

            Assert.Equal( "[bar] foo", exception.Message );
        }

        [Fact]
        public void Message_NoFilenameAndLine()
        {
            var exception = new FileProcessingException( "foo", null, 773 );

            Assert.Equal( "[Line 773] foo", exception.Message );
        }

        [Fact]
        public void Message_NoFilenameAndNoLine()
        {
            var exception = new FileProcessingException( "foo", null, 0 );

            Assert.Equal( "foo", exception.Message );
        }

    }
}
