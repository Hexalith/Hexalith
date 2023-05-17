// <copyright file="ArtificialIntelligenceTest.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.IntegrationTests.ArtificialIntelligence.CommandActivities;

using System.Text.Json;
using System.Threading.Tasks;

using FluentAssertions;

using Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;
using Hexalith.Infrastructure.MicrosoftSemanticKernel.Helpers;
using Hexalith.Infrastructure.MicrosoftSemanticKernel.Prompts;
using Hexalith.TestMocks;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Planning;
using Microsoft.SemanticKernel.Planning.Sequential;

public class ArtificialIntelligenceTest
{
    [Fact]
    public async Task Should_add_a_country()
    {
        Microsoft.Extensions.Logging.ILogger<ArtificialIntelligenceTest> logger = new LoggerBuilder<ArtificialIntelligenceTest>()
            .Build();
        Microsoft.Extensions.Options.IOptions<ArtificialIntelligenceServiceSettings> settings = new OptionsBuilder<ArtificialIntelligenceServiceSettings>()
            .WithValueFromConfiguration<ArtificialIntelligenceTest>()
            .Build();

        // Arrange
        AddCountryCommand expectedCommand = new(
            "FR",
            "FRA",
            250,
            "France",
            "Euro",
            "€");
        CommandPromptGenerator prompter = new();
        IKernel kernel = Kernel.Builder
            .WithLogger(logger)
            .Configure(c => c.AddCompletionService(settings.Value))
            .Build();
        _ = kernel.ImportCommandSkill(expectedCommand);
        string ask = "Add the french country.";
        SKContext context = kernel.CreateNewContext();
        context["AssistantEmail"] = "hexai@hexalith.com";
        context["AssistantName"] = "Hexai";
        context["UserEmail"] = "jdoe@hexalith.com";
        context["UserName"] = "John Doe";
        SequentialPlanner planner = new(kernel, new SequentialPlannerConfig() { MaxTokens = 2000 });
        Plan plan = await planner.CreatePlanAsync(ask);
        string jsonPlan = plan.ToJson();
        plan = Plan.FromJson(jsonPlan, context);
        SKContext response = await kernel.RunAsync(plan);
        _ = response.Result.Should().NotBeNullOrWhiteSpace();
        AddCountryCommand aiCommand = JsonSerializer.Deserialize<AddCountryCommand>(response.Result);
        _ = aiCommand.Should().BeEquivalentTo(expectedCommand);
    }
}