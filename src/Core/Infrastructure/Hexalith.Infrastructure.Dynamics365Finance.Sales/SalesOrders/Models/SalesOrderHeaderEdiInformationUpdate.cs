// ***********************************************************************
// Assembly         : Christofle.Infrastructure.Dynamics365Finance.RestClient
// Author           : Jérôme Piquot
// Created          : 03-11-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 03-15-2023
// ***********************************************************************
// <copyright file="SalesOrderHeaderEdiInformationUpdate.cs" company="Fiveforty">
//     Copyright (c) Fiveforty S.A.S.. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hexalith.Infrastructure.Dynamics365Finance.Sales.SalesOrders.Models;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// Class SalesOrderHeaderAdditionalUpdate.
/// </summary>
[DataContract]
public class SalesOrderHeaderEdiInformationUpdate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SalesOrderHeaderEdiInformationUpdate" /> class.
    /// </summary>
    /// <param name="reservationNumber">The reservation number.</param>
    /// <param name="storeCode">The store code.</param>
    /// <param name="storeDepartmentCode">The store department code.</param>
    /// <param name="serviceCode">The service code.</param>
    /// <param name="shippingServiceLevel">The shipping service level.</param>
    /// <param name="finalCustomerOrderNumber">The final customer order number.</param>
    /// <param name="billToCode">The bill to code.</param>
    /// <param name="billToName">Name of the bill to.</param>
    /// <param name="billToAddress1">The bill to address1.</param>
    /// <param name="billToAddress2">The bill to address2.</param>
    /// <param name="billToPostal">The bill to postal.</param>
    /// <param name="billToCountry">The bill to country.</param>
    /// <param name="billToCity">The bill to city.</param>
    /// <param name="billToState">State of the bill to.</param>
    /// <param name="shipOpenDate">The ship order date.</param>
    [JsonConstructor]
    public SalesOrderHeaderEdiInformationUpdate(
        string? reservationNumber,
        string? storeCode,
        string? storeDepartmentCode,
        string? serviceCode,
        string? shippingServiceLevel,
        string? finalCustomerOrderNumber,
        string? billToCode,
        string? billToName,
        string? billToAddress1,
        string? billToAddress2,
        string? billToPostal,
        string? billToCountry,
        string? billToCity,
        string? billToState,
        DateTimeOffset? shipOpenDate)
    {
        ReservationNumber = reservationNumber;
        StoreCode = storeCode;
        StoreDepartmentCode = storeDepartmentCode;
        ServiceCode = serviceCode;
        ShippingServiceLevel = shippingServiceLevel;
        FinalCustomerOrderNumber = finalCustomerOrderNumber;
        BillToCode = billToCode;
        BillToName = billToName;
        BillToAddress1 = billToAddress1;
        BillToAddress2 = billToAddress2;
        BillToPostal = billToPostal;
        BillToCountry = billToCountry;
        BillToCity = billToCity;
        BillToState = billToState;
        ShipOpenDate = shipOpenDate;
    }

    /// <summary>
    /// Gets the bill to address1.
    /// </summary>
    /// <value>The bill to address1.</value>
    [JsonPropertyName("FFYBillToAddress")]
    public string? BillToAddress1 { get; }

    /// <summary>
    /// Gets the bill to address2.
    /// </summary>
    /// <value>The bill to address2.</value>
    [JsonPropertyName("FFYBillToAddress2")]
    public string? BillToAddress2 { get; }

    /// <summary>
    /// Gets the bill to city.
    /// </summary>
    /// <value>The bill to city.</value>
    [JsonPropertyName("FFYBillToCity")]
    public string? BillToCity { get; }

    /// <summary>
    /// Gets the bill to code.
    /// </summary>
    /// <value>The bill to code.</value>
    [JsonPropertyName("FFYBillToCode")]
    public string? BillToCode { get; }

    /// <summary>
    /// Gets the bill to country.
    /// </summary>
    /// <value>The bill to country.</value>
    [JsonPropertyName("FFYBillToCountry")]
    public string? BillToCountry { get; }

    /// <summary>
    /// Gets the name of the bill to.
    /// </summary>
    /// <value>The name of the bill to.</value>
    [JsonPropertyName("FFYBillToName")]
    public string? BillToName { get; }

    /// <summary>
    /// Gets the bill to postal.
    /// </summary>
    /// <value>The bill to postal.</value>
    [JsonPropertyName("FFYBillToPostal")]
    public string? BillToPostal { get; }

    /// <summary>
    /// Gets the state of the bill to.
    /// </summary>
    /// <value>The state of the bill to.</value>
    [JsonPropertyName("FFYBillToState")]
    public string? BillToState { get; }

    /// <summary>
    /// Gets the final customer order number.
    /// </summary>
    /// <value>The final customer order number.</value>
    [JsonPropertyName("FFYCustomerOrderNumber")]
    public string? FinalCustomerOrderNumber { get; }

    /// <summary>
    /// Gets the reservation number.
    /// </summary>
    /// <value>The reservation number.</value>
    [JsonPropertyName("FFYReservationNum")]
    public string? ReservationNumber { get; }

    /// <summary>
    /// Gets the service code.
    /// </summary>
    /// <value>The service code.</value>
    [JsonPropertyName("FFYServiceCode")]
    public string? ServiceCode { get; }

    /// <summary>
    /// Gets the ship order date.
    /// </summary>
    /// <value>The ship order date.</value>
    [JsonPropertyName("FFYOrderDate")]
    public DateTimeOffset? ShipOpenDate { get; }

    /// <summary>
    /// Gets the shipping service level.
    /// </summary>
    /// <value>The shipping service level.</value>
    [JsonPropertyName("FFYShippingServiceLevel")]
    public string? ShippingServiceLevel { get; }

    /// <summary>
    /// Gets the store code.
    /// </summary>
    /// <value>The store code.</value>
    [JsonPropertyName("FFYStore")]
    public string? StoreCode { get; }

    /// <summary>
    /// Gets the store department code.
    /// </summary>
    /// <value>The store department code.</value>
    [JsonPropertyName("FFYDepartment")]
    public string? StoreDepartmentCode { get; }
}