namespace Hexalith.Infrastructure.CodeGeneration.WebApiClient
{
    public static class QueryCommandWebApiClientTemplate
    {
        public const string Value = @"
namespace {{namespace}}
{
{{ for u in usings }}
    using {{ u }};
{{ end }}

    public sealed class {{ modulename }}WebApiClient : QueryCommandWebApiClientBase
    {
        public {{ modulename }}WebApiClient(HttpClient httpClient) : base(httpClient, ""{{ moduleName }}"")
        {
        }

{{ for command in commands }}
        public Task {{ command.name }}(
    {{ for property in command.properties }}
            {{ property.type }} {{ property.name }},
    {{ end }}
            string? MessageId = null)
        {
            return Tell(new {{ command.name }}
                {
    {{ for property in command.properties }}
                    {{ property.name }} = {{ property.name }}{{if for.last != true}},{{end}}
    {{ end }}
                },
                MessageId);
       }
{{ end }}
    }
}
";
    }
}