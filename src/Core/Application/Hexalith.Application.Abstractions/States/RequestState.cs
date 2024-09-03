// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-29-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-05-2023
// ***********************************************************************
// <copyright file="RequestState.cs" company="Jérôme Piquot">
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

using Hexalith.Application.Metadatas;
using Hexalith.Application.Requests;

/// <summary>
/// Class RequestState.
/// </summary>
[DataContract]
public class RequestState : MessageState<BaseRequest, BaseMetadata>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestState" /> class.
    /// </summary>
    public RequestState()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestState" /> class.
    /// </summary>
    /// <param name="receivedDate">The received date.</param>
    /// <param name="message">The request.</param>
    /// <param name="metadata">The metadata.</param>
    [JsonConstructor]
    public RequestState(
        DateTimeOffset? receivedDate,
        BaseRequest? message,
        BaseMetadata? metadata)
        : base(receivedDate, message, metadata)
    {
    }
}