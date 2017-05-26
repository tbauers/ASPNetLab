// <copyright file="GetByIdTests.cs" company="Engaged Technologies">
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

    using EngagedTechnologies.Middleware.Errors;

    using FluentAssertions;

    using Keymaster.Configuration;
    using Keymaster.ServiceTests.Configuration;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Newtonsoft.Json;
    using Xunit;

    [Collection(nameof(KeymasterTestCollection))]
    public class GetByIdTests
    {
        public GetByIdTests(KeymasterTestFixture testFixture)
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
        public async void GetByIdWithValidTokenReturnsTokenAndResponse()
        {
            var token = this.TokensConfiguration.Token.First();
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource + this.KeymasterConfiguration.Endpoints.TokensResource), new PathString("/" + token.Id)));
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var result = await this.Client.SendAsync(requestMessage);

            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var resultobj =
                JsonConvert.DeserializeObject<Keymaster.Model.External.TokenDetail>(result.Content.ReadAsStringAsync().Result);

            resultobj.Should().NotBeNull();
            resultobj.Id.ShouldBeEquivalentTo(token.Id);
        }

        [Fact]
        public async void GetByIdWithInvalidTokenReturnsTokenAndResponse()
        {
            var token = this.TokensConfiguration.Token.First();
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource + this.KeymasterConfiguration.Endpoints.TokensResource), new PathString("/NOTREAL1-77f8-4c90-9e70-c7b91ec1f80d")));
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var result = await this.Client.SendAsync(requestMessage);

            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void GetByIdWithQueryParamsIgnoresTheQueryParams()
        {
            var token = this.TokensConfiguration.Token.First();
            var queryBuilder = new QueryBuilder();
            queryBuilder.Add("goat", "BILLY");
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource + this.KeymasterConfiguration.Endpoints.TokensResource), new PathString("/" + token.Id), queryBuilder.ToQueryString()));
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var result = await this.Client.SendAsync(requestMessage);

            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var resultobj =
                JsonConvert.DeserializeObject<Keymaster.Model.External.TokenDetail>(result.Content.ReadAsStringAsync().Result);

            resultobj.Should().NotBeNull();
            resultobj.Id.ShouldBeEquivalentTo(token.Id);
        }

        [Fact]
        public async void GetByIdPostReturnsNotFound()
        {
            var token = this.TokensConfiguration.Token.First();
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource + this.KeymasterConfiguration.Endpoints.TokensResource), new PathString("/" + token.Id)));
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);

            var result = await this.Client.SendAsync(requestMessage);

            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.NotFound);
        }
    }
}
