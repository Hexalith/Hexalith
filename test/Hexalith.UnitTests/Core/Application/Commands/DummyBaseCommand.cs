// <copyright file="DummyBaseCommand.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Commands;
using Hexalith.Application.Metadatas;
using Hexalith.Extensions.Helpers;

[DataContract]
public class DummyBaseCommand : BaseCommand
{
    public DummyBaseCommand() => BaseValue = string.Empty;

    [JsonConstructor]
    public DummyBaseCommand(string baseValue) => BaseValue = baseValue;

    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string BaseValue { get; private set; }

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