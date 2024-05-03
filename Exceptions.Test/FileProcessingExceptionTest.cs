/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;
using Utilities.DotNet.Exceptions;
using Xunit;

namespace Utilities.DotNet.Test.Exceptions
{
    public class FileProcessingExceptionTest
    {
        public FileProcessingExceptionTest()
        {
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
        }

        [Fact]
        public void FilenameAndLine()
        {
            var exception = new FileProcessingException( "foo11", "bar22", 234 );

            Assert.Equal( "foo11", exception.ShortMessage );
            Assert.Equal( "bar22", exception.Filename );
            Assert.Equal( 234, exception.Line );

            Assert.Null( exception.InnerException );

            Assert.Equal( "[bar22 @ Line 234] foo11", exception.Message );
        }

        [Fact]
        public void FilenameAndNoLine()
        {
            var exception = new FileProcessingException( "foo43", "bar54", 0 );

            Assert.Equal( "foo43", exception.ShortMessage );
            Assert.Equal( "bar54", exception.Filename );
            Assert.Equal( 0, exception.Line );

            Assert.Null( exception.InnerException );

            Assert.Equal( "[bar54] foo43", exception.Message );
        }

        [Fact]
        public void NoFilenameAndLine()
        {
            var exception = new FileProcessingException( "foo89", 773 );

            Assert.Equal( "foo89", exception.ShortMessage );
            Assert.Null( exception.Filename );
            Assert.Equal( 773, exception.Line );

            Assert.Null( exception.InnerException );

            Assert.Equal( "[Line 773] foo89", exception.Message );
        }

        [Fact]
        public void NoFilenameAndNoLine()
        {
            var exception = new FileProcessingException( "foo31", 0 );

            Assert.Equal( "foo31", exception.ShortMessage );
            Assert.Null( exception.Filename );
            Assert.Equal( 0, exception.Line );

            Assert.Null( exception.InnerException );

            Assert.Equal( "foo31", exception.Message );
        }

        [Fact]
        public void FilenameAndLine_InnerException()
        {
            var innerException = new Exception( "chazz" );
            var exception = new FileProcessingException( "foo90", "bar57", 55, innerException );

            Assert.Equal( "foo90", exception.ShortMessage );
            Assert.Equal( "bar57", exception.Filename );
            Assert.Equal( 55, exception.Line );

            Assert.Same( innerException, exception.InnerException );

            Assert.Equal( "[bar57 @ Line 55] foo90", exception.Message );
        }

        [Fact]
        public void NoFilenameAndLine_InnerException()
        {
            var innerException = new Exception( "chazz" );
            var exception = new FileProcessingException( "foo55",  5987765, innerException );

            Assert.Equal( "foo55", exception.ShortMessage );
            Assert.Null( exception.Filename );
            Assert.Equal( 5987765, exception.Line );

            Assert.Same( innerException, exception.InnerException );

            Assert.Equal( "[Line 5987765] foo55", exception.Message );
        }
    }
}
