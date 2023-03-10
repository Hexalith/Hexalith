// <copyright file="BaseRequest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Requests;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Hexalith.Domain.Abstractions.Messages;
using Hexalith.Extensions.Serialization;

/// <summary>
/// Class BaseRequest.
/// Implements the <see cref="BaseMessage" />
/// Implements the <see cref="Application.Requests.IRequest" />.
/// </summary>
/// <seealso cref="BaseMessage" />
/// <seealso cref="Application.Requests.IRequest" />
[DataContract]
[JsonConverter(typeof(PolymorphicJsonConverter<BaseRequest>))]
public class BaseRequest : BaseMessage, IRequest
{
}