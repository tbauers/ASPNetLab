// <copyright file="KeymasterController.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//      Keymaster controller.
// </summary>

namespace Keymaster.Controllers
{
    using System;
    using System.Threading.Tasks;

    using EngagedTechnologies.Exceptions;
    using EngagedTechnologies.Middleware.Errors;

    using Keymaster.Accessor;
    using Keymaster.Model;
    using Keymaster.Model.External;

    using Microsoft.AspNetCore.Mvc;

    public class KeymasterController : Controller
    {
        private readonly IKeymasterAccessor keymasterAccessor;

        public KeymasterController(IKeymasterAccessor keymasterAccessor)
        {
            this.keymasterAccessor = keymasterAccessor;
        }

        /// <summary>
        /// Get tokens for the specified account.
        /// </summary>
        /// <param name="accountId">The account identifier</param>
        /// <param name="token">The object that contains the query parameters</param>
        /// <remarks>Errors with response bodies include the following headers:<br/>
        ///     "tms-error-type": "xxxxxxx"<br/>
        ///     "tms-error-location-code": "xxxxxxx"
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <response code="400">The request contains an invalid parameter.</response>
        /// <response code="500">The system encountered an unexpected problem.</response>
        // TODO: In the future we may want to create a custom attribute to display the error headers in Swagger.
        [HttpGet("/accounts/{accountId}/tokens")]
        [ProducesResponseType(typeof(TokenCollection), 200)]
        [ProducesResponseType(typeof(ErrorResponseBody), 400)]
        [ProducesResponseType(typeof(ErrorResponseBody), 500)]
        public async Task<IActionResult> GetTokens(
            string accountId,
            [FromQuery] TokenQueryParameter token)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    throw new TmsException("MB4P9N1", ErrorTypeDefinitions.RESOURCE_SPECIFIED_CONTAINS_INVALID_PARAMETER);
                }

                return this.Ok(await this.keymasterAccessor.GetApiTokensAsync(token));
            }
            catch (TmsException ex)
            {
                this.Response.Headers.Add(ErrorResponseHeaderKey.ERROR_LOCATION_CODE, ex.LocationCode);
                this.Response.Headers.Add(ErrorResponseHeaderKey.ERROR_TYPE, ex.ErrorTypeCode);

                return ErrorTypeToActionResultMap.GetActionResult(
                    ex.ErrorTypeCode,
                    new ErrorResponseBody { ErrorMessage = ex.DisplayMessage, ErrorType = ex.ErrorTypeCode, LocationCode = ex.LocationCode });
            }
        }

        /// <summary>
        /// Get an individual token by Id for the specified account.
        /// </summary>
        /// <param name="accountId">The account identifier</param>
        /// <param name="id">The identifier of the requested token</param>
        /// <remarks>Errors with response bodies include the following headers:<br/>
        ///     "tms-error-type": "xxxxxxx"<br/>
        ///     "tms-error-location-code": "xxxxxxx"
        /// </remarks>
        /// <response code="200">Ok</response>
        /// <response code="404">The specified token does not exist.</response>
        /// <response code="500">The system encountered an unexpected problem.</response>
        // TODO: In the future we may want to create a custom attribute to display the error headers in Swagger.
        [Route("accounts/{accountId}/tokens/{id:Guid}")]
        [HttpGet]
        [ProducesResponseType(typeof(TokenDetail), 200)]
        [ProducesResponseType(typeof(ErrorResponseBody), 404)]
        [ProducesResponseType(typeof(ErrorResponseBody), 500)]
        public async Task<IActionResult> GetTokenById(string accountId, Guid id)
        {
            TokenDetail tokenResult = null;

            try
            {
                tokenResult = await this.keymasterAccessor.GetApiTokenAsync(id);

                if (tokenResult == null)
                {
                    throw new TmsException("6D288N1", ErrorTypeDefinitions.RESOURCE_SPECIFIED_DOES_NOT_EXIST);
                }

                return this.Ok(tokenResult);
            }
            catch (TmsException ex)
            {
                this.Response.Headers.Add(ErrorResponseHeaderKey.ERROR_LOCATION_CODE, ex.LocationCode);
                this.Response.Headers.Add(ErrorResponseHeaderKey.ERROR_TYPE, ex.ErrorTypeCode);

                return ErrorTypeToActionResultMap.GetActionResult(
                    ex.ErrorTypeCode,
                    new ErrorResponseBody { ErrorMessage = ex.DisplayMessage, ErrorType = ex.ErrorTypeCode, LocationCode = ex.LocationCode });
            }
        }
    }
}
