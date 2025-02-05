/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

namespace Utilities.DotNet.Services
{
    /// <summary>
    /// Represents a service that is automatically registered with the <see cref="ServiceProvider.GlobalServices">global service provider</see>.
    /// </summary>
    /// <remarks>
    /// Derived classes must implement a default constructor.
    /// </remarks>
    public interface IAutoRegisteredGlobalServiceCreator<TService> where TService : class
    {
        //===========================================================================
        //                                  METHODS
        //===========================================================================

        /// <summary>
        /// Creates an instance of the service.
        /// </summary>
        /// <returns>Instance of the service.</returns>
        TService CreateServiceInstance();
    }
}

