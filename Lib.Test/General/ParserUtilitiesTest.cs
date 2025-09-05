/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Globalization;
using Xunit;

namespace Utilities.DotNet.Test
{
    public class ParserUtilitiesTest : IDisposable
    {
        private readonly CultureInfo m_originalCulture;

        public ParserUtilitiesTest()
        {
            m_originalCulture = CultureInfo.CurrentCulture;
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = m_originalCulture;
        }

        //***************************************************************************
        //                               Int32
        //***************************************************************************

        [Theory]
        [InlineData( "9234", EIntParseOptions.Default, "en-us", 9234 )]
        [InlineData( "-345", EIntParseOptions.Default, "en-us", -345 )]
        [InlineData( " -82345", EIntParseOptions.Default, "en-us", -82345 )]
        [InlineData( "273   ", EIntParseOptions.Default, "en-us", 273 )]
        [InlineData( "   62346   ", EIntParseOptions.Default, "en-us", 62346 )]
        [InlineData( "9234", EIntParseOptions.AnyNumber, "en-us", 9234 )]
        [InlineData( "-7734", EIntParseOptions.AnyNumber, "en-us", -7734 )]
        [InlineData( "0xCAFE123", EIntParseOptions.AnyNumber, "en-us", 0xCAFE123 )]
        [InlineData( "0XBEEF", EIntParseOptions.AnyNumber, "en-us", 0XBEEF )]
#if NET8_0_OR_GREATER
        [InlineData( "0b100101", EIntParseOptions.AnyNumber, "en-us", 0b100101 )]
        [InlineData( "0B1100010100101", EIntParseOptions.AnyNumber, "en-us", 0b1100010100101 )]
#endif
        [InlineData( "345,446", EIntParseOptions.AllowThousands, "en-us", 345446 )]
        [InlineData( "345.446", EIntParseOptions.AllowThousands, "es", 345446 )]
        [InlineData( "(345)", EIntParseOptions.AllowParentheses, "en-us", -345 )]
        [InlineData( "345-", EIntParseOptions.AllowTrailingSign, "en-us", -345 )]
        public void ParseInt_Valid( string input, EIntParseOptions parseOptions, string cultureName, int expectedResult )
        {
            // Arrange

            var culture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            var result = ParserUtilities.ParseInt( input, parseOptions, culture );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( "", EIntParseOptions.Default, "en-us" )]
        [InlineData( "ABC", EIntParseOptions.Default, "en-us" )]
        [InlineData( "(345)", EIntParseOptions.Default, "en-us" )]
        [InlineData( "345-", EIntParseOptions.Default, "en-us" )]
        [InlineData( "0x1234", EIntParseOptions.Default, "en-us" )]
        [InlineData( "0b1001", EIntParseOptions.Default, "en-us" )]
        [InlineData( "345,446", EIntParseOptions.Default, "en-us" )]
        [InlineData( "345.446", EIntParseOptions.Default, "es" )]
        public void ParseInt_InvalidFormat( string input, EIntParseOptions parseOptions, string cultureName )
        {
            // Arrange

            var culture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            Assert.Throws<FormatException>( () => ParserUtilities.ParseInt( input, parseOptions, culture ) );
        }

        [Theory]
        [InlineData( "9917235542712", EIntParseOptions.Default, "en-us" )]
        [InlineData( "-9917235542712", EIntParseOptions.Default, "en-us" )]
        public void ParseInt_Overflow( string input, EIntParseOptions parseOptions, string cultureName )
        {
            // Arrange

            var culture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            Assert.Throws<OverflowException>( () => ParserUtilities.ParseInt( input, parseOptions, culture ) );
        }

        [Theory]
        [InlineData( "9234", EIntParseOptions.Default, "fr", 9234 )]
        [InlineData( "345,446", EIntParseOptions.AllowThousands, "en-us", 345446 )]
        [InlineData( "764.542", EIntParseOptions.AllowThousands, "es", 764542 )]
        public void ParseInt_CurrentCulture( string input, EIntParseOptions parseOptions, string cultureName, int expectedResult )
        {
            // Arrange

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            var result = ParserUtilities.ParseInt( input, parseOptions );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( "-9234", EIntParseOptions.Default, -9234 )]
        [InlineData( "345,446", EIntParseOptions.AllowThousands, 345446 )]
        public void ParseInvariantInt( string input, EIntParseOptions parseOptions, int expectedResult )
        {
            // Act

            var result = ParserUtilities.ParseInvariantInt( input, parseOptions );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( "9234", EIntParseOptions.Default, "en-us", 9234 )]
        [InlineData( "-345", EIntParseOptions.Default, "en-us", -345 )]
        [InlineData( " -82345", EIntParseOptions.Default, "en-us", -82345 )]
        [InlineData( "273   ", EIntParseOptions.Default, "en-us", 273 )]
        [InlineData( "   62346   ", EIntParseOptions.Default, "en-us", 62346 )]
        [InlineData( "9234 ", EIntParseOptions.AnyNumber, "en-us", 9234 )]
        [InlineData( " -7734", EIntParseOptions.AnyNumber, "en-us", -7734 )]
        [InlineData( " 0xCAFE123", EIntParseOptions.AnyNumber, "en-us", 0xCAFE123 )]
        [InlineData( " 0XBEEF ", EIntParseOptions.AnyNumber, "en-us", 0XBEEF )]
#if NET8_0_OR_GREATER
        [InlineData( "0b100101", EIntParseOptions.AnyNumber, "en-us", 0b100101 )]
        [InlineData( " 0B1100010100101", EIntParseOptions.AnyNumber, "en-us", 0b1100010100101 )]
#endif
        [InlineData( "345,446", EIntParseOptions.AllowThousands, "en-us", 345446 )]
        [InlineData( "345.446", EIntParseOptions.AllowThousands, "es", 345446 )]
        [InlineData( "(345)", EIntParseOptions.AllowParentheses, "en-us", -345 )]
        [InlineData( "345-", EIntParseOptions.AllowTrailingSign, "en-us", -345 )]
        public void TryParseInt_Valid( string input, EIntParseOptions parseOptions, string cultureName, int expectedValue )
        {
            // Arrange

            var culture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            var result = ParserUtilities.TryParseInt( input, parseOptions, culture, out var parsedValue );

            // Assert

            Assert.True( result );
            Assert.Equal( expectedValue, parsedValue );
        }

        [Theory]
        [InlineData( "", EIntParseOptions.Default, "en-us" )]
        [InlineData( "9917235542712", EIntParseOptions.Default, "en-us" )]
        [InlineData( "-9917235542712", EIntParseOptions.Default, "en-us" )]
        [InlineData( "ABC", EIntParseOptions.Default, "en-us" )]
        [InlineData( "(345)", EIntParseOptions.Default, "en-us" )]
        [InlineData( "345-", EIntParseOptions.Default, "en-us" )]
        [InlineData( "0x1234", EIntParseOptions.Default, "en-us" )]
        [InlineData( "0b1001", EIntParseOptions.Default, "en-us" )]
        [InlineData( "345,446", EIntParseOptions.Default, "en-us" )]
        [InlineData( "345.446", EIntParseOptions.Default, "es" )]
        [InlineData( " 0x1234", EIntParseOptions.AllowPrefix, "en-us" )]
        public void TryParseInt_Invalid( string input, EIntParseOptions parseOptions, string cultureName )
        {
            // Arrange

            var culture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            var result = ParserUtilities.TryParseInt( input, parseOptions, culture, out var parsedValue );

            // Assert

            Assert.False( result );
            Assert.Equal( 0, parsedValue );
        }

