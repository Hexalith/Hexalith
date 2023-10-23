// ***********************************************************************
// Assembly         : Hexalith.WebApp.Client
// Author           : Jérôme Piquot
// Created          : 10-21-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-22-2023
// ***********************************************************************
// <copyright file="UserInfo.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.WebApp.Client;

/// <summary>
/// Class UserInfo.
/// </summary>
public class UserInfo
{
    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    /// <value>The email.</value>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the user identifier.
    /// </summary>
    /// <value>The user identifier.</value>
    public string? UserId { get; set; }
}