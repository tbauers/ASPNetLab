// <copyright file="LogAdapter.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     Encapsulates logging to Log API
// </summary>

namespace Keymaster.Logger
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    public class LogAdapter : ILogAdapter
    {
        private readonly LogAdapterConfiguration logConfig;

        public LogAdapter(IOptions<LogAdapterConfiguration> logConfig)
        {
            this.logConfig = logConfig.Value;
        }

        public async Task LogAsync(LogItem logItem)
        {
            if (logItem != null)
            {
                try
                {
                    var client = new HttpClient();

                    using (var httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(this.logConfig.BaseAddress);

                        httpClient.DefaultRequestHeaders
                            .Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        var content = new StringContent(JsonConvert.SerializeObject(logItem), Encoding.UTF8, "application/json");

                        var response = await httpClient.PostAsync(this.logConfig.LogSubPath, content);
                    }
                }
                catch
                {
                    // if we can't log? what are we gonna do?  log an exception?  bwaahahahaa
                }
            }
        }
    }
}
