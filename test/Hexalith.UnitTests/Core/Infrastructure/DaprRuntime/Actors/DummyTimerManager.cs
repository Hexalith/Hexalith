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

    public override Task<IActorReminder> GetReminderAsync(ActorReminderToken reminder)
        => Task.FromResult((IActorReminder)Reminders[reminder.Name]);

    public override Task RegisterReminderAsync(ActorReminder reminder)
    {
        return !Reminders.TryAdd(reminder.Name, reminder)
            ? Task.FromException(new ArgumentException($"Reminder {reminder.Name} already exists."))
            : Task.CompletedTask;
    }

    public override Task RegisterTimerAsync(ActorTimer timer)
    {
        return !Timers.TryAdd(timer.Name, timer)
            ? Task.FromException(new ArgumentException($"Timer {timer.Name} already exists."))
            : Task.CompletedTask;
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
    {
        if (Timers.ContainsKey(timer.Name))
        {
            _ = Timers.Remove(timer.Name);
            return Task.CompletedTask;
        }

        return Task.FromException(new ArgumentException($"Timer {timer.Name} does not exist."));
    }
}