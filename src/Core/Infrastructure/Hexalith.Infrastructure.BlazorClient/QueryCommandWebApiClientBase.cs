namespace Hexalith.Infrastructure.BlazorClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using System.Web;

    using Hexalith.Domain.ValueTypes;

    public abstract class QueryCommandWebApiClientBase
    {
        protected QueryCommandWebApiClientBase(HttpClient httpClient, string moduleName)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            ModuleName = moduleName;
        }

        public HttpClient HttpClient { get; }
        public string ModuleName { get; }

        public async Task<TResult?> Ask<TResult>(string queryName, IEnumerable<KeyValuePair<string, string?>> queryValues, string? messageId = null)
        {
            try
            {
                var queryString = string.Join
                (
                    '&',
                    queryValues
                        .OrderBy(p => p.Key)
                        .Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}")
                );
                if (!string.IsNullOrWhiteSpace(queryString))
                {
                    queryString = '&' + queryString;
                }
                MessageId msgId = string.IsNullOrWhiteSpace(messageId) ? new MessageId() : new MessageId(messageId);
                return await HttpClient
                    .GetFromJsonAsync<TResult>(
                        $"api/{ModuleName}/{queryName}/?MessageId={msgId.Value}" + queryString
                        );
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Handle 404
                Console.WriteLine($"Api call for query '{queryName}' failed with not found error : " + ex.Message);
                throw;
            }
        }

        public async Task Tell<TCommand>(TCommand command, string? messageId = null) where TCommand : class
        {
            try
            {
                MessageId msgId = string.IsNullOrWhiteSpace(messageId) ? new MessageId() : new MessageId(messageId);
                await HttpClient
                    .PostAsJsonAsync(
                        $"api/{ModuleName}/{typeof(TCommand).Name}/?MessageId={msgId.Value}",
                        command
                        );
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                // Handle 404
                Console.WriteLine($"Api call for command '{typeof(TCommand).Name}' failed with not found error : " + ex.Message);
                throw;
            }
        }
    }
}