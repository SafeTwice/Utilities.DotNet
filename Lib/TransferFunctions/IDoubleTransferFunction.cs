/// @file
/// @copyright  Copyright (c) 2020 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.Net.TransferFunctions
{
    /// <summary>
    /// Transfer function for double numbers.
    /// </summary>
    public interface IDoubleTransferFunction
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        double Calculate( double inputValue );
        double CalculateInverse( double inputValue );
    }
}
