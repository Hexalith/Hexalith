namespace Hexalith.Infrastructure.InMemory.Commands
{
    using System;
    using System.Collections.Generic;

    using Hexalith.Application.Commands;

    public class InMemoryCommandBusBuilder : ICommandBusBuilder
    {
        private readonly Dictionary<Type, Func<ICommandHandler>> _handlers = new();

        public ICommandBusBuilder AddCommandHandler<TCommand>(Func<ICommandHandler<TCommand>> handler)
            where TCommand : class
        {
            _handlers.Add(typeof(TCommand), handler);
            return this;
        }

        public ICommandBus Build()
        {
            return new InMemoryCommandBus(_handlers);
        }
    }
}