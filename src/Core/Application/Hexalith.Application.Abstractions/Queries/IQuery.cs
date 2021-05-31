namespace Hexalith.Application.Queries
{
    using Hexalith.Domain.Messages;

    public interface IQuery : IMessage
    {
    }

    public interface IQuery<TResult> : IQuery
    {
    }
}