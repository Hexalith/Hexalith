// <copyright file="DummyEntity.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Dynamics365Finance.Client;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

public record DummyEntity : ODataElement, IODataElement
{
    public DummyEntity(string etag, string dataAreaId, string message)
        : base(etag, dataAreaId)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
        Message = message;
    }

    public string Message { get; }

    public static string EntityName() => "Dummy";
}