// <copyright file="DummyBaseCommand.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Abstractions.Commands;

[DataContract]
public abstract class DummyBaseCommand : BaseCommand
{
    protected DummyBaseCommand()
    {
        BaseValue = string.Empty;
    }

    [JsonConstructor]
    protected DummyBaseCommand(string baseValue)
    {
        BaseValue = baseValue;
    }

    public string BaseValue { get; }

    protected override string DefaultAggregateName()
    {
        return "Test";
    }
}