// <copyright file="DaprApplicationBusTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprBus;

using Dapr.Client;

using Hexalith.Application.Metadatas;
using Hexalith.Application.States;
using Hexalith.Domain.Events;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.DaprRuntime.Buses;
using Hexalith.UnitTests.Core.Application.Metadatas;
using Hexalith.UnitTests.Core.Domain.Events;

using Microsoft.Extensions.Logging;

using Moq;

public class DaprApplicationBusTest
{
    [Fact]
    public async Task VerifyPublishCallsDaprPublishWithCompliantValues()
    {
        const string busName = "TestBus";
        const string topicSuffix = "-event";
        DummyEvent2 @event = new("hello test base", 123456);
        DummyMetadata meta = new(@event);

        Mock<DaprClient> client = new();
        Mock<ILogger> logger = new();
        DaprApplicationBus<BaseEvent, BaseMetadata, EventState> bus = new(client.Object, new DateTimeService(), busName, topicSuffix, logger.Object);
        await bus.PublishAsync(@event, meta, CancellationToken.None);
        client.Verify(
            p => p.PublishEventAsync(
            It.Is<string>(p => p == busName),
            It.Is<string>(p => p.Equals(@event.AggregateName + topicSuffix, StringComparison.OrdinalIgnoreCase)),
            It.Is<EventState>(
                p => p.Message == @event && p.Metadata == meta),
            It.IsAny<Dictionary<string, string>>(),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}