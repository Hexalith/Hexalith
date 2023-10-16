// <copyright file="DummyRequest2.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Requests;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions.Helpers;

[DataContract]
public class DummyRequest2 : DummyBaseRequest
{
    [JsonConstructor]
    public DummyRequest2(string baseValue, int value2)
        : base(baseValue) => Value2 = value2;

    public DummyRequest2()
    {
    }

    public int Value2 { get; }

    protected override string DefaultAggregateId() => BaseValue + "-" + Value2.ToInvariantString();
}