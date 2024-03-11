// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-25-2023
// ***********************************************************************
// <copyright file="SalesOrderHeader.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.PackingSlips.Models;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class SalesOrderHeader.
/// Implements the <see cref="ODataElement" />
/// Implements the <see cref="IEquatable{ODataElement}" />
/// Implements the <see cref="IODataElement" />
/// Implements the <see cref="IEquatable{SalesOrderHeader}" />.
/// </summary>
/// <seealso cref="ODataElement" />
/// <seealso cref="IEquatable{ODataElement}" />
/// <seealso cref="IODataElement" />
/// <seealso cref="IEquatable{SalesOrderHeader}" />
[DataContract]
public record PackingSlipTrackingInformation(
    string Etag,
    string DataAreaId,
    string SalesOrderNumber,
    string PackingSlipNumber,
    DateTimeOffset DeliveryDate,
    string TrackingNumber,
    string TrackingURL)
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName()
    {
        return "PackingSlipTrackingInformation";
    }
}