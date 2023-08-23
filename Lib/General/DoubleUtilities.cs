/// @file
/// @copyright  Copyright (c) 2020 SafeTwice S.L. All rights reserved.
/// @license    MIT (https://opensource.org/licenses/MIT)

using System;

namespace Utilities.DotNet
{
    /// <summary>
    /// Utilities for working with double precision real numbers.
    /// </summary>
    public static class DoubleUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Compares two doubles.
        /// </summary>
        /// <param name="a">A double</param>
        /// <param name="b">Another double</param>
        /// <param name="precision">Comparison precision</param>
        /// <returns><c>true</c> if <paramref name="a"/> and <paramref name="b"/> have an absolute difference lower than <paramref name="precision"/></returns>
        public static bool Equals( double a, double b, double precision )
        {
            return ( Math.Abs( a - b ) < precision );
        }
    }
}
