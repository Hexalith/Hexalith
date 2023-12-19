// ***********************************************************************
// Assembly         : Hexalith.Server.Dynamics365Finance
// Author           : Jérôme Piquot
// Created          : 12-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-15-2023
// ***********************************************************************
// <copyright file="CustomerEventsController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

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
{"message":{"$type_name":"CustomerInformationChanged","$version_major":0,"$version_minor":0,"commissionSalesGroupId":"","companyId":"FRRT","contact":{"email":"jean@valjean.com","mobile":"+33611223344","person":{"birthDate":null,"firstName":"Jean A","gender":"Unknown","lastName":"Valjean","name":null,"title":null},"phone":"","postalAddress":{"city":"","countryId":"","countryIso2":"","countryName":"","countyId":null,"description":null,"name":null,"postBox":null,"stateId":"","stateName":"","street":"","streetNumber":"","zipCode":""}},"date":"2023-12-19T07:17:41.6780365+00:00","groupId":null,"id":"473882","name":"Jean A Valjean","originId":"Bspk","partitionId":"FFY","partyType":"Person","salesCurrencyId":null,"warehouseId":"Paris"},"metadata":{"$type_name":"Metadata","$version_major":0,"$version_minor":0,"context":{"correlationId":"71C64076-E677-47D6-8E59-44024D814A41","receivedDate":"2023-12-18T21:31:16+00:00","sequenceNumber":null,"sessionId":null,"userId":"{3AF3FAE3-CF09-45FA-80ED-548822D4F02E}"},"message":{"aggregate":{"id":"Customer-FFY-FRRT-Bspk-473882","name":"Customer"},"createdDate":"2023-12-19T07:17:41.6798412+00:00","id":"7wHWnD7oj0WjeBAa2wwOyA","name":"CustomerInformationChanged","version":{"major":0,"minor":0}},"scopes":[]},"receivedDate":"2023-12-19T07:17:42.0510821+00:00"}
 */