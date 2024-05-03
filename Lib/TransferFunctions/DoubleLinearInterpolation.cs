/// @file
/// @copyright  Copyright (c) 2020-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.TransferFunctions
{
    /// <summary>
    /// Transfer function for a linear interpolation defined using 2 points.
    /// </summary>
    public class DoubleLinearInterpolation : IDoubleTransferFunction
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="x0">X coordinate for the first point.</param>
        /// <param name="x1">X coordinate for the second point.</param>
        /// <param name="y0">Y coordinate for the first point.</param>
        /// <param name="y1">Y coordinate for the second point.</param>
        public DoubleLinearInterpolation( double x0, double x1, double y0, double y1 )
        {
            m_xBase = x0;
            m_xOffset = x1 - x0;
            m_yBase = y0;
            m_yOffset = y1 - y0;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public double Calculate( double value )
        {
            return m_yBase + (value - m_xBase) * m_yOffset / m_xOffset;
        }

        /// <inheritdoc/>
        public double CalculateInverse( double value )
        {
            return m_xBase + (value - m_yBase) * m_xOffset / m_yOffset;
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private double m_xBase;
        private double m_xOffset;
        private double m_yBase;
        private double m_yOffset;
    }
}
