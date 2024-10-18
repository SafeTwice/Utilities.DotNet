/// @file
/// @copyright  Copyright (c) 2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections;
using System.Collections.Generic;

namespace Utilities.DotNet.Comparers
{
    /// <summary>
    /// {Enter brief class description here...}
    /// </summary>
    public static class ComparerUtilities
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <summary>
        /// Creates a comparer based on the provided data collector function.
        /// </summary>
        /// <typeparam name="T">The type of objects being compared.</typeparam>
        /// <param name="dataCollector">The function that collects the data to be compared.</param>
        /// <returns>A comparer for the specified type.</returns>
        public static IComparer<T> CreateComparer<T>( Func<T, IEnumerable> dataCollector )
        {
            return new DataComparer<T>( dataCollector );
        }

        //===========================================================================
        //                          PRIVATE NESTED TYPES
        //===========================================================================

        private class DataComparer<T> : IComparer<T>
        {
            public DataComparer( Func<T, IEnumerable> dataCollector )
            {
                m_dataCollector = dataCollector;
            }

            public int Compare( T? x, T? y )
            {
                if( x == null && y == null )
                {
                    return 0;
                }
                else if( x == null )
                {
                    return -1;
                }
                else if( y == null )
                {
                    return 1;
                }

                var xData = m_dataCollector( x );
                var yData = m_dataCollector( y );

                var xEnumerator = xData.GetEnumerator();
                var yEnumerator = yData.GetEnumerator();

                while( true )
                {
                    var xHasNext = xEnumerator.MoveNext();
                    var yHasNext = yEnumerator.MoveNext();

                    if( !xHasNext && !yHasNext )
                    {
                        return 0;
                    }

                    if( !xHasNext )
                    {
                        return -1;
                    }

                    if( !yHasNext )
                    {
                        return 1;
                    }

                    var comparison = Comparer.Default.Compare( xEnumerator.Current, yEnumerator.Current );

                    if( comparison != 0 )
                    {
                        return comparison;
                    }
                }
            }

            private readonly Func<T, IEnumerable> m_dataCollector;
        }
    }
}
