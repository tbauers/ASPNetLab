// <copyright file="GetAllQueryParamTests.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     Class that contains all tests relevent to get all calls that have query params
// </summary>

namespace Keymaster.ServiceTests.GET
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using EngagedTechnologies.Middleware.Errors;

    using FluentAssertions;

    using Keymaster.Configuration;
    using Keymaster.ServiceTests.Configuration;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Extensions;
    using Newtonsoft.Json;
    using Xunit;

    [Collection(nameof(KeymasterTestCollection))]
    public class GetAllQueryParamTests
    {
        public GetAllQueryParamTests(KeymasterTestFixture testFixture)
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
        public async void GetAllValidTypeParamReturnsValidTokenListAndResponse()
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.Add("type", "ET_TRACKING");
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource), new PathString(this.KeymasterConfiguration.Endpoints.TokensResource), queryBuilder.ToQueryString()));
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
        public async void GetAllValidProviderParamReturnsValidTokenListAndResponse()
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.Add("provider", "ET_SMC3");
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource), new PathString(this.KeymasterConfiguration.Endpoints.TokensResource), queryBuilder.ToQueryString()));
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
        public async void GetAllValidCarrierScacParamReturnsValidTokenListAndResponse()
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.Add("carrierScac", "aact");
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource), new PathString(this.KeymasterConfiguration.Endpoints.TokensResource), queryBuilder.ToQueryString()));
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
        public async void GetAllWithAllValidParamsReturnsValidTokenListAndResponse()
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.Add("provider", "ET_SMC3");
            queryBuilder.Add("carrierScac", "aact");
            queryBuilder.Add("type", "ET_TRACKING");
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource), new PathString(this.KeymasterConfiguration.Endpoints.TokensResource), queryBuilder.ToQueryString()));
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
        public async void GetAllInvalidParamsReturnsValidTokenListAndResponse()
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.Add("goat", "BILLY");
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource), new PathString(this.KeymasterConfiguration.Endpoints.TokensResource), queryBuilder.ToQueryString()));
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
        public async void GetAllNonexistentCarrierScacReturnsEmptyListAndResponse()
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.Add("carrierScac", "NONEXISTENT");
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource), new PathString(this.KeymasterConfiguration.Endpoints.TokensResource), queryBuilder.ToQueryString()));
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            var expectedTokenCount = 0;

            var result = await this.Client.SendAsync(requestMessage);

            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.OK);

            var resultobj =
                JsonConvert.DeserializeObject<Keymaster.Model.External.TokenCollection>(result.Content.ReadAsStringAsync().Result);

            resultobj.Tokens.Count.Should().Be(expectedTokenCount);
        }

        [Fact]
        public async void GetAllInvalidParamValueReturnsBadRequestResponse()
        {
            var queryBuilder = new QueryBuilder();
            queryBuilder.Add("type", "TWOIFBYSEA");
            var requestUri = new Uri(UriHelper.BuildAbsolute("http", new HostString(this.KeymasterConfiguration.Endpoints.Host), new PathString(this.KeymasterConfiguration.Endpoints.TestAccountResource), new PathString(this.KeymasterConfiguration.Endpoints.TokensResource), queryBuilder.ToQueryString()));
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);

            var result = await this.Client.SendAsync(requestMessage);

            result.StatusCode.ShouldBeEquivalentTo(HttpStatusCode.BadRequest);

            // TODO: Remove these hardcoded headers when the global exception handler is implemented in the middleware.errors library
            result.Headers.Contains(ErrorResponseHeaderKey.ERROR_LOCATION_CODE).ShouldBeEquivalentTo(true);
            result.Headers.Contains(ErrorResponseHeaderKey.ERROR_TYPE).ShouldBeEquivalentTo(true);
        }
    }
}
