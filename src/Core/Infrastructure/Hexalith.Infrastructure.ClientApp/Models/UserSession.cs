// <copyright file="UserSession.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

public class UserSession
{
    public DateTime CreatedAt { get; set; }

    public string Id { get; set; }

    public DateTime LastActivity { get; set; }

    public string PartitionId { get; set; }
    public string UserId { get; set; }

    public string Username { get; set; }
}