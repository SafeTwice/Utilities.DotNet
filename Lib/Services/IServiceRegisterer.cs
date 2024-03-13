/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.Services
{
    /// <summary>
    /// Represents a class that registers services.
    /// </summary>
    public interface IServiceRegisterer
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Registers services with the given service provider.
        /// </summary>
        /// <param name="serviceProvider">Service provided into which services must be registered.</param>
        void RegisterServices( IServiceProvider serviceProvider );
    }
}

