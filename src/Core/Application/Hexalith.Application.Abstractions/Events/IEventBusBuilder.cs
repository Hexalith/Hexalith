namespace Hexalith.Application.Events
{
    using System;

    public interface IEventBusBuilder
    {
        IEventBusBuilder AddEventHandler<TEvent>(Func<IEventHandler<TEvent>> handler)
            where TEvent : class;

        IEventBus Build();
    }
}