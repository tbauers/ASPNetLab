// <copyright file="TokenType.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      Enum for TokenType
// </summary>

namespace Keymaster.Model
{
    /// <summary>
    /// Type of the Token
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// This token type would be used for issuing a dispatch/pickup request
        /// </summary>
        ET_DISPATCH = 1,

        /// <summary>
        /// This token type would be used for issuing a tracking request
        /// </summary>
        ET_TRACKING = 2
    }
}
