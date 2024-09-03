// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 02-04-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-01-2023
// ***********************************************************************
// <copyright file="BaseRequest.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Requests;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Messages;
using Hexalith.Extensions.Serialization;

/// <summary>
/// Class BaseRequest.
/// Implements the <see cref="BaseMessage" />
/// Implements the <see cref="IRequest" />.
/// </summary>
/// <seealso cref="BaseMessage" />
/// <seealso cref="IRequest" />
[DataContract]
[JsonConverter(typeof(PolymorphicJsonConverter<BaseRequest>))]
public class BaseRequest : BaseMessage, IRequest
{
    /// <summary>
    /// Gets the type of the result.
    /// </summary>
    /// <value>The type of the result.</value>
    [JsonIgnore]
    [IgnoreDataMember]
    public Type ResultType => GetDefaultResultType();

    /// <summary>
    /// Gets the default type of the result.
    /// </summary>
    /// <returns>Type.</returns>
    public virtual Type GetDefaultResultType() => typeof(string);
}