using Hexalith.Application.Messages;
using Hexalith.Application.Repositories;

using System;

namespace Hexalith.Application.Helpers
{
    public static class EnvelopeHelpers
    {
        public static IRepositoryMetadata ToMetadata(this IEnvelope envelope)
            => new RepositoryMetadata
            {
                MessageId = envelope.MessageId,
                CausationId = envelope.CausationId?.Value,
                CorrelationId = envelope.CorrelationId?.Value,
                SystemUtcDateTime = DateTime.UtcNow,
                UserDateTime = envelope.UserDateTime,
                UserName = envelope.UserName
            };
    }
}