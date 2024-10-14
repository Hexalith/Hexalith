// <copyright file="ApplyFailedResult.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Aggregates;

using System.Collections.Generic;

/// <summary>
/// Represents the failed result of applying domain events to an aggregate.
/// </summary>
/// <param name="Aggregate">The domain aggregate.</param>
/// <param name="Messages">The collection of messages produced during the application of events.</param>
public record ApplyFailedResult(
    IDomainAggregate Aggregate,
    IEnumerable<object> Messages)
    : ApplyResult(Aggregate, Messages, true)
{
}