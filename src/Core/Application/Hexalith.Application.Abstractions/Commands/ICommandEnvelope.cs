// <copyright file="ICommandEnvelope.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Commands;

using Hexalith.Application.Abstractions.Envelopes;
using Hexalith.Application.Abstractions.Metadatas;

/// <summary>
/// Interface for all command envelopes.
/// </summary>
public interface ICommandEnvelope : IEnvelope
{
	new ICommand Message { get; }
}

/// <summary>
/// Interface for all command envelopes.
/// </summary>
public interface ICommandEnvelope<TCommand, TMetadata> : IEnvelope<TCommand, TMetadata>, ICommandEnvelope
	where TCommand : ICommand
	where TMetadata : IMetadata
{
	new TCommand Message { get; }
}