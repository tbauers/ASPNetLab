// <copyright file="ITokenContext.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      Interface for token data
// </summary>

namespace Keymaster.Data
{
    using System.Collections.Generic;

    using Keymaster.Model.Internal;

    public interface ITokenContext
    {
        List<Token> Tokens { get; }
    }
}
