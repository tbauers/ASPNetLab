// <copyright file="ILogAdapter.cs" company="Engaged Technologies">
//      Copyright (c) Engaged Technologies. All rights reserved.
//  </copyright>
// <summary>
//     interface definition for encapsulates logging to Log API
// </summary>

namespace Keymaster.Logger
{
    using System.Threading.Tasks;

    public interface ILogAdapter
    {
        Task LogAsync(LogItem logItem);
    }
}
