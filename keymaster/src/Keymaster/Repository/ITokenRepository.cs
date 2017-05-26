// <copyright file="ITokenRepository.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      Interface for a token repository.
// </summary>

namespace Keymaster.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Keymaster.Model;
    using Keymaster.Model.Internal;

    public interface ITokenRepository
    {
        Task<List<Token>> GetApiTokensAsync(TokenQueryParameter token);

        Task<Token> GetApiTokenAsync(Guid id);
    }
}
