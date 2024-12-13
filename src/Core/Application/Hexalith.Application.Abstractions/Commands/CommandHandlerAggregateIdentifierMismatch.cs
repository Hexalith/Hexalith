// <copyright file="CommandHandlerAggregateIdentifierMismatch.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System;

using Hexalith.Application.Metadatas;

/// <summary>
/// Exception thrown when the aggregate identifier does not match the expected identifier.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CommandHandlerAggregateIdentifierMismatch"/> class.
/// </remarks>
/// <param name="expectedAggregateId">The expected identifier.</param>
/// <param name="metadata">The metadata associated with the command.</param>
[Serializable]
public class CommandHandlerAggregateIdentifierMismatch(string expectedAggregateId, Metadata metadata)
        : Exception($"Could not handle command because the aggregate identifier does not match the expected identifier '{expectedAggregateId}'. "
            + metadata.ToLogString())
{
    /// <summary>
    /// Gets the expected aggregate identifier.
    /// </summary>
    public string ExpectedAggregateId { get; } = expectedAggregateId;

    /// <summary>
    /// Gets the metadata associated with the command.
    /// </summary>
    public Metadata Metadata { get; } = metadata;
}