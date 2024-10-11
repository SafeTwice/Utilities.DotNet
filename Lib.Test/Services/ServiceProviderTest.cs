/// @file
/// @copyright  Copyright (c) 2023 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using Utilities.DotNet.Services;
using Xunit;

namespace Utilities.DotNet.Test.Services
{
    public class ServiceProviderTest
    {
        public interface ITestService
        {
        }

        public class TestService : ITestService
        {
        }

        public class TestServiceEx : ITestService, IDisposable
        {
            public void Dispose() => throw new NotImplementedException();
        }

        [Fact]
        public void RegisterService_TypeAndInstance_Success()
        {
            // Prepare

            var serviceProvider = new ServiceProvider();
            var service = new TestService();

            // Execute

            serviceProvider.RegisterService( typeof( ITestService), service );

            // Verify

            Assert.Equal( service, serviceProvider.GetService<ITestService>() );
        }

        [Fact]
        public void RegisterService_TypeAndInstance_AlreadyRegistered()
        {
            // Prepare

            var serviceProvider = new ServiceProvider();
            serviceProvider.RegisterService( typeof( ITestService ), new TestService() );

            // Execute & Verify

            Assert.Throws<InvalidOperationException>( () => serviceProvider.RegisterService( typeof( ITestService ), new TestService() ) );
        }

        [Fact]
        public void RegisterService_TypeAndInstance_InvalidInstance()
        {
            // Prepare

            var serviceProvider = new ServiceProvider();

            // Execute & Verify

            Assert.Throws<ArgumentException>( () => serviceProvider.RegisterService( typeof( ITestService ), this ) );
        }

        [Fact]
        public void RegisterService_GenericParameter_Success()
        {
            // Prepare

            var serviceProvider = new ServiceProvider();
            var service = new TestService();

            // Execute

            serviceProvider.RegisterService<ITestService>( service );

            // Verify

            Assert.Equal( service, serviceProvider.GetService<ITestService>() );
        }

        [Fact]
        public void RegisterService_GenericParameter_AlreadyRegistered()
        {
            // Prepare

            var serviceProvider = new ServiceProvider();
            serviceProvider.RegisterService( typeof( ITestService ), new TestService() );

            // Execute & Verify

            Assert.Throws<InvalidOperationException>( () => serviceProvider.RegisterService<ITestService>( new TestService() ) );
        }

        [Fact]
        public void RegisterServiceByInterface_Success()
        {
            // Prepare

            var serviceProvider = new ServiceProvider();
            var service = new TestService();

            // Execute

            serviceProvider.RegisterServiceByInterface( service );

            // Verify

            Assert.Equal( service, serviceProvider.GetService<ITestService>() );
        }

        [Fact]
        public void RegisterServiceByInterface_AlreadyRegistered()
        {
            // Prepare

            var serviceProvider = new ServiceProvider();
            serviceProvider.RegisterService( typeof( ITestService ), new TestService() );

            // Execute & Verify

            Assert.Throws<InvalidOperationException>( () => serviceProvider.RegisterServiceByInterface( new TestService() ) );
        }

        [Fact]
        public void RegisterServiceByInterface_TooManyInterfaces()
        {
            // Prepare

            var serviceProvider = new ServiceProvider();
            serviceProvider.RegisterService( typeof( ITestService ), new TestService() );

            // Execute & Verify

            Assert.Throws<ArgumentException>( () => serviceProvider.RegisterServiceByInterface( new TestServiceEx() ) );
        }

        [Fact]
        public void RegisterServiceByInterface_NoInterfaces()
        {
            // Prepare

            var serviceProvider = new ServiceProvider();
            serviceProvider.RegisterService( typeof( ITestService ), new TestServiceEx() );

            // Execute & Verify

            Assert.Throws<ArgumentException>( () => serviceProvider.RegisterServiceByInterface( this ) );
        }

        [Fact]
        public void GetService_NotRegistered()
        {
            // Prepare

            var serviceProvider = new ServiceProvider();
            serviceProvider.RegisterService( typeof( IDisposable ), new TestServiceEx() );

            // Execute & Verify

            Assert.Throws<InvalidOperationException>( () => serviceProvider.GetService<ITestService>() );
        }

        [Fact]
        public void GetGlobalService()
        {
            // Prepare

            var service = new TestService();

            // Execute

            ServiceProvider.GlobalServices.RegisterServiceByInterface( service );

            // Verify

            Assert.Equal( service, ServiceProvider.GetGlobalService<ITestService>() );
        }
    }
}
