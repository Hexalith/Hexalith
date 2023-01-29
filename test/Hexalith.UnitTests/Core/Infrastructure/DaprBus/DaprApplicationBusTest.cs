// <copyright file="DaprApplicationBusTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.DaprBus;

using Dapr.Client;

using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Domain.Abstractions.Events;
using Hexalith.Extensions.Helpers;
using Hexalith.Infrastructure.DaprBus;
using Hexalith.UnitTests.Core.Application.Metadatas;
using Hexalith.UnitTests.Core.Domain.Events;
using Microsoft.Extensions.Logging;

using Moq;

public class DaprApplicationBusTest
{
    [Fact]
    public async Task Verify_publish_calls_dapr_publish_with_compliant_values()
    {
        const string busName = "TestBus";
        const string topicSuffix = "-event";
        DummyEvent2 @event = new("hello test base", 123456);
        DummyMetadata meta = new(@event);

        Mock<DaprClient> client = new();
        Mock<ILogger> logger = new();
        DaprApplicationBus<BaseEvent, Metadata> bus = new(client.Object, busName, topicSuffix, logger.Object);
        await bus.PublishAsync(@event, meta, CancellationToken.None);
        client.Verify(
            p => p.PublishEventAsync(
            It.Is<string>(p => p == busName),
            It.Is<string>(p => p == @event.AggregateName + topicSuffix),
            It.Is<string>(
                p => p.Contains("$type", StringComparison.InvariantCulture) &&
                p.Contains(nameof(DummyEvent2), StringComparison.InvariantCulture) &&
                p.Contains(123456.ToInvariantString(), StringComparison.InvariantCulture) &&
                p.Contains("hello test base", StringComparison.InvariantCulture)),
            It.IsAny<Dictionary<string, string>>(),
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
}