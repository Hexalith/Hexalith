// <copyright file="TaskProcessingHistoryTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Tasks;

using System;

using FluentAssertions;

using Hexalith.Application.Tasks;

public class TaskProcessingHistoryTest
{
    [Fact]
    public void CancelledHistoryDateShouldBeNow()
    {
        TaskProcessingHistory history = new TaskProcessingHistory(DateTimeOffset.UtcNow, null, null, null, null)
            .Canceled();
        _ = history.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        _ = history.CanceledDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        _ = history.CompletedDate.Should().BeNull();
        _ = history.ProcessingStartDate.Should().BeNull();
        _ = history.SuspendedDate.Should().BeNull();
    }

    [Fact]
    public void CompletedHistoryDateShouldBeNow()
    {
        TaskProcessingHistory history = new TaskProcessingHistory(DateTimeOffset.UtcNow, null, null, null, null)
            .Completed();
        _ = history.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        _ = history.CompletedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        _ = history.ProcessingStartDate.Should().BeNull();
        _ = history.CanceledDate.Should().BeNull();
        _ = history.SuspendedDate.Should().BeNull();
    }

    [Fact]
    public void CreatedHistoryDateShouldBeNow()
    {
        TaskProcessingHistory history = new(DateTimeOffset.UtcNow, null, null, null, null);
        _ = history.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        _ = history.CanceledDate.Should().BeNull();
        _ = history.CompletedDate.Should().BeNull();
        _ = history.ProcessingStartDate.Should().BeNull();
        _ = history.SuspendedDate.Should().BeNull();
    }

    [Fact]
    public void StartedHistoryDateShouldBeNow()
    {
        TaskProcessingHistory history = new TaskProcessingHistory(DateTimeOffset.UtcNow, null, null, null, null)
            .ProcessingStarted();
        _ = history.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(100));
        _ = history.ProcessingStartDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(10));
        _ = history.CompletedDate.Should().BeNull();
        _ = history.CanceledDate.Should().BeNull();
        _ = history.SuspendedDate.Should().BeNull();
    }
}