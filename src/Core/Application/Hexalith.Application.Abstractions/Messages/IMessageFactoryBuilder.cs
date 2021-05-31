using System;
using System.Reflection;

using Hexalith.Domain.Messages;

namespace Hexalith.Application.Messages
{
    public interface IMessageFactoryBuilder
    {
        IMessageFactoryBuilder AddAssemblyMessages(Assembly assembly);

        IMessageFactoryBuilder AddMessage<T>() where T : class, IMessage, new();

        IMessageFactory Build();
    }
}