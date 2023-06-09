// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="ExternalInformationSkill.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Skills.ChatSkills;

using System;

// Copyright (c) Microsoft. All rights reserved.
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Hexalith.Infrastructure.MicrosoftSemanticKernel.Configurations;
using Hexalith.Infrastructure.MicrosoftSemanticKernel.Skills.OpenApiSkills.GitHubSkill.Model;
using Hexalith.Infrastructure.MicrosoftSemanticKernel.Skills.OpenApiSkills.JiraSkill.Model;

using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Planning;
using Microsoft.SemanticKernel.Security;
using Microsoft.SemanticKernel.SkillDefinition;

/// <summary>
/// This skill provides the functions to acquire external information.
/// </summary>
public partial class ExternalInformationSkill
{
    /// <summary>
    /// Postamble to add to the related information text.
    /// </summary>
    private const string PromptPostamble = "[RELATED END]";

    /// <summary>
    /// Preamble to add to the related information text.
    /// </summary>
    private const string PromptPreamble = "[RELATED START]";

    /// <summary>
    /// CopilotChat's planner to gather additional information for the chat context.
    /// </summary>
    private readonly CopilotChatPlanner _planner;

    /// <summary>
    /// Prompt settings.
    /// </summary>
    private readonly PromptsSettings _promptOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExternalInformationSkill"/> class.
    /// Create a new instance of ExternalInformationSkill.
    /// </summary>
    public ExternalInformationSkill(
        PromptsSettings promptOptions,
        CopilotChatPlanner planner)
    {
        _promptOptions = promptOptions;
        _planner = planner;
    }

    /// <summary>
    /// Gets proposed plan to return for approval.
    /// </summary>
    public Plan? ProposedPlan { get; private set; }

    /// <summary>
    /// Extract relevant additional knowledge using a planner.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [SKFunction("Acquire external information")]
    [SKFunctionName("AcquireExternalInformation")]
    [SKFunctionInput(Description = "The intent to whether external information is needed")]
    [SKFunctionContextParameter(Name = "tokenLimit", Description = "Maximum number of tokens")]
    [SKFunctionContextParameter(Name = "proposedPlan", Description = "Previously proposed plan that is approved")]
    public async Task<string> AcquireExternalInformationAsync(string userIntent, SKContext context)
    {
        FunctionsView functions = _planner.Kernel.Skills.GetFunctionsView(true, true);
        if (functions.NativeFunctions.IsEmpty && functions.SemanticFunctions.IsEmpty)
        {
            return string.Empty;
        }

        // Check if plan exists in ask's context variables.
        // If plan was returned at this point, that means it was approved and should be run
        bool planApproved = context.Variables.TryGetValue("proposedPlan", out TrustAwareString? planJson);

        if (planApproved && planJson is not null)
        {
            // Reload the plan with the planner's kernel so
            // it has full context to be executed
            SKContext newPlanContext = new(
                null,
                _planner.Kernel.Memory,
                _planner.Kernel.Skills,
                _planner.Kernel.Log);
            Plan plan = Plan.FromJson(planJson, newPlanContext);

            // Invoke plan
            newPlanContext = await plan.InvokeAsync(newPlanContext);
            int tokenLimit =
                int.Parse(context["tokenLimit"], new NumberFormatInfo()) -
                Utilities.TokenCount(PromptPreamble) -
                Utilities.TokenCount(PromptPostamble);

            // The result of the plan may be from an OpenAPI skill. Attempt to extract JSON from the response.
            bool extractJsonFromOpenApi =
                TryExtractJsonFromOpenApiPlanResult(newPlanContext, newPlanContext.Result, out string planResult);
            if (extractJsonFromOpenApi)
            {
                planResult = OptimizeOpenApiSkillJson(planResult, tokenLimit, plan);
            }
            else
            {
                // If not, use result of the plan execution result directly.
                planResult = newPlanContext.Variables.Input;
            }

            return $"{PromptPreamble}\n{planResult.Trim()}\n{PromptPostamble}\n";
        }
        else
        {
            // Create a plan and set it in context for approval.
            string contextString = string.Join("\n", context.Variables.Where(v => v.Key != "userIntent").Select(v => $"{v.Key}: {v.Value}"));
            Plan plan = await _planner.CreatePlanAsync($"Given the following context, accomplish the user intent.\nContext:{contextString}\nUser Intent:{userIntent}");

            if (plan.Steps.Count > 0)
            {
                // Merge any variables from the context into plan's state
                // as these will be used on plan execution.
                // These context variables come from user input, so they are prioritized.
                ContextVariables variables = context.Variables;
                foreach (KeyValuePair<string, Microsoft.SemanticKernel.Security.TrustAwareString> param in plan.State)
                {
                    if (param.Key.Equals("INPUT", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (variables.TryGetValue(param.Key, out TrustAwareString? value))
                    {
                        plan.State.Set(param.Key, value);
                    }
                }

                ProposedPlan = plan;
            }
        }

        return string.Empty;
    }

    [GeneratedRegex("[\\n\\r]")]
    private static partial Regex MyRegex();

    private Type GetGithubSkillResponseType(ref JsonDocument document) => document.RootElement.ValueKind == JsonValueKind.Array ? typeof(PullRequest[]) : typeof(PullRequest);

    private Type GetJiraSkillResponseType(ref JsonDocument document, ref string lastSkillFunctionInvoked)
    {
        return lastSkillFunctionInvoked == "GetIssue"
            ? document.RootElement.ValueKind == JsonValueKind.Array ? typeof(IssueResponse[]) : typeof(IssueResponse)
            : typeof(IssueResponse);
    }

    private Type GetOpenApiSkillResponseType(ref JsonDocument document, ref string lastSkillInvoked, ref string lastSkillFunctionInvoked, ref bool trimSkillResponse)
    {
        Type skillResponseType = typeof(object); // Use a reasonable default response type

        // Different operations under the skill will return responses as json structures;
        // Prune each operation response according to the most important/contextual fields only to avoid going over the token limit
        // Check what the last skill invoked was and deserialize the JSON content accordingly
        if (string.Equals(lastSkillInvoked, "GitHubSkill", StringComparison.Ordinal))
        {
            trimSkillResponse = true;
            skillResponseType = GetGithubSkillResponseType(ref document);
        }
        else if (string.Equals(lastSkillInvoked, "JiraSkill", StringComparison.Ordinal))
        {
            trimSkillResponse = true;
            skillResponseType = GetJiraSkillResponseType(ref document, ref lastSkillFunctionInvoked);
        }

        return skillResponseType;
    }

    /// <summary>
    /// Try to optimize json from the planner response
    /// based on token limit.
    /// </summary>
    private string OptimizeOpenApiSkillJson(string jsonContent, int tokenLimit, Plan plan)
    {
        // Remove all new line characters + leading and trailing white space
        jsonContent = MyRegex().Replace(jsonContent.Trim(), string.Empty);
        JsonDocument document = JsonDocument.Parse(jsonContent);
        string lastSkillInvoked = plan.Steps[^1].SkillName;
        string lastSkillFunctionInvoked = plan.Steps[^1].Name;
        bool trimSkillResponse = false;

        // The json will be deserialized based on the response type of the particular operation that was last invoked by the planner
        // The response type can be a custom trimmed down json structure, which is useful in staying within the token limit
        Type skillResponseType = GetOpenApiSkillResponseType(ref document, ref lastSkillInvoked, ref lastSkillFunctionInvoked, ref trimSkillResponse);

        if (trimSkillResponse)
        {
            // Deserializing limits the json content to only the fields defined in the respective OpenApiSkill's Model classes
            object? skillResponse = JsonSerializer.Deserialize(jsonContent, skillResponseType);
            jsonContent = skillResponse != null ? JsonSerializer.Serialize(skillResponse) : string.Empty;
            document = JsonDocument.Parse(jsonContent);
        }

        int jsonContentTokenCount = Utilities.TokenCount(jsonContent);

        // Return the JSON content if it does not exceed the token limit
        if (jsonContentTokenCount < tokenLimit)
        {
            return jsonContent;
        }

        List<object> itemList = new();

        // Some APIs will return a JSON response with one property key representing an embedded answer.
        // Extract this value for further processing
        string resultsDescriptor = string.Empty;

        if (document.RootElement.ValueKind == JsonValueKind.Object)
        {
            int propertyCount = 0;
            foreach (JsonProperty property in document.RootElement.EnumerateObject())
            {
                propertyCount++;
            }

            if (propertyCount == 1)
            {
                // Save property name for result interpolation
                JsonProperty firstProperty = document.RootElement.EnumerateObject().First();
                tokenLimit -= Utilities.TokenCount(firstProperty.Name);
                resultsDescriptor = string.Format(CultureInfo.InvariantCulture, "{0}: ", firstProperty.Name);

                // Extract object to be truncated
                JsonElement value = firstProperty.Value;
                document = JsonDocument.Parse(value.GetRawText());
            }
        }

        // Detail Object
        // To stay within token limits, attempt to truncate the list of properties
        if (document.RootElement.ValueKind == JsonValueKind.Object)
        {
            foreach (JsonProperty property in document.RootElement.EnumerateObject())
            {
                int propertyTokenCount = Utilities.TokenCount(property.ToString());

                if (tokenLimit - propertyTokenCount > 0)
                {
                    itemList.Add(property);
                    tokenLimit -= propertyTokenCount;
                }
                else
                {
                    break;
                }
            }
        }

        // Summary (List) Object
        // To stay within token limits, attempt to truncate the list of results
        if (document.RootElement.ValueKind == JsonValueKind.Array)
        {
            foreach (JsonElement item in document.RootElement.EnumerateArray())
            {
                int itemTokenCount = Utilities.TokenCount(item.ToString());

                if (tokenLimit - itemTokenCount > 0)
                {
                    itemList.Add(item);
                    tokenLimit -= itemTokenCount;
                }
                else
                {
                    break;
                }
            }
        }

        return itemList.Count > 0
            ? string.Format(CultureInfo.InvariantCulture, "{0}{1}", resultsDescriptor, JsonSerializer.Serialize(itemList))
            : string.Format(CultureInfo.InvariantCulture, "JSON response for {0} is too large to be consumed at this time.", lastSkillInvoked);
    }

    /// <summary>
    /// Try to extract json from the planner response as if it were from an OpenAPI skill.
    /// </summary>
    private bool TryExtractJsonFromOpenApiPlanResult(SKContext context, string openApiSkillResponse, out string json)
    {
        try
        {
            JsonNode? jsonNode = JsonNode.Parse(openApiSkillResponse);
            string contentType = jsonNode?["contentType"]?.ToString() ?? string.Empty;
            if (contentType.StartsWith("application/json", StringComparison.InvariantCultureIgnoreCase))
            {
                string content = jsonNode?["content"]?.ToString() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(content))
                {
                    json = content;
                    return true;
                }
            }
        }
        catch (JsonException)
        {
            context.Log.LogDebug("Unable to extract JSON from planner response, it is likely not from an OpenAPI skill.");
        }
        catch (InvalidOperationException)
        {
            context.Log.LogDebug("Unable to extract JSON from planner response, it may already be proper JSON.");
        }

        json = string.Empty;
        return false;
    }
}