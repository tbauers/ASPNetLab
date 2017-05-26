// <copyright file="TokenCollection.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      The result of getting tokens.
// </summary>

namespace Keymaster.Model.External
{
    using System.Collections.Generic;

    /// <summary>
    /// A list of tokens.
    /// </summary>
    public class TokenCollection
    {
        /// <summary>
        /// A list of tokens that meet the supplied query parameters.
        /// </summary>
        public List<TokenItem> Tokens { get; set; }
    }
}
