/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Utilities.DotNet.Comparers;
using Xunit;

namespace Utilities.DotNet.Test.Comparers
{
    public class ComparerUtilitiesTest
    {
        private class TestClass
        {
            public int IntValue { get; set; }

            public string StringValue { get; set; } = string.Empty;
        }

        [Fact]
        public void CreateComparer_Simple()
        {
            // Arrange

            var testObject1 = new TestClass { IntValue = 1 };
            var testObject2 = new TestClass { IntValue = 2 };
            var testObject3 = new TestClass { IntValue = 3 };
            var testObject4 = new TestClass { IntValue = 1 };

            // Act

            var comparer = ComparerUtilities.CreateComparer<TestClass?>( o => new object[] { o.IntValue } );

            // Assert

            Assert.Equal( 0, comparer.Compare( testObject1, testObject1 ) );
            Assert.Equal( 0, comparer.Compare( testObject1, testObject4 ) );
            Assert.Equal( -1, comparer.Compare( testObject1, testObject2 ) );
            Assert.Equal( 1, comparer.Compare( testObject3, testObject2 ) );

            Assert.Equal( 0, comparer.Compare( null, null ) );
            Assert.Equal( 1, comparer.Compare( testObject1, null ) );
            Assert.Equal( -1, comparer.Compare( null, testObject1 ) );
        }

        [Fact]
        public void CreateComparer_Complex()
        {
            // Arrange

            var testObject1 = new TestClass { IntValue = 1, StringValue = "Uno" };
            var testObject2 = new TestClass { IntValue = 2, StringValue = "Dos" };
            var testObject3 = new TestClass { IntValue = 3, StringValue = "Tres" };
            var testObject4 = new TestClass { IntValue = 1, StringValue = "Uno" };
            var testObject5 = new TestClass { IntValue = 1, StringValue = "UnoYMedio" };

            // Act

            var comparer = ComparerUtilities.CreateComparer<TestClass>( o => new object[] { o.IntValue, o.StringValue } );

            // Assert

            Assert.Equal( 0, comparer.Compare( testObject1, testObject1 ) );
            Assert.Equal( -1, comparer.Compare( testObject1, testObject2 ) );
            Assert.Equal( 1, comparer.Compare( testObject3, testObject2 ) );
            Assert.Equal( 0, comparer.Compare( testObject1, testObject4 ) );
            Assert.Equal( -1, comparer.Compare( testObject1, testObject5 ) );
        }

        [Fact]
        public void CreateComparer_DifferentDataLengths()
        {
            // Arrange

            var testObject1 = new TestClass { IntValue = 1, StringValue = "Uno" };
            var testObject2 = new TestClass { IntValue = 2 };
            var testObject3 = new TestClass { IntValue = 1 };

            // Act

            var comparer = ComparerUtilities.CreateComparer<TestClass>( o => ( o.StringValue.Length > 0 ) ? new object[] { o.IntValue, o.StringValue } :
                                                                                                            new object[] { o.IntValue } );

            // Assert

            Assert.Equal( 0, comparer.Compare( testObject1, testObject1 ) );
            Assert.Equal( -1, comparer.Compare( testObject1, testObject2 ) );
            Assert.Equal( 1, comparer.Compare( testObject1, testObject3 ) );
            Assert.Equal( -1, comparer.Compare( testObject3, testObject1 ) );
        }
    }
}
