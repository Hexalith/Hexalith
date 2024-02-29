// <copyright file="DummyTimerManager.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

using System;
using System.Threading.Tasks;

using Dapr.Actors.Runtime;

internal class DummyTimerManager : ActorTimerManager
{
    public Dictionary<string, ActorReminder> Reminders { get; } = [];

    public Dictionary<string, ActorTimer> Timers { get; } = [];

    public override async Task<IActorReminder> GetReminderAsync(ActorReminderToken reminder)
        => await Task.FromResult((IActorReminder)Reminders[reminder.Name]);

    public override async Task RegisterReminderAsync(ActorReminder reminder)
    {
        if (!Reminders.TryAdd(reminder.Name, reminder))
        {
            throw new ArgumentException($"Reminder {reminder.Name} already exists.");
        }

        await Task.CompletedTask;
    }

    public override async Task RegisterTimerAsync(ActorTimer timer)
    {
        Timers[timer.Name] = timer;
        await Task.CompletedTask;
    }

    public override Task UnregisterReminderAsync(ActorReminderToken reminder)
    {
        if (Reminders.ContainsKey(reminder.Name))
        {
            _ = Reminders.Remove(reminder.Name);
            return Task.CompletedTask;
        }

        return Task.FromException(new ArgumentException($"Reminder {reminder.Name} does not exist."));
    }

    public override Task UnregisterTimerAsync(ActorTimerToken timer)
        => Task.FromException(new ArgumentException($"Timer should not be unregistered : Name={timer.Name}."));
}