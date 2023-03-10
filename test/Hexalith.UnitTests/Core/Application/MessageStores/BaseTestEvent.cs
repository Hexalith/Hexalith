// <copyright file="BaseTestEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.MessageStores;

using System.Runtime.Serialization;

using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Common;

[DataContract]
public class BaseTestEvent : BaseEvent, IEvent, IIdempotent
{
    public string Id { get; set; }

    public string IdempotencyId { get; set; }

    public string Message { get; set; }

    protected override string DefaultAggregateId()
    {
        return Id;
    }

    protected override string DefaultAggregateName()
    {
        return "Test";
    }

    protected override string DefaultTypeName()
    {
        return GetType().Name;
    }
}