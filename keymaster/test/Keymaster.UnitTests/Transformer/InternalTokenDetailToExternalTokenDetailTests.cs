// <copyright file="InternalTokenDetailToExternalTokenDetailTests.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     Test class for InternalTokenDetailToExternalTokenDetailTests
// </summary>
namespace Keymaster.UnitTests.Transformer
{
    using System;
    using System.Collections.Generic;

    using EngagedTechnologies.Exceptions;

    using FluentAssertions;

    using Keymaster.Model;
    using Keymaster.Transformer;

    using Xunit;

    public class InternalTokenDetailToExternalTokenDetailTests
    {
        private readonly Model.Internal.Token testObject;

        private readonly Model.External.TokenDetail expected;

        private readonly Keymaster.Transformer.InternalTokenToExternalTokenDetail transformer;

        public InternalTokenDetailToExternalTokenDetailTests()
        {
            this.testObject = new Model.Internal.Token()
            {
                Id = new Guid("6f8fb7d0-17a5-4f5e-aabf-d1a68cffd731"),
                Type = TokenType.ET_DISPATCH,
                Provider = Provider.ET_SMC3,
                CarrierScac = "AACT",
                Key = new Guid("7c5147e9-35b2-4b8d-be5d-fc76bd9be187").ToString()
            };

            this.expected = new Model.External.TokenDetail()
            {
                Id = new Guid("6f8fb7d0-17a5-4f5e-aabf-d1a68cffd731"),
                Type = TokenType.ET_DISPATCH,
                Provider = Provider.ET_SMC3,
                CarrierScac = "AACT",
                Key = "N2M1MTQ3ZTktMzViMi00YjhkLWJlNWQtZmM3NmJkOWJlMTg3"
            };

            this.transformer = new InternalTokenToExternalTokenDetail();
        }

        [Fact]
        public void TransformReturnsExternalTokenDetail()
        {
            var result = this.transformer.Transform(this.testObject);

            result.Should().BeOfType<Model.External.TokenDetail>();
            result.ShouldBeEquivalentTo(this.expected);
        }

        [Fact]
        public void TransformReturnsNullOnNullObject()
        {
            Model.Internal.Token internalToken = null;

            var result = this.transformer.Transform(internalToken);

            result.ShouldBeEquivalentTo(null);
        }

        [Fact]
        public void TransformNullKeyThrowsTmsException()
        {
            var fromTokens = new Model.Internal.Token
                                 {
                                     Id = Guid.NewGuid(),
                                     CarrierScac = "BADD",
                                     Provider = Provider.ET_SMC3,
                                     Type = TokenType.ET_TRACKING,
                                     Key = null
                                 };

            Action transform = () => this.transformer.Transform(fromTokens);

            transform.ShouldThrow<TmsException>().Where(ex => ex.ErrorTypeCode == ErrorTypeDefinitions.INTERNAL_RESOURCE_INVALID);
        }

        [Fact]
        public void TransformEmptyKeyThrowsTmsException()
        {
            var fromTokens = new Model.Internal.Token
                                 {
                                     Id = Guid.NewGuid(),
                                     CarrierScac = "BADD",
                                     Provider = Provider.ET_SMC3,
                                     Type = TokenType.ET_TRACKING,
                                     Key = string.Empty
                                 };

            Action transform = () => this.transformer.Transform(fromTokens);

            transform.ShouldThrow<TmsException>().Where(ex => ex.ErrorTypeCode == ErrorTypeDefinitions.INTERNAL_RESOURCE_INVALID);
        }
    }
}
