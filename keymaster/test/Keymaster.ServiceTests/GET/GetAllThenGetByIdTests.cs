// <copyright file="GetAllThenGetByIdTests.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     Class that contains all tests relevent to both get all and get by id calls
// </summary>

namespace Keymaster.ServiceTests.GET
{
    using System;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using Keymaster.Configuration;
    using Keymaster.ServiceTests.Configuration;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;

    using Newtonsoft.Json;

    using Xunit;

    [Collection(nameof(KeymasterTestCollection))]
    public class GetAllThenGetByIdTests
    {
        public GetAllThenGetByIdTests(KeymasterTestFixture testFixture)
        {
            this.Client = testFixture.Client;
            this.KeymasterConfiguration = testFixture.KeymasterConfiguration;
            this.TokensConfiguration = testFixture.TokensConfiguration;
        }

        public KeymasterTestConfiguration KeymasterConfiguration { get; }

        public Tokens TokensConfiguration { get; }

        public HttpClient Client { get; }

        [Fact]
        public async void GetAllNoParamsReturnsValidTokenListAndResponse()
        {
            var requestTokensUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource), new PathString(this.KeymasterConfiguration.Endpoints.TokensResource)));
            var requestTokensMessage = new HttpRequestMessage(HttpMethod.Get, requestTokensUri);

            var requestTokensResult = await this.Client.SendAsync(requestTokensMessage);

            var requestTokensResultObj =
                JsonConvert.DeserializeObject<Keymaster.Model.External.TokenCollection>(
                    requestTokensResult.Content.ReadAsStringAsync().Result);

            var tokenId = requestTokensResultObj.Tokens.Select(token => token.Id).First();

            var requestTokenUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource + this.KeymasterConfiguration.Endpoints.TokensResource), new PathString("/" + tokenId)));
            var requestTokenMessage = new HttpRequestMessage(HttpMethod.Get, requestTokenUri);

            var requestTokenResult = await this.Client.SendAsync(requestTokenMessage);

            var requestTokenResultObj =
                JsonConvert.DeserializeObject<Keymaster.Model.External.TokenDetail>(
                    await requestTokenResult.Content.ReadAsStringAsync());

            requestTokenResultObj.Should().NotBeNull();
        }
    }
}
