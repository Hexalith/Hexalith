// <copyright file="IRequestBus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Application.Requests;

using Hexalith.Application.Envelopes;

/// <summary>
/// A request bus is a component that allows to send requests.
/// </summary>
public interface IRequestBus : IMessageBus
{
}