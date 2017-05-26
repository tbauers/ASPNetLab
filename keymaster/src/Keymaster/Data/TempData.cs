// <copyright file="TempData.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      Hard coded token data
// </summary>

namespace Keymaster.Data
{
    using System.Collections.Generic;

    using Keymaster.Configuration;
    using Keymaster.Model.Internal;

    using Microsoft.Extensions.Options;

    public class TempData : ITokenContext
    {
        public TempData(IOptions<Tokens> tokensConfiguration)
        {
            this.Tokens = tokensConfiguration.Value.Token;
        }

        public List<Token> Tokens { get; }
    }
}
