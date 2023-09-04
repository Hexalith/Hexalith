// ***********************************************************************
// Assembly         : Hexalith.Application.ExternalSystems
// Author           : Jérôme Piquot
// Created          : 08-28-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 09-04-2023
// ***********************************************************************
// <copyright file="RemoveExternalSystemReference.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.ExternalSystems.Commands;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class ExternalSystemReferenceUnmapped.
/// Implements the <see cref="ExternalSystemReferenceCommand" />.
/// </summary>
/// <seealso cref="ExternalSystemReferenceCommand" />
[DataContract]
public class RemoveExternalSystemReference : ExternalSystemReferenceCommand
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveExternalSystemReference" /> class.
    /// </summary>
    /// <param name="systemId">The identifier.</param>
    /// <param name="referenceAggregateName">Type of the aggregate.</param>
    /// <param name="externalId">The external identifier.</param>
    [JsonConstructor]
    public RemoveExternalSystemReference(
        string systemId,
        string referenceAggregateName,
        string externalId)
        : base(systemId, referenceAggregateName, externalId)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveExternalSystemReference" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public RemoveExternalSystemReference()
    {
    }
}