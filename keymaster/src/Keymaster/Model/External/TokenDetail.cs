// <copyright file="TokenDetail.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      The result of getting a token.
// </summary>

namespace Keymaster.Model.External
{
    using System;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// The token.
    /// </summary>
    public class TokenDetail
    {
        /// <summary>
        /// The token identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The token type
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public TokenType Type { get; set; }

        /// <summary>
        /// The providing entity of the token
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Provider Provider { get; set; }

        /// <summary>
        /// The Standard Carrier Alpha Code associated with the token
        /// </summary>
        public string CarrierScac { get; set; }

        /// <summary>
        /// The base64 encoded key
        /// </summary>
        public string Key { get; set; }
    }
}
