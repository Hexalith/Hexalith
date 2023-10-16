// <copyright file="DummyCommand2.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions.Helpers;

[DataContract]
public class DummyCommand2 : DummyBaseCommand
{
    [JsonConstructor]
    public DummyCommand2(string baseValue, int value2)
        : base(baseValue) => Value2 = value2;

    public DummyCommand2()
    {
    }

    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public int Value2 { get; }

    public static DummyCommand2 Create() => new("Test123", 35453);

    protected override string DefaultAggregateId() => BaseValue + "-" + Value2.ToInvariantString();
}