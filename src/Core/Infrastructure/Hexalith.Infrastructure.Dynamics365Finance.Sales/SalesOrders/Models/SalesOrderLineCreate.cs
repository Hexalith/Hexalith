// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 11-18-2022
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-17-2022
// ***********************************************************************
// <copyright file="SalesOrderLineCreate.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class SalesOrderLineCreate.
/// Implements the <see cref="IEquatable{SalesOrderLineCreate}" />.
/// </summary>
/// <seealso cref="IEquatable{SalesOrderLineCreate}" />
[DataContract]
public record SalesOrderLineCreate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesOrderLineCreate"/> class.
    /// </summary>
    /// <param name="dataAreaId">The data area identifier.</param>
    /// <param name="salesOrderNumber">The sales order number.</param>
    /// <param name="customersLineNumber">The customers line number.</param>
    /// <param name="itemNumber">The item number.</param>
    /// <param name="productStyleId">The product style identifier.</param>
    /// <param name="productColorId">The product color identifier.</param>
    /// <param name="productSizeId">The product size identifier.</param>
    /// <param name="salesPrice">The sales price.</param>
    /// <param name="orderedSalesQuantity">The ordered sales quantity.</param>
    /// <param name="salesUnitSymbol">The sales unit symbol.</param>
    /// <param name="giftCardGiftMessage">The gift card gift message.</param>
    [JsonConstructor]
    public SalesOrderLineCreate(
        string dataAreaId,
        string salesOrderNumber,
        int? customersLineNumber,
        string itemNumber,
        string? productStyleId,
        string? productColorId,
        string? productSizeId,
        decimal salesPrice,
        decimal orderedSalesQuantity,
        string? salesUnitSymbol,
        string? giftCardGiftMessage)
    {
        DataAreaId = dataAreaId;
        SalesOrderNumber = salesOrderNumber;
        CustomersLineNumber = customersLineNumber;
        ItemNumber = itemNumber;
        ProductStyleId = productStyleId;
        ProductColorId = productColorId;
        ProductSizeId = productSizeId;
        SalesPrice = salesPrice;
        OrderedSalesQuantity = orderedSalesQuantity;
        SalesUnitSymbol = salesUnitSymbol;
        GiftCardGiftMessage = giftCardGiftMessage;
    }

    /// <summary>
    /// Gets the data area identifier.
    /// </summary>
    /// <value>The data area identifier.</value>
    [JsonPropertyName("dataAreaId")]
    public string DataAreaId { get; }

    /// <summary>
    /// Gets the sales order number.
    /// </summary>
    /// <value>The sales order number.</value>
    public string SalesOrderNumber { get; }

    /// <summary>
    /// Gets the customers line number.
    /// </summary>
    /// <value>The customers line number.</value>
    public int? CustomersLineNumber { get; }

    /// <summary>
    /// Gets the item number.
    /// </summary>
    /// <value>The item number.</value>
    public string ItemNumber { get; }

    /// <summary>
    /// Gets the product style identifier.
    /// </summary>
    /// <value>The product style identifier.</value>
    public string? ProductStyleId { get; }

    /// <summary>
    /// Gets the product color identifier.
    /// </summary>
    /// <value>The product color identifier.</value>
    public string? ProductColorId { get; }

    /// <summary>
    /// Gets the product size identifier.
    /// </summary>
    /// <value>The product size identifier.</value>
    public string? ProductSizeId { get; }

    /// <summary>
    /// Gets the sales price.
    /// </summary>
    /// <value>The sales price.</value>
    public decimal SalesPrice { get; }

    /// <summary>
    /// Gets the ordered sales quantity.
    /// </summary>
    /// <value>The ordered sales quantity.</value>
    public decimal OrderedSalesQuantity { get; }

    /// <summary>
    /// Gets the sales unit symbol.
    /// </summary>
    /// <value>The sales unit symbol.</value>
    public string? SalesUnitSymbol { get; }

    /// <summary>
    /// Gets the gift card gift message.
    /// </summary>
    /// <value>The gift card gift message.</value>
    public string? GiftCardGiftMessage { get; }
}