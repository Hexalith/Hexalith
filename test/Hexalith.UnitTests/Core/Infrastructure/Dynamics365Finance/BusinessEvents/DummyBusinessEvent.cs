// <copyright file="DummyBusinessEvent.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Dynamics365Finance.BusinessEvents;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.BusinessEvents;

using System.Runtime.Serialization;

[DataContract]
public class DummyBusinessEvent1 : Dynamics365BusinessEventBase
{
    public override string AggregateId => ValueOne;

    public override string AggregateName => nameof(DummyBusinessEvent1);

    public string ValueOne { get; set; }

    public override BaseCommand ToCommand()
    {
        throw new NotSupportedException();
    }
}

[DataContract]
public class DummyBusinessEvent2 : Dynamics365BusinessEventBase
{
    public override string AggregateId => ValueTwo.ToInvariantString();

    public override string AggregateName => nameof(DummyBusinessEvent1);

    public int ValueTwo { get; set; }

    public override BaseCommand ToCommand()
    {
        throw new NotSupportedException();
    }
}