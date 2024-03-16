// <copyright file="SalesOrderOriginUpdate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;

[DataContract]
public record SalesOrderOriginUpdate([property: DataMember(Order = 1)] string SalesOrderOriginCode)
{
}