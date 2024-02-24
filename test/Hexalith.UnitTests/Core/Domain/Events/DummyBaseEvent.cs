// <copyright file="DummyBaseEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Metadatas;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Helpers;

[DataContract]
public abstract class DummyBaseEvent : BaseEvent
{
    protected DummyBaseEvent() => BaseValue = string.Empty;

    [JsonConstructor]
    protected DummyBaseEvent(string baseValue) => BaseValue = baseValue;

    public string BaseValue { get; }

    public Metadata CreateMetadata()
    {
        return new Metadata(
                UniqueIdHelper.GenerateUniqueStringId(),
                this,
                DateTimeOffset.UtcNow,
                new ContextMetadata(
                    UniqueIdHelper.GenerateUniqueStringId(),
                    "Test user",
                    DateTimeOffset.UtcNow.AddMinutes(-1),
                    1,
                    "Test session"),
                ["TestScope"]);
    }

    protected override string DefaultAggregateName() => "Test";
}