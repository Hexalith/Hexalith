// <copyright file="ExecuteCommandResult.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System.Collections.Generic;

using Hexalith.Domains;

/// <summary>
/// Command execution result.
/// </summary>
/// <param name="Aggregate">The aggregate.</param>
/// <param name="SourceEvents">The source events.</param>
/// <param name="IntegrationEvents">The integration events.</param>
/// <param name="Failed">if set to <c>true</c> the command failed.</param>
public record ExecuteCommandResult(
    IDomainAggregate Aggregate,
    IEnumerable<object> SourceEvents,
    IEnumerable<object> IntegrationEvents,
    bool Failed)
{
}