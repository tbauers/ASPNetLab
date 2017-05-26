// <copyright file="AuthorizationTests.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     Class that contains all tests relevent to get by id calls
// </summary>

namespace Keymaster.ServiceTests.GET
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    using FluentAssertions;

    using Keymaster.Configuration;
    using Keymaster.ServiceTests.Configuration;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Xunit;

    [Collection(nameof(KeymasterTestCollection))]
    public class AuthorizationTests
    {
        public AuthorizationTests(KeymasterTestFixture testFixture)
        {
            this.Client = testFixture.Client;

            // The test fixture adds default authorization, make sure that without that we get an unauthorized response
            this.Client.DefaultRequestHeaders.Clear();

            this.KeymasterConfiguration = testFixture.KeymasterConfiguration;
            this.TokensConfiguration = testFixture.TokensConfiguration;
        }

        public KeymasterTestConfiguration KeymasterConfiguration { get; }

        public Tokens TokensConfiguration { get; }

        public HttpClient Client { get; }

        [Fact]
        public async void GetByIdRequiresAuthorization()
        {
            var token = this.TokensConfiguration.Token.First();
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource + this.KeymasterConfiguration.Endpoints.TokensResource), new PathString("/" + token.Id)));
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var result = await this.Client.SendAsync(requestMessage);

            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async void GetAllPostRequiresAuthorizationHeader()
        {
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource), new PathString(this.KeymasterConfiguration.Endpoints.TokensResource)));
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var result = await this.Client.SendAsync(requestMessage);

            // If the exact return type changes because of the controller implementation please note it and update this.
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.Unauthorized);
        }
    }
}
