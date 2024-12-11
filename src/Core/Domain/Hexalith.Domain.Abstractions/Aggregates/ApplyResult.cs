// <copyright file="ApplyResult.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Aggregates;

using System.Collections.Generic;

/// <summary>
/// Represents the result of applying domain events to an aggregate.
/// </summary>
/// <param name="Aggregate">The domain aggregate.</param>
/// <param name="Messages">The collection of messages produced during the application of events.</param>
/// <param name="Failed">A flag indicating whether the application of events failed.</param>
/// <param name="Reason">The reason why the application of events failed.</param>
public record ApplyResult(
    IDomainAggregate Aggregate,
    IEnumerable<object> Messages,
    bool Failed,
    string? Reason = null)
{
}