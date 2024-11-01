// <copyright file="ContactInformation.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Models;

using System.Runtime.Serialization;

/// <summary>
/// Represents contact information for a user.
/// </summary>
[DataContract]
public record ContactInformation(
    /// <summary>
    /// Gets the unique identifier of the contact.
    /// </summary>
    [property: DataMember(Order = 1)]
    string Id,
    /// <summary>
    /// Gets the email address of the contact.
    /// </summary>
    [property: DataMember(Order = 2)]
    string Email,
    /// <summary>
    /// Gets the name of the contact.
    /// </summary>
    [property: DataMember(Order = 3)]
    string Name);
