// <copyright file="BaseTestEvent2.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.MessageStores;

using Hexalith.PolymorphicSerialization;

[PolymorphicSerialization]
public partial record BaseTestEvent2(string Id, string IdempotencyId, string Message, string Value2)
    : BaseTestEvent(Id, IdempotencyId, Message);