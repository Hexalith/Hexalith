namespace Hexalith.Application.Queries
{
    using System.Threading;
    using System.Threading.Tasks;

    using Hexalith.Application.Messages;

    using Microsoft.Extensions.Options;

    public abstract class SettingsQueryHandler<TQuery, TSettings>
        : QueryHandler<TQuery, TSettings>
        where TQuery : QueryBase<TSettings>
        where TSettings : class
    {
        private readonly IOptions<TSettings> _options;

        protected SettingsQueryHandler(IOptions<TSettings> options)
        {
            _options = options;
        }

        public override Task<TSettings> Handle(Envelope<TQuery> envelope, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_options.Value);
        }
    }
}