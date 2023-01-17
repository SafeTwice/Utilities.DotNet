/// @file
/// @copyright  Copyright (c) 2020 SafeTwice S.L. All rights reserved.
/// @license    MIT (https://opensource.org/licenses/MIT)

using System.Collections.Generic;

namespace Utilities.Net
{
    /// <summary>
    /// Dictionary utilities.
    /// <summary>
    public static class DictionaryUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Gets the value for a key from the dictionary, or a default value if no associated value exists.
        /// </summary>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <typeparam name="TValue">Type of the value</typeparam>
        /// <param name="dictionary">A dictionary</param>
        /// <param name="key">Key to find the value for in <paramref name="dictionary"/></param>
        /// <param name="defaultValue">Value returned if no value is associated to <paramref name="key"/></param>
        /// <returns></returns>
        public static TValue? GetValue<TKey, TValue>( this IDictionary<TKey, TValue> dictionary, TKey key, TValue? defaultValue = default( TValue ) )
        {
            TValue? value;
            if( dictionary.TryGetValue( key, out value ) )
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Adds or replaces a value in a dictionary.
        /// </summary>
        /// <typeparam name="TKey">Type of the key</typeparam>
        /// <typeparam name="TValue">Type of the value</typeparam>
        /// <param name="dictionary">A dictionary</param>
        /// <param name="key">Key for the value</param>
        /// <param name="value">Value to be added</param>
        public static void AddOrReplace<TKey, TValue>( this IDictionary<TKey, TValue> dictionary, TKey key, TValue value )
        {
            dictionary.Remove( key );
            dictionary.Add( key, value );
        }
    }
}
