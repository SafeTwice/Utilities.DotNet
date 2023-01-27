﻿/// @file
/// @copyright  Copyright (c) 2019-2021 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using Xunit;

namespace Utilities.Net.Test
{
    public class ArrayUtilitiesTest
    {
        [Fact]
        public void CreateArray()
        {
            var array = ArrayUtilities.CreateArray<int>( 55, 8958 );

            Assert.Equal( 55, array.Length );
            foreach( var item in array )
            {
                Assert.Equal( 8958, item );
            }
        }

        [Fact]
        public void FillArray()
        {
            var array = new long[ 89 ];

            array.Fill( 543898876L );

            Assert.Equal( 89, array.Length );
            foreach( var item in array )
            {
                Assert.Equal( 543898876L, item );
            }
        }

        [Fact]
        public void ShallowClone()
        {
            var initialArray = new DateTime[ 34 ];
            for( int i = 0; i < initialArray.Length; i++ )
            {
                initialArray[ i ] = DateTime.Now.AddDays( i );
            }

            var clonedArray = initialArray.ShallowClone();

            Assert.Equal( initialArray.Length, clonedArray.Length );
            for( int i = 0; i < initialArray.Length; i++ )
            {
                Assert.Equal( initialArray[ i ], clonedArray[ i ] );
            }
        }

        [Theory]
        [InlineData( new byte[] { 0x46, 0x6F, 0x6F }, "Foo" )]
        [InlineData( new byte[] { 0x46, 0x6F, 0x6F, 0x00, 0x46, 0x00 }, "Foo" )]
        [InlineData( new byte[] { 0x00, 0x46, 0x6F, 0x6F }, "" )]
        public void DecodeASCII( byte[] input, string expectedOutput )
        {
            var result = input.DecodeASCII();

            Assert.Equal( expectedOutput, result );
        }

        [Fact]
        public void EncodeASCII()
        {
            Assert.Equal( new byte[] { 0x46, 0x6F, 0x6F }, "Foo".EncodeASCII() );
        }
    }
}