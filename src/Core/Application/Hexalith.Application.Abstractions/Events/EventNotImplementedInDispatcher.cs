// <copyright file="EventNotImplementedInDispatcher.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Events;

using Hexalith.Application.Abstractions.Errors;

using System.Runtime.Serialization;

[DataContract]
public record EventNotSupportedByDispatcher : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventNotSupportedByDispatcher"/> class.
    /// </summary>
    /// <param name="event"></param>
    public EventNotSupportedByDispatcher(string dispatcher)
    {
        Title = "Dispatcher event unsupported.";
        Type = nameof(EventNotSupportedByDispatcher);
        Detail = "The event could not be handled. It has an unsupported format.";
        TechnicalDetail = "The event is unsupported by {DispatcherName}.";
    }
}
