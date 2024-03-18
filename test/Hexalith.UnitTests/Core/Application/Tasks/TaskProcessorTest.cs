// ***********************************************************************
// Assembly         : Hexalith.UnitTests
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-15-2023
// ***********************************************************************
// <copyright file="TaskProcessorTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.UnitTests.Core.Application.Tasks;

using System;
using System.Text.Json;

using FluentAssertions;
using FluentAssertions.Equivalency;

using Hexalith.Application.Tasks;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class TaskProcessorTest.
/// </summary>
public class TaskProcessorTest
{
    /// <summary>
    /// Defines the test method CompleteProcessingTaskShouldHaveCompleteDate.
    /// </summary>
    [Fact]
    public void CompleteProcessingTaskShouldHaveCompleteDate()
    {
        TaskProcessor processor = new TaskProcessor(DateTimeOffset.UtcNow, ResiliencyPolicy.None)
            .Start()
            .Complete();
        _ = processor.History.CompletedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Defines the test method CompleteRunningProcessShouldBeCompleted.
    /// </summary>
    [Fact]
    public void CompleteRunningProcessShouldBeCompleted()
    {
        TaskProcessor processor = new TaskProcessor(DateTimeOffset.UtcNow, ResiliencyPolicy.None)
            .Start()
            .Complete();
        _ = processor.Status.Should().Be(TaskProcessorStatus.Completed);
    }

    /// <summary>
    /// Defines the test method DataContractSerializeAndDeserializeTaskShouldReturnSame.
    /// </summary>
    [Fact]
    public void DataContractSerializeAndDeserializeTaskShouldReturnSame()
    {
        TaskProcessor processor = GetTestProcessor();
        _ = processor.Should().BeDataContractSerializable<TaskProcessor>(ExcludeProperties);
    }

    /// <summary>
    /// Defines the test method FailRunningProcessWithoutResiliencyShouldBeCanceled.
    /// </summary>
    [Fact]
    public void FailRunningProcessWithoutResiliencyShouldBeCanceled()
    {
        TaskProcessor processor = new TaskProcessor(DateTimeOffset.UtcNow, ResiliencyPolicy.None)
            .Start()
            .Fail("fail", "error");
        _ = processor.Status.Should().Be(TaskProcessorStatus.Canceled);
    }

    /// <summary>
    /// Defines the test method FailRunningProcessWithResiliencyShouldBeSuspended.
    /// </summary>
    [Fact]
    public void FailRunningProcessWithResiliencyShouldBeSuspended()
    {
        TaskProcessor processor = new TaskProcessor(
            DateTimeOffset.UtcNow,
            new ResiliencyPolicy(
                10,
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(10),
                true))
            .Start()
            .Fail("fail", "error");
        _ = processor.Status.Should().Be(TaskProcessorStatus.Suspended);
    }

    /// <summary>
    /// Defines the test method JsonSerializeAndDeserializeTaskShouldReturnSame.
    /// </summary>
    [Fact]
    public void JsonSerializeAndDeserializeTaskShouldReturnSame()
    {
        TaskProcessor processor = GetTestProcessor();
        string json = JsonSerializer.Serialize(processor);
        TaskProcessor fromJson = JsonSerializer.Deserialize<TaskProcessor>(json);
        _ = fromJson.Should().NotBeNull();
        _ = fromJson.Should().BeEquivalentTo(processor, ExcludeProperties);
    }

    /// <summary>
    /// Defines the test method NewProcessShouldBeInNewState.
    /// </summary>
    [Fact]
    public void NewProcessShouldBeInNewState()
    {
        TaskProcessor processor = new(DateTimeOffset.UtcNow, ResiliencyPolicy.None);
        _ = processor.Status.Should().Be(TaskProcessorStatus.New);
    }

    /// <summary>
    /// Defines the test method NewProcessShouldHaveCreatedDate.
    /// </summary>
    [Fact]
    public void NewProcessShouldHaveCreatedDate()
    {
        TaskProcessor processor = new(DateTimeOffset.UtcNow, ResiliencyPolicy.None);
        _ = processor.History.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    /// <summary>
    /// Defines the test method StartNewProcessShouldBeActive.
    /// </summary>
    [Fact]
    public void StartNewProcessShouldBeActive()
    {
        TaskProcessor processor = new TaskProcessor(DateTimeOffset.UtcNow, ResiliencyPolicy.None)
            .Start();
        _ = processor.Status.Should().Be(TaskProcessorStatus.Active);
    }

    /// <summary>
    /// Defines the test method StartNewProcessShouldHaveCurrentProcessingStartDate.
    /// </summary>
    [Fact]
    public void StartNewProcessShouldHaveCurrentProcessingStartDate()
    {
        TaskProcessor processor = new TaskProcessor(DateTimeOffset.UtcNow, ResiliencyPolicy.None)
            .Start();
        _ = processor.History.ProcessingStartDate.Should().NotBeNull();
        _ = processor.History.ProcessingStartDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Theory]
    [InlineData(5, 710000)]
    [InlineData(10, 8570000)]
    [InlineData(20, 86390000)]
    [InlineData(25, 86390000)]
    [InlineData(70, 86390000)]
    public void SuspendedWaitTimeShouldBePositive(int retries, int milliseconds)
    {
        DateTimeOffset startDate = DateTimeOffset.UtcNow.AddSeconds(-10);
        TaskProcessor processor = new TaskProcessor(startDate, ResiliencyPolicy.CreateDefaultExponentialRetry())
            .Start();
        for (int i = 0; i < retries; i++)
        {
            processor = processor.Fail($"Fail {retries}", null);
        }

        DateTimeOffset? retryDate = processor.RetryDate;
        DateTimeOffset now = DateTimeOffset.Now;
        TimeSpan waitTime = now.WaitTime(retryDate);
        _ = waitTime.TotalMilliseconds.Should().BeApproximately(milliseconds, 10d);
    }

    /*
        /// <summary>
        /// Defines the test method XmlSerializeAndDeserializeTaskShouldReturnSame.
        /// </summary>
        [Fact]
        public void XmlSerializeAndDeserializeTaskShouldReturnSame()
        {
            TaskProcessor processor = GetTestProcessor();
            _ = processor.Should().BeXmlSerializable();
        }
    */

    /// <summary>
    /// Gets the test processor.
    /// </summary>
    /// <returns>TaskProcessor.</returns>
    private static TaskProcessor GetTestProcessor()
    {
        return new TaskProcessor(
                TaskProcessorStatus.New,
                new TaskProcessingHistory(DateTimeOffset.UtcNow),
                ResiliencyPolicy.CreateEternalRetry(TimeSpan.FromMinutes(1)),
                failure: null)
            .Start()
            .Fail("my test fail message", "error")
            .Complete();
    }

    private EquivalencyAssertionOptions<TaskProcessor> ExcludeProperties(EquivalencyAssertionOptions<TaskProcessor> options) => options;
}