// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.DaprRuntime
// Author           : Jérôme Piquot
// Created          : 10-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-28-2023
// ***********************************************************************
// <copyright file="NumberSequenceActor.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.DaprRuntime.Actors;

using System;

using Dapr.Actors.Runtime;

using Hexalith.Application;

/// <summary>
/// Number sequence actor class.
/// </summary>
public class NumberSequenceActor : Actor, INumberSequenceActor
{
    /// <summary>
    /// The value.
    /// </summary>
    private long? _currentValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="NumberSequenceActor" /> class.
    /// </summary>
    /// <param name="host">The <see cref="ActorHost" /> that will host this actor instance.</param>
    public NumberSequenceActor(ActorHost host)
        : base(host) => ArgumentNullException.ThrowIfNull(host);

    /// <inheritdoc/>
    public async Task<long> NextAsync()
    {
        if (_currentValue is null)
        {
            ConditionalValue<long> result = await StateManager
                .TryGetStateAsync<long>(ApplicationConstants.StateName)
                .ConfigureAwait(false);
            _currentValue = result.HasValue ? result.Value : 0L;
        }

        _currentValue++;
        await StateManager
            .SetStateAsync(ApplicationConstants.StateName, _currentValue.Value)
            .ConfigureAwait(false);
        await StateManager
            .SaveStateAsync()
            .ConfigureAwait(false);
        return _currentValue.Value;
    }
}