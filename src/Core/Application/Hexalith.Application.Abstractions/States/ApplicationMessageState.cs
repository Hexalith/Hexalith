// <copyright file="ApplicationMessageState.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Hexalith.Application.States;

using Hexalith.Application.MessageMetadatas;

/// <summary>
/// Represents the state of an application message.
/// </summary>
/// <param name="Message">The message.</param>
/// <param name="Metadata">The metadata associated with the message.</param>
public record ApplicationMessageState(object Message, Metadata Metadata)
{
}