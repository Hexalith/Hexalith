// <copyright file="DaprApplicationBusTest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprBus;

using Dapr.Client;

using Hexalith.Infrastructure.DaprRuntime.Buses;
using Hexalith.UnitTests.Core.Application.Metadatas;
using Hexalith.UnitTests.Core.Domain.Events;

using Microsoft.Extensions.Logging;

using NSubstitute;

public class DaprApplicationBusTest
{
    public DaprApplicationBusTest() => Extensions.HexalithUnitTestsSerialization.RegisterPolymorphicMappers();

    [Fact]
    public async Task VerifyPublishCallsDaprPublishWithCompliantValues()
    {
        DummyEvent2 @event = new("hello test base", 123456);
        DummyMetadata meta = new(@event);

        const string busName = "TestBus";

        const string topicName = "test-event";
        const string topicSuffix = "-event";

        DaprClient client = Substitute.For<DaprClient>();
        ILogger logger = Substitute.For<ILogger>();
        DaprApplicationBus bus = new(client, TimeProvider.System, busName, topicSuffix, logger);
        await bus.PublishAsync(@event, meta, CancellationToken.None);
        await client.Received(1).PublishEventAsync<BusMessage>(
            Arg.Is<string>(p => p == busName),
            Arg.Is<string>(p => p == topicName),
            Arg.Is<BusMessage>(p => p.Metadata == meta && p.Message.Contains(@event.BaseValue)),
            Arg.Any<Dictionary<string, string>>(),
            Arg.Any<CancellationToken>());
    }
}
