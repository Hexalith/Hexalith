// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 10-26-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-26-2023
// ***********************************************************************
// <copyright file="IMessageState.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.States;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Messages;
using Hexalith.Extensions.Common;

/// <summary>
/// Interface IMessageState
/// Extends the <see cref="IIdempotent" />.
/// </summary>
/// <seealso cref="IIdempotent" />
public interface IMessageState : IIdempotent
{
    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <value>The message.</value>
    BaseMessage? Message { get; }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    /// <value>The metadata.</value>
    BaseMetadata? Metadata { get; }
}