/// @file
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

            ArrayUtilities.Fill( array, 543898876L );

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

            var clonedArray = ArrayUtilities.ShallowClone( initialArray );

            Assert.Equal( initialArray.Length, clonedArray.Length );
            for( int i = 0; i < initialArray.Length; i++ )
            {
                Assert.Equal( initialArray[ i ], clonedArray[ i ] );
            }
        }
    }
}
