/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Diagnostics.CodeAnalysis;

namespace Utilities.Net.Services
{
    /// <summary>
    /// Base class for services that registers themselves.
    /// </summary>
    /// <remarks>
    /// The derived class must implement <typeparamref name="TService"/>.
    /// </remarks>
    public abstract class SelfRegisteredService<TService> : IStaticServiceRegisterer where TService : class
    {
        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

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
