/// @file
/// @copyright  Copyright (c) 2023-2024 SafeTwice S.L. All rights reserved.
/// @license    See LICENSE.txt

using Utilities.DotNet.Services;
using Xunit;

namespace Utilities.DotNet.Test.Services
{
    public class SelfRegisteredServiceTest
    {
        public interface ITestService
        {
        }

        public class TestService : SelfRegisteredGlobalService<ITestService>, ITestService
        {
        }

        [Fact]
        public void SelfRegistration()
        {
            // Prepare

            // Execute

            var service = ServiceProvider.GlobalServices.GetService<ITestService>();

            // Verify

            Assert.NotNull( service );
        }
    }
}
