// <copyright file="TaskProcessingHistoryTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Tasks;

using System;

using FluentAssertions;

using Hexalith.Application.Abstractions.Tasks;

public class TaskProcessingHistoryTest
{
    [Fact]
    public void Cancelled_history_date_should_be_now()
    {
        TaskProcessingHistory history = new TaskProcessingHistory()
            .Canceled();
        _ = history.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(100));
        _ = history.CanceledDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(100));
        _ = history.CompletedDate.Should().BeNull();
        _ = history.ProcessingStartDate.Should().BeNull();
        _ = history.SuspendedDate.Should().BeNull();
    }

    [Fact]
    public void Completed_history_date_should_be_now()
    {
        TaskProcessingHistory history = new TaskProcessingHistory()
            .Completed();
        _ = history.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(100));
        _ = history.CompletedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(100));
        _ = history.ProcessingStartDate.Should().BeNull();
        _ = history.CanceledDate.Should().BeNull();
        _ = history.SuspendedDate.Should().BeNull();
    }

    [Fact]
    public void Created_history_date_should_be_now()
    {
        TaskProcessingHistory history = new();
        _ = history.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(100));
        _ = history.CanceledDate.Should().BeNull();
        _ = history.CompletedDate.Should().BeNull();
        _ = history.ProcessingStartDate.Should().BeNull();
        _ = history.SuspendedDate.Should().BeNull();
    }

    [Fact]
    public void Started_history_date_should_be_now()
    {
        TaskProcessingHistory history = new TaskProcessingHistory()
            .ProcessingStarted();
        _ = history.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(100));
        _ = history.ProcessingStartDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(10));
        _ = history.CompletedDate.Should().BeNull();
        _ = history.CanceledDate.Should().BeNull();
        _ = history.SuspendedDate.Should().BeNull();
    }

    [Fact]
    public void Suspended_history_date_should_be_now()
    {
        TaskProcessingHistory history = new TaskProcessingHistory()
            .Suspended();
        _ = history.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(100));
        _ = history.SuspendedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(100));
        _ = history.CompletedDate.Should().BeNull();
        _ = history.CanceledDate.Should().BeNull();
        _ = history.ProcessingStartDate.Should().BeNull();
    }
}