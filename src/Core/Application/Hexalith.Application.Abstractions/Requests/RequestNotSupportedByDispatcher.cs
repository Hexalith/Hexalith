// <copyright file="RequestNotSupportedByDispatcher.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using System.Runtime.Serialization;

using Hexalith.Extensions.Common;

/// <summary>
/// Class RequestNotSupportedByDispatcher.
/// Implements the <see cref="ApplicationError" />
/// Implements the <see cref="IEquatable{Error}" />
/// Implements the <see cref="IEquatable{RequestNotSupportedByDispatcher}" />.
/// </summary>
/// <seealso cref="ApplicationError" />
/// <seealso cref="IEquatable{Error}" />
/// <seealso cref="IEquatable{RequestNotSupportedByDispatcher}" />
[DataContract]
public record RequestNotSupportedByDispatcher : ApplicationError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequestNotSupportedByDispatcher" /> class.
    /// </summary>
    /// <param name="dispatcher">The dispatcher.</param>
    public RequestNotSupportedByDispatcher(string dispatcher)
    {
        Title = "Dispatcher request unsupported.";
        Type = nameof(RequestNotSupportedByDispatcher);
        Detail = "The request could not be handled. It has an unsupported format.";
        TechnicalDetail = "The request is unsupported by {DispatcherName}.";
        TechnicalArguments = [dispatcher];
    }
}