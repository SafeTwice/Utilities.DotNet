/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Security;
using Xunit;

namespace Utilities.DotNet.Test
{
    public class SecureStringUtilitiesTest
    {
        [Fact]
        public void ToPlainString()
        {
            // Arrange

            var secureString = new SecureString();

            secureString.AppendChar( 'a' );
            secureString.AppendChar( 'h' );
            secureString.AppendChar( '#' );

            // Act

            var plainText = secureString.ToPlainString();

            // Assert

            Assert.Equal( "ah#", plainText );
        }

        [Theory]
        [InlineData( "ah#", "ah#", true )]
        [InlineData( "88345^98?23", "88345^98!2", false )]
        public void Comparison( string string1, string string2, bool expectedResult )
        {
            // Arrange

            var secureString1 = CreateSecureString( string1 );
            var secureString2 = CreateSecureString( string2 );

            // Act
            var result = secureString1.EqualTo( secureString2 );

            // Assert
            Assert.Equal( expectedResult, result );
        }

        private static SecureString CreateSecureString( string value )
        {
            var secureString = new SecureString();
            foreach( var c in value )
            {
                secureString.AppendChar( c );
            }
            return secureString;
        }
    }
}
