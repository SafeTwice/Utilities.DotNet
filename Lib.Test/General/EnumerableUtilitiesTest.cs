/// @file
/// @copyright  Copyright (c) 2019-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.DotNet.Exceptions;
using Xunit;

namespace Utilities.DotNet.Test
{
    public class EnumerableUtilitiesTest
    {
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

        [Fact]
        public void ForEach()
        {
            // Arrange

            var input = new int[] { 1, 2, 3, 4, 5 };
            var output = new List<int>();

            var expectedOutput = new int[] { 2, 3, 4, 5, 6 };

            // Act

            input.ForEach( x => output.Add( x + 1 ) );

            // Assert

            Assert.Equal( expectedOutput, output );
        }

        [Fact]
        public void ForEachWithParameter()
        {
            // Arrange

            var input = new int[] { 1, 2, 3, 4, 5 };
            var parameters = new int[] { 2, 3, 15, 5, 0 };
            var output = new List<int>();

            var expectedOutput = new int[] { 2, 6, 45, 20, 0 };

            // Act

            input.ForEach( ( x, y ) => output.Add( x * y ), parameters );

            // Assert

            Assert.Equal( expectedOutput, output );
        }

        [Fact]
        public void ForEachWithParameters_InvalidParametersLength()
        {
            // Arrange

            var input = new int[] { 1, 2, 3, 4, 5 };
            var parameters = new double[] { 2.0, 3.3, 1.5, 0.5 };

            // Act & Assert

            Assert.Throws<ArgumentException>( () => input.ForEach( ( x, y ) => { }, parameters ) );
        }

        [Fact]
        public void Select()
        {
            // Arrange

            var input = new int[] { 1, 2, 3, 4, 5 };
            var parameters = new int[] { 2, 3, 15, 5, 0 };

            var expectedOutput = new int[] { 2, 6, 45, 20, 0 };

            // Act

            var output = input.Select( ( x, y ) => x * y, parameters );

            // Assert

            Assert.Equal( expectedOutput, output );
        }

        [Fact]
        public void Select_InvalidParametersLength()
        {
            // Arrange

            var input = new int[] { 1, 2, 3, 4, 5 };
            var parameters = new double[] { 2.0, 3.3, 1.5, 0.5 };

            IEnumerable<double> output;

            // Act & Assert

            Assert.Throws<ArgumentException>( () => input.Select( ( x, y ) => x + y, parameters ).Count() );
        }

        [Fact]
        public void SelectWithIndex()
        {
            // Arrange

            var input = new int[] { 1, 2, 3, 4, 5 };
            var parameters = new int[] { 2, 3, 15, 5, 0 };

            var expectedOutput = new int[] { 2, 7, 47, 23, 4 };

            // Act

            var output = input.Select( ( x, i, y ) => ( x * y ) + i, parameters );

            // Assert

            Assert.Equal( expectedOutput, output );
        }

        [Fact]
        public void SelectWithIndex_InvalidParametersLength()
        {
            // Arrange

            var input = new int[] { 1, 2, 3, 4, 5 };
            var parameters = new double[] { 2.0, 3.3, 1.5, 0.5 };

            IEnumerable<double> output;

            // Act & Assert

            Assert.Throws<ArgumentException>( () => input.Select( ( x, i, y ) => x + y * i, parameters ).Count() );
        }

        [Fact]
        public void CastAll()
        {
            // Arrange

            var input = new object[] { 1.0, 12.1, 32.2 };

            var expectedOutput = new double[] { 1.0, 12.1, 32.2 };

            IEnumerable<double> output;

            // Act

            output = input.CastAll<double>();

            // Assert

            Assert.Equal( expectedOutput, output );
        }

        [Fact]
        public void CastAll_InvalidValues()
        {
            // Arrange

            var input = new object[] { 1.0, 12.1, 32 };

            // Act & Assert

            Assert.Throws<InvalidCastExceptionEx>( () => input.CastAll<double>().Count() );
        }

        [Theory]
        [InlineData( new double[] { 1.0, 12.1, 32.2 }, true )]
        [InlineData( new double[] { 1.0, 12.1, 1.0 }, false )]
        [InlineData( new int[] { 3, 33, 56 }, true )]
        [InlineData( new int[] { 3, 33, 56, 33 }, false )]
        [InlineData( new object[] { 1.0, 12.1, 1 }, true )]
        [InlineData( new object[] { 3.0, 12.1, 3.0 }, false )]
        public void AllDistinct<T>( T[] input, bool expectedResult )
        {
            Assert.Equal( expectedResult, input.AllDistinct() );
        }

        private class IntComparer : IEqualityComparer<int>
        {
            public bool Equals( int x, int y )
            {
                return ( x % 10 ) == ( y % 10 );
            }

            public int GetHashCode( int obj )
            {
                return ( obj % 10 );
            }
        }

        [Theory]
        [InlineData( new int[] { 3, 32, 56 }, true )]
        [InlineData( new int[] { 3, 33, 56 }, false )]
        public void AllDistinct_WithComparer( int[] input, bool expectedResult )
        {
            Assert.Equal( expectedResult, input.AllDistinct( new IntComparer() ) );
        }
    }
}
