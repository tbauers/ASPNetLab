// <copyright file="InternalTokensToExternalTokenCollectionTests.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      Tests for InternalTokensToExternalTokenCollection
// </summary>

namespace Keymaster.UnitTests.Transformer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Castle.Components.DictionaryAdapter;

    using EngagedTechnologies.Exceptions;

    using FluentAssertions;

    using Keymaster.Model;
    using Keymaster.Model.External;
    using Keymaster.Model.Internal;
    using Keymaster.Transformer;

    using Xunit;

    public class InternalTokensToExternalTokenCollectionTests
    {
        private readonly InternalTokensToExternalTokenCollection transformer;

        public InternalTokensToExternalTokenCollectionTests()
        {
            this.transformer = new InternalTokensToExternalTokenCollection();
        }

        [Fact]
        public void TransformValidInternalTokenListReturnsEquivalentTokenCollection()
        {
            var fromTokens = new List<Token>
                                 {
                                     new Token
                                         {
                                             Id = new Guid("e77b5df4-7b48-40fa-a9a8-76a3b11efd84"),
                                             CarrierScac = "FISH",
                                             Provider = Provider.ET_SMC3,
                                             Type = TokenType.ET_TRACKING,
                                             Key = "d9d3b2fb-7d64-4dee-b835-6b49d63413fa"
                                         }
                                 };

            var toTokens = new List<TokenItem>
                               {
                                   new TokenItem
                                       {
                                           Id =
                                               new Guid(
                                                   "e77b5df4-7b48-40fa-a9a8-76a3b11efd84"),
                                           CarrierScac = "FISH",
                                           Provider = Provider.ET_SMC3,
                                           Type = TokenType.ET_TRACKING,
                                           Key = "ZDlkM2IyZmItN2Q2NC00ZGVlLWI4MzUtNmI0OWQ2MzQxM2Zh"
                                       }
                               };

            var expectedResult = new TokenCollection { Tokens = toTokens };

            var result = this.transformer.Transform(fromTokens);

            result.ShouldBeEquivalentTo(expectedResult);
        }

        [Fact]
        public void TransformNullKeyThrowsTmsException()
        {
            var fromTokens = new List<Token>
                                 {
                                     new Token
                                         {
                                             Id = Guid.NewGuid(),
                                             CarrierScac = "BADD",
                                             Provider = Provider.ET_SMC3,
                                             Type = TokenType.ET_TRACKING,
                                             Key = null
                                         }
                                 };

            Action transform = () => this.transformer.Transform(fromTokens);

            transform.ShouldThrow<TmsException>().Where(ex => ex.ErrorTypeCode == ErrorTypeDefinitions.INTERNAL_RESOURCE_INVALID);
        }

        [Fact]
        public void TransformEmptyKeyThrowsTmsException()
        {
            var fromTokens = new List<Token>
                                 {
                                     new Token
                                         {
                                             Id = Guid.NewGuid(),
                                             CarrierScac = "BADD",
                                             Provider = Provider.ET_SMC3,
                                             Type = TokenType.ET_TRACKING,
                                             Key = string.Empty
                                         }
                                 };

            Action transform = () => this.transformer.Transform(fromTokens);

            transform.ShouldThrow<TmsException>().Where(ex => ex.ErrorTypeCode == ErrorTypeDefinitions.INTERNAL_RESOURCE_INVALID);
        }
    }
}
