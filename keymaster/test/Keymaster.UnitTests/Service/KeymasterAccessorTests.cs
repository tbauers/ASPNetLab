// <copyright file="KeymasterAccessorTests.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     Test class for KeymasterAccessor
// </summary>

namespace Keymaster.UnitTests.Service
{
    using System;
    using System.Collections.Generic;

    using FluentAssertions;

    using Keymaster.Accessor;
    using Keymaster.Model;
    using Keymaster.Model.External;
    using Keymaster.Model.Internal;
    using Keymaster.Repository;
    using Keymaster.Transformer;

    using Moq;

    using Xunit;

    public class KeymasterAccessorTests
    {
        private readonly Mock<ITransformer<Model.Internal.Token, Model.External.TokenDetail>> mockTokenTransformer;

        private readonly Mock<ITransformer<List<Model.Internal.Token>, Model.External.TokenCollection>>
            mockTokensTransformer;

        private readonly Mock<ITokenRepository> mockRepository;

        private readonly KeymasterAccessor accessor;

        public KeymasterAccessorTests()
        {
            this.mockTokenTransformer = new Mock<ITransformer<Keymaster.Model.Internal.Token, Model.External.TokenDetail>>();
            this.mockTokensTransformer =
                new Mock<ITransformer<List<Model.Internal.Token>, Model.External.TokenCollection>>();
            this.mockRepository = new Mock<ITokenRepository>();

            this.accessor = new KeymasterAccessor(
                this.mockTokenTransformer.Object,
                this.mockTokensTransformer.Object,
                this.mockRepository.Object);
        }

        [Fact]
        public async void GetTokensReturnsValidTokenCollection()
        {
            var expectedResult = new TokenCollection
                             {
                                 Tokens =
                                     new List<TokenItem>
                                         {
                                             new TokenItem
                                                 {
                                                     Id =
                                                         new Guid(
                                                         "6f8fb7d0-17a5-4f5e-aabf-d1a68cffd731"),
                                                     Type =
                                                         TokenType.ET_TRACKING,
                                                     Provider =
                                                         Provider.ET_SMC3,
                                                     CarrierScac = "AACT",
                                                     Key = 
                                                         "N2M1MTQ3ZTktMzViMi00YjhkLWJlNWQtZmM3NmJkOWJlMTg3"
                                                 }
                                         }
                             };

            this.mockTokensTransformer.Setup(t => t.Transform(It.IsAny<List<Token>>())).Returns(expectedResult);

            var result = await this.accessor.GetApiTokensAsync(new TokenQueryParameter());

            result.Should().BeOfType<TokenCollection>().And.Subject.Should().BeSameAs(expectedResult);
        }

        [Fact]
        public async void GetTokensReturnsNull()
        {
            this.mockTokensTransformer.Setup(t => t.Transform(It.IsAny<List<Token>>())).
                Returns((TokenCollection)null);

            var tokenResult = await this.accessor.GetApiTokenAsync(Guid.NewGuid());

            tokenResult.Should().BeNull();
        }

        [Fact]
        public async void GetTokenReturnsValidToken()
        {
            var mockToken = new Model.External.TokenDetail()
                                {
                                    Id = new Guid("6f8fb7d0-17a5-4f5e-aabf-d1a68cffd731"),
                                    Type = TokenType.ET_DISPATCH,
                                    Provider = Provider.ET_SMC3,
                                    CarrierScac = "AACT",
                                    Key =
                                        new Guid("7c5147e9-35b2-4b8d-be5d-fc76bd9be187")
                                            .ToString()
                                };

            this.mockTokenTransformer.Setup(t => t.Transform(It.IsAny<Model.Internal.Token>())).Returns(mockToken);

            var tokenResult = await this.accessor.GetApiTokenAsync(Guid.NewGuid());

            tokenResult.Should().BeOfType<Model.External.TokenDetail>();
            tokenResult.Should().BeSameAs(mockToken);
        }

        [Fact]
        public async void GetTokenReturnsNull()
        {
            this.mockTokenTransformer.Setup(t => t.Transform(It.IsAny<Model.Internal.Token>())).
                Returns((TokenDetail)null);

            var tokenResult = await this.accessor.GetApiTokenAsync(Guid.NewGuid());

            tokenResult.Should().BeNull();
        }
    }
}
