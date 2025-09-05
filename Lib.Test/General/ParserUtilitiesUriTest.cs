/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using Xunit;

namespace Utilities.DotNet.Test
{
    public class ParserUtilitiesUriTest
    {
        [Theory]
        [InlineData( "https://www.example.com:8080/path", "https", "www.example.com", 8080, "/path" )]
        [InlineData( "http://www.foo.com/path2", "http", "www.foo.com", 80, "/path2" )]
        [InlineData( "ftp://www.bar.com", "ftp", "www.bar.com", 21, "/" )]
        [InlineData( "www.baz.com/foobar", "https", "www.baz.com", 443, "/foobar" )]
        public void TryParseUri( string input, string expectedScheme, string expectedHost, int expectedPort, string expectedPath )
        {
            // Act

            var result = ParserUtilities.TryParseUri( input, out var uri );

            // Assert

            Assert.True( result );
            Assert.NotNull( uri );
            Assert.Equal( expectedScheme, uri.Scheme );
            Assert.Equal( expectedHost, uri.Host );
            Assert.Equal( expectedPort, uri.Port );
            Assert.Equal( expectedPath, uri.PathAndQuery );
        }

        [Theory]
        [InlineData( "https:www.example.com:8080/path" )]
        [InlineData( ":www.foo.com/path" )]
        [InlineData( "www.bar.com:802832" )]
        public void TryParseUri_InvalidUri( string input )
        {
            // Act

            var result = ParserUtilities.TryParseUri( input, out var uri );

            // Assert

            Assert.False( result );
            Assert.Null( uri );
        }

        [Theory]
        [InlineData( "https://www.example.com:8080/path", "https", "www.example.com", 8080, "/path" )]
        [InlineData( "http://www.foo.com/path2", "http", "www.foo.com", 80, "/path2" )]
        [InlineData( "ftp://www.bar.com", "ftp", "www.bar.com", 21, "/" )]
        [InlineData( "www.baz.com/foobar", "poo", "www.baz.com", -1, "/foobar" )]
        public void TryParseUri_NonDefaultScheme( string input, string expectedScheme, string expectedHost, int expectedPort, string expectedPath )
        {
            // Act

            var result = ParserUtilities.TryParseUri( input, out var uri, defaultScheme: "poo" );

            // Assert

            Assert.True( result );
            Assert.NotNull( uri );
            Assert.Equal( expectedScheme, uri.Scheme );
            Assert.Equal( expectedHost, uri.Host );
            Assert.Equal( expectedPort, uri.Port );
            Assert.Equal( expectedPath, uri.PathAndQuery );
        }

        [Theory]
        [InlineData( "https://www.example.com:8080/path", "https", "www.example.com", 8080, "/path" )]
        [InlineData( "http://www.foo.com/path2", "http", "www.foo.com", 555, "/path2" )]
        [InlineData( "ftp://www.bar.com", "ftp", "www.bar.com", 555, "/" )]
        [InlineData( "www.baz.com/foobar", "https", "www.baz.com", 555, "/foobar" )]
        public void TryParseUri_NonDefaultPort( string input, string expectedScheme, string expectedHost, int expectedPort, string expectedPath )
        {
            // Act

            var result = ParserUtilities.TryParseUri( input, out var uri, defaultPort: 555 );

            // Assert

            Assert.True( result );
            Assert.NotNull( uri );
            Assert.Equal( expectedScheme, uri.Scheme );
            Assert.Equal( expectedHost, uri.Host );
            Assert.Equal( expectedPort, uri.Port );
            Assert.Equal( expectedPath, uri.PathAndQuery );
        }

        [Theory]
        [InlineData( "https://www.example.com:8080/path", "https", "www.example.com", 8080, "/path" )]
        [InlineData( "http://www.foo.com/path2", "http", "www.foo.com", 80, "/path2" )]
        [InlineData( "ftp://www.bar.com", "ftp", "www.bar.com", 21, "/" )]
        [InlineData( "www.baz.com/foobar", "https", "www.baz.com", 443, "/foobar" )]
        public void ParseUri( string input, string expectedScheme, string expectedHost, int expectedPort, string expectedPath )
        {
            // Act

            var uri = ParserUtilities.ParseUri( input );

            // Assert

            Assert.Equal( expectedScheme, uri.Scheme );
            Assert.Equal( expectedHost, uri.Host );
            Assert.Equal( expectedPort, uri.Port );
            Assert.Equal( expectedPath, uri.PathAndQuery );
        }

        [Theory]
        [InlineData( "https:www.example.com:8080/path" )]
        [InlineData( ":www.foo.com/path" )]
        [InlineData( "www.bar.com:802832" )]
        public void ParseUri_InvalidUri( string input )
        {
            // Act & Assert

            Assert.Throws<ArgumentException>( () => ParserUtilities.ParseUri( input ) );
        }
    }
}
