// <copyright file="ContactInformation.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Models;

using System.Runtime.Serialization;

/// <summary>
/// Represents contact information for a user.
/// </summary>
/// <param name="Id">The unique identifier of the contact.</param>
/// <param name="Email">The email address of the contact.</param>
/// <param name="Name">The name of the contact.</param>
[DataContract]
public record ContactInformation(
    [property: DataMember(Order = 1)]
    string Id,
    [property: DataMember(Order = 2)]
    string Email,
    [property: DataMember(Order = 3)]
    string Name);