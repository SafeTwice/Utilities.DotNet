/// @file
/// @copyright  Copyright (c) 2019-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Net.General
{
    /// <summary>
    /// Enumerable utilities.
    /// </summary>
    public static class EnumerableUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================


        /// <summary>
        /// Converts a byte sequence to its corresponding hexadecimal representation.
        /// </summary>
        /// <param name="value">A byte sequence.</param>
        /// <param name="separator">An optional string to include between byte's representations.</param>
        /// <returns>String containing the hexadecimal representation of the input sequence.</returns>
        public static string ToHexString( this IEnumerable<byte> value, string separator = "" )
        {
            var length = value.Count();

            if( length == 0 )
            {
                return string.Empty;
            }

            var result = new StringBuilder( ( length * 2 ) + ( ( length - 1 ) * separator.Length ) );
            bool addSeparator = false;

            foreach( byte b in value )
            {
                if( addSeparator )
                {
                    result.Append( separator );
                }

                result.AppendFormat( "{0:X2}", b );

                addSeparator = true;
            }

            return result.ToString();
        }
    }
}
