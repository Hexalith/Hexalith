// <copyright file="TaskProcessorTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Tasks;

using System;
using System.Text.Json;

using FluentAssertions;

using Hexalith.Application.Abstractions.Tasks;

public class TaskProcessorTest
{
    [Fact]
    public void Binary_serialize_and_deserialize_task_should_return_same()
    {
        TaskProcessor processor = GetTestProcessor();
        _ = processor.Should().BeBinarySerializable();
    }

    [Fact]
    public void Complete_processing_task_should_have_complete_date()
    {
        TaskProcessor processor = new TaskProcessor(DateTimeOffset.UtcNow, ResiliencyPolicy.None)
            .Start()
            .Complete();
        _ = processor.History.CompletedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Complete_running_process_should_be_completed()
    {
        TaskProcessor processor = new TaskProcessor(DateTimeOffset.UtcNow, ResiliencyPolicy.None)
            .Start()
            .Complete();
        _ = processor.Status.Should().Be(TaskProcessorStatus.Completed);
    }

    [Fact]
    public void Data_contract_serialize_and_deserialize_task_should_return_same()
    {
        TaskProcessor processor = GetTestProcessor();
        _ = processor.Should().BeDataContractSerializable();
    }

    [Fact]
    public void Fail_running_process_with_resiliency_should_be_suspended()
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
            .Fail("fail");
        _ = processor.Status.Should().Be(TaskProcessorStatus.Suspended);
    }

    [Fact]
    public void Fail_running_process_without_resiliency_should_be_canceled()
    {
        TaskProcessor processor = new TaskProcessor(DateTimeOffset.UtcNow, ResiliencyPolicy.None)
            .Start()
            .Fail("fail");
        _ = processor.Status.Should().Be(TaskProcessorStatus.Canceled);
    }

    [Fact]
    public void Json_serialize_and_deserialize_task_should_return_same()
    {
        TaskProcessor processor = GetTestProcessor();
        string json = JsonSerializer.Serialize(processor);
        TaskProcessor fromJson = JsonSerializer.Deserialize<TaskProcessor>(json);
        _ = fromJson.Should().NotBeNull();
        _ = fromJson.Should().BeEquivalentTo(processor);
    }

    [Fact]
    public void New_process_should_be_in_new_state()
    {
        TaskProcessor processor = new(DateTimeOffset.UtcNow, ResiliencyPolicy.None);
        _ = processor.Status.Should().Be(TaskProcessorStatus.New);
    }

    [Fact]
    public void New_process_should_have_created_date()
    {
        TaskProcessor processor = new(DateTimeOffset.UtcNow, ResiliencyPolicy.None);
        _ = processor.History.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Start_new_process_should_be_active()
    {
        TaskProcessor processor = new TaskProcessor(DateTimeOffset.UtcNow, ResiliencyPolicy.None)
            .Start();
        _ = processor.Status.Should().Be(TaskProcessorStatus.Active);
    }

    [Fact]
    public void Start_new_process_should_have_current_processing_start_date()
    {
        TaskProcessor processor = new TaskProcessor(DateTimeOffset.UtcNow, ResiliencyPolicy.None)
            .Start();
        _ = processor.History.ProcessingStartDate.Should().NotBeNull();
        _ = processor.History.ProcessingStartDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Xml_serialize_and_deserialize_task_should_return_same()
    {
        TaskProcessor processor = GetTestProcessor();
        _ = processor.Should().BeXmlSerializable();
    }

    private static TaskProcessor GetTestProcessor()
    {
        return new TaskProcessor(
                TaskProcessorStatus.New,
                new TaskProcessingHistory(DateTimeOffset.UtcNow),
                ResiliencyPolicy.CreateEternalRetry(TimeSpan.FromMinutes(1)),
                failure: null)
            .Start()
            .Fail("my test fail message")
            .Complete();
    }
}