// <copyright file="IMessage.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Messages;

/// <summary>
/// Base interface for messages.
/// </summary>
public interface IMessage
{
    /// <summary>
    /// Gets the domain aggregate id.
    /// </summary>
    string AggregateId { get; }

    /// <summary>
    /// Gets the domain aggregate name.
    /// </summary>
    string AggregateName { get; }

    /// <summary>
    /// Gets the message major version.
    /// </summary>
    int MajorVersion { get; }

    /// <summary>
    /// Gets the message minor version.
    /// </summary>
    int MinorVersion { get; }

    /// <summary>
    /// Gets the message name.
    /// </summary>
    string TypeName { get; }
}