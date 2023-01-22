// <copyright file="DummyCommand1.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Domain;

using Hexalith.Extensions.Helpers;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public class DummyCommand1 : DummyBaseCommand
{
    [JsonConstructor]
    public DummyCommand1(string baseValue, int value1)
        : base(baseValue)
    {
        Value1 = value1;
    }

    public DummyCommand1()
    {
    }

    public int Value1 { get; }

    protected override string DefaultAggregateId()
    {
        return BaseValue + "-" + Value1.ToInvariantString();
    }
}