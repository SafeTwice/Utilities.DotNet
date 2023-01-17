/// @file
/// @copyright  Copyright (c) 2020 SafeTwice S.L. All rights reserved.
/// @license    MIT (https://opensource.org/licenses/MIT)
namespace Utilities.Net.TransferFunctions
{
    /// <summary>
    /// Calculates a linear transfer function based on gain and offset.
    /// </summary>
    public class DoubleLinear : IDoubleTransferFunction
    {
        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        public DoubleLinear(double gain, double offset)
        {
            m_gain = gain;
            m_offset = offset;
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        public double Calculate(double inputValue)
        {
            return inputValue * m_gain + m_offset;
        }

        public double CalculateInverse(double inputValue)
        {
            return (inputValue - m_offset) / m_gain;
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private double m_gain;
        private double m_offset;
    }
}
