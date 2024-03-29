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
        public void CreateArray_Generator_NoParam()
        {
            int x = 777;
            var array = ArrayUtilities.CreateArray<int>( 78, () => x++ );

            Assert.Equal( 78, array.Length );
            for( int i = 0; i < array.Length; i++ )
            {
                Assert.Equal( 777 + i, array[ i ] );
            }
        }

        [Fact]
        public void CreateArray_Generator_WithParam()
        {
            var array = ArrayUtilities.CreateArray<int>( 34, ( n ) => ( n * 2 ) );

            Assert.Equal( 34, array.Length );
            for( int i = 0; i < array.Length; i++ )
            {
                Assert.Equal( i * 2, array[ i ] );
            }
        }

        [Fact]
        public void FillArray_Subarray()
        {
            var array = new long[ 75 ];

            array.Fill( 543898876L, 10, 25 );

            Assert.Equal( 75, array.Length );
            for( int i = 0; i < array.Length; i++ )
            {
                if( ( i < 10 ) || ( i >= 35 ) )
                {
                    Assert.Equal( 0L, array[ i ] );
                }
                else
                {
                    Assert.Equal( 543898876L, array[ i ] );
                }
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
        public void Subarray()
        {
            var inputArray = new int[] { 5, 234, -35, 4, 33 };
            var expectedSubarray = new int[] { 234, -35, 4 };

            var subarray = inputArray.Subarray( 1, 3 );

            Assert.Equal( expectedSubarray, subarray );
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

        [Fact]
        public void ReversedSubarray()
        {
            var inputArray = new int[] { 5, 234, -35, 4, 33 };
            var expectedSubarray = new int[] { 4, -35, 234 };

            var subarray = inputArray.ReversedSubarray( 1, 3 );

            Assert.Equal( expectedSubarray, subarray );
        }

        [Fact]
        public void Reverse()
        {
            var initialArray = new DateTime[ 23 ];
            for( int i = 0; i < initialArray.Length; i++ )
            {
                initialArray[ i ] = DateTime.Now.AddDays( i );
            }

            var reversedArray = initialArray.ReversedShallowClone();

            Assert.Equal( initialArray.Length, reversedArray.Length );
            for( int i = 0; i < initialArray.Length; i++ )
            {
                Assert.Equal( initialArray[ reversedArray.Length - 1 - i ], reversedArray[ i ] );
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

        [Theory]
        [InlineData( new byte[] { 0x46, 0x6F, 0x6F }, "", "466F6F" )]
        [InlineData( new byte[] { 0x46, 0x6F, 0x6F, 0x00, 0x46, 0x00 }, " ", "46 6F 6F 00 46 00" )]
        [InlineData( new byte[] { 0x00, 0x46, 0x6F, 0x6F }, ".", "00.46.6F.6F" )]
        [InlineData( new byte[] { }, ".", "" )]
        public void ToHexString( byte[] input, string separator, string expectedOutput )
        {
            var result = input.ToHexString( separator );
            Assert.Equal( expectedOutput, result );
        }
    }
}
