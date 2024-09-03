// <copyright file="UserPresence.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components;

// <summary>
// Represents the presence status of a user.
// </summary>
public enum UserPresence
{
    /// <summary>
    /// The user is busy.
    /// </summary>
    Busy,

    /// <summary>
    /// The user is out of office.
    /// </summary>
    OutOfOffice,

    /// <summary>
    /// The user is away.
    /// </summary>
    Away,

    /// <summary>
    /// The user is available.
    /// </summary>
    Available,

    /// <summary>
    /// The user is offline.
    /// </summary>
    Offline,

    /// <summary>
    /// The user does not want to be disturbed.
    /// </summary>
    DoNotDisturb,

    /// <summary>
    /// The presence status is unknown.
    /// </summary>
    Unknown,
}