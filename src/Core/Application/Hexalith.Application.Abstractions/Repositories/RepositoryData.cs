#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8604 // Possible null reference argument.

namespace Hexalith.Application.Repositories
{
    using System;
    using System.Collections.Generic;

    using Hexalith.Application.Messages;

    public class RepositoryData<TState>
        : IRepositoryData<TState>
    {
        public RepositoryData(
            string correlationId,
            string causationId,
            string userName,
            DateTimeOffset userDateTime,
            TState state,
            IEnumerable<object> events)
        {
            Metadata = new RepositoryMetadata()
            {
                CorrelationId = correlationId,
                CausationId = causationId,
                UserName = userName,
                UserDateTime = userDateTime
            };
            State = state;
            Events = events;
        }

        public RepositoryData(
            string correlationId,
            string causationId,
            string userName,
            DateTimeOffset userDateTime,
            IEnumerable<object> events)
            : this(
                 correlationId,
                 causationId,
                 userName,
                 userDateTime,
                 default,
                 events
                 )
        {
        }

        public RepositoryData(
            string correlationId,
            string causationId,
            string userName,
            DateTimeOffset userDateTime,
            TState state)
            : this(
                 correlationId,
                 causationId,
                 userName,
                 userDateTime,
                 state,
                 Array.Empty<object>()
                 )
        {
        }

        public RepositoryData(IEnvelope enveloppe, TState state, IEnumerable<object> events)
            : this(
                enveloppe.CorrelationId ?? enveloppe.MessageId,
                enveloppe.MessageId,
                enveloppe.UserName,
                enveloppe.UserDateTime,
                state,
                events
            )
        {
        }

        public RepositoryData(IEnvelope enveloppe, IEnumerable<object> events)
            : this(
                enveloppe.CorrelationId ?? enveloppe.MessageId,
                enveloppe.MessageId,
                enveloppe.UserName,
                enveloppe.UserDateTime,
                events
            )
        {
        }

        public RepositoryData(IEnvelope enveloppe, TState state)
            : this(
                enveloppe.CorrelationId ?? enveloppe.MessageId,
                enveloppe.MessageId,
                enveloppe.UserName,
                enveloppe.UserDateTime,
                state,
                Array.Empty<object>()
            )
        {
        }

        public RepositoryData(IRepositoryData state)
        {
            Events = state.Events;
            Metadata = state.Metadata;
            State = (TState)state.State;
        }

        public IEnumerable<object> Events { get; }
        public IRepositoryMetadata Metadata { get; }
        public TState State { get; init; }
        object IRepositoryData.State => State;
    }
}