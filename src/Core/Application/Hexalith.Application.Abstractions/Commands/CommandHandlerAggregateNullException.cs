// <copyright file="CommandHandlerAggregateNullException.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using System;

using Hexalith.Application.Metadatas;

/// <summary>
/// Exception thrown when the aggregate is null in the command handler.
/// </summary>
/// <param name="metadata">The metadata associated with the command.</param>
[Serializable]
public class CommandHandlerAggregateNullException(Metadata metadata)
    : Exception("Could not handle command because the aggregate is null." + metadata.ToLogString())
{
    /// <summary>
    /// Gets the metadata associated with the command.
    /// </summary>
    public Metadata Metadata => metadata;
}