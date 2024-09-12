// <copyright file="ExecuteCommandResult.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.Commands;

using System.Collections.Generic;

using Hexalith.Domain.Aggregates;

/// <summary>
/// Command execution result.
/// </summary>
/// <param name="Aggregate">The aggregate.</param>
/// <param name="SourceEvents">The source events.</param>
/// <param name="IntegrationEvents">The integration events.</param>
public record ExecuteCommandResult(IDomainAggregate Aggregate, IEnumerable<object> SourceEvents, IEnumerable<object> IntegrationEvents)
{
}