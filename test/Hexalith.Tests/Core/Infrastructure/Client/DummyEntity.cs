// <copyright file="DummyEntity.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Tests.Core.Infrastructure.Client;

using Ardalis.GuardClauses;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Models;

public record DummyEntity : ODataElement, IODataElement
{
    public DummyEntity(string etag, string dataAreaId, string message)
        : base(etag, dataAreaId)
    {
        Message = Guard.Against.NullOrWhiteSpace(message);
    }

    public string Message { get; }

    public static string EntityName()
    {
        return "Dummy";
    }
}