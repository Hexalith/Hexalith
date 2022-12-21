// <copyright file="Command.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Commands;

using Hexalith.Domain.Abstractions.Messages;

using System.Runtime.Serialization;

[DataContract]
public abstract class Command : BaseMessage, ICommand
{
}
