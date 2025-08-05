/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.TransferFunctions
{
    /// <summary>
    /// Transfer function for a linear interpolation defined using 2 points.
    /// </summary>
    public class DecimalLinearInterpolation : IDecimalTransferFunction
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
        public DecimalLinearInterpolation( decimal x0, decimal x1, decimal y0, decimal y1 )
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
        public decimal Calculate( decimal value )
        {
            return m_yBase + ( value - m_xBase ) * m_yOffset / m_xOffset;
        }

        /// <inheritdoc/>
        public decimal CalculateInverse( decimal value )
        {
            return m_xBase + ( value - m_yBase ) * m_xOffset / m_yOffset;
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly decimal m_xBase;
        private readonly decimal m_xOffset;
        private readonly decimal m_yBase;
        private readonly decimal m_yOffset;
    }
}
