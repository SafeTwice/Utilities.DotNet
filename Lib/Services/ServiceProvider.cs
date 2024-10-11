/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
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
        public static IServiceProvider GlobalServices { get; } = new ServiceProvider( true );

        //===========================================================================
        //                          PUBLIC CONSTRUCTORS
        //===========================================================================

        /// <summary>
        /// Constructor.
        /// </summary>
        public ServiceProvider() : this( false )
        {
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
                if( m_isGlobal )
                {
                    service = GetAutoRegisteredGlobalService<TService>();
                }
                else
                {
                    throw new InvalidOperationException( "Service type not registered" );
                }
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
        //                          PRIVATE CONSTRUCTORS
        //===========================================================================

        private ServiceProvider( bool isGlobal )
        {
            m_isGlobal = isGlobal;
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

        private TService GetAutoRegisteredGlobalService<TService>() where TService : class
        {
            var serviceCreatorTypes = TypeUtilities.FindSubclasses( typeof( IAutoRegisteredGlobalServiceCreator<TService> ) );

            var serviceCreatorsCount = serviceCreatorTypes.Count();

            if( serviceCreatorsCount == 0 )
            {
                throw new InvalidOperationException( "Service type not registered" );
            }
            else if( serviceCreatorsCount > 1 )
            {
                throw new InvalidOperationException( "Multiple auto-registered global service implementations found for this service" );
            }

            var serviceCreatorType = serviceCreatorTypes.First();

            var serviceCreator = (IAutoRegisteredGlobalServiceCreator<TService>) Activator.CreateInstance( serviceCreatorType )!;

            var serviceInstance = serviceCreator.CreateServiceInstance();

            RegisterService( serviceInstance );

            return serviceInstance;
        }

        //===========================================================================
        //                           PRIVATE ATTRIBUTES
        //===========================================================================

        private Dictionary<Type, object> m_services = new();

        private bool m_isGlobal;
    }
}
