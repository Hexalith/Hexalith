namespace Hexalith.Infrastructure.BlazorClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application;
    using Hexalith.Application.Client.Exceptions;
    using Hexalith.Application.Commands;
    using Hexalith.Application.Queries;
    using Hexalith.Infrastructure.Helpers;

    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.WebUtilities;

    public class HexalithHttpClient : IQueryService, ICommandService
    {
        public HexalithHttpClient(HttpClient httpClient, NavigationManager navigationManager)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            NavigationManager = navigationManager;
        }

        public HttpClient HttpClient { get; }
        public NavigationManager NavigationManager { get; }

        public async Task<TResult> Ask<TQuery, TResult>(TQuery query, string? messageId = null, CancellationToken cancellationToken = default)
            where TQuery : class
        {
            var queryType = typeof(TQuery);
            try
            {
                var queryList = new Dictionary<string, string>(
                    query
                    .GetPropertyNotNullValues()
                    .Select(p => new KeyValuePair<string, string>(p.Key, JsonSerializer.Serialize(p.Value))))
                {
                    // { "MessageId", string.IsNullOrWhiteSpace(messageId) ? new MessageId() :
                    // messageId }
                };

                var url = QueryHelpers.AddQueryString($"api/ask/{queryType.Name.ToLowerInvariant()}/", queryList);
                var result = await HttpClient
                    .GetFromJsonAsync<TResult>(url, cancellationToken);
                if (result == null)
                {
                    throw new QueryResultNullException(query);
                }
                return result;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Handle 404
                Console.WriteLine($"Query handler for '{queryType.Name}' not found: " + ex.Message);
                throw;
            }
        }

        public async Task Tell<TCommand>(TCommand command, string? messageId = null, CancellationToken cancellationToken = default) where TCommand : class
        {
            var commandType = typeof(TCommand);
            try
            {
                await HttpClient.PostAsJsonAsync($"api/tell/{commandType.Name.ToLowerInvariant()}/?MessageId={messageId}", command?.Json(), cancellationToken);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Handle 404
                Console.WriteLine($"Query handler for '{commandType.Name}' not found: " + ex.Message);
                throw;
            }
        }
    }
}