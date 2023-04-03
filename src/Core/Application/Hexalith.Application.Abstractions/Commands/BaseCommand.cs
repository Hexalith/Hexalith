// <copyright file="BaseCommand.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Abstractions.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Abstractions.Messages;
using Hexalith.Extensions.Serialization;

/// <summary>
/// Class BaseCommand.
/// Implements the <see cref="BaseMessage" />
/// Implements the <see cref="Application.Commands.ICommand" />.
/// </summary>
/// <seealso cref="BaseMessage" />
/// <seealso cref="Application.Commands.ICommand" />
[DataContract]
[JsonConverter(typeof(PolymorphicJsonConverter<BaseCommand>))]
public class BaseCommand : BaseMessage, ICommand
{
}