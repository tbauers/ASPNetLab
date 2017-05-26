// <copyright file="GetAllVanillaTests.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     Class that contains all tests relevent to get all calls that don't have query params
// </summary>

namespace Keymaster.ServiceTests.GET
{
    using System;
    using System.Net;
    using System.Net.Http;
    using FluentAssertions;

    using Keymaster.Configuration;
    using Keymaster.ServiceTests.Configuration;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Newtonsoft.Json;
    using Xunit;

    [Collection(nameof(KeymasterTestCollection))]
    public class GetAllVanillaTests
    {
        public GetAllVanillaTests(KeymasterTestFixture testFixture)
        {
            this.Client = testFixture.Client;

            // Add the most basic of authorization headers
            this.Client.DefaultRequestHeaders.Clear();
            this.Client.DefaultRequestHeaders.Add("Authorization", "TmsCustom tmsAuthorization");

            this.KeymasterConfiguration = testFixture.KeymasterConfiguration;
            this.TokensConfiguration = testFixture.TokensConfiguration;
        }

        public KeymasterTestConfiguration KeymasterConfiguration { get; }

        public Tokens TokensConfiguration { get; }

        public HttpClient Client { get; }

        [Fact]
        public async void GetAllNoParamsReturnsValidTokenListAndResponse()
        {
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource), new PathString(this.KeymasterConfiguration.Endpoints.TokensResource)));
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var result = await this.Client.SendAsync(requestMessage);

            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var resultobj =
                JsonConvert.DeserializeObject<Keymaster.Model.External.TokenCollection>(result.Content.ReadAsStringAsync().Result);

            // We might need to refactor how we verfiy the correct token is in place once we need to check for more than one token?
            resultobj.Tokens.Find(
                x =>
                    this.TokensConfiguration.Token.Find(configToken => configToken.Id == x.Id) != null).Should().NotBeNull();
        }

        [Fact]
        public async void GetAllPostReturnsNotAllowedResponse()
        {
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource), new PathString(this.KeymasterConfiguration.Endpoints.TokensResource)));
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var result = await this.Client.SendAsync(requestMessage);

            // If the exact return type changes because of the controller implementation please note it and update this.
            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }
    }
}
