// <copyright file="KeymasterTestConfiguration.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     Class used to get the service endpoints that are being tested
// </summary>

namespace Keymaster.ServiceTests.Configuration
{
    using System;

    public class KeymasterTestConfiguration
    {
        public string ApplicationContentRoot { get; set; }

        public Uri ServerAddress { get; set; }

        public EndpointsConfiguration Endpoints { get; set; }

        public class EndpointsConfiguration
        {
            public string Host { get; set; }

            public string TestAccountResource { get; set; }

            public string TokensResource { get; set; }
        }
    }
}
