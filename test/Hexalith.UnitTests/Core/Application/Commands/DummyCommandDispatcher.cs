// ***********************************************************************
// Assembly         : Hexalith.UnitTests
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 10-16-2023
// ***********************************************************************
// <copyright file="DummyCommandDispatcher.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The Commands namespace.
/// </summary>
namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Commands;
using Hexalith.Domain.Aggregates;
using Hexalith.Domain.Messages;

/// <summary>
/// Class DummyCommandDispatcher.
/// Implements the <see cref="ICommandDispatcher" />.
/// </summary>
/// <seealso cref="ICommandDispatcher" />
internal class DummyCommandDispatcher : ICommandDispatcher
{
    /// <summary>
    /// Does the asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;BaseMessage&gt;&gt;.</returns>
    public async Task<IEnumerable<BaseMessage>> DoAsync(ICommand command, IAggregate aggregate, CancellationToken cancellationToken)
        => await Task.FromResult<IEnumerable<BaseMessage>>([new CommandDispatchDoEvent()]);

    /// <summary>
    /// Uns the do asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="aggregate">The aggregate.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;BaseMessage&gt;&gt;.</returns>
    public async Task<IEnumerable<BaseMessage>> UnDoAsync(ICommand command, IAggregate aggregate, CancellationToken cancellationToken)
        => await Task.FromResult<IEnumerable<BaseMessage>>([new CommandDispatchUndoEvent()]);
}