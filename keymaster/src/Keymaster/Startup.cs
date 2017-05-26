// <copyright file="Startup.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     This is ASP.NET Core startup file.
// </summary>

using Microsoft.Extensions.Configuration.UserSecrets;

[assembly: UserSecretsId("Keymaster")]

namespace Keymaster
{
    using System.Collections.Generic;
    using System.IO;

    using Keymaster.Accessor;
    using Keymaster.Configuration;
    using Keymaster.Data;
    using Keymaster.Middleware;
    using Keymaster.Model.External;
    using Keymaster.Repository;
    using Keymaster.Transformer;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.PlatformAbstractions;

    using Newtonsoft.Json.Converters;

    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder =
                new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new Info { Title = "Keymaster Service", Version = "v1", Description = "Provide tokens to other services.", TermsOfService = "None", Contact = new Contact() { Name = "Engaged Technologies", Url = "http://www.engagedtechnologies.com" } });
                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Keymaster.xml");
                c.IncludeXmlComments(filePath);
                c.DescribeAllParametersInCamelCase();
            });

            services.AddMvc()
                .AddJsonOptions(
                    options =>
                        {
                            options.SerializerSettings.Converters.Add(new StringEnumConverter() { });
                        });

            services.Configure<Tokens>(this.Configuration.GetSection("TokensConfiguration"));

            services.TryAddScoped<ITransformer<Model.Internal.Token, Model.External.TokenDetail>, InternalTokenToExternalTokenDetail>();
            services.TryAddScoped<ITransformer<List<Model.Internal.Token>, TokenCollection>, InternalTokensToExternalTokenCollection>();

            services.TryAddScoped<ITokenRepository, TokenRepository>();
            services.TryAddScoped<IKeymasterAccessor, KeymasterAccessor>();

            services.TryAddScoped<ITokenContext, TempData>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMiddleware<TmsAuthenticationMiddleware>();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Keymaster API V1");
            });
            
            app.UseMvc();
        }
    }
}
