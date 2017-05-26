// <copyright file="InternalTokenToExternalTokenDetail.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     Transforms between a list of our internal TokenDetail and the external facing TokenDetail for the API.
// </summary>
namespace Keymaster.Transformer
{
    using System;

    using EngagedTechnologies.Exceptions;

    using Keymaster.Model.External;

    public class InternalTokenToExternalTokenDetail :
        ITransformer<Model.Internal.Token, Model.External.TokenDetail>
    {
        public Model.External.TokenDetail Transform(Keymaster.Model.Internal.Token fromTokenDetail)
        {
            if (fromTokenDetail == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(fromTokenDetail.Key))
            {
                throw new TmsException("T77X9N1", ErrorTypeDefinitions.INTERNAL_RESOURCE_INVALID);
            }

            return new TokenDetail()
                       {
                           Id = fromTokenDetail.Id,
                           Type = fromTokenDetail.Type,
                           Provider = fromTokenDetail.Provider,
                           CarrierScac = fromTokenDetail.CarrierScac,
                           Key = this.Base64Encode(fromTokenDetail.Key)
                       };
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
