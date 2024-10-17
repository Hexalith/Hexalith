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
    public DaprApplicationBusTest() => Extensions.HexalithUnitTestsMapperExtension.Initialize();

    [Fact]
    public async Task VerifyPublishCallsDaprPublishWithCompliantValues()
    {
        DummyEvent2 @event = new("hello test base", 123456);
        DummyMetadata meta = new(@event);

        const string busName = "TestBus";

        const string topicName = "test-event";
        const string topicSuffix = "-event";

        Mock<DaprClient> client = new();
        Mock<ILogger> logger = new();
        DaprApplicationBus bus = new(client.Object, TimeProvider.System, busName, topicSuffix, logger.Object);
        await bus.PublishAsync(@event, meta, CancellationToken.None);
        client.Verify(
            p => p.PublishEventAsync<MessageState>(
             It.Is<string>(p => p == busName),
             It.Is<string>(p => p == topicName),
             It.Is<MessageState>(
                p => p.Metadata == meta && (DummyEvent2)p.Message == @event),
             It.IsAny<Dictionary<string, string>>(),
             It.IsAny<CancellationToken>()),
            Times.Once);
    }
}