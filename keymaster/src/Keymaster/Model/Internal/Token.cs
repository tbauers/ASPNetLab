// <copyright file="Token.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      Internal token thing.
// </summary>

namespace Keymaster.Model.Internal
{
    using System;

    public class Token
    {
        public Guid Id { get; set; }

        public TokenType Type { get; set; }

        public Provider Provider { get; set; }

        public string CarrierScac { get; set; }

        public string Key { get; set; }
    }
}
