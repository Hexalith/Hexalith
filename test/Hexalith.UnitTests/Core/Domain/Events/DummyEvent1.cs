// <copyright file="DummyEvent1.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions.Helpers;

[DataContract]
public class DummyEvent1 : DummyBaseEvent
{
    [JsonConstructor]
    public DummyEvent1(string baseValue, int value1)
        : base(baseValue) => Value1 = value1;

    public DummyEvent1()
    {
    }

    public int Value1 { get; }

    protected override string DefaultAggregateId() => BaseValue + "-" + Value1.ToInvariantString();
}