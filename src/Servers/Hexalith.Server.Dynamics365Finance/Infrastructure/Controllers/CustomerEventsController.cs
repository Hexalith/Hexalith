// <copyright file="CustomerEventsController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Server.Dynamics365Finance.Infrastructure.Controllers;

using Hexalith.Application.Events;
using Hexalith.Application.Projection;
using Hexalith.Infrastructure.WebApis.PartiesEvents.Controllers;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Class CustomerEventsController.
/// Implements the <see cref="CustomerIntegrationEventsController" />.
/// </summary>
/// <seealso cref="CustomerIntegrationEventsController" />
[ApiController]
public class CustomerEventsController : CustomerIntegrationEventsController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerEventsController" /> class.
    /// </summary>
    /// <param name="eventProcessor">The event processor.</param>
    /// <param name="projectionProcessor">The projection processor.</param>
    /// <param name="hostEnvironment">The host environment.</param>
    /// <param name="logger">The logger.</param>
    public CustomerEventsController(
        IIntegrationEventProcessor eventProcessor,
        IProjectionUpdateProcessor projectionProcessor,
        IHostEnvironment hostEnvironment,
        ILogger<CustomerEventsController> logger)
        : base(eventProcessor, projectionProcessor, hostEnvironment, logger)
    {
    }
}

/*
{
    "message": {
        "$type_name": "CustomerInformationChanged",
        "$version_major": 0,
        "$version_minor": 0,
        "commissionSalesGroupId": "TEST1",
        "companyId": "FRRT",
        "contact": {
            "email": "mark.d@email.com",
            "mobile": "+1 555 98765998",
            "person": {
                "birthDate": "1980-01-23T00:00:00+00:00",
                "firstName": "Mark",
                "gender": "Male",
                "lastName": "Desmound Toto",
                "name": "Mark Desmound Toto",
                "title": "Mr."
            },
            "phone": "+1 555 4431010",
            "postalAddress": {
                "city": "Shawnee",
                "countryId": "USA",
                "countryIso2": "FR",
                "countryName": "France",
                "countyId": null,
                "description": "Mark",
                "name": "Mark Desmound Toto",
                "postBox": "661 N Park Ave\nApartment #106\ntoto\ntiti",
                "stateId": "OK",
                "stateName": "",
                "street": "",
                "streetNumber": "Desmound Toto",
                "zipCode": "74801"
            }
        },
        "date": "2023-12-11T17:46:15.2564335+00:00",
        "groupId": null,
        "id": "2004",
        "name": "Mark Desmound Toto",
        "originId": "FinOps",
        "partitionId": "FFY",
        "partyType": "Person",
        "salesCurrencyId": null,
        "warehouseId": "Bordeau"
    },
    "metadata": {
        "$type_name": "Metadata",
        "$version_major": 0,
        "$version_minor": 0,
        "context": {
            "correlationId": "0HMVR0ACMQTLO:00000001",
            "receivedDate": "2023-12-12T12:25:33.5008093+00:00",
            "sequenceNumber": null,
            "sessionId": null,
            "userId": "bspk-system"
        },
        "message": {
            "aggregate": {
                "id": "Customer-FFY-FRRT-FinOps-2004",
                "name": "Customer"
            },
            "createdDate": "2023-12-12T12:25:34.0492011+00:00",
            "id": "8PcAfEzu0U62wlR_ye8iCg",
            "name": "CustomerInformationChanged",
            "version": {
                "major": 0,
                "minor": 0
            }
        },
        "scopes": []
    },
    "receivedDate": "2023-12-12T12:25:34.0492007+00:00"
}
 */