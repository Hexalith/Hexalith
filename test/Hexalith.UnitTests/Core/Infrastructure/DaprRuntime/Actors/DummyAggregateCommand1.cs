// <copyright file="DummyAggregateCommand1.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprRuntime.Actors;

using System.Runtime.Serialization;

using Hexalith.Application.Commands;

[DataContract]
[Serializable]
public class DummyAggregateCommand1 : BaseCommand
{
    public string Id { get; set; }

    public string Name { get; set; } = string.Empty;

    protected override string DefaultAggregateId() => DummyAggregate.GetAggregateId(Id);

    protected override string DefaultAggregateName() => DummyAggregate.GetAggregateName();
}