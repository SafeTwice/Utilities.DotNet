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
        /// Compares two doubles using absolute difference comparison.
        /// </summary>
        /// <param name="a">A double.</param>
        /// <param name="b">Another double.</param>
        /// <param name="tolerance">Maximum allowed absolute difference.</param>
        /// <returns><c>true</c> if <paramref name="a"/> and <paramref name="b"/> have an absolute difference lower than <paramref name="tolerance"/>.</returns>
        public static bool Equals( this double a, double b, double tolerance )
        {
            if( a.Equals( b ) )
            {
                return true;
            }

            var absDiff = Math.Abs( a - b );

            return !double.IsNaN( absDiff ) && ( absDiff < tolerance );
        }

        /// <summary>
        /// Compares two doubles using relative difference comparison.
        /// </summary>
        /// <param name="a">A double.</param>
        /// <param name="b">Another double.</param>
        /// <param name="tolerance">Maximum allowed relative difference.</param>
        /// <returns><c>true</c> if <paramref name="a"/> and <paramref name="b"/> have a relative difference lower than <paramref name="tolerance"/>.</returns>
        public static bool EqualsRelative( this double a, double b, double tolerance )
        {
            if( a.Equals( b ) )
            {
                return true;
            }

            var absDiff = Math.Abs( a - b );
            var absMax = Math.Max( Math.Abs( a ), Math.Abs( b ) );

            return !double.IsNaN( absMax ) && ( ( absDiff / absMax ) < tolerance );
        }

        /// <summary>
        /// Compares two doubles using absolute difference comparison and returns an indication of their relative values.
        /// </summary>
        /// <remarks>
        /// The returned value is:
        /// <list type="bullet">
        /// <item>Zero: when <paramref name="a"/> is equal to <paramref name="b"/>.</item>
        /// <item>Less than zero: when <paramref name="a"/> is less than <paramref name="b"/>.</item>
        /// <item>Greater than zero: when <paramref name="a"/> is greater than <paramref name="b"/>.</item>
        /// </list>
        /// </remarks>
        /// <param name="a">A double.</param>
        /// <param name="b">Another double.</param>
        /// <param name="tolerance">Maximum allowed absolute difference.</param>
        /// <returns>A signed number indicating the relative values of the compared numbers.
        /// </returns>
        public static int CompareTo( this double a, double b, double tolerance )
        {
            if( a.Equals( b, tolerance ) )
            {
                return 0;
            }
            else
            {
                return a.CompareTo( b );
            }
        }

        /// <summary>
        /// Compares two doubles using relative difference comparison and returns an indication of their relative values.
        /// </summary>
        /// <remarks>
        /// The returned value is:
        /// <list type="bullet">
        /// <item>Zero: when <paramref name="a"/> is equal to <paramref name="b"/>.</item>
        /// <item>Less than zero: when <paramref name="a"/> is less than <paramref name="b"/>.</item>
        /// <item>Greater than zero: when <paramref name="a"/> is greater than <paramref name="b"/>.</item>
        /// </list>
        /// </remarks>
        /// <param name="a">A double.</param>
        /// <param name="b">Another double.</param>
        /// <param name="tolerance">Maximum allowed absolute difference.</param>
        /// <returns>A signed number indicating the relative values of the compared numbers.
        /// </returns>
        public static int CompareRelativeTo( this double a, double b, double tolerance )
        {
            if( a.EqualsRelative( b, tolerance ) )
            {
                return 0;
            }
            else
            {
                return a.CompareTo( b );
            }
        }
    }
}
