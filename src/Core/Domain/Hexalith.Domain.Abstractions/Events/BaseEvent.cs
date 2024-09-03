// <copyright file="BaseEvent.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

/*
 * <Your-Product-Name>
 * Copyright (c) <Year-From>-<Year-To> <Your-Company-Name>
 *
 * Please configure this header in your SonarCloud/SonarQube quality profile.
 * You can also set it in SonarLint.xml additional file for SonarLint or standalone NuGet analyzer.
 */

namespace Hexalith.Domain.Events;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Domain.Messages;
using Hexalith.Extensions.Serialization;

/// <summary>
/// Base class for business events.
/// </summary>
[DataContract]
[Serializable]
[JsonConverter(typeof(PolymorphicJsonConverter<BaseEvent>))]
public class BaseEvent : BaseMessage, IEvent
{
}