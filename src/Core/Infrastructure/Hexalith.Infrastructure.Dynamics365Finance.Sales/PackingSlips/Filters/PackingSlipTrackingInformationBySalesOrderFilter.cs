// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 03-08-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-08-2023
// ***********************************************************************
// <copyright file="PackingSlipTrackingInformationByOrderFilter.cs" company="Fiveforty">
//     Copyright (c) Fiveforty S.A.S.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.PackingSlips.Filters;

using System.Runtime.Serialization;

using Hexalith.Infrastructure.Dynamics365Finance.Models;

/// <summary>
/// Class SalesOrderKey.
/// Implements the <see cref="PerCompanyPrimaryKey" />
/// Implements the <see cref="IPerCompanyPrimaryKey" />
/// Implements the <see cref="IPrimaryKey" />
/// Implements the <see cref="IEquatable{PerCompanyPrimaryKey}" />
/// Implements the <see cref="IEquatable{SalesOrderKey}" />.
/// </summary>
/// <seealso cref="PerCompanyPrimaryKey" />
/// <seealso cref="IPerCompanyPrimaryKey" />
/// <seealso cref="IPrimaryKey" />
/// <seealso cref="IEquatable{PerCompanyPrimaryKey}" />
/// <seealso cref="IEquatable{SalesOrderKey}" />
[DataContract]
public record PackingSlipTrackingInformationBySalesOrderFilter(
    string DataAreaId,
    string SalesOrderNumber)
    : PerCompanyFilter(DataAreaId)
{
}