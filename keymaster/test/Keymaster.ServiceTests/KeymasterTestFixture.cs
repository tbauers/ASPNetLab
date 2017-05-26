// <copyright file="KeymasterTestFixture.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     Contains the service test fixture for the users api server
// </summary>

namespace Keymaster.ServiceTests
{
    using System;
    using System.IO;
    using System.Net.Http;

    using Keymaster.Configuration;
    using Keymaster.ServiceTests.Configuration;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;

    public class KeymasterTestFixture : TestConfigurationFixture, IDisposable
    {
        private const string TEST_SERVER_ENVIRONMENT = "development";

        private const string KEYMASTER_SECTION_NAME = "Keymaster";

        private const string TOKEN_SECTION_NAME = "TokensConfiguration";

        private readonly TestServer server;

        public KeymasterTestFixture()
        {
            this.Configuration.GetSection(KEYMASTER_SECTION_NAME).Bind(this.KeymasterConfiguration);
            this.Configuration.GetSection(TOKEN_SECTION_NAME).Bind(this.TokensConfiguration);

            var builder =
                new WebHostBuilder().UseEnvironment(TEST_SERVER_ENVIRONMENT)
                    .UseContentRoot(Path.GetFullPath(this.KeymasterConfiguration.ApplicationContentRoot))
                    .UseStartup<Startup>();
            this.server = new TestServer(builder);

            this.Client = this.server.CreateClient();
            this.Client.BaseAddress = this.KeymasterConfiguration.ServerAddress;
        }

        public KeymasterTestConfiguration KeymasterConfiguration { get; } = new KeymasterTestConfiguration();

        public Tokens TokensConfiguration { get; } = new Tokens();

        public HttpClient Client { get; }

        public void Dispose()
        {
            this.Client.Dispose();
            this.server.Dispose();
        }
    }
}
