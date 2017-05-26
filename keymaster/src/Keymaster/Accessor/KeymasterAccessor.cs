// <copyright file="KeymasterAccessor.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      KeymasterAccessor
// </summary>

namespace Keymaster.Accessor
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Keymaster.Model;
    using Keymaster.Model.External;
    using Keymaster.Repository;
    using Keymaster.Transformer;

    public class KeymasterAccessor : IKeymasterAccessor
    {
        private readonly ITransformer<Model.Internal.Token, Model.External.TokenDetail>
            tokenDetailToExternalTransformer;

        private readonly ITransformer<List<Model.Internal.Token>, TokenCollection> tokensTransformer;
        private readonly ITokenRepository tokenRepository;

        public KeymasterAccessor(
            ITransformer<Model.Internal.Token, Model.External.TokenDetail> tokenTransformer,
            ITransformer<List<Model.Internal.Token>, TokenCollection> tokensTransformer,
            ITokenRepository tokenRepository)
        {
            this.tokenDetailToExternalTransformer = tokenTransformer;
            this.tokensTransformer = tokensTransformer;
            this.tokenRepository = tokenRepository;
        }

        public async Task<TokenCollection> GetApiTokensAsync(TokenQueryParameter token)
        {
            return this.tokensTransformer.Transform(await this.tokenRepository.GetApiTokensAsync(token));
        }

        public async Task<Model.External.TokenDetail> GetApiTokenAsync(Guid id)
        {
            var token = await this.tokenRepository.GetApiTokenAsync(id);

            return this.tokenDetailToExternalTransformer.Transform(token);
        }
    }
}
