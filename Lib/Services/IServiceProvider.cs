/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;

namespace Utilities.DotNet.Services
{
    /// <summary>
    /// Represents a class that provides registered services.
    /// </summary>
    public interface IServiceProvider
    {
        /// <summary>
        /// Registers a service instance to be provided for a service type.
        /// </summary>
        /// <param name="serviceType">Type of service</param>
        /// <param name="serviceInstance">Instance that implements the service</param>
        /// <exception cref="InvalidOperationException">Thrown when another instance was previously registered for the type of service</exception>
        /// <exception cref="ArgumentException">Thrown when the service instance does not implement the service type</exception>
        void RegisterService( Type serviceType, object serviceInstance );

        /// <summary>
        /// Registers a service instance to be provided for a service type.
        /// </summary>
        /// <remarks>
        /// Only one instance can be registered for each different type of service.
        /// </remarks>
        /// <typeparam name="TService">Type of service</typeparam>
        /// <param name="serviceInstance">Instance that implements the service</param>
        /// <exception cref="InvalidOperationException">Thrown when another instance was previously registered for the type of service</exception>
        void RegisterService<TService>( TService serviceInstance ) where TService : class;

        /// <summary>
        /// Registers a service instance to be provided for the service type which instance it implements.
        /// </summary>
        /// <param name="serviceInstance">Instance that implements the service</param>
        /// <exception cref="InvalidOperationException">Thrown when another instance was previously registered for the type of service</exception>
        /// <exception cref="ArgumentException">Thrown when the service instance implements more than one interface</exception>
        void RegisterServiceByInterface( object serviceInstance );

        /// <summary>
        /// Gets a registered service instance for a service type.
        /// </summary>
        /// <typeparam name="TService">Type of service</typeparam>
        /// <returns>Instance that implements the service</returns>
        /// <exception cref="InvalidOperationException">Thrown when no instance has been registered for the type of service</exception>
        TService GetService<TService>() where TService : class;
    }
}