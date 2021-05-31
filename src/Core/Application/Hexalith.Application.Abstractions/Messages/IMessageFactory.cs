using System;
using System.Collections.Generic;

using Hexalith.Domain.Messages;

using Microsoft.Extensions.Primitives;

namespace Hexalith.Application.Messages
{
    public interface IMessageFactory
    {
        IMessage GetMessage(string name, string jsonValue);

        IMessage GetMessage(string name, IEnumerable<KeyValuePair<string, StringValues>> values);

        Type GetMessageType(string name);
    }
}