// <copyright file="LogItem.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     Model used for logging
// </summary>

namespace Keymaster.Logger
{
    using System;
    using System.Collections.Generic;

    public class LogItem
    {
        public string Api { get; set; }

        public string ErrorType { get; set; }

        public Guid Id { get; set; }

        public string LocationCode { get; set; }

        public DateTime LogDate { get; set; }

        public string Message { get; set; }

        public string Severity { get; set; }

        public string StackTrace { get; set; }

        public Dictionary<string, string> ExtendedProperties { get; set; }
    }
}
