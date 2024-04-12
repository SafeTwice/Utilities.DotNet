/// @file
/// @copyright  Copyright (c) 2020 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.Net.TransferFunctions
{
    /// <summary>
    /// Calculates linear interpolations.
    /// </summary>
    public class DoubleLinearInterpolation : IDoubleTransferFunction
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

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

        public double Calculate( double inputValue )
        {
            return m_yBase + ( inputValue - m_xBase ) * m_yOffset / m_xOffset;
        }

        public double CalculateInverse( double inputValue )
        {
            return m_xBase + ( inputValue - m_yBase ) * m_xOffset / m_yOffset;
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
