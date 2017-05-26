// <copyright file="TokenRepository.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      Repository for interacting with a token data source.
// </summary>

namespace Keymaster.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Keymaster.Data;
    using Keymaster.Model;

    public class TokenRepository : ITokenRepository
    {
        private readonly ITokenContext tokenContext;

        public TokenRepository(ITokenContext tokenContext)
        {
            this.tokenContext = tokenContext;
        }

        public async Task<List<Model.Internal.Token>> GetApiTokensAsync(TokenQueryParameter token)
        {
            // This will eventually call another API and can then be asynchronous
            return this.tokenContext.Tokens
                .Where(d => d.Type == (token.Type ?? d.Type))
                .Where(d => d.Provider == (token.Provider ?? d.Provider))
                .Where(d => d.CarrierScac == (token.CarrierScac?.ToUpper() ?? d.CarrierScac))
                .ToList();
        }

        public async Task<Model.Internal.Token> GetApiTokenAsync(Guid id)
        {
            return this.tokenContext.Tokens.FirstOrDefault(a => a.Id.Equals(id));
        }
    }
}
