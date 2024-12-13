// <copyright file="CommandHandlerAggregateNameMismatch.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System;

using Hexalith.Application.Metadatas;

/// <summary>
/// Exception thrown when there is a mismatch between the expected aggregate name and the actual aggregate name in the command handler.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CommandHandlerAggregateNameMismatch"/> class.
/// </remarks>
/// <param name="expectedAggregateName">The expected aggregate name.</param>
/// <param name="metadata">The metadata associated with the command.</param>
[Serializable]
public class CommandHandlerAggregateNameMismatch(string expectedAggregateName, Metadata metadata)
        : Exception($"Could not handle command because the aggregate name does not match the expected name '{expectedAggregateName}'. "
            + metadata.ToLogString())
{
    /// <summary>
    /// Gets the expected aggregate name.
    /// </summary>
    public string ExpectedAggregateName { get; } = expectedAggregateName;

    /// <summary>
    /// Gets the metadata associated with the command.
    /// </summary>
    public Metadata Metadata { get; } = metadata;
}