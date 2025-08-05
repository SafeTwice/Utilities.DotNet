/// @file
/// @copyright  Copyright (c) 2025 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.TransferFunctions
{
    /// <summary>
    /// Linear transfer function based on gain and offset.
    /// </summary>
    public class DecimalLinear : IDecimalTransferFunction
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="gain">Gain.</param>
        /// <param name="offset">Offset.</param>
        public DecimalLinear( decimal gain, decimal offset )
        {
            m_gain = gain;
            m_offset = offset;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public decimal Calculate( decimal value )
        {
            return value * m_gain + m_offset;
        }

        /// <inheritdoc/>
        public decimal CalculateInverse( decimal value )
        {
            return ( value - m_offset ) / m_gain;
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private readonly decimal m_gain;
        private readonly decimal m_offset;
    }
}
