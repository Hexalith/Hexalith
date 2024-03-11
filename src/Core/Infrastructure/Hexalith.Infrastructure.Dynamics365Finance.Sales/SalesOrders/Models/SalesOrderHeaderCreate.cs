// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 01-25-2023
// ***********************************************************************
// <copyright file="SalesOrderHeaderCreate.cs" company="Fiveforty">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Infrastructure.Serialization.Serialization;

/// <summary>
/// Class SalesOrderHeaderCreate.
/// Implements the <see cref="IEquatable{SalesOrderHeaderCreate}" />.
/// </summary>
/// <seealso cref="IEquatable{SalesOrderHeaderCreate}" />
[DataContract]
public record SalesOrderHeaderCreate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesOrderHeaderCreate"/> class.
    /// </summary>
    /// <param name="dataAreaId">The data area identifier.</param>
    /// <param name="salesOrderOriginCode">The sales order origin code.</param>
    /// <param name="customerRequisitionNumber">The customer requisition number.</param>
    /// <param name="customersOrderReference">The customers order reference.</param>
    /// <param name="orderingCustomerAccountNumber">The ordering customer account number.</param>
    /// <param name="invoiceCustomerAccountNumber">The invoice customer account number.</param>
    /// <param name="salesUnitId">The sales unit identifier.</param>
    /// <param name="revRecReallocationId">The rev record reallocation identifier.</param>
    /// <param name="deliveryAddressName">Name of the delivery address.</param>
    /// <param name="deliveryAddressDescription">The delivery address description.</param>
    /// <param name="deliveryAddressStreetNumber">The delivery address street number.</param>
    /// <param name="deliveryAddressStreet">The delivery address street.</param>
    /// <param name="deliveryAddressPostBox">The delivery address post box.</param>
    /// <param name="deliveryAddressZipCode">The delivery address zip code.</param>
    /// <param name="deliveryAddressCity">The delivery address city.</param>
    /// <param name="deliveryAddressCountyId">The delivery address county identifier.</param>
    /// <param name="deliveryAddressStateId">The delivery address state identifier.</param>
    /// <param name="deliveryAddressCountryRegionISOCode">The delivery address country region iso code.</param>
    /// <param name="deliveryAddressCountryRegionId">The delivery address country region identifier.</param>
    /// <param name="email">The email.</param>
    /// <param name="requestedShippingDate">The requested shipping date.</param>
    /// <param name="requestedReceiptDate">The requested receipt date.</param>
    public SalesOrderHeaderCreate(
        string? dataAreaId,
        string? salesOrderOriginCode,
        string? customerRequisitionNumber,
        string? customersOrderReference,
        string? orderingCustomerAccountNumber,
        string? invoiceCustomerAccountNumber,
        string? salesUnitId,
        string? revRecReallocationId,
        string? deliveryAddressName,
        string? deliveryAddressDescription,
        string? deliveryAddressStreetNumber,
        string? deliveryAddressStreet,
        string? deliveryAddressPostBox,
        string? deliveryAddressZipCode,
        string? deliveryAddressCity,
        string? deliveryAddressCountyId,
        string? deliveryAddressStateId,
        string? deliveryAddressCountryRegionISOCode,
        string? deliveryAddressCountryRegionId,
        string? email,
        DateTimeOffset? requestedShippingDate,
        DateTimeOffset? requestedReceiptDate)
    {
        DataAreaId = dataAreaId;
        SalesOrderOriginCode = salesOrderOriginCode;
        CustomerRequisitionNumber = customerRequisitionNumber;
        CustomersOrderReference = customersOrderReference;
        OrderingCustomerAccountNumber = orderingCustomerAccountNumber;
        InvoiceCustomerAccountNumber = invoiceCustomerAccountNumber;
        SalesUnitId = salesUnitId;
        RevRecReallocationId = revRecReallocationId;
        DeliveryAddressName = deliveryAddressName;
        DeliveryAddressDescription = deliveryAddressDescription;
        DeliveryAddressStreetNumber = deliveryAddressStreetNumber;
        DeliveryAddressStreet = deliveryAddressStreet;
        DeliveryAddressPostBox = deliveryAddressPostBox;
        DeliveryAddressZipCode = deliveryAddressZipCode;
        DeliveryAddressCity = deliveryAddressCity;
        DeliveryAddressCountyId = deliveryAddressCountyId;
        DeliveryAddressStateId = deliveryAddressStateId;
        DeliveryAddressCountryRegionCode = deliveryAddressCountryRegionISOCode;
        DeliveryAddressCountryRegionId = deliveryAddressCountryRegionId;
        Email = email;
        RequestedShippingDate = requestedShippingDate;
        RequestedReceiptDate = requestedReceiptDate;
    }

    /// <summary>
    /// Gets the data area identifier.
    /// </summary>
    /// <value>The data area identifier.</value>
    [JsonPropertyName("dataAreaId")]
    public string? DataAreaId { get; }

    /// <summary>
    /// Gets the sales order origin code.
    /// </summary>
    /// <value>The sales order origin code.</value>
    public string? SalesOrderOriginCode { get; }

    /// <summary>
    /// Gets the customer requisition number.
    /// </summary>
    /// <value>The customer requisition number.</value>
    public string? CustomerRequisitionNumber { get; }

    /// <summary>
    /// Gets the customers order reference.
    /// </summary>
    /// <value>The customers order reference.</value>
    public string? CustomersOrderReference { get; }

    /// <summary>
    /// Gets the ordering customer account number.
    /// </summary>
    /// <value>The ordering customer account number.</value>
    public string? OrderingCustomerAccountNumber { get; }

    /// <summary>
    /// Gets the invoice customer account number.
    /// </summary>
    /// <value>The invoice customer account number.</value>
    public string? InvoiceCustomerAccountNumber { get; }

    /// <summary>
    /// Gets the sales unit identifier.
    /// </summary>
    /// <value>The sales unit identifier.</value>
    public string? SalesUnitId { get; }

    /// <summary>
    /// Gets the rev record reallocation identifier.
    /// </summary>
    /// <value>The rev record reallocation identifier.</value>
    public string? RevRecReallocationId { get; }

    /// <summary>
    /// Gets the name of the delivery address.
    /// </summary>
    /// <value>The name of the delivery address.</value>
    public string? DeliveryAddressName { get; }

    /// <summary>
    /// Gets the delivery address description.
    /// </summary>
    /// <value>The delivery address description.</value>
    public string? DeliveryAddressDescription { get; }

    /// <summary>
    /// Gets the delivery address street number.
    /// </summary>
    /// <value>The delivery address street number.</value>
    public string? DeliveryAddressStreetNumber { get; }

    /// <summary>
    /// Gets the delivery address street.
    /// </summary>
    /// <value>The delivery address street.</value>
    public string? DeliveryAddressStreet { get; }

    /// <summary>
    /// Gets the delivery address post box.
    /// </summary>
    /// <value>The delivery address post box.</value>
    public string? DeliveryAddressPostBox { get; }

    /// <summary>
    /// Gets the delivery address zip code.
    /// </summary>
    /// <value>The delivery address zip code.</value>
    public string? DeliveryAddressZipCode { get; }

    /// <summary>
    /// Gets the delivery address city.
    /// </summary>
    /// <value>The delivery address city.</value>
    public string? DeliveryAddressCity { get; }

    /// <summary>
    /// Gets the delivery address county identifier.
    /// </summary>
    /// <value>The delivery address county identifier.</value>
    public string? DeliveryAddressCountyId { get; }

    /// <summary>
    /// Gets the delivery address state identifier.
    /// </summary>
    /// <value>The delivery address state identifier.</value>
    public string? DeliveryAddressStateId { get; }

    /// <summary>
    /// Gets the delivery address country region iso code.
    /// </summary>
    /// <value>The delivery address country region iso code.</value>
    [JsonPropertyName("DeliveryAddressCountryRegionISOCode")]
    public string? DeliveryAddressCountryRegionCode { get; }

    /// <summary>
    /// Gets the delivery address country region identifier.
    /// </summary>
    /// <value>The delivery address country region identifier.</value>
    public string? DeliveryAddressCountryRegionId { get; }

    /// <summary>
    /// Gets the email.
    /// </summary>
    /// <value>The email.</value>
    public string? Email { get; }

    /// <summary>
    /// Gets the requested shipping date.
    /// </summary>
    /// <value>The requested shipping date.</value>
    [JsonConverter(typeof(IsoUtcDateTimeOffsetConverter))]
    public DateTimeOffset? RequestedShippingDate { get; }

    /// <summary>
    /// Gets the requested receipt date.
    /// </summary>
    /// <value>The requested receipt date.</value>
    [JsonConverter(typeof(IsoUtcDateTimeOffsetConverter))]
    public DateTimeOffset? RequestedReceiptDate { get; }
}