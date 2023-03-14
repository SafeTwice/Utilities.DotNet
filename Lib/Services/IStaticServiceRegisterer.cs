/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.Net.Services
{
    /// <summary>
    /// Represents a class that registers services automatically.
    /// </summary>
    /// <remarks>
    /// Derived classes must implement a default constructor.
    /// </remarks>
    public interface IStaticServiceRegisterer
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        void RegisterServices( IServiceProvider serviceProvider );
    }
}

