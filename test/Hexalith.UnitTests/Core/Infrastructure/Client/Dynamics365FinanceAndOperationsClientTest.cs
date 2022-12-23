// <copyright file="Dynamics365FinanceAndOperationsClientTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Client;

using FluentAssertions;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.TestMocks;

using System.Text.Json;

public class Dynamics365FinanceAndOperationsClientTest
{
    [Fact]
    public async Task Get_should_return_hello()
    {
        Dynamics365FinanceAndOperationsClientSettings settings = new()
        {
            Company = "CIE",
            Instance = new Uri("https://test.dynamics.com"),
        };
        const string message = "Hello world";
        const string etag = "123etag123";
        const string company = "CIE";
        const string response =
            $$"""
            {
                "@odata.etag":"{{etag}}",
                "dataAreaId":"{{company}}",
                "Message":"{{message}}"
            }
            """;
        Dynamics365FinanceAndOperationsClientBuilder<DummyEntity> builder = new();
        _ = builder.Settings.WithValue(settings);
        _ = builder.HttpClientfactory.SetMockHttpMessageHandler(response);
        IDynamics365FinanceAndOperationsClient<DummyEntity> client = builder.Build();
        DummyEntity result = await client.GetSingleAsync(
            company,
            new Dictionary<string, object>(
            StringComparer.Ordinal)
            { { "id", "3525" } },
            CancellationToken.None);
        _ = result.Message.Should().Be(message);
        _ = result.DataAreaId.Should().Be(company);
        _ = result.Etag.Should().Be(etag);
    }

    [Fact]
    public async Task Patch_should_succeed()
    {
        Dynamics365FinanceAndOperationsClientSettings settings = new()
        {
            Company = "CIE",
            Instance = new Uri("https://test.dynamics.com"),
        };
        DummyEntity dummy = new("123etag123", "CIE", "Hello world");

        Dynamics365FinanceAndOperationsClientBuilder<DummyEntity> builder = new();
        _ = builder.Settings.WithValue(settings);
        _ = builder.HttpClientfactory.SetMockHttpMessageHandler(JsonSerializer.Serialize(dummy));
        IDynamics365FinanceAndOperationsClient<DummyEntity> client = builder.Build();
        HttpResponseMessage response = await client.SendPatchAsync(
            new Dictionary<string, object>(
            StringComparer.Ordinal)
            { { "id", "3525" } },
            dummy,
            CancellationToken.None);
        _ = response.IsSuccessStatusCode.Should().BeTrue();
    }
}
