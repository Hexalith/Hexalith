// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 02-06-2023
// ***********************************************************************
// <copyright file="EventNotSupportedByDispatcher.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Events;

using System.Runtime.Serialization;

using Hexalith.Extensions.Common;

/// <summary>
/// Class EventNotSupportedByDispatcher.
/// Implements the <see cref="Error" />
/// Implements the <see cref="IEquatable{Error}" />
/// Implements the <see cref="IEquatable{EventNotSupportedByDispatcher}" />.
/// </summary>
/// <seealso cref="Error" />
/// <seealso cref="IEquatable{Error}" />
/// <seealso cref="IEquatable{EventNotSupportedByDispatcher}" />
[DataContract]
public record EventNotSupportedByDispatcher : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventNotSupportedByDispatcher" /> class.
    /// </summary>
    /// <param name="dispatcher">The dispatcher.</param>
    public EventNotSupportedByDispatcher(string dispatcher)
    {
        Title = "Dispatcher event unsupported.";
        Type = nameof(EventNotSupportedByDispatcher);
        Detail = "The event could not be handled. It has an unsupported format.";
        TechnicalDetail = "The event is unsupported by {DispatcherName}.";
    }
}