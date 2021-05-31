namespace Hexalith.Infrastructure.CodeGeneration.Generators.WebApi
{
    public static class QueryCommandControllerTemplate
    {
        public const string Value = @"
namespace {{namespace}}
{
{{ for u in usings }}
    using {{ u }};
{{ end }}

    [ApiController]
    [Authorize]
    [Route(""api/{{ modulename | string.downcase }}/[action]"")]
    public sealed partial class {{ modulename }}ApiController : QueryCommandControllerBase
    {
         public {{ modulename }}ApiController(
            IServiceProvider serviceProvider,
            IQueryBus queryDispatcher,
            ILogger<{{ modulename }}ApiController> logger)
            : base(queryDispatcher, logger)
        {
            ServiceProvider = serviceProvider;
        }

        private IServiceProvider ServiceProvider {get;}

{{ for command in commands }}
        [HttpPost]
        public Task<IActionResult> {{ command.name }}([FromBody] {{ command.name }} command, string? MessageId = null)
        {
            return base.Tell(command, MessageId);
        }

{{ end }}
{{ for query in queries }}
        [HttpGet]
        public Task<IActionResult> {{ query.name }}(
{{ for property in query.properties }}
            {{ property.type }} {{ property.name }},
{{ end }}
            string? MessageId = null
        )
        {
            return base.Ask(new {{ query.name }}(
{{ for property in query.properties }}
            {{ property.name }} = {{ property.name }}{{if for.last != true}},{{end}}
{{ end }}
            ), MessageId);
        }

{{ end }}
    }
}
";
    }
}