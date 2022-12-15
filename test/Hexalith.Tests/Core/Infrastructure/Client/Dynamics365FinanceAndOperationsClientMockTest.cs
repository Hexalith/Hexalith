// <copyright file="Dynamics365FinanceAndOperationsClientMockTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Tests.Core.Infrastructure.Client;

using FluentAssertions;

using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.TestMocks;

public class Dynamics365FinanceAndOperationsClientMockTest
{
    [Fact]
    public void Build_client_with_mocked_response_should_succeed()
    {
        Dynamics365FinanceAndOperationsClientBuilder<DummyEntity> builder = new Dynamics365FinanceAndOperationsClientBuilder<DummyEntity>()
            .WithSettingsValue(new Dynamics365FinanceAndOperationsClientSettings { Company = "CIE", Instance = new Uri("https://test.dynamics.com") });
        _ = builder.HttpClientfactory.SetMockHttpMessageHandler("dummy response");
        _ = builder
            .Invoking(y => y.Build())
            .Should()
            .NotThrow();
    }

    [Fact]
    public void Client_with_application_json_settings_should_succeed()
    {
        Dynamics365FinanceAndOperationsClientBuilder<DummyEntity> builder = new Dynamics365FinanceAndOperationsClientBuilder<DummyEntity>()
            .WithValueFromConfiguration<Dynamics365FinanceAndOperationsClientMockTest>();
        IDynamics365FinanceAndOperationsClient<DummyEntity> result = builder.Build();
        _ = result.Should().NotBeNull();
    }
}