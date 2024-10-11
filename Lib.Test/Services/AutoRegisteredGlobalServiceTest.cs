/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using System;
using Utilities.DotNet.Services;
using Xunit;

namespace Utilities.DotNet.Test.Services
{
    public class AutoRegisteredGlobalServiceTest
    {
        public interface ITestService
        {
        }

        public class TestService : AutoRegisteredGlobalService<ITestService>, ITestService
        {
        }

        public interface ITestService2
        {
        }

        public class TestService2A : AutoRegisteredGlobalService<ITestService2>, ITestService2
        {
        }

        public class TestService2B : AutoRegisteredGlobalService<ITestService2>, ITestService2
        {
        }

        public interface ITestService3
        {
        }

        public interface ITestService4
        {
        }

        public class TestService4 : AutoRegisteredGlobalService<ITestService4>
        {
        }

        [Fact]
        public void AutoRegistration()
        {
            // Prepare

            // Execute

            var service = ServiceProvider.GetGlobalService<ITestService>();

            // Verify

            Assert.NotNull( service );
        }

        [Fact]
        public void AutoRegistration_TooManyImplementations()
        {
            // Act & Assert

            Assert.Throws<InvalidOperationException>( () => ServiceProvider.GetGlobalService<ITestService2>() );
        }

        [Fact]
        public void AutoRegistration_NoImplementations()
        {
            // Act & Assert

            Assert.Throws<InvalidOperationException>( () => ServiceProvider.GetGlobalService<ITestService3>() );
        }

        [Fact]
        public void AutoRegistration_BadImplementation()
        {
            // Act & Assert

            Assert.Throws<InvalidOperationException>( () => ServiceProvider.GetGlobalService<ITestService4>() );
        }
    }
}
