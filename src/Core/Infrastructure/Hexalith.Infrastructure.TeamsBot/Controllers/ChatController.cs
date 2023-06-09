// <copyright file="ChatController.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.TeamsBot.Controllers;

using System;

// Copyright (c) Microsoft. All rights reserved.
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

using Hexalith.Application.ArtificialIntelligence;
using Hexalith.Infrastructure.TeamsBot.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

/// <summary>
/// Controller responsible for handling chat messages and responses.
/// </summary>
[ApiController]
public class ChatController : ControllerBase, IDisposable
{
    private const string ChatFunctionName = "Chat";
    private const string ChatSkillName = "ChatSkill";
    private readonly List<IDisposable> _disposables;
    private readonly ILogger<ChatController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatController"/> class.
    /// </summary>
    /// <param name="logger"></param>
    public ChatController(ILogger<ChatController> logger)
    {
        _logger = logger;
        _disposables = new List<IDisposable>();
    }

    /// <summary>
    /// Invokes the chat skill to get a response from the bot.
    /// </summary>
    /// <param name="kernel">Semantic kernel obtained through dependency injection.</param>
    /// <param name="planner">Planner to use to create function sequences.</param>
    /// <param name="plannerOptions">Options for the planner.</param>
    /// <param name="ask">Prompt along with its parameters.</param>
    /// <param name="openApiSkillsAuthHeaders">Authentication headers to connect to OpenAPI Skills.</param>
    /// <returns>Results containing the response from the model.</returns>
    [Authorize]
    [Route("chat")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChatAsync(
        [FromServices] IChatService chatService,
        [FromBody] Ask ask,
        [FromHeader] OpenApiSkillsAuthHeaders openApiSkillsAuthHeaders)
    {
        _logger.LogDebug("Chat request received.");
        Answer answer;
        answer = await chatService.ChatAsync(ask, openApiSkillsAuthHeaders.GetAuthentication(), CancellationToken.None);
        return Ok(answer);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose of the object.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (IDisposable disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }

    /// <summary>
    /// Create a Microsoft Graph service client.
    /// </summary>
    /// <param name="authenticateRequestAsyncDelegate">The delegate to authenticate the request.</param>
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
    private async Task RegisterPlannerSkillsAsync(CopilotChatPlanner planner, OpenApiSkillsAuthHeaders openApiSkillsAuthHeaders, ContextVariables variables)
    {
        // Register authenticated skills with the planner's kernel only if the request includes an auth header for the skill.

        // Klarna Shopping
        if (openApiSkillsAuthHeaders.KlarnaAuthentication != null)
        {
            // Register the Klarna shopping ChatGPT plugin with the planner's kernel.
            using DefaultHttpRetryHandler retryHandler = new(new HttpRetryConfig(), _logger)
            {
                InnerHandler = new HttpClientHandler() { CheckCertificateRevocationList = true },
            };
            using HttpClient importHttpClient = new(retryHandler, false);
            importHttpClient.DefaultRequestHeaders.Add("User-Agent", "Microsoft.CopilotChat");
            _ = await planner.Kernel.ImportChatGptPluginSkillFromUrlAsync("KlarnaShoppingSkill", new Uri("https://www.klarna.com/.well-known/ai-plugin.json"),
                importHttpClient);
        }

        // GitHub
        if (!string.IsNullOrWhiteSpace(openApiSkillsAuthHeaders.GithubAuthentication))
        {
            _logger.LogInformation("Enabling GitHub skill.");
            BearerAuthenticationProvider authenticationProvider = new(() => Task.FromResult(openApiSkillsAuthHeaders.GithubAuthentication));
            _ = await planner.Kernel.ImportOpenApiSkillFromFileAsync(
                skillName: "GitHubSkill",
                filePath: Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "CopilotChat", "Skills", "OpenApiSkills/GitHubSkill/openapi.json"),
                authCallback: authenticationProvider.AuthenticateRequestAsync);
        }

        // Jira
        if (!string.IsNullOrWhiteSpace(openApiSkillsAuthHeaders.JiraAuthentication))
        {
            _logger.LogInformation("Registering Jira Skill");
            BasicAuthenticationProvider authenticationProvider = new(() => Task.FromResult(openApiSkillsAuthHeaders.JiraAuthentication));
            bool hasServerUrlOverride = variables.Get("jira-server-url", out string serverUrlOverride);

            _ = await planner.Kernel.ImportOpenApiSkillFromFileAsync(
                skillName: "JiraSkill",
                filePath: Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "CopilotChat", "Skills", "OpenApiSkills/JiraSkill/openapi.json"),
                authCallback: authenticationProvider.AuthenticateRequestAsync,
                serverUrlOverride: hasServerUrlOverride ? new Uri(serverUrlOverride) : null);
        }

        // Microsoft Graph
        if (!string.IsNullOrWhiteSpace(openApiSkillsAuthHeaders.GraphAuthentication))
        {
            _logger.LogInformation("Enabling Microsoft Graph skill(s).");
            BearerAuthenticationProvider authenticationProvider = new(() => Task.FromResult(openApiSkillsAuthHeaders.GraphAuthentication));
            GraphServiceClient graphServiceClient = this.CreateGraphServiceClient(authenticationProvider.AuthenticateRequestAsync);

            _ = planner.Kernel.ImportSkill(new TaskListSkill(new MicrosoftToDoConnector(graphServiceClient)), "todo");
            _ = planner.Kernel.ImportSkill(new CalendarSkill(new OutlookCalendarConnector(graphServiceClient)), "calendar");
            _ = planner.Kernel.ImportSkill(new EmailSkill(new OutlookMailConnector(graphServiceClient)), "email");
        }
    }
}