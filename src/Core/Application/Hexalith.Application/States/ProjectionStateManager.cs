// <copyright file="ProjectionStateManager.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.States;

using System;
using System.Threading;

using Hexalith.Application.MessageMetadatas;
using Hexalith.Application.Projections;
using Hexalith.Application.StreamStores;
using Hexalith.Application.Tasks;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Projection State Manager.
/// </summary>
public class ProjectionStateManager : IProjectionStateManager
{
    /// <summary>
    /// The reminder name.
    /// </summary>
    private const string _reminderName = "Continue";

    /// <summary>
    /// The date time service.
    /// </summary>
    private readonly TimeProvider _dateTimeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectionStateManager" /> class.
    /// </summary>
    /// <param name="dateTimeService">The date time service.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public ProjectionStateManager(
        TimeProvider dateTimeService)
    {
        ArgumentNullException.ThrowIfNull(dateTimeService);
        _dateTimeService = dateTimeService;
    }

    /// <summary>
    /// Gets to do state name.
    /// </summary>
    /// <value>The name of to do state.</value>
    public virtual string EventsStreamName => "Events";

    /// <summary>
    /// Gets the name of the resiliency policy state.
    /// </summary>
    /// <value>The name of the resiliency policy state.</value>
    public virtual string ResiliencyPolicyStateName => nameof(ResiliencyPolicy);

    /// <summary>
    /// Gets the separator.
    /// </summary>
    /// <value>The separator.</value>
    public virtual string Separator => "-";

    /// <summary>
    /// Gets the name of the state.
    /// </summary>
    /// <value>The name of the state.</value>
    public virtual string StateName => "State";

    /// <summary>
    /// Add command as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="events">The events.</param>
    /// <param name="metadatas">The metadatas.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    [Obsolete]
    public async Task AddEventAsync(
        IStateStoreProvider stateProvider,
        object[] events,
        Metadata[] metadatas,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        ArgumentNullException.ThrowIfNull(metadatas);
        ArgumentNullException.ThrowIfNull(events);
        ArgumentNullException.ThrowIfNull(registerReminder);
        await SetReminderAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(15), registerReminder).ConfigureAwait(false);
        ProjectionState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        MessageStore<Hexalith.Application.MessageMetadatas.MessageState> eventStore = new(stateProvider, EventsStreamName);
        List<Hexalith.Application.MessageMetadatas.MessageState> states = [];
        for (int i = 0; i < events.Length; i++)
        {
            states.Add(Hexalith.Application.MessageMetadatas.MessageState.Create(events[i], metadatas[i]));
        }

        long version = await eventStore.AddAsync(
            states,
            state.EventStreamVersion,
            cancellationToken).ConfigureAwait(false);
        await PersistStateAsync(
            stateProvider,
            new ProjectionState(
                version,
                state.EventStreamVersion),
            cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Continue as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="projection">The projection.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <param name="unregisterReminder">The remove reminder.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;IProjection&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public async Task ContinueAsync(
        IStateStoreProvider stateProvider,
        ResiliencyPolicy resiliencyPolicy,
        IProjection projection,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder,
        Func<string, Task> unregisterReminder,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        ArgumentNullException.ThrowIfNull(registerReminder);
        ArgumentNullException.ThrowIfNull(unregisterReminder);
        ArgumentNullException.ThrowIfNull(projection);
        ArgumentNullException.ThrowIfNull(resiliencyPolicy);

        DateTimeOffset? retry = await ExecuteEventsAsync(stateProvider, projection, resiliencyPolicy, cancellationToken).ConfigureAwait(false);
        ProjectionState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);

        if (state.LastEventDone < state.EventStreamVersion)
        {
            if (retry != null)
            {
                TimeSpan waitTime = retry.Value - _dateTimeService.GetLocalNow();
                if (waitTime < TimeSpan.Zero)
                {
                    waitTime = TimeSpan.Zero;
                }

                await SetReminderAsync(waitTime.Add(TimeSpan.FromSeconds(1)), TimeSpan.FromMinutes(1), registerReminder).ConfigureAwait(false);
            }
            else
            {
                await SetReminderAsync(resiliencyPolicy.MaximumExponentialPeriod, resiliencyPolicy.MaximumExponentialPeriod, registerReminder).ConfigureAwait(false);
            }
        }

        await RemoveReminderAsync(unregisterReminder).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<long> GetEventCountAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stateProvider);
        ProjectionState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        return state.EventStreamVersion;
    }

    /// <inheritdoc/>
    [Obsolete]
    public async Task<IEnumerable<object>> GetEventsAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        MessageStore<MessageState> commandStore = new(stateProvider, EventsStreamName);
        ProjectionState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        List<BaseEvent> events = [];
        while (state.LastEventDone < state.EventStreamVersion)
        {
            long nextEvent = state.LastEventDone + 1;
            MessageState eventState = await commandStore.GetAsync(nextEvent, cancellationToken).ConfigureAwait(false);
            events.Add(eventState.Message ?? throw new InvalidOperationException($"Event state {nextEvent} message is null."));
        }

        return events;
    }

    /// <summary>
    /// Execute events as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="projection">The aggregate.</param>
    /// <param name="resiliencyPolicy">The resiliency policy.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;System.ValueTuple&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.InvalidOperationException">Event {nextEvent} content is null.</exception>
    /// <exception cref="System.InvalidOperationException">Event {nextEvent} metadata is null.</exception>
    protected async Task<DateTimeOffset?> ExecuteEventsAsync(
        IStateStoreProvider stateProvider,
        IProjection projection,
        ResiliencyPolicy resiliencyPolicy,
        CancellationToken cancellationToken)
    {
        MessageStore<MessageState> commandStore = new(stateProvider, EventsStreamName);
        _ = new MessageStore<MessageState>(stateProvider, EventsStreamName);
        ProjectionState state = await GetStateAsync(stateProvider, cancellationToken).ConfigureAwait(false);
        while (state.LastEventDone < state.EventStreamVersion)
        {
            long nextEvent = state.LastEventDone + 1;
            MessageState command = await commandStore.GetAsync(nextEvent, cancellationToken).ConfigureAwait(false);
            _ = command.Message ?? throw new InvalidOperationException($"Event {nextEvent} content is null.");
            _ = command.Metadata ?? throw new InvalidOperationException($"Event {nextEvent} metadata is null.");
            ResilientProjectionEventProcessor processor = new(
                resiliencyPolicy,
                projection,
                stateProvider);

            DateTimeOffset? retry = await processor.ProcessAsync(nextEvent.ToInvariantString(), command.Message, cancellationToken).ConfigureAwait(false);

            state = new ProjectionState(
                    state.EventStreamVersion,
                    retry == null ? nextEvent : state.LastEventDone);
            await PersistStateAsync(
                stateProvider,
                state,
                cancellationToken).ConfigureAwait(false);
            if (retry != null)
            {
                return retry;
            }
        }

        return null;
    }

    /// <summary>
    /// Remove reminder as an asynchronous operation.
    /// </summary>
    /// <param name="removeReminder">The remove reminder.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private static async Task RemoveReminderAsync(Func<string, Task> removeReminder) => await removeReminder(_reminderName).ConfigureAwait(false);

    /// <summary>
    /// Set reminder as an asynchronous operation.
    /// </summary>
    /// <param name="next">The next.</param>
    /// <param name="retry">The retry.</param>
    /// <param name="registerReminder">The register reminder.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private static async Task SetReminderAsync(
        TimeSpan next,
        TimeSpan retry,
        Func<string, byte[], TimeSpan, TimeSpan, Task> registerReminder)
        => await registerReminder(_reminderName, [], next, retry).ConfigureAwait(false);

    /// <summary>
    /// Get state as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;ProjectionState&gt; representing the asynchronous operation.</returns>
    private async Task<ProjectionState> GetStateAsync(IStateStoreProvider stateProvider, CancellationToken cancellationToken)
    {
        ConditionalValue<ProjectionState> result = await stateProvider.TryGetStateAsync<ProjectionState>(StateName, cancellationToken).ConfigureAwait(false);
        return result.HasValue ? result.Value : new ProjectionState(0, 0);
    }

    /// <summary>
    /// Set state as an asynchronous operation.
    /// </summary>
    /// <param name="stateProvider">The state provider.</param>
    /// <param name="state">The state.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task PersistStateAsync(IStateStoreProvider stateProvider, ProjectionState state, CancellationToken cancellationToken)
    {
        await stateProvider.SetStateAsync(StateName, state, CancellationToken.None).ConfigureAwait(false);
        await stateProvider.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}