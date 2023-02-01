// <copyright file="BaseTestEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.MessageStores;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Common;
using Hexalith.Extensions.Serialization;

[DataContract]
[JsonPolymorphicBaseClass]
public class BaseTestEvent : BaseEvent, IEvent, IIdempotent
{
    [JsonConstructor]
    public BaseTestEvent(string idempotencyId, string id, string message)
    {
        Id = id;
        Message = message;
        IdempotencyId = idempotencyId;
    }

    public string Id { get; }

    public string IdempotencyId { get; }

    public string Message { get; }

    protected override string DefaultAggregateId()
    {
        return Id;
    }

    protected override string DefaultAggregateName()
    {
        return "Test";
    }

    protected override string DefaultMessageName()
    {
        return GetType().Name;
    }
}