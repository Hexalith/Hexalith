// <copyright file="TaskProcessorStatus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Tasks;

/// <summary>
/// The task processor status.
/// </summary>
public enum TaskProcessorStatus
{
    /// <summary>
    /// The task is not started.
    /// </summary>
    New = 0,

    /// <summary>
    /// The task is suspended.
    /// </summary>
    Suspended = 1,

    /// <summary>
    /// The task is active.
    /// </summary>
    Active = 2,

    /// <summary>
    /// The task completed.
    /// </summary>
    Completed = 10,

    /// <summary>
    /// The task is canceled.
    /// </summary>
    Canceled = 11,
}