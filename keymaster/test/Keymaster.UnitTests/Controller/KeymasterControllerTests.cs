// <copyright file="KeymasterControllerTests.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      Tests for the Keymaster controller.
// </summary>

namespace Keymaster.UnitTests.Controller
{
    using System;

    using EngagedTechnologies.Exceptions;
    using EngagedTechnologies.Middleware.Errors;

    using FluentAssertions;

    using Keymaster.Accessor;
    using Keymaster.Controllers;
    using Keymaster.Model;
    using Keymaster.Model.External;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;

    using Moq;
    using Xunit;

    public class KeymasterControllerTests
    {
        private readonly Mock<IKeymasterAccessor> keymasterAccessor;
        private readonly KeymasterController controller;

        public KeymasterControllerTests()
        {
            this.keymasterAccessor = new Mock<IKeymasterAccessor>();
            this.controller = new KeymasterController(this.keymasterAccessor.Object);
        }

        [Fact]
        public async void GetTokensNoParamsReturnsValidTokenListAndOkResult()
        {
            var input = new TokenQueryParameter();
            var tokenCollection = new TokenCollection();
            var expectedResult = new OkObjectResult(tokenCollection);

            this.keymasterAccessor
                .Setup(a => a.GetApiTokensAsync(input))
                .ReturnsAsync(tokenCollection);

            var result = await this.controller.GetTokens(string.Empty, input);

            result.Should().BeOfType(expectedResult.GetType()).And.Subject.ShouldBeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void GetTokensValidTypeParamReturnsValidTokenListAndOkResult()
        {
            var input = new TokenQueryParameter { Type = TokenType.ET_TRACKING };
            var tokenCollection = new TokenCollection();
            var expectedResult = new OkObjectResult(tokenCollection);

            this.keymasterAccessor
                .Setup(a => a.GetApiTokensAsync(input))
                .ReturnsAsync(tokenCollection);

            var result = await this.controller.GetTokens(string.Empty, input);

            result.Should().BeOfType(expectedResult.GetType()).And.Subject.ShouldBeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void GetTokensInvalidTypeParamReturnsBadRequest()
        {
            var input = new TokenQueryParameter();
            var tokenCollection = new TokenCollection();
            var expectedResult = new OkObjectResult(tokenCollection);

            this.keymasterAccessor
                .Setup(a => a.GetApiTokensAsync(input))
                .ReturnsAsync(tokenCollection);

            var mockHttpHeaders = new Mock<IHeaderDictionary>();

            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpResponse.Setup(r => r.Headers).Returns(mockHttpHeaders.Object);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Response).Returns(mockHttpResponse.Object);

            this.controller.ControllerContext = new ControllerContext();
            this.controller.ControllerContext.HttpContext = mockHttpContext.Object;
            this.controller.ModelState.AddModelError("Type", "Fakin' the break");

            var result = await this.controller.GetTokens(string.Empty, input);

            result.Should().BeOfType<BadRequestObjectResult>();
            mockHttpHeaders.Verify(h => h.Add(It.Is<string>(i => i.Equals(ErrorResponseHeaderKey.ERROR_LOCATION_CODE)), It.IsAny<StringValues>()), Times.Once);
            mockHttpHeaders.Verify(
                h =>
                h.Add(
                    It.Is<string>(i => i.Equals(ErrorResponseHeaderKey.ERROR_TYPE)),
                    It.Is<StringValues>(i => i.Equals(ErrorTypeDefinitions.RESOURCE_SPECIFIED_CONTAINS_INVALID_PARAMETER))),
                Times.Once);
        }

        [Fact]
        public async void GetTokenByIdShouldReturnTokenResult()
        {
            var mockToken = new Model.External.TokenDetail()
            {
                Id = new Guid("6f8fb7d0-17a5-4f5e-aabf-d1a68cffd731"),
                Type = TokenType.ET_DISPATCH,
                Provider = Provider.ET_SMC3,
                CarrierScac = "AACT",
                Key = new Guid("7c5147e9-35b2-4b8d-be5d-fc76bd9be187").ToString()
            };

            this.keymasterAccessor.Setup(a => a.GetApiTokenAsync(It.IsAny<Guid>())).ReturnsAsync(mockToken);

            var expectedResult = new OkObjectResult(mockToken);

            var result = await this.controller.GetTokenById(string.Empty, Guid.NewGuid());
            result.Should().BeOfType<OkObjectResult>().Which.ShouldBeEquivalentTo(expectedResult);
        }

        [Fact]
        public async void GetTokenByIdShouldReturnNotFoundWhenNoTokenFound()
        {
            this.keymasterAccessor.Setup(a => a.GetApiTokenAsync(It.IsAny<Guid>())).ReturnsAsync((TokenDetail)null);

            var mockHttpHeaders = new Mock<IHeaderDictionary>();

            var mockHttpResponse = new Mock<HttpResponse>();
            mockHttpResponse.Setup(r => r.Headers).Returns(mockHttpHeaders.Object);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Response).Returns(mockHttpResponse.Object);

            this.controller.ControllerContext = new ControllerContext();
            this.controller.ControllerContext.HttpContext = mockHttpContext.Object;

            var result = await this.controller.GetTokenById(string.Empty, Guid.NewGuid());

            result.Should().BeOfType<NotFoundObjectResult>();
            mockHttpHeaders.Verify(
                h => h.Add(It.Is<string>(i => i.Equals(ErrorResponseHeaderKey.ERROR_LOCATION_CODE)), It.IsAny<StringValues>()), Times.Once);
            mockHttpHeaders.Verify(
                h => h.Add(It.Is<string>(i => i.Equals(ErrorResponseHeaderKey.ERROR_TYPE)), It.Is<StringValues>(i => i.Equals(ErrorTypeDefinitions.RESOURCE_SPECIFIED_DOES_NOT_EXIST))), Times.Once);
        }
    }
}
