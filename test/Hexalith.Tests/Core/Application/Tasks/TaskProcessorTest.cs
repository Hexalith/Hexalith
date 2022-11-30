// <copyright file="TaskProcessorTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Tests.Core.Application.Tasks;

using FluentAssertions;

using Hexalith.Application.Abstractions.Tasks;

using System;
using System.Text.Json;

public class TaskProcessorTest
{
	[Fact]
	public void Complete_processing_task_should_have_complete_date()
	{
		ITaskProcessor processor = new TaskProcessor()
			.Start()
			.Complete();
		_ = processor.History.CompletedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
	}

	[Fact]
	public void Complete_running_process_should_be_completed()
	{
		ITaskProcessor processor = new TaskProcessor()
			.Start()
			.Complete();
		_ = processor.Status.Should().Be(TaskProcessorStatus.Completed);
	}

	[Fact]
	public void Failed_process_should_be_suspended()
	{
		ITaskProcessor processor = new TaskProcessor()
			.Fail("test");
		_ = processor.Status.Should().Be(TaskProcessorStatus.Suspended);
	}

	[Fact]
	public void New_process_should_be_in_new_state()
	{
		TaskProcessor processor = new();
		_ = processor.Status.Should().Be(TaskProcessorStatus.New);
	}

	[Fact]
	public void New_process_should_have_created_date()
	{
		TaskProcessor processor = new();
		_ = processor.History.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
	}

	[Fact]
	public void Serialize_and_deserialize_task_should_be_same()
	{
		ITaskProcessor processor = new TaskProcessor(
				TaskProcessorStatus.New,
				new TaskProcessingHistory(),
				RetryPolicy.CreateEternalRetry(TimeSpan.FromMinutes(1)),
				failure: null)
			.Start()
			.Fail("my test fail message")
			.Complete();
		string json = JsonSerializer.Serialize((TaskProcessor)processor);
		TaskProcessor? fromJson = JsonSerializer.Deserialize<TaskProcessor>(json);
		_ = fromJson.Should().NotBeNull();
		_ = fromJson!.Status.Should().Be(processor.Status);
		_ = fromJson!.History.CreatedDate.Should().BeCloseTo(processor.History.CreatedDate, TimeSpan.FromSeconds(1));
		_ = fromJson!.History.ProcessingStartDate.Should().Be(processor.History.ProcessingStartDate);
		_ = fromJson!.History.CompletedDate.Should().Be(processor.History.CompletedDate);
		_ = fromJson!.History.CanceledDate.Should().Be(processor.History.CanceledDate);
		_ = fromJson!.Failure.Should().NotBeNull();
		_ = fromJson!.Failure!.Count.Should().Be(processor.Failure!.Count);
		_ = fromJson!.Failure!.Date.Should().Be(processor.Failure!.Date);
		_ = fromJson!.Failure!.Message.Should().Be(processor.Failure!.Message);
	}

	[Fact]
	public void Start_new_process_should_be_active()
	{
		ITaskProcessor processor = new TaskProcessor()
			.Start();
		_ = processor.Status.Should().Be(TaskProcessorStatus.Active);
	}

	[Fact]
	public void Start_new_process_should_have_current_processing_start_date()
	{
		ITaskProcessor processor = new TaskProcessor()
			.Start();
		_ = processor.History.ProcessingStartDate.Should().NotBeNull();
		_ = processor.History.ProcessingStartDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
	}
}