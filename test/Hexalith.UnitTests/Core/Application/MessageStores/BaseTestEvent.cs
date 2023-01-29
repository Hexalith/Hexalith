// <copyright file="BaseTestEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.MessageStores;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Serialization;

[DataContract]
[JsonPolymorphicBaseClass]
public class BaseTestEvent : BaseEvent, IEvent
{
    [JsonConstructor]
    public BaseTestEvent(string id, string message)
    {
        Id = id;
        Message = message;
    }

    public string Id { get; }

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