/// @file
/// @copyright  Copyright (c) 2026 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

#if NET5_0_OR_GREATER

using Xunit;

namespace Utilities.DotNet.Threading.Test
{

    public class InterlockedExTest
    {
        public enum TestEnumInt { Zero = 0, One = 1, Two = 2, Three = 3 }
        public enum TestEnumLong : long { Zero = 0, One = 1, Two = 2, Three = 3 }
        public enum TestEnumByte : byte { Zero = 0, One = 1, Two = 2 }
        public enum TestEnumShort : short { Zero = 0, One = 1, Two = 2 }

        #region CompareExchange tests

        [Theory]
        [InlineData( TestEnumInt.Zero, TestEnumInt.Two, TestEnumInt.Zero, TestEnumInt.Two )]
        [InlineData( TestEnumInt.One, TestEnumInt.Two, TestEnumInt.Zero, TestEnumInt.One )]
        public void CompareExchange_IntEnum( TestEnumInt initialValue, TestEnumInt newValue, TestEnumInt comparedValue, TestEnumInt expectedFinal )
        {
            // Arrange
            var value = initialValue;

            // Act
            var original = InterlockedEx.CompareExchange( ref value, newValue, comparedValue );

            // Assert
            Assert.Equal( initialValue, original );
            Assert.Equal( expectedFinal, value );
        }

        [Theory]
        [InlineData( TestEnumLong.Zero, TestEnumLong.Two, TestEnumLong.Zero, TestEnumLong.Two )]
        [InlineData( TestEnumLong.One, TestEnumLong.Two, TestEnumLong.Zero, TestEnumLong.One )]
        public void CompareExchange_LongEnum( TestEnumLong initialValue, TestEnumLong newValue, TestEnumLong comparedValue, TestEnumLong expectedFinal )
        {
            // Arrange
            var value = initialValue;

            // Act
            var original = InterlockedEx.CompareExchange( ref value, newValue, comparedValue );

            // Assert
            Assert.Equal( initialValue, original );
            Assert.Equal( expectedFinal, value );
        }

#if NET9_0_OR_GREATER

        [Theory]
        [InlineData( TestEnumByte.Zero, TestEnumByte.Two, TestEnumByte.Zero, TestEnumByte.Two )]
        [InlineData( TestEnumByte.One, TestEnumByte.Two, TestEnumByte.Zero, TestEnumByte.One )]
        public void CompareExchange_ByteEnum( TestEnumByte initialValue, TestEnumByte newValue, TestEnumByte comparedValue, TestEnumByte expectedFinal )
        {
            // Arrange
            var value = initialValue;

            // Act
            var original = InterlockedEx.CompareExchange( ref value, newValue, comparedValue );

            // Assert
            Assert.Equal( initialValue, original );
            Assert.Equal( expectedFinal, value );
        }

        [Theory]
        [InlineData( TestEnumShort.Zero, TestEnumShort.Two, TestEnumShort.Zero, TestEnumShort.Two )]
        [InlineData( TestEnumShort.One, TestEnumShort.Two, TestEnumShort.Zero, TestEnumShort.One )]
        public void CompareExchange_ShortEnum( TestEnumShort initialValue, TestEnumShort newValue, TestEnumShort comparedValue, TestEnumShort expectedFinal )
        {
            // Arrange
            var value = initialValue;

            // Act
            var original = InterlockedEx.CompareExchange( ref value, newValue, comparedValue );

            // Assert
            Assert.Equal( initialValue, original );
            Assert.Equal( expectedFinal, value );
        }

#endif
        #endregion

        #region Exchange tests

        [Theory]
        [InlineData( TestEnumInt.Zero, TestEnumInt.Two )]
        [InlineData( TestEnumInt.One, TestEnumInt.Three )]
        public void Exchange_IntEnum( TestEnumInt initialValue, TestEnumInt newValue )
        {
            // Arrange
            var value = initialValue;

            // Act
            var original = InterlockedEx.Exchange( ref value, newValue );

            // Assert
            Assert.Equal( initialValue, original );
            Assert.Equal( newValue, value );
        }

        [Theory]
        [InlineData( TestEnumLong.Zero, TestEnumLong.Two )]
        [InlineData( TestEnumLong.One, TestEnumLong.Three )]
        public void Exchange_LongEnum( TestEnumLong initialValue, TestEnumLong newValue )
        {
            // Arrange
            var value = initialValue;

            // Act
            var original = InterlockedEx.Exchange( ref value, newValue );

            // Assert
            Assert.Equal( initialValue, original );
            Assert.Equal( newValue, value );
        }

#if NET9_0_OR_GREATER

        [Theory]
        [InlineData( TestEnumByte.Zero, TestEnumByte.Two )]
        [InlineData( TestEnumByte.One, TestEnumByte.Zero )]
        public void Exchange_ByteEnum( TestEnumByte initialValue, TestEnumByte newValue )
        {
            // Arrange
            var value = initialValue;

            // Act
            var original = InterlockedEx.Exchange( ref value, newValue );

            // Assert
            Assert.Equal( initialValue, original );
            Assert.Equal( newValue, value );
        }

        [Theory]
        [InlineData( TestEnumShort.Zero, TestEnumShort.Two )]
        [InlineData( TestEnumShort.One, TestEnumShort.Zero )]
        public void Exchange_ShortEnum( TestEnumShort initialValue, TestEnumShort newValue )
        {
            // Arrange
            var value = initialValue;

            // Act
            var original = InterlockedEx.Exchange( ref value, newValue );

            // Assert
            Assert.Equal( initialValue, original );
            Assert.Equal( newValue, value );
        }

#endif
        #endregion

        #region And tests

        [Theory]
        [InlineData( TestEnumInt.Zero, TestEnumInt.Two, TestEnumInt.Zero )]
        [InlineData( TestEnumInt.One, TestEnumInt.Two, TestEnumInt.Zero )]
        [InlineData( TestEnumInt.Three, TestEnumInt.Two, TestEnumInt.Two )]
        public void And_IntEnum( TestEnumInt firstValue, TestEnumInt secondValue, TestEnumInt finalValue )
        {
            // Arrange
            var value = firstValue;

            // Act
            var original = InterlockedEx.And( ref value, secondValue );

            // Assert
            Assert.Equal( firstValue, original );
            Assert.Equal( finalValue, value );
        }

        [Theory]
        [InlineData( TestEnumLong.Zero, TestEnumLong.Two, TestEnumLong.Zero )]
        [InlineData( TestEnumLong.One, TestEnumLong.Two, TestEnumLong.Zero )]
        [InlineData( TestEnumLong.Three, TestEnumLong.Two, TestEnumLong.Two )]
        public void And_LongEnum( TestEnumLong firstValue, TestEnumLong secondValue, TestEnumLong finalValue )
        {
            // Arrange
            var value = firstValue;

            // Act
            var original = InterlockedEx.And( ref value, secondValue );

            // Assert
            Assert.Equal( firstValue, original );
            Assert.Equal( finalValue, value );
        }

        #endregion

        #region Or tests

        [Theory]
        [InlineData( TestEnumInt.Zero, TestEnumInt.Two, TestEnumInt.Two )]
        [InlineData( TestEnumInt.One, TestEnumInt.Two, TestEnumInt.Three )]
        [InlineData( TestEnumInt.Three, TestEnumInt.Two, TestEnumInt.Three )]
        public void Or_IntEnum( TestEnumInt firstValue, TestEnumInt secondValue, TestEnumInt finalValue )
        {
            // Arrange
            var value = firstValue;

            // Act
            var original = InterlockedEx.Or( ref value, secondValue );

            // Assert
            Assert.Equal( firstValue, original );
            Assert.Equal( finalValue, value );
        }

        [Theory]
        [InlineData( TestEnumLong.Zero, TestEnumLong.Two, TestEnumLong.Two )]
        [InlineData( TestEnumLong.One, TestEnumLong.Two, TestEnumLong.Three )]
        [InlineData( TestEnumLong.Three, TestEnumLong.Two, TestEnumLong.Three )]
        public void Or_LongEnum( TestEnumLong firstValue, TestEnumLong secondValue, TestEnumLong finalValue )
        {
            // Arrange
            var value = firstValue;

            // Act
            var original = InterlockedEx.Or( ref value, secondValue );

            // Assert
            Assert.Equal( firstValue, original );
            Assert.Equal( finalValue, value );
        }

        #endregion
    }
}

#endif