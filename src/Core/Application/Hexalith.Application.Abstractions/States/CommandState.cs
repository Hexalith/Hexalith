// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-05-2023
// ***********************************************************************
// <copyright file="CommandState.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.States;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;

/// <summary>
/// Class CommandState.
/// </summary>
[DataContract]
[Serializable]
public class CommandState : MessageState<BaseCommand, BaseMetadata>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandState" /> class.
    /// </summary>
    public CommandState()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandState" /> class.
    /// </summary>
    /// <param name="receivedDate">The received date.</param>
    /// <param name="message">The command.</param>
    /// <param name="metadata">The metadata.</param>
    [JsonConstructor]
    public CommandState(
        DateTimeOffset? receivedDate,
        BaseCommand? message,
        BaseMetadata? metadata)
        : base(receivedDate, message, metadata)
    {
    }
}