// <copyright file="DummyNotification1.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Notifications;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions.Helpers;

[DataContract]
[method: JsonConstructor]
public class DummyNotification1(string baseValue, int value1) : DummyBaseNotification(baseValue)
{
    public DummyNotification1()
        : this("Test Value", 99)
    {
    }

    public int Value1 { get; } = value1;

    protected override string DefaultAggregateId() => BaseValue + "-" + Value1.ToInvariantString();
}