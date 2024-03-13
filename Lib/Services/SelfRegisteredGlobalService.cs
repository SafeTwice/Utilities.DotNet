/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics.CodeAnalysis;

namespace Utilities.DotNet.Services
{
    /// <summary>
    /// Base class for services that registers themselves with the <see cref="ServiceProvider.GlobalServices">global service provider</see>.
    /// </summary>
    /// <remarks>
    /// The derived class must implement <typeparamref name="TService"/>.
    /// </remarks>
    /// <typeparam name="TService">Type of service.</typeparam>
    public abstract class SelfRegisteredGlobalService<TService> : IGlobalServiceRegisterer where TService : class
    {
        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        /// <inheritdoc/>
        public void RegisterServices( IServiceProvider serviceProvider )
        {
            var service = GetServiceInstance();

            serviceProvider.RegisterService( service );
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        [ExcludeFromCodeCoverage]
        private TService GetServiceInstance()
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
