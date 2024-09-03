// <copyright file="ApplySucceededResult.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Aggregates;

using System.Collections.Generic;

/// <summary>
/// Represents the succeeded of applying domain events to an aggregate.
/// </summary>
/// <param name="Aggregate">The domain aggregate.</param>
/// <param name="Messages">The collection of messages produced during the application of events.</param>
public record ApplySucceededResult(
    IDomainAggregate Aggregate,
    IEnumerable<object> Messages)
    : ApplyResult(Aggregate, Messages, false)
{
}