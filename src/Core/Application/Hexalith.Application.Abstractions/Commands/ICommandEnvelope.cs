// <copyright file="ICommandEnvelope.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;
using Hexalith.Application.Envelopes;
using Hexalith.Application.Metadatas;

/// <summary>
/// Interface for all command envelopes.
/// </summary>
public interface ICommandEnvelope : IEnvelope
{
    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <value>The message.</value>
    new ICommand Message { get; }
}

/// <summary>
/// Interface for all command envelopes.
/// </summary>
/// <typeparam name="TCommand">The type of the t command.</typeparam>
/// <typeparam name="TMetadata">The type of the t metadata.</typeparam>
public interface ICommandEnvelope<TCommand, TMetadata> : IEnvelope<TCommand, TMetadata>, ICommandEnvelope
    where TCommand : ICommand
    where TMetadata : IMetadata
{
    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <value>The message.</value>
    new TCommand Message { get; }
}