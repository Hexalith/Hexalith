// <copyright file="DateTimeService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Common;

using System;

/// <summary>
/// Simple date time service implementation.
/// </summary>
public sealed class DateTimeService : IDateTimeService
{
    /// <inheritdoc/>
    public DateTimeOffset Now => DateTimeOffset.Now;

    /// <inheritdoc/>
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}