/// @file
/// @copyright  Copyright (c) 2020-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.TransferFunctions
{
    /// <summary>
    /// Transfer function for double numbers.
    /// </summary>
    public interface IDoubleTransferFunction
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Calculates the output value for the given input <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>The output value.</returns>
        double Calculate( double value );

        /// <summary>
        /// Calculates the input value for the given output <paramref name="value"/>.
        /// </summary>
        /// <param name="value">Output value.</param>
        /// <returns>The input value if the transfer function is invertible, <c>NaN</c> otherwise.</returns>
        double CalculateInverse( double value );
    }
}
