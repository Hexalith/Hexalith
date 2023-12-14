// <copyright file="Dynamics365FinanceClientTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Dynamics365Finance.Client;

using System.Net;

using FluentAssertions;

using Hexalith.Application.Organizations.Configurations;
using Hexalith.Infrastructure.Dynamics365Finance.Client;
using Hexalith.Infrastructure.Dynamics365Finance.Configurations;
using Hexalith.Infrastructure.Dynamics365Finance.TestMocks;

using Moq;
using Moq.Contrib.HttpClient;

public class Dynamics365FinanceClientTest
{
    [Fact]
    public async Task GetShouldReturnHello()
    {
        Mock<HttpMessageHandler> mockHandler = new(MockBehavior.Strict);
        using HttpClient httpClient = mockHandler.CreateClient();
        Dynamics365FinanceClientSettings settings = new()
        {
            Instance = new Uri("https://test.dynamics.com"),
        };
        OrganizationSettings orgSettings = new()
        {
            DefaultCompanyId = "CIE",
        };
        const string message = "Hello world";
        const string etag = "123etag123";
        const string company = "CIE";
        const string id = "3525";
        DummyEntity dummyEntity = new(etag, company, message);
        string mockUrl = $"https://test.dynamics.com/data/Dummy(dataAreaId%3d%27CIE%27%2cid%3d%273525%27)";
        Moq.Language.Flow.IReturnsResult<HttpMessageHandler> mockResponse = mockHandler
            .SetupRequest(HttpMethod.Get, new Uri(mockUrl))
            .ReturnsJsonResponse(HttpStatusCode.OK, dummyEntity);
        Dynamics365FinanceClientBuilder<DummyEntity> builder = new();
        _ = builder.FinOpsSettings.WithValue(settings);
        _ = builder.OrganizationSettings.WithValue(orgSettings);
        IDynamics365FinanceClient<DummyEntity> client = builder.Build(httpClient);
        DummyEntity result = await client.GetSingleAsync(
            company,
            new Dictionary<string, object>(
            StringComparer.Ordinal)
            { { "id", id } },
            CancellationToken.None);
        _ = result.Should().NotBeNull();
        _ = result.Should().BeEquivalentTo(dummyEntity);
        mockHandler.VerifyRequest(HttpMethod.Get, mockUrl, Times.Once());
        mockHandler.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task PatchShouldSucceed()
    {
        Mock<HttpMessageHandler> mockHandler = new(MockBehavior.Strict);
        using HttpClient httpClient = mockHandler.CreateClient();
        Dynamics365FinanceClientSettings settings = new()
        {
            Instance = new Uri("https://test.dynamics.com"),
        };
        OrganizationSettings orgSettings = new()
        {
            DefaultCompanyId = "CIE",
        };
        DummyEntity dummyEntity = new("123etag123", "CIE", "Hello world");
        string mockUrl = $"https://test.dynamics.com/data/Dummy(dataAreaId%3d%27CIE%27%2cid%3d%273525%27)";
        Moq.Language.Flow.IReturnsResult<HttpMessageHandler> mockResponse = mockHandler
            .SetupRequest(HttpMethod.Patch, new Uri(mockUrl))
            .ReturnsResponse(HttpStatusCode.OK);

        Dynamics365FinanceClientBuilder<DummyEntity> builder = new();
        _ = builder.FinOpsSettings.WithValue(settings);
        _ = builder.OrganizationSettings.WithValue(orgSettings);
        IDynamics365FinanceClient<DummyEntity> client = builder.Build(httpClient);
        HttpResponseMessage response = await client.SendPatchAsync(
            new Dictionary<string, object>(
            StringComparer.Ordinal)
            { { "id", "3525" } },
            dummyEntity,
            CancellationToken.None);
        _ = response.IsSuccessStatusCode.Should().BeTrue();
        mockHandler.VerifyRequest(HttpMethod.Patch, mockUrl, Times.Once());
        mockHandler.VerifyNoOtherCalls();
    }
}