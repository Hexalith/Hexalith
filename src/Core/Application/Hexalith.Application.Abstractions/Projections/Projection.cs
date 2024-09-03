// ***********************************************************************
// Assembly         : Hexalith.Domain.Abstractions
// Author           : Jérôme Piquot
// Created          : 05-01-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-01-2023
// ***********************************************************************
// <copyright file="Projection.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Projections;

using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Notifications;
using Hexalith.Application.Requests;
using Hexalith.Domain.Events;
using Hexalith.Domain.Notifications;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Helpers;

/// <summary>
/// Class Projection.
/// Implements the <see cref="Abstractions.Projections.IProjection" />.
/// </summary>
/// <seealso cref="Abstractions.Projections.IProjection" />
[DataContract]
public abstract class Projection : IProjection
{
    private readonly IDateTimeService _dateTimeService;

    /// <summary>
    /// The notification bus.
    /// </summary>
    private readonly INotificationBus _notificationBus;

    /// <summary>
    /// Initializes a new instance of the <see cref="Projection"/> class.
    /// </summary>
    /// <param name="notificationBus">The notification bus.</param>
    /// <param name="dateTimeService">The date time service.</param>
    /// <exception cref="ArgumentNullException">null.</exception>
    protected Projection(INotificationBus notificationBus, IDateTimeService dateTimeService)
    {
        ArgumentNullException.ThrowIfNull(notificationBus);
        ArgumentNullException.ThrowIfNull(dateTimeService);
        _notificationBus = notificationBus;
        _dateTimeService = dateTimeService;
    }

    /// <inheritdoc/>
    public abstract Task<BaseNotification> ExecuteAsync(BaseRequest request, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public abstract Task HandleAsync(BaseEvent domainEvent, CancellationToken cancellationToken);

    /// <inheritdoc/>
    public async Task SubmitAsync(BaseRequest request, string correlationId, string userId, string sessionId, CancellationToken cancellationToken)
    {
        BaseNotification result = await ExecuteAsync(request, cancellationToken)
            .ConfigureAwait(false);
        await _notificationBus
            .PublishAsync(
                result,
                new Metadata(
                    UniqueIdHelper.GenerateUniqueStringId(),
                    result,
                    _dateTimeService.Now,
                    new ContextMetadata(correlationId, userId, _dateTimeService.Now, null, sessionId),
                    null),
                cancellationToken)
            .ConfigureAwait(false);
    }
}