/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Utilities.DotNet.Converters;
using Xunit;

namespace Utilities.DotNet.Test.Converters
{
    public class BigEndianBitConverterTest
    {
        [Theory]
        [InlineData( new byte[] { 0x07, 0x82 }, 0,  1922u )]
        [InlineData( new byte[] { 0x00, 0xFE, 0xD0 }, 1,  65232u )]
        public void ToUInt16( byte[] inputValue, int startIndex, ushort expectedValue )
        {
            // Execute

            var outputValue = BigEndianBitConverter.ToUInt16( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0xDA, 0x89, 0xE0, 0x2C }, 0, 3666468908u )]
        [InlineData( new byte[] { 0xFE, 0xD0, 0xDA, 0x89, 0xE0, 0x2A }, 2, 3666468906u )]
        public void ToUInt32( byte[] inputValue, int startIndex, uint expectedValue )
        {
            // Execute

            var outputValue = BigEndianBitConverter.ToUInt32( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0xDA, 0x89, 0xE0, 0x2C, 0x82, 0xA6, 0xCF, 0x11 }, 0, 15747364053852802833ul )]
        [InlineData( new byte[] { 0x33, 0xFE, 0xD0, 0xDA, 0x89, 0xE0, 0x2C, 0x82, 0xA6, 0xCF, 0x10 }, 3, 15747364053852802832ul )]
        public void ToUInt64( byte[] inputValue, int startIndex, ulong expectedValue )
        {
            // Execute

            var outputValue = BigEndianBitConverter.ToUInt64( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x7F, 0xD5  }, 0, 32725 )]
        [InlineData( new byte[] { 0x00, 0x83, 0x87 }, 1, -31865 )]
        public void ToInt16( byte[] inputValue, int startIndex, short expectedValue )
        {
            // Execute

            var outputValue = BigEndianBitConverter.ToInt16( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x7F, 0xD3, 0x87, 0xA5 }, 0, 2144569253 )]
        [InlineData( new byte[] { 0x00, 0x85, 0x78, 0xDC, 0x67 }, 1, -2055676825 )]
        public void ToInt32( byte[] inputValue, int startIndex, int expectedValue )
        {
            // Execute

            var outputValue = BigEndianBitConverter.ToInt32( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x72, 0xD8, 0x68, 0xFA, 0xF2, 0x2C, 0x9B, 0x76 }, 0, 8275479742307408758L )]
        [InlineData( new byte[] { 0x00, 0x85, 0x78, 0x82, 0xD8, 0x68, 0xFA, 0xF2, 0x2C, 0x9B, 0x76, 0xDC, 0x67 }, 3, -9018342826795295882L )]
        public void ToInt64( byte[] inputValue, int startIndex, long expectedValue )
        {
            // Execute

           var outputValue = BigEndianBitConverter.ToInt64( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x43, 0xE9, 0xE3, 0xD7 }, 0, 467.78f )]
        public void ToSingle( byte[] inputValue, int startIndex, float expectedValue )
        {
            // Execute

            var outputValue = BigEndianBitConverter.ToSingle( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }

        [Theory]
        [InlineData( new byte[] { 0x40, 0x7D, 0x3C, 0x7A, 0xE1, 0x47, 0xAE, 0x14 }, 0, 467.78 )]
        public void ToDouble( byte[] inputValue, int startIndex, double expectedValue )
        {
            // Execute

            var outputValue = BigEndianBitConverter.ToDouble( inputValue, startIndex );

            // Verify

            Assert.Equal( expectedValue, outputValue );
        }
    }
}