        [Theory]
        [InlineData( "9234", EIntParseOptions.Default, "fr", 9234 )]
        [InlineData( "345,446", EIntParseOptions.AllowThousands, "en-us", 345446 )]
        [InlineData( "764.542", EIntParseOptions.AllowThousands, "es", 764542 )]
        public void TryParseInt_CurrentCulture( string input, EIntParseOptions parseOptions, string cultureName, int expectedValue )
        {
            // Arrange

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            var result = ParserUtilities.TryParseInt( input, parseOptions, out var parsedValue );

            // Assert

            Assert.True( result );
            Assert.Equal( expectedValue, parsedValue );
        }

        [Theory]
        [InlineData( "-9234", EIntParseOptions.Default, true, -9234 )]
        [InlineData( "345,446", EIntParseOptions.AllowThousands, true, 345446 )]
        [InlineData( "345.446", EIntParseOptions.AllowThousands, false, 0 )]
        public void TryParseInvariantInt( string input, EIntParseOptions parseOptions, bool expectedResult, int expectedValue )
        {
            // Act

            var result = ParserUtilities.TryParseInvariantInt( input, parseOptions, out var parsedValue );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedValue, parsedValue );
        }

        //***************************************************************************
        //                               UInt32
        //***************************************************************************

        [Theory]
        [InlineData( "9234", EUIntParseOptions.Default, "en-us", 9234 )]
        [InlineData( "273   ", EUIntParseOptions.Default, "en-us", 273 )]
        [InlineData( "   62346   ", EUIntParseOptions.Default, "en-us", 62346 )]
        [InlineData( "345,446", EUIntParseOptions.AllowThousands, "en-us", 345446 )]
        [InlineData( "345.446", EUIntParseOptions.AllowThousands, "es", 345446 )]
        [InlineData( "9234", EUIntParseOptions.AnyNumber, "en-us", 9234 )]
        [InlineData( "0xCAFE1234", EUIntParseOptions.AnyNumber, "en-us", 0xCAFE1234u )]
        [InlineData( "0XBEEF", EUIntParseOptions.AnyNumber, "en-us", 0XBEEF )]
#if NET8_0_OR_GREATER
        [InlineData( "0b100101", EUIntParseOptions.AnyNumber, "en-us", 0b100101 )]
        [InlineData( "0B1100010100101", EUIntParseOptions.AnyNumber, "en-us", 0b1100010100101 )]
#endif
        public void ParseUInt_Valid( string input, EUIntParseOptions parseOptions, string cultureName, uint expectedResult )
        {
            // Arrange

            var culture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            var result = ParserUtilities.ParseUInt( input, parseOptions, culture );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( "", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "ABC", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "-345", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "(345)", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "345-", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "0x1234", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "0b1001", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "345,446", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "345.446", EUIntParseOptions.Default, "es" )]
        public void ParseUInt_InvalidFormat( string input, EUIntParseOptions parseOptions, string cultureName )
        {
            // Arrange

            var culture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            Assert.Throws<FormatException>( () => ParserUtilities.ParseUInt( input, parseOptions, culture ) );
        }

        [Theory]
        [InlineData( "9917235542712", EUIntParseOptions.Default, "en-us" )]
        public void ParseUInt_Overflow( string input, EUIntParseOptions parseOptions, string cultureName )
        {
            // Arrange

            var culture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            Assert.Throws<OverflowException>( () => ParserUtilities.ParseUInt( input, parseOptions, culture ) );
        }

        [Theory]
        [InlineData( "9234", EUIntParseOptions.Default, "fr", 9234 )]
        [InlineData( "345,446", EUIntParseOptions.AllowThousands, "en-us", 345446 )]
        [InlineData( "764.542", EUIntParseOptions.AllowThousands, "es", 764542 )]
        public void ParseUInt_CurrentCulture( string input, EUIntParseOptions parseOptions, string cultureName, uint expectedResult )
        {
            // Arrange

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            var result = ParserUtilities.ParseUInt( input, parseOptions );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( "9234", EUIntParseOptions.Default, 9234 )]
        [InlineData( "345,446", EUIntParseOptions.AllowThousands, 345446 )]
        public void ParseInvariantUInt( string input, EUIntParseOptions parseOptions, uint expectedResult )
        {
            // Act

            var result = ParserUtilities.ParseInvariantUInt( input, parseOptions );

            // Assert

            Assert.Equal( expectedResult, result );
        }

        [Theory]
        [InlineData( "9234", EUIntParseOptions.Default, "en-us", 9234 )]
        [InlineData( "273   ", EUIntParseOptions.Default, "en-us", 273 )]
        [InlineData( "   62346   ", EUIntParseOptions.Default, "en-us", 62346 )]
        [InlineData( "345,446", EUIntParseOptions.AllowThousands, "en-us", 345446 )]
        [InlineData( "345.446", EUIntParseOptions.AllowThousands, "es", 345446 )]
        [InlineData( "9234", EUIntParseOptions.AnyNumber, "en-us", 9234 )]
        [InlineData( "0xCAFE1234", EUIntParseOptions.AnyNumber, "en-us", 0xCAFE1234u )]
        [InlineData( "0XBEEF", EUIntParseOptions.AnyNumber, "en-us", 0XBEEF )]
#if NET8_0_OR_GREATER
        [InlineData( "0b100101", EUIntParseOptions.AnyNumber, "en-us", 0b100101 )]
        [InlineData( "0B1100010100101", EUIntParseOptions.AnyNumber, "en-us", 0b1100010100101 )]
#endif
        public void TryParseUInt_Valid( string input, EUIntParseOptions parseOptions, string cultureName, uint expectedValue )
        {
            // Arrange

            var culture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            var result = ParserUtilities.TryParseUInt( input, parseOptions, culture, out var parsedValue );

            // Assert

            Assert.True( result );
            Assert.Equal( expectedValue, parsedValue );
        }

        [Theory]
        [InlineData( "", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "9917235542712", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "ABC", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "-345", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "(345)", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "345-", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "0x1234", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "0b1001", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "345,446", EUIntParseOptions.Default, "en-us" )]
        [InlineData( "345.446", EUIntParseOptions.Default, "es" )]
        public void TryParseUInt_Invalid( string input, EUIntParseOptions parseOptions, string cultureName )
        {
            // Arrange

            var culture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            var result = ParserUtilities.TryParseUInt( input, parseOptions, culture, out var parsedValue );

            // Assert

            Assert.False( result );
            Assert.Equal( 0u, parsedValue );
        }

        [Theory]
        [InlineData( "9234", EUIntParseOptions.Default, "fr", 9234 )]
        [InlineData( "345,446", EUIntParseOptions.AllowThousands, "en-us", 345446 )]
        [InlineData( "764.542", EUIntParseOptions.AllowThousands, "es", 764542 )]
        public void TryParseUInt_CurrentCulture( string input, EUIntParseOptions parseOptions, string cultureName, uint expectedValue )
        {
            // Arrange

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo( cultureName );

            // Act

            var result = ParserUtilities.TryParseUInt( input, parseOptions, out var parsedValue );

            // Assert

            Assert.True( result );
            Assert.Equal( expectedValue, parsedValue );
        }

        [Theory]
        [InlineData( "9234", EUIntParseOptions.Default, true, 9234 )]
        [InlineData( "345,446", EUIntParseOptions.AllowThousands, true, 345446 )]
        [InlineData( "345.446", EUIntParseOptions.AllowThousands, false, 0 )]
        public void TryParseInvariantUInt( string input, EUIntParseOptions parseOptions, bool expectedResult, uint expectedValue )
        {
            // Act

            var result = ParserUtilities.TryParseInvariantUInt( input, parseOptions, out var parsedValue );

            // Assert

            Assert.Equal( expectedResult, result );
            Assert.Equal( expectedValue, parsedValue );
        }
    }
}
