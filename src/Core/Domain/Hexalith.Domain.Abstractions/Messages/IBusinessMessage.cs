// <copyright file="IBusinessMessage.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Messages;

using Hexalith.Extensions.Serialization;

/// <summary>
/// Base interface for messages.
/// </summary>
public interface IBusinessMessage : IPolymorphicSerializable
{
    /// <summary>
    /// Gets a value indicating whether this message is an integration message.
    /// </summary>
    bool IsIntegrationMessage => false;
}