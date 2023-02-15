/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Utilities.Net.Converters;
using Xunit;

namespace Utilities.Net.Test.Converters
{
    public class BitConverterTest
    {
        [Theory]
        [InlineData( new byte[] { 0x83, 0x07 }, 0, false, 1923u )]
        [InlineData( new byte[] { 0xFF, 0x22, 0xD2, 0xFE, 0x00 }, 2, false, 65234u )]
        [InlineData( new byte[] { 0x07, 0x82 }, 0, true, 1922u )]
        [InlineData( new byte[] { 0x00, 0xFE, 0xD0 }, 1, true, 65232u )]
        public void ToUInt16( byte[] inputValue, int startIndex, bool reversed, ushort expectedValue )
        {
            // Prepare

            var bitConverter = new BitConverter( reversed );

            // Execute

            var outputValue = bitConverter.ToUInt16( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x2D, 0xE0, 0x89, 0xDA }, 0, false, 3666468909u )]
        [InlineData( new byte[] { 0xD2, 0x2E, 0xE0, 0x89, 0xDA, 0xFE }, 1, false, 3666468910u )]
        [InlineData( new byte[] { 0xDA, 0x89, 0xE0, 0x2C }, 0, true, 3666468908u )]
        [InlineData( new byte[] { 0xFE, 0xD0, 0xDA, 0x89, 0xE0, 0x2A }, 2, true, 3666468906u )]
        public void ToUInt32( byte[] inputValue, int startIndex, bool reversed, uint expectedValue )
        {
            // Prepare

            var bitConverter = new BitConverter( reversed );

            // Execute

            var outputValue = bitConverter.ToUInt32( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x0E, 0xCF, 0xA6, 0x82, 0x2C, 0xE0, 0x89, 0xDA }, 0, false, 15747364053852802830ul )]
        [InlineData( new byte[] { 0xD2, 0x0F, 0xCF, 0xA6, 0x82, 0x2C, 0xE0, 0x89, 0xDA, 0xFE }, 1, false, 15747364053852802831ul )]
        [InlineData( new byte[] { 0xDA, 0x89, 0xE0, 0x2C, 0x82, 0xA6, 0xCF, 0x11 }, 0, true, 15747364053852802833ul )]
        [InlineData( new byte[] { 0x33, 0xFE, 0xD0, 0xDA, 0x89, 0xE0, 0x2C, 0x82, 0xA6, 0xCF, 0x10 }, 3, true, 15747364053852802832ul )]
        public void ToUInt64( byte[] inputValue, int startIndex, bool reversed, ulong expectedValue )
        {
            // Prepare

            var bitConverter = new BitConverter( reversed );

            // Execute

            var outputValue = bitConverter.ToUInt64( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0xD3, 0x7F }, 0, false, 32723 )]
        [InlineData( new byte[] { 0xFF, 0x22, 0x85, 0x83, 0x00 }, 2, false, -31867 )]
        [InlineData( new byte[] { 0x7F, 0xD5  }, 0, true, 32725 )]
        [InlineData( new byte[] { 0x00, 0x83, 0x87 }, 1, true, -31865 )]
        public void ToInt16( byte[] inputValue, int startIndex, bool reversed, short expectedValue )
        {
            // Prepare

            var bitConverter = new BitConverter( reversed );

            // Execute

            var outputValue = bitConverter.ToInt16( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0xA7, 0x87, 0xD3, 0x7F }, 0, false, 2144569255 )]
        [InlineData( new byte[] { 0xFF, 0x22, 0x6A, 0xDC, 0x78, 0x85, 0x00 }, 2, false, -2055676822 )]
        [InlineData( new byte[] { 0x7F, 0xD3, 0x87, 0xA5 }, 0, true, 2144569253 )]
        [InlineData( new byte[] { 0x00, 0x85, 0x78, 0xDC, 0x67 }, 1, true, -2055676825 )]
        public void ToInt32( byte[] inputValue, int startIndex, bool reversed, int expectedValue )
        {
            // Prepare

            var bitConverter = new BitConverter( reversed );

            // Execute

            var outputValue = bitConverter.ToInt32( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x75, 0x9B, 0x2C, 0xF2, 0xFA, 0x68, 0xD8, 0x72 }, 0, false, 8275479742307408757L )]
        [InlineData( new byte[] { 0xFF, 0x22, 0x6A, 0xDC, 0x78, 0x85, 0x70, 0x9B, 0x2C, 0xF2, 0xFA, 0x68, 0xD8, 0x82, 0x00 }, 6, false, -9018342826795295888L )]
        [InlineData( new byte[] { 0x72, 0xD8, 0x68, 0xFA, 0xF2, 0x2C, 0x9B, 0x76 }, 0, true, 8275479742307408758L )]
        [InlineData( new byte[] { 0x00, 0x85, 0x78, 0x82, 0xD8, 0x68, 0xFA, 0xF2, 0x2C, 0x9B, 0x76, 0xDC, 0x67 }, 3, true, -9018342826795295882L )]
        public void ToInt64( byte[] inputValue, int startIndex, bool reversed, long expectedValue )
        {
            // Prepare

            var bitConverter = new BitConverter( reversed );

            // Execute

           var outputValue = bitConverter.ToInt64( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x43, 0xE9, 0xE3, 0xD7 }, 0, true, 467.78f )]
        [InlineData( new byte[] { 0x00, 0x00, 0x63, 0xDF, 0x95, 0xC7, 0x00 }, 2, false, -76734.776f )]
        public void ToSingle( byte[] inputValue, int startIndex, bool reversed, float expectedValue )
        {
            // Prepare

            var bitConverter = new BitConverter( reversed );

            // Execute

            var outputValue = bitConverter.ToSingle( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x40, 0x7D, 0x3C, 0x7A, 0xE1, 0x47, 0xAE, 0x14 }, 0, true, 467.78 )]
        [InlineData( new byte[] { 0x00, 0xDB, 0xF9, 0x7E, 0x6A, 0xEC, 0xBB, 0xF2, 0xC0, 0xFF }, 1, false, -76734.776 )]
        public void ToDouble( byte[] inputValue, int startIndex, bool reversed, double expectedValue )
        {
            // Prepare

            var bitConverter = new BitConverter( reversed );

            // Execute

            var outputValue = bitConverter.ToDouble( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }
    }
}
