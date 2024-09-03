// <copyright file="RetryStatus.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Tasks;

/// <summary>
/// The retry policy status.
/// </summary>
public enum RetryStatus
{
    /// <summary>
    /// The retry is enabled.
    /// </summary>
    Enabled = 0,

    /// <summary>
    /// Waiting for policy retry wait time to end.
    /// </summary>
    Suspended = 1,

    /// <summary>
    /// The retry policy completed by maximum retry count or timeout.
    /// </summary>
    Stopped = 10,
}