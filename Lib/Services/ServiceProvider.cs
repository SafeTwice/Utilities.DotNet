/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using Utilities.DotNet.Types;

namespace Utilities.DotNet.Services
{
    /// <summary>
    /// Implements a class that provides registered services.
    /// </summary>
    public class ServiceProvider : IServiceProvider
    {
        //===========================================================================
        //                           PUBLIC PROPERTIES
        //===========================================================================

        /// <summary>
        /// Global services provider.
        /// </summary>
        public static IServiceProvider GlobalServices { get; } = new ServiceProvider();

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        static ServiceProvider()
        {
            var registererTypes = TypeUtilities.FindSubclasses( typeof( IGlobalServiceRegisterer ) );

            foreach( var registererType in registererTypes )
            {
                var serviceRegisterer = (IGlobalServiceRegisterer) Activator.CreateInstance( registererType )!;
                serviceRegisterer.RegisterServices( GlobalServices );
            }
        }

        //===========================================================================
        //                            PUBLIC METHODS
        //===========================================================================

        /// <inheritdoc/>
        public void RegisterService( Type serviceType, object serviceInstance )
        {
            if( !serviceType.IsAssignableFrom( serviceInstance.GetType() ) )
            {
                throw new ArgumentException( "The service instance does not implement the service type" );
            }

            DoRegisterService( serviceType, serviceInstance );
        }

        /// <inheritdoc/>
        public void RegisterService<TService>( TService serviceInstance ) where TService : class
        {
            DoRegisterService( typeof( TService ), serviceInstance );
        }

        /// <inheritdoc/>
        public void RegisterServiceByInterface( object serviceInstance )
        {
            var serviceInstanceType = serviceInstance.GetType();

            var interfaces = serviceInstanceType.GetInterfaces();

            if( interfaces.Length != 1 )
            {
                throw new ArgumentException( "The object does not implement a single interface" );
            }
            else
            {
                DoRegisterService( interfaces[ 0 ], serviceInstance );
            }
        }

        /// <inheritdoc/>
        public TService GetService<TService>() where TService : class
        {
            var service = m_services.GetValue( typeof( TService ) ) as TService;

            if( service == null )
            {
                throw new InvalidOperationException( "Service type not registered" );
            }

            return service;
        }

        /// <summary>
        /// Gets a registered global service instance for a service type.
        /// </summary>
        /// <typeparam name="TService">Type of service.</typeparam>
        /// <returns>Instance that implements the service.</returns>
        /// <exception cref="InvalidOperationException">Thrown when no instance has been registered for the type of service</exception>
        public static TService GetGlobalService<TService>() where TService : class
        {
            return GlobalServices.GetService<TService>();
        }

        //===========================================================================
        //                            PRIVATE METHODS
        //===========================================================================

        private void DoRegisterService( Type serviceType, object serviceInstance )
        {
            if( m_services.ContainsKey( serviceType ) )
            {
                throw new InvalidOperationException( "Service type already registered" );
            }

            m_services.Add( serviceType, serviceInstance );
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private Dictionary<Type, object> m_services = new();
    }
}
