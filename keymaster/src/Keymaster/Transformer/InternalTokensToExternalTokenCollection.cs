// <copyright file="InternalTokensToExternalTokenCollection.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     Transforms between a list of our internal token objects and the external facing token objects for the API.
// </summary>

namespace Keymaster.Transformer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using EngagedTechnologies.Exceptions;

    using Keymaster.Model.External;

    public class InternalTokensToExternalTokenCollection : ITransformer<List<Model.Internal.Token>, TokenCollection>
    {
        public TokenCollection Transform(List<Model.Internal.Token> fromTokens)
        {
            var tokens = fromTokens.Select(
                t =>
                    {
                        if (string.IsNullOrWhiteSpace(t.Key))
                        {
                            throw new TmsException("FTCW9N1", ErrorTypeDefinitions.INTERNAL_RESOURCE_INVALID);
                        }

                        return new TokenItem
                                   {
                                       Id = t.Id,
                                       CarrierScac = t.CarrierScac,
                                       Provider = t.Provider,
                                       Type = t.Type,
                                       Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(t.Key))
                                   };
                    }).ToList();

            return new TokenCollection { Tokens = tokens };
        }
    }
}
