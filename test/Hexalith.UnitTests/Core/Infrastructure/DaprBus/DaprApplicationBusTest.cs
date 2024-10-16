﻿// <copyright file="DaprApplicationBusTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprBus;

using Dapr.Client;

using Hexalith.Application.MessageMetadatas;
using Hexalith.Infrastructure.DaprRuntime.Buses;
using Hexalith.UnitTests.Core.Application.Metadatas;
using Hexalith.UnitTests.Core.Domain.Events;

using Microsoft.Extensions.Logging;

using Moq;

public class DaprApplicationBusTest
{
    [Fact]
    [Obsolete]
    public async Task VerifyPublishCallsDaprPublishWithCompliantValues()
    {
        const string busName = "TestBus";
        const string topicSuffix = "-event";
        DummyEvent2 @event = new("hello test base", 123456);
        DummyMetadata meta = new(@event);

        Mock<DaprClient> client = new();
        Mock<ILogger> logger = new();
        DaprApplicationBus bus = new(client.Object, TimeProvider.System, busName, topicSuffix, logger.Object);
        await bus.PublishAsync(@event, meta, CancellationToken.None);
        client.Verify(
            p => p.PublishEventAsync(
            It.Is<string>(p => p == busName),
            It.Is<string>(p => p.Equals(@event.AggregateName + topicSuffix, StringComparison.OrdinalIgnoreCase)),
            It.Is<MessageState>(
                p => (DummyEvent2)p.Message == @event && p.Metadata == meta),
            It.IsAny<Dictionary<string, string>>(),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}