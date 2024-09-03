// <copyright file="IMessage.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Messages;

/// <summary>
/// Base interface for messages.
/// </summary>
[Obsolete("Use plain poco objects.", false)]
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
    /// Gets a value indicating whether this instance is private to aggregate.
    /// </summary>
    /// <value><c>true</c> if this instance is private to aggregate; otherwise, <c>false</c>.</value>
    bool IsPrivateToAggregate { get; }

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