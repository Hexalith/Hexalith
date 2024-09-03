// <copyright file="IDateTimeService.cs">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Common;

using System;

/// <summary>
/// Generic date time service used to simplify mocking for tests.
/// </summary>
public interface IDateTimeService
{
    /// <summary>
    /// Gets get the current date time.
    /// </summary>
    DateTimeOffset Now { get; }

    /// <summary>
    /// Gets get the current universal date time (GMT+0).
    /// </summary>
    DateTimeOffset UtcNow { get; }
}