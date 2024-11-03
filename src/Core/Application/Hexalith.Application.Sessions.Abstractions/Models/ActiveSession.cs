// <copyright file="ActiveSession.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Models;

using System;
using System.Runtime.Serialization;

/// <summary>
/// Represents aa active session with its details.
/// </summary>
/// <param name="Id">The unique identifier for the session.</param>
/// <param name="Expiration">The expiration date of the session.</param>
[DataContract]
public record ActiveSession(
    [property: DataMember(Order = 1)]
    string Id,
    [property: DataMember(Order = 2)]
    DateTimeOffset Expiration);