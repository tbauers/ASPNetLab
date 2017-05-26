// <copyright file="TokenQueryParameter.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      Token query parameter model
// </summary>

namespace Keymaster.Model
{
    /// <summary>
    /// The object that holds the query parameters for a token search.
    /// </summary>
    public class TokenQueryParameter
    {
        /// <summary>
        /// The type of token used for filtering results. Default returns all token types.
        /// </summary>
        public TokenType? Type { get; set; }

        /// <summary>
        /// The provider of the token used for filtering results. Default returns all providers.
        /// </summary>
        public Provider? Provider { get; set; }

        /// <summary>
        /// The Standard Carrier Alpha Code (SCAC) used for filtering results. Default returns all SCACs.
        /// </summary>
        public string CarrierScac { get; set; }
    }
}
