// <copyright file="Dynamics365FinanceAndOperationsClientTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Tests.Core.Infrastructure.Client;

using FluentAssertions;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.TestMocks;

public class Dynamics365FinanceAndOperationsClientTest
{
    [Fact]
    public async Task Get_should_return_helloAsync()
    {
        Dynamics365FinanceAndOperationsClientSettings settings = new()
        {
            Company = "CIE",
            Instance = new Uri("https://test.dynamics.com"),
        };
        const string message = "Hello world";
        const string response =
            $$"""
            {
                "@odata.context":"hello context",
                "message":"this is a message",
                "Message":"{{message}}"
            }
            """;
        Dynamics365FinanceAndOperationsClientBuilder builder = new();
        _ = builder.Settings.WithValue(settings);
        _ = builder.HttpClientfactory.SetMockHttpMessageHandler(response);
        IDynamics365FinanceAndOperationsClient client = builder.Build();
        Hello result = await client.GetSingleAsync<Hello>(
            "hello",
            new Dictionary<string, object>(
            StringComparer.Ordinal)
            { { "id", "3525" } },
            CancellationToken.None);
        _ = result.Message.Should().Be(message);
    }

    public class Hello
    {
        public string? Message { get; init; }
    }
}