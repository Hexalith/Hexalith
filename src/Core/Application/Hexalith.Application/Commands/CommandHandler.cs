// <copyright file="CommandHandler.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using Ardalis.GuardClauses;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Common;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Class CommandHandler.
/// Implements the <see cref="ICommandHandler" />.
/// </summary>
/// <seealso cref="ICommandHandler" />
public abstract class CommandHandler : ICommandHandler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandler"/> class.
    /// </summary>
    /// <param name="dateTimeService">The date time service.</param>
    protected CommandHandler(IDateTimeService dateTimeService)
    {
        DateTimeService = Guard.Against.Null(dateTimeService);
    }

    /// <summary>
    /// Gets the date time service.
    /// </summary>
    /// <value>The date time service.</value>
    protected IDateTimeService DateTimeService { get; }

    /// <inheritdoc/>
    public async Task<IEnumerable<(BaseEvent Event, Metadata Metadata)>> DoAsync(BaseCommand command, Metadata metadata, CancellationToken cancellationToken)
    {
        IEnumerable<BaseEvent> events = await DoAsync(command, cancellationToken);
        List<(BaseEvent, Metadata)> result = new(1);
        foreach (IEvent e in events)
        {
            Metadata m = Metadata.CreateNew(e, metadata, DateTimeService.UtcNow);
            result.Add(((BaseEvent, Metadata))(e, m));
        }

        return result;
    }

    /// <summary>
    /// Does the asynchronous.
    /// </summary>
    /// <param name="command">The command.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>Task&lt;IEnumerable&lt;BaseEvent&gt;&gt;.</returns>
    protected abstract Task<IEnumerable<BaseEvent>> DoAsync(BaseCommand command, CancellationToken cancellationToken);
}
