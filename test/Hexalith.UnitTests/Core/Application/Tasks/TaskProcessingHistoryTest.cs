// <copyright file="TaskProcessingHistoryTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Tasks;

using System;

using Shouldly;

using Hexalith.Application.Tasks;

public class TaskProcessingHistoryTest
{
    [Fact]
    public void CancelledHistoryDateShouldBeNow()
    {
        TaskProcessingHistory history = new TaskProcessingHistory(DateTimeOffset.UtcNow, null, null, null, null)
            .Canceled();
        (DateTimeOffset.UtcNow - history.CreatedDate).TotalSeconds.ShouldBeLessThan(1);
        (DateTimeOffset.UtcNow - history.CanceledDate.Value).TotalSeconds.ShouldBeLessThan(1);
        history.CompletedDate.ShouldBeNull();
        history.ProcessingStartDate.ShouldBeNull();
        history.SuspendedDate.ShouldBeNull();
    }

    [Fact]
    public void CompletedHistoryDateShouldBeNow()
    {
        TaskProcessingHistory history = new TaskProcessingHistory(DateTimeOffset.UtcNow, null, null, null, null)
            .Completed();
        (DateTimeOffset.UtcNow - history.CreatedDate).TotalSeconds.ShouldBeLessThan(1);
        (DateTimeOffset.UtcNow - history.CompletedDate.Value).TotalSeconds.ShouldBeLessThan(1);
        history.ProcessingStartDate.ShouldBeNull();
        history.CanceledDate.ShouldBeNull();
        history.SuspendedDate.ShouldBeNull();
    }

    [Fact]
    public void CreatedHistoryDateShouldBeNow()
    {
        TaskProcessingHistory history = new(DateTimeOffset.UtcNow, null, null, null, null);
        (DateTimeOffset.UtcNow - history.CreatedDate).TotalSeconds.ShouldBeLessThan(1);
        history.CanceledDate.ShouldBeNull();
        history.CompletedDate.ShouldBeNull();
        history.ProcessingStartDate.ShouldBeNull();
        history.SuspendedDate.ShouldBeNull();
    }

    [Fact]
    public void StartedHistoryDateShouldBeNow()
    {
        TaskProcessingHistory history = new TaskProcessingHistory(DateTimeOffset.UtcNow, null, null, null, null)
            .ProcessingStarted();
        (DateTimeOffset.UtcNow - history.CreatedDate).TotalMilliseconds.ShouldBeLessThan(100);
        (DateTimeOffset.UtcNow - history.ProcessingStartDate.Value).TotalMilliseconds.ShouldBeLessThan(10);
        history.CompletedDate.ShouldBeNull();
        history.CanceledDate.ShouldBeNull();
        history.SuspendedDate.ShouldBeNull();
    }
}