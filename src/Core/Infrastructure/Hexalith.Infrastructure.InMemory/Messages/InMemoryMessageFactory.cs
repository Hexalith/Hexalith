namespace Hexalith.Infrastructure.InMemory.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;

    using Hexalith.Application.Exceptions;
    using Hexalith.Application.Messages;
    using Hexalith.Domain.Messages;
    using Hexalith.Infrastructure.Helpers;

    using Microsoft.Extensions.Primitives;

    public class InMemoryMessageFactory : IMessageFactory
    {
        private readonly Dictionary<string, Type> _messageTypes;

        public InMemoryMessageFactory(IEnumerable<Type> messageTypes)
        {
            _messageTypes = messageTypes.ToDictionary(p => p.Name.ToUpperInvariant());
        }

        public IMessage GetMessage(string name, string jsonValue)
        {
            Type messageType = GetMessageType(name);
            return (JsonSerializer.Deserialize(jsonValue, messageType) as IMessage) ?? throw new InvalidMessageException(messageType, jsonValue);
        }

        public IMessage GetMessage(string name, IEnumerable<KeyValuePair<string, StringValues>> values)
            => (IMessage)values.ToObject(GetMessageType(name));

        public Type GetMessageType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("The type name is empty.", nameof(name));
            }
            if (_messageTypes.TryGetValue(name.ToUpperInvariant(), out Type? type))
            {
                return type;
            }
            throw new MessageTypeNotFoundException(name);
        }
    }
}