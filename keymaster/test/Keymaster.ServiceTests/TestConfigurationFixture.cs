// <copyright file="TestConfigurationFixture.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     This fixture provides access to the test configuration
// </summary>

namespace Keymaster.ServiceTests
{
    using System;
    using System.IO;
    using Microsoft.Extensions.Configuration;

    public class TestConfigurationFixture
    {
        private const string CONFIGURATION_JSON_FILE = "keymasterTestConfig.json";

        private const string SERVICE_TESTROOT_ENV_NAME = "SERVICE_TESTROOT";

        public TestConfigurationFixture()
        {
            this.LoadConfiguration();
        }

        protected IConfigurationRoot Configuration { get; private set; }

        private static string GetServiceTestRoot()
        {
            return Environment.GetEnvironmentVariable(SERVICE_TESTROOT_ENV_NAME) ?? Directory.GetCurrentDirectory();
        }

        private void LoadConfiguration()
        {
            var configBuilder = new ConfigurationBuilder().SetBasePath(GetServiceTestRoot());
            configBuilder.AddJsonFile(CONFIGURATION_JSON_FILE);
            configBuilder.AddUserSecrets<Startup>();
            configBuilder.AddEnvironmentVariables();
            this.Configuration = configBuilder.Build();
        }
    }
}
