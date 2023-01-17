/// @file
/// @copyright  Copyright (c) 2020 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;
using Xunit;

namespace Utilities.Net.Test
{
    public class DictionaryUtilitiesTest
    {
        private Dictionary<int, string> GetTestDictionary()
        {
            Dictionary<int, string> value = new();
            for( int i = 10; i < 20; i++ )
            {
                value.Add( i, i.ToString() );
            }
            return value;
        }

        [Fact]
        public void GetValue_Existing()
        {
            var dictionary = GetTestDictionary();

            var value = dictionary.GetValue( 15 );

            Assert.NotNull( value );
            Assert.Equal( "15", value );
        }

        [Fact]
        public void GetValue_NotExisting_WithoutDefault()
        {
            var dictionary = GetTestDictionary();

            var value = dictionary.GetValue( 25 );

            Assert.Null( value );
        }

        [Fact]
        public void GetValue_NotExisting_WithDefault()
        {
            var dictionary = GetTestDictionary();

            var value = dictionary.GetValue( 25, "NOT-FOUND" );

            Assert.NotNull( value );
            Assert.Equal( "NOT-FOUND", value );
        }

        [Fact]
        public void AddOrReplace()
        {
            var dictionary = GetTestDictionary();

            dictionary.AddOrReplace( 11, "FOO" );

            Assert.Equal( "FOO", dictionary[ 11 ] );
            Assert.Equal( "12", dictionary[ 12 ] );
        }
    }
}
