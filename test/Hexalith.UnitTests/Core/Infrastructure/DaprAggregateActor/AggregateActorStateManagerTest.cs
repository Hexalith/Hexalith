namespace Hexalith.UnitTests.Core.Infrastructure.DaprAggregateActor;

using Dapr.Actors.Runtime;

using Hexalith.Application.Abstractions.Commands;
using Hexalith.Application.Abstractions.Metadatas;
using Hexalith.Extensions.Common;
using Hexalith.Infrastructure.DaprAggregateActor;
using Hexalith.UnitTests.Core.Domain;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class AggregateActorStateManagerTest
{
    const string _json =
        $$"""
        [
            {
                "message": {
                    "establishedDate": "2023-01-25T12:13:28.5020742+00:00",
                    "paymentAccount": "PayCust",
                    "paymentMode": {
                        "id": "PAY",
                        "bankAccount": {
                            "iban": "IB123456789"
                        },
                        "directDebitMandate": {
                            "id": "123456789",
                            "creditorBankAccount": "SGBANK"
                        },
                        "paymentTerm": "13M"
                    },
                    "companyId": "SOL",
                    "id": "TEST-5-1",
                    "$type": "EstablishContract",
                    "customerAccount": "TesTCust"
                },
                "processedDate": null,
                "idempotencyId": "GIsuKVJmDk6JWAkSG-wy1g",
                "metadata": {
                    "context": {
                        "correlationId": "uldCqDzzCUy_Dg1oCOAmAw",
                        "receivedDate": "2023-01-25T12:13:28.5020742+00:00",
                        "sequenceNumber": null,
                        "sessionId": null,
                        "userId": "test"
                    },
                    "message": {
                        "id": "GIsuKVJmDk6JWAkSG-wy1g",
                        "name": "EstablishContract",
                        "version": {
                            "major": 0,
                            "minor": 0
                        },
                        "aggregate": {
                            "id": "SOL-TEST-5-1",
                            "name": "Contract"
                        },
                        "date": "2023-01-28T07:26:14.5542661+00:00"
                    },
                    "scopes": [],
                    "version": {
                        "major": 0,
                        "minor": 0
                    }
                },
                "receivedDate": "2023-01-28T07:26:14.7363923+00:00"
            }
        ]
        """;
    [Fact]
    public async Task Try_get_state_should_succeed()
    {
        var todo = JsonSerializer.Deserialize<List<BaseCommand>>(_json);
        var command = todo.First();
        
    }
    [Fact]
    public async Task Initialize_should_succeed()
    {
        var command = new DummyCommand1("Test", 123456);
        var meta = new Metadata(
                "TEST123456",
                command,
                DateTimeOffset.UtcNow,
                new ContextMetadata(
                    "COR123",
                    "TestUser",
                    null,
                    null,
                    null),
                null);
        var state = new CommandState(
            DateTimeOffset.UtcNow,
            "TEST123456",
            command,
            meta,
            null);
        var actorStateManager = new Mock<IActorStateManager>();
        actorStateManager
            .Setup(p => p.TryGetStateAsync<CommandState>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dapr.Actors.Runtime.ConditionalValue<CommandState>(true, state));
        var dispatcher = new Mock<ICommandDispatcher>();
        var storeProvider = new ActorStateStoreProvider(actorStateManager.Object);
        var stateManager = new AggregateActorStateManager(
            dispatcher.Object,
            new DateTimeService());
        await stateManager.InitializeAsync(
            storeProvider,
            async (name, data, start, period)
               => await Task.CompletedTask,
            CancellationToken.None);
    }
}