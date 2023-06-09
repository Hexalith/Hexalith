// ***********************************************************************
// Assembly         : Hexalith.Infrastructure.MicrosoftSemanticKernel
// Author           : Jérôme Piquot
// Created          : 05-31-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 05-31-2023
// ***********************************************************************
// <copyright file="ChatService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Infrastructure.MicrosoftSemanticKernel.Services;

using System;

// Copyright (c) Microsoft. All rights reserved.
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

using Hexalith.Application.ArtificialIntelligence;
using Hexalith.Infrastructure.MicrosoftSemanticKernel.Skills.ChatSkills;

using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Orchestration;
using Microsoft.SemanticKernel.Reliability;
using Microsoft.SemanticKernel.SkillDefinition;
using Microsoft.SemanticKernel.Skills.MsGraph;
using Microsoft.SemanticKernel.Skills.MsGraph.Connectors;
using Microsoft.SemanticKernel.Skills.MsGraph.Connectors.Client;
using Microsoft.SemanticKernel.Skills.OpenAPI.Authentication;

/// <summary>
/// Controller responsible for handling chat messages and responses.
/// </summary>
public class ChatService : IChatService
{
    /// <summary>
    /// The chat function name.
    /// </summary>
    private const string _chatFunctionName = "Chat";

    /// <summary>
    /// The chat skill name.
    /// </summary>
    private const string _chatSkillName = "ChatSkill";

    /// <summary>
    /// The disposables.
    /// </summary>
    private readonly List<IDisposable> _disposables;

    /// <summary>
    /// The kernel.
    /// </summary>
    private readonly IKernel _kernel;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<ChatService> _logger;

    /// <summary>
    /// The planner.
    /// </summary>
    private readonly CopilotChatPlanner _planner;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatService" /> class.
    /// </summary>
    /// <param name="kernel">The kernel.</param>
    /// <param name="planner">The planner.</param>
    /// <param name="logger">The logger.</param>
    public ChatService(IKernel kernel, CopilotChatPlanner planner, ILogger<ChatService> logger)
    {
        _kernel = kernel;
        _planner = planner;
        _logger = logger;
        _disposables = new List<IDisposable>();
    }

    /// <summary>
    /// Chat as an asynchronous operation.
    /// </summary>
    /// <param name="ask">The ask.</param>
    /// <param name="openApiAuthentication">The open API skills authentication headers.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;Answer&gt; representing the asynchronous operation.</returns>
    public async Task<Answer> ChatAsync(Ask ask, OpenApiAuthentication openApiAuthentication, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Chat request received.");

        // Put ask's variables in the context we will use.
        ContextVariables contextVariables = new(ask.Input);
        foreach (KeyValuePair<string, string> input in ask.Variables)
        {
            contextVariables.Set(input.Key, input.Value);
        }

        // Register plugins that have been enabled
        await RegisterPlannerSkillsAsync(_planner, openApiAuthentication, contextVariables);

        // Get the function to invoke
        ISKFunction? function = null;
        function = _kernel.Skills.GetFunction(_chatSkillName, _chatFunctionName);

        // Run the function.
        SKContext result = await _kernel.RunAsync(contextVariables, function!);
        return new Answer { Value = result.Result, Variables = result.Variables.Select(v => new KeyValuePair<string, string>(v.Key, v.Value)) };
    }

    /// <summary>
    /// Create a Microsoft Graph service client.
    /// </summary>
    /// <param name="authenticateRequestAsyncDelegate">The delegate to authenticate the request.</param>
    /// <returns>GraphServiceClient.</returns>
    private GraphServiceClient CreateGraphServiceClient(AuthenticateRequestAsyncDelegate authenticateRequestAsyncDelegate)
    {
        MsGraphClientLoggingHandler graphLoggingHandler = new(_logger);
        _disposables.Add(graphLoggingHandler);

        IList<DelegatingHandler> graphMiddlewareHandlers =
            GraphClientFactory.CreateDefaultHandlers(new DelegateAuthenticationProvider(authenticateRequestAsyncDelegate));
        graphMiddlewareHandlers.Add(graphLoggingHandler);

        HttpClient graphHttpClient = GraphClientFactory.Create(graphMiddlewareHandlers);
        _disposables.Add(graphHttpClient);

        GraphServiceClient graphServiceClient = new(graphHttpClient);
        return graphServiceClient;
    }

    /// <summary>
    /// Register skills with the planner's kernel.
    /// </summary>
    /// <param name="planner">The planner.</param>
    /// <param name="openApiAuthentication">The open API skills authentication headers.</param>
    /// <param name="variables">The variables.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    private async Task RegisterPlannerSkillsAsync(CopilotChatPlanner planner, OpenApiAuthentication openApiAuthentication, ContextVariables variables)
    {
        const string klarnaSkill = "KlarnaShoppingSkill";
        const string githubSkill = "GitHubSkill";
        const string jiraSkill = "JiraSkill";
        const string msGraphSkill = "MsGraphSkill";

        // Register authenticated skills with the planner's kernel only if the request includes an auth header for the skill.

        // Klarna Shopping
        if (openApiAuthentication.TryGetValue(klarnaSkill, out string? klarna))
        {
            // Register the Klarna shopping ChatGPT plugin with the planner's kernel.
            using DefaultHttpRetryHandler retryHandler = new(new HttpRetryConfig(), _logger)
            {
                InnerHandler = new HttpClientHandler() { CheckCertificateRevocationList = true },
            };
            using HttpClient importHttpClient = new(retryHandler, false);
            importHttpClient.DefaultRequestHeaders.Add("User-Agent", "Microsoft.CopilotChat");
            _ = await planner.Kernel.ImportChatGptPluginSkillFromUrlAsync(klarna, new Uri("https://www.klarna.com/.well-known/ai-plugin.json"),
                importHttpClient);
        }

        // GitHub
        if (openApiAuthentication.TryGetValue(githubSkill, out string? github))
        {
            _logger.LogInformation("Enabling GitHub skill.");
            BearerAuthenticationProvider authenticationProvider = new(() => Task.FromResult(github));
            _ = await planner.Kernel.ImportOpenApiSkillFromFileAsync(
                skillName: "GitHubSkill",
                filePath: Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "CopilotChat", "Skills", "OpenApiSkills/GitHubSkill/openapi.json"),
                authCallback: authenticationProvider.AuthenticateRequestAsync);
        }

        // Jira
        if (openApiAuthentication.TryGetValue(jiraSkill, out string? jira))
        {
            _logger.LogInformation("Registering Jira Skill");
            BasicAuthenticationProvider authenticationProvider = new(() => Task.FromResult(jira));
            bool hasServerUrlOverride = variables.Get("jira-server-url", out string serverUrlOverride);

            _ = await planner.Kernel.ImportOpenApiSkillFromFileAsync(
                skillName: "JiraSkill",
                filePath: Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "CopilotChat", "Skills", "OpenApiSkills/JiraSkill/openapi.json"),
                authCallback: authenticationProvider.AuthenticateRequestAsync,
                serverUrlOverride: hasServerUrlOverride ? new Uri(serverUrlOverride) : null);
        }

        // Microsoft Graph
        if (openApiAuthentication.TryGetValue(msGraphSkill, out string? graph))
        {
            _logger.LogInformation("Enabling Microsoft Graph skill(s).");
            BearerAuthenticationProvider authenticationProvider = new(() => Task.FromResult(graph));
            GraphServiceClient graphServiceClient = CreateGraphServiceClient(authenticationProvider.AuthenticateRequestAsync);

            _ = planner.Kernel.ImportSkill(new TaskListSkill(new MicrosoftToDoConnector(graphServiceClient)), "todo");
            _ = planner.Kernel.ImportSkill(new CalendarSkill(new OutlookCalendarConnector(graphServiceClient)), "calendar");
            _ = planner.Kernel.ImportSkill(new EmailSkill(new OutlookMailConnector(graphServiceClient)), "email");
        }
    }
}