﻿/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;

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
            var registererInterfaceType = typeof( IGlobalServiceRegisterer );

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<Type> allTypes = new();

            foreach( var assembly in assemblies )
            {
                try
                {
                    var allAssemblyTypes = assembly.GetTypes();

                    allTypes.AddRange( allAssemblyTypes );
                }
                catch( Exception )
                {
                }
            }

            var registererTypes = allTypes.Where( t => t.IsClass && !t.IsAbstract && registererInterfaceType.IsAssignableFrom( t ) );

            foreach( var subType in registererTypes )
            {
                var serviceRegisterer = (IGlobalServiceRegisterer) Activator.CreateInstance( subType )!;
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
