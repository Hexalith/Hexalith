namespace Hexalith.Application.Commands
{
    using System;

    public interface ICommandBusBuilder
    {
        ICommandBusBuilder AddCommandHandler<TCommand>(Func<ICommandHandler<TCommand>> handler)
            where TCommand : class;

        ICommandBus Build();
    }
}