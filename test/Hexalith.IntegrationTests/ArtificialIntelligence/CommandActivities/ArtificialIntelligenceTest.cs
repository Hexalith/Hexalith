// <copyright file="ArtificialIntelligenceTest.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
/*
namespace Hexalith.IntegrationTests.ArtificialIntelligence.CommandActivities;

using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;

using Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;
using Hexalith.Infrastructure.MicrosoftSemanticKernel.Helpers;
using Hexalith.TestMocks;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Planning;
using Microsoft.SemanticKernel.SkillDefinition;

public class ArtificialIntelligenceTest
{
    [Fact]
    public async Task Should_add_a_country_function()
    {
        Microsoft.Extensions.Logging.ILogger<ArtificialIntelligenceTest> logger = new LoggerBuilder<ArtificialIntelligenceTest>()
            .Build();
        Microsoft.Extensions.Options.IOptions<ArtificialIntelligenceServiceSettings> settings = new OptionsBuilder<ArtificialIntelligenceServiceSettings>()
            .WithValueFromConfiguration<ArtificialIntelligenceTest>()
            .Build();

        // Arrange
        AddCountryCommand expectedCommand = new(
            "US",
            "USA",
            1,
            "United States of America",
            "US Dollar",
            "$",
            "USD");
        IKernel kernel = Kernel.Builder
            .WithLogger(logger)
            .Configure(c => c.AddCompletionService(settings.Value))
            .Build();
        SKContext context = kernel.CreateNewContext();
        ISKFunction function = kernel.ImportCommandSkill<AddCountryCommand>();
        context["AssistantEmail"] = "hexai@hexalith.com";
        context["AssistantName"] = "Hexai";
        context["UserEmail"] = "jdoe@hexalith.com";
        context["UserName"] = "John Doe";
        function = kernel.Skills.GetFunction(expectedCommand.AggregateName, expectedCommand.TypeName);
        SKContext response = await kernel.RunAsync("Add the USA country", function);
        _ = response.Result.Should().NotBeNullOrWhiteSpace();
        string[] parts = response.Result.Split("__JSON__", 2);
        _ = parts.Length.Should().Be(2);
        string json = parts[1];
        AddCountryCommand aiCommand = JsonSerializer.Deserialize<AddCountryCommand>(json);
        _ = aiCommand.Should().BeEquivalentTo(expectedCommand);
    }

    [Fact]
    public async Task Should_add_a_country_plan()
    {
        Microsoft.Extensions.Logging.ILogger<ArtificialIntelligenceTest> logger = new LoggerBuilder<ArtificialIntelligenceTest>()
            .Build();
        Microsoft.Extensions.Options.IOptions<ArtificialIntelligenceServiceSettings> settings = new OptionsBuilder<ArtificialIntelligenceServiceSettings>()
            .WithValueFromConfiguration<ArtificialIntelligenceTest>()
            .Build();

        // Arrange
        AddCountryCommand expectedCommand = new(
            "DE",
            "DEU",
            276,
            "Germany",
            "Euro",
            "€",
            "EUR");
        IKernel kernel = Kernel.Builder
            .WithLogger(logger)
            .Configure(c => c.AddCompletionService(settings.Value))
            .Build();
        string ask = "Add Germany.";
        _ = kernel.AddSkills();
        SKContext context = kernel.CreateNewContext();
        context["AssistantEmail"] = "hexai@hexalith.com";
        context["AssistantName"] = "Hexai";
        context["UserEmail"] = "jdoe@hexalith.com";
        context["UserName"] = "John Doe";
        ActionPlanner planner = new(kernel);
        Plan plan = await planner.CreatePlanAsync(ask);
        string jsonPlan = plan.ToJson();
        plan = Plan.FromJson(jsonPlan, context);
        int iterations = 0;

        while (plan.HasNextStep &&
               iterations < 100)
        {
            plan = await kernel.StepAsync(context.Variables, plan);
            iterations++;
        }

        _ = plan.State.Input.Should().NotBeNull();
        string input = plan.State.Input;
        string[] parts = input.Split("__JSON__", 2);
        _ = parts.Length.Should().Be(2);
        AddCountryCommand aiCommand = JsonSerializer.Deserialize<AddCountryCommand>(parts[1]);
        _ = aiCommand.Should().BeEquivalentTo(expectedCommand);
    }
}
*/