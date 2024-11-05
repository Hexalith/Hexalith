// <copyright file="UserIdentity.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Sessions.Models;

using System.Runtime.Serialization;

/// <summary>
/// Represents a user identity in the application.
/// </summary>
[DataContract]
public class UserIdentity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserIdentity"/> class.
    /// </summary>
    public UserIdentity()
    {
        Id = string.Empty;
        Provider = string.Empty;
        Name = string.Empty;
        Email = string.Empty;
        IsGlobalAdministrator = false;
        Disabled = false;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserIdentity"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="provider">The authentication provider or identity service that manages this user.</param>
    /// <param name="name">The display name of the user.</param>
    /// <param name="email">The email address associated with the user account.</param>
    /// <param name="isGlobalAdministrator">A value indicating whether the user has global administrator privileges.</param>
    /// <param name="disabled">A value indicating whether the user account is disabled.</param>
    public UserIdentity(string id, string provider, string name, string email, bool isGlobalAdministrator, bool disabled)
    {
        Id = id;
        Provider = provider;
        Name = name;
        Email = email;
        IsGlobalAdministrator = isGlobalAdministrator;
        Disabled = disabled;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the user account is disabled.
    /// </summary>
    [DataMember(Order = 6)]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the email address associated with the user account.
    /// </summary>
    [DataMember(Order = 4)]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user.
    /// </summary>
    [DataMember(Order = 1)]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user has global administrator privileges.
    /// </summary>
    [DataMember(Order = 5)]
    public bool IsGlobalAdministrator { get; set; }

    /// <summary>
    /// Gets or sets the display name of the user.
    /// </summary>
    [DataMember(Order = 3)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the authentication provider or identity service that manages this user.
    /// </summary>
    [DataMember(Order = 2)]
    public string Provider { get; set; }
}