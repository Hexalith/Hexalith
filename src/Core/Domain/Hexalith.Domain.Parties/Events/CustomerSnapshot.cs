// <copyright file="CustomerSnapshot.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Domain.Events;

using Hexalith.Domain.Aggregates;
using Hexalith.Extensions;

/// <summary>
/// Class CustomerSnapshot.
/// Implements the <see cref="SnapshotEvent{Customer}" />.
/// </summary>
/// <seealso cref="SnapshotEvent{Customer}" />
public class CustomerSnapshot : SnapshotEvent<Customer>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerSnapshot"/> class.
    /// </summary>
    /// <param name="customer">The customer.</param>
    public CustomerSnapshot(Customer customer)
        : base(customer)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerSnapshot"/> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public CustomerSnapshot()
    {
    }
}