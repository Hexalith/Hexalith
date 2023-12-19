// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.Dynamics365Finance.Parties
// Author           : Jérôme Piquot
// Created          : 12-19-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-19-2023
// ***********************************************************************
// <copyright file="Dynamics365FinanceCustomerState.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.Projections;

using System;
using System.Runtime.Serialization;

using Hexalith.Domain.ValueObjets;
using Hexalith.Infrastructure.Dynamics365Finance.Parties.Customers.BusinessEvents;

using KellermanSoftware.CompareNetObjects;

/// <summary>
/// Class Dynamics365FinanceCustomerState.
/// Implements the <see cref="Dynamics365FinanceCustomerInformationBusinessEvent" />.
/// </summary>
/// <seealso cref="Dynamics365FinanceCustomerInformationBusinessEvent" />
[DataContract]
[Serializable]
public class Dynamics365FinanceCustomerState : Dynamics365FinanceCustomerInformationBusinessEvent
{
    /// <summary>
    /// Creates the specified e.
    /// </summary>
    /// <param name="e">The e.</param>
    /// <returns>Dynamics365FinanceCustomerState.</returns>
    public static Dynamics365FinanceCustomerState Create(Dynamics365FinanceCustomerInformationBusinessEvent e)
    {
        ArgumentNullException.ThrowIfNull(e);
        return new Dynamics365FinanceCustomerState()
        {
            Account = e.Account,
            BusinessEventId = e.BusinessEventId,
            BusinessEventLegalEntity = e.BusinessEventLegalEntity,
            CommissionSalesGroupId = e.CommissionSalesGroupId,
            Contact = e.Contact,
            EventId = e.EventId,
            OriginId = e.OriginId,
            PartitionId = e.PartitionId,
            ControlNumber = e.ControlNumber,
            EventTime = e.EventTime,
            EventTimeIso8601 = e.EventTimeIso8601,
            ExternalReferences = (e.ExternalReferences == null) ? null : new List<ExternalReference>(e.ExternalReferences),
            GroupId = e.GroupId,
            InitiatingUserAzureActiveDirectoryObjectId = e.InitiatingUserAzureActiveDirectoryObjectId,
            InterCompanyDirectDelivery = e.InterCompanyDirectDelivery,
            MajorVersion = e.MajorVersion,
            MinorVersion = e.MinorVersion,
            Name = e.Name,
            PartyType = e.PartyType,
            SalesCurrencyId = e.SalesCurrencyId,
            WarehouseId = e.WarehouseId,
        };
    }

    /// <summary>
    /// Determines whether the specified event has changes.
    /// </summary>
    /// <param name="event">The event.</param>
    /// <returns>System.Nullable&lt;System.String&gt;.</returns>
    public string? HasChanges(Dynamics365FinanceCustomerChanged @event)
    {
        ArgumentNullException.ThrowIfNull(@event);
        Dynamics365FinanceCustomerState newValue = Create(@event);
        CompareLogic compareLogic = new();
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.BusinessEventId);
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.EventId);
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.ControlNumber);
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.EventTime);
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.EventTimeIso8601);
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.InitiatingUserAzureActiveDirectoryObjectId);
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.MajorVersion);
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.MinorVersion);
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.Scopes);
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.Context);
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.Message);
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.ReceivedDateTime);
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.TypeName);
        compareLogic.Config.IgnoreProperty<Dynamics365FinanceCustomerState>(x => x.Version);

        ComparisonResult result = compareLogic.Compare(this, newValue);

        return !result.AreEqual ? result.DifferencesString : null;
    }
}