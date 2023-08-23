/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Utilities.DotNet.Converters;
using Xunit;

namespace Utilities.DotNet.Test.Converters
{
    public class LittleEndianBitConverterTest
    {
        [Theory]
        [InlineData( new byte[] { 0x83, 0x07 }, 0, 1923u )]
        [InlineData( new byte[] { 0xFF, 0x22, 0xD2, 0xFE, 0x00 }, 2, 65234u )]
        public void ToUInt16( byte[] inputValue, int startIndex, ushort expectedValue )
        {
            // Execute

            var outputValue = LittleEndianBitConverter.ToUInt16( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x2D, 0xE0, 0x89, 0xDA }, 0, 3666468909u )]
        [InlineData( new byte[] { 0xD2, 0x2E, 0xE0, 0x89, 0xDA, 0xFE }, 1, 3666468910u )]
        public void ToUInt32( byte[] inputValue, int startIndex, uint expectedValue )
        {
            // Execute

            var outputValue = LittleEndianBitConverter.ToUInt32( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x0E, 0xCF, 0xA6, 0x82, 0x2C, 0xE0, 0x89, 0xDA }, 0, 15747364053852802830ul )]
        [InlineData( new byte[] { 0xD2, 0x0F, 0xCF, 0xA6, 0x82, 0x2C, 0xE0, 0x89, 0xDA, 0xFE }, 1, 15747364053852802831ul )]
        public void ToUInt64( byte[] inputValue, int startIndex, ulong expectedValue )
        {
            // Execute

            var outputValue = LittleEndianBitConverter.ToUInt64( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0xD3, 0x7F }, 0, 32723 )]
        [InlineData( new byte[] { 0xFF, 0x22, 0x85, 0x83, 0x00 }, 2, -31867 )]
        public void ToInt16( byte[] inputValue, int startIndex, short expectedValue )
        {
            // Execute

            var outputValue = LittleEndianBitConverter.ToInt16( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0xA7, 0x87, 0xD3, 0x7F }, 0, 2144569255 )]
        [InlineData( new byte[] { 0xFF, 0x22, 0x6A, 0xDC, 0x78, 0x85, 0x00 }, 2, -2055676822 )]
        public void ToInt32( byte[] inputValue, int startIndex, int expectedValue )
        {
            // Execute

            var outputValue = LittleEndianBitConverter.ToInt32( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x75, 0x9B, 0x2C, 0xF2, 0xFA, 0x68, 0xD8, 0x72 }, 0, 8275479742307408757L )]
        [InlineData( new byte[] { 0xFF, 0x22, 0x6A, 0xDC, 0x78, 0x85, 0x70, 0x9B, 0x2C, 0xF2, 0xFA, 0x68, 0xD8, 0x82, 0x00 }, 6, -9018342826795295888L )]
        public void ToInt64( byte[] inputValue, int startIndex, long expectedValue )
        {
            // Execute

           var outputValue = LittleEndianBitConverter.ToInt64( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x00, 0x00, 0x63, 0xDF, 0x95, 0xC7, 0x00 }, 2, -76734.776f )]
        public void ToSingle( byte[] inputValue, int startIndex, float expectedValue )
        {
            // Execute

            var outputValue = LittleEndianBitConverter.ToSingle( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x00, 0xDB, 0xF9, 0x7E, 0x6A, 0xEC, 0xBB, 0xF2, 0xC0, 0xFF }, 1, -76734.776 )]
        public void ToDouble( byte[] inputValue, int startIndex, double expectedValue )
        {
            // Execute

            var outputValue = LittleEndianBitConverter.ToDouble( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }
    }
}
