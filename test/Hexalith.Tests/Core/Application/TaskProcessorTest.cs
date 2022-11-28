// <copyright file="TaskProcessorTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Tests.Core.Application;

using FluentAssertions;

using Hexalith.Application.Abstractions.Tasks;

using System;

public class TaskProcessorTest
{
	[Fact]
	public void Complete_processing_task_should_have_complete_date()
	{
		ITaskProcessor processor = new TaskProcessor()
			.Start()
			.Complete();
		_ = processor.CompletedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
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
	public void Expired_suspend_process_should_be_expired()
	{
		ITaskProcessor processor = new TaskProcessor()
			.SuspendUntil(DateTimeOffset.UtcNow);
		Thread.Sleep(100);
		_ = processor.IsSuspensionExpired
			.Should()
			.BeTrue();
	}

	[Fact]
	public void New_process_should_be_in_new_state()
	{
		ITaskProcessor processor = new TaskProcessor();
		_ = processor.Status.Should().Be(TaskProcessorStatus.New);
	}

	[Fact]
	public void New_process_should_have_created_date()
	{
		ITaskProcessor processor = new TaskProcessor();
		_ = processor.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
	}

	[Fact]
	public void Start_new_process_should_be_processing()
	{
		ITaskProcessor processor = new TaskProcessor()
			.Start();
		_ = processor.Status.Should().Be(TaskProcessorStatus.Processing);
	}

	[Fact]
	public void Start_new_process_should_have_current_processing_start_date()
	{
		ITaskProcessor processor = new TaskProcessor()
			.Start();
		_ = processor.ProcessingStartDate.Should().NotBeNull();
		_ = processor.ProcessingStartDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
	}

	[Fact]
	public void Suspend_process_should_be_suspended()
	{
		ITaskProcessor processor = new TaskProcessor()
			.SuspendUntil(DateTimeOffset.UtcNow.AddDays(1));
		_ = processor.Status.Should().Be(TaskProcessorStatus.Suspended);
	}

	[Fact]
	public void Suspend_process_should_have_suspension_date()
	{
		DateTimeOffset until = DateTimeOffset.UtcNow.AddDays(1);
		ITaskProcessor processor = new TaskProcessor()
			.SuspendUntil(until);
		_ = processor.SuspendedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
		_ = processor.SuspendedUntilDate.Should().Be(until);
	}
}