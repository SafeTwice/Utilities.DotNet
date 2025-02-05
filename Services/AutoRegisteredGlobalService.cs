/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.Services
{
    /// <summary>
    /// Base class for services that are automatically registered with the <see cref="ServiceProvider.GlobalServices">global service provider</see>.
    /// </summary>
    /// <remarks>
    /// The derived class must implement <typeparamref name="TService"/> and a default constructor.
    /// </remarks>
    /// <typeparam name="TService">Type of service.</typeparam>
    public abstract class AutoRegisteredGlobalService<TService> : IAutoRegisteredGlobalServiceCreator<TService> where TService : class
    {
        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        //[ExcludeFromCodeCoverage]
        public TService CreateServiceInstance()
        {
            var service = this as TService;

            if( service == null )
            {
                throw new InvalidOperationException( "Self registered service must implement the declared service" );
            }

            return service;
        }
    }
}
