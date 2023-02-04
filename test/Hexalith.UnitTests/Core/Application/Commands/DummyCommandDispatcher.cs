// <copyright file="DummyCommandDispatcher.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Helpers;

internal class DummyCommandDispatcher : ICommandDispatcher
{
    public Task<IEnumerable<BaseEvent>> DoAsync(ICommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult<IEnumerable<BaseEvent>>(new CommandDispatchDoEvent().IntoArray());
    }

    public Task<IEnumerable<BaseEvent>> UnDoAsync(ICommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult<IEnumerable<BaseEvent>>(new CommandDispatchUndoEvent().IntoArray());
    }
}