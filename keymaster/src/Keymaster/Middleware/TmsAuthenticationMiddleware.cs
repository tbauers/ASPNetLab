// <copyright file="TmsAuthenticationMiddleware.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     The program file.
// </summary>

namespace Keymaster.Middleware
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class TmsAuthenticationMiddleware
    {
        private readonly RequestDelegate nextRequestDelegate;

        public TmsAuthenticationMiddleware(RequestDelegate nextRequestDelegate)
        {
            this.nextRequestDelegate = nextRequestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("TmsCustom"))
            {
                string[] authHeaderParts = authHeader.Split(' ');
                if (authHeaderParts.Length != 2)
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    return;
                }

                if (authHeaderParts[0] == "TmsCustom" && authHeaderParts[1] == "tmsAuthorization")
                {
                    await this.nextRequestDelegate(context);
                }
                else
                {
                    context.Response.StatusCode = 401; // Unauthorized
                    return;
                }
            }
            else
            {
                // no authorization header
                context.Response.StatusCode = 401; // Unauthorized
                return;
            }
        }
    }
}
