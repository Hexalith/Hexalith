// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-25-2023
// ***********************************************************************
// <copyright file="PackingSlipTrackingInformation.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
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
    [property: DataMember(Order = 3)] string SalesOrderNumber,
    [property: DataMember(Order = 4)] string PackingSlipNumber,
    [property: DataMember(Order = 5)] DateTimeOffset DeliveryDate,
    [property: DataMember(Order = 6)] string TrackingNumber,
#pragma warning disable CA1054 // URI-like parameters should not be strings
#pragma warning disable CA1056 // URI-like properties should not be strings
    [property: DataMember(Order = 7)] string TrackingURL)
#pragma warning restore CA1056 // URI-like properties should not be strings
#pragma warning restore CA1054 // URI-like parameters should not be strings
    : ODataElement(Etag, DataAreaId), IODataElement
{
    /// <summary>
    /// Entities the name.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string EntityName() => "PackingSlipTrackingInformation";
}