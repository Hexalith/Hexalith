// ***********************************************************************
// Assembly         : Hexalith.Application.Abstractions
// Author           : Jérôme Piquot
// Created          : 01-13-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 04-30-2023
// ***********************************************************************
// <copyright file="BaseCommand.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Application.Commands;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Messages;
using Hexalith.Extensions.Serialization;

/// <summary>
/// Class BaseCommand.
/// Implements the <see cref="BaseMessage" />
/// Implements the <see cref="ICommand" />.
/// </summary>
/// <seealso cref="BaseMessage" />
/// <seealso cref="ICommand" />
[DataContract]
[Serializable]
[JsonConverter(typeof(PolymorphicJsonConverter<BaseCommand>))]
public class BaseCommand : BaseMessage, ICommand
{
}