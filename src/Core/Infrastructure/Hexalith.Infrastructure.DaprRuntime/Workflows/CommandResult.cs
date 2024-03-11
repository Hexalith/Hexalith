// <copyright file="CommandResult.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.DaprRuntime.Workflows;

public class CommandResult
{
    public bool Completed { get; internal set; }
    public long LastCommandId { get; set; }
}