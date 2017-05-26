// <copyright file="IKeymasterAccessor.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      Keymaster accessor interface.
// </summary>

namespace Keymaster.Accessor
{
    using System;
    using System.Threading.Tasks;

    using Keymaster.Model;
    using Keymaster.Model.External;

    public interface IKeymasterAccessor
    {
        Task<TokenCollection> GetApiTokensAsync(TokenQueryParameter token);

        Task<TokenDetail> GetApiTokenAsync(Guid id);
    }
}
