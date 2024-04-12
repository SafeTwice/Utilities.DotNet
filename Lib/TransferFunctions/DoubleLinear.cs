/// @file
/// @copyright  Copyright (c) 2020-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.TransferFunctions
{
    /// <summary>
    /// Linear transfer function based on gain and offset.
    /// </summary>
    public class DoubleLinear : IDoubleTransferFunction
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="gain">Gain.</param>
        /// <param name="offset">Offset.</param>
        public DoubleLinear( double gain, double offset )
        {
            m_gain = gain;
            m_offset = offset;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public double Calculate( double value )
        {
            return value * m_gain + m_offset;
        }

        /// <inheritdoc/>
        public double CalculateInverse( double value )
        {
            return ( value - m_offset ) / m_gain;
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private double m_gain;
        private double m_offset;
    }
}
