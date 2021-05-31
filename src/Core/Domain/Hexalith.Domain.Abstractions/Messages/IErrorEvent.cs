namespace Hexalith.Domain.Messages
{
    public interface IErrorEvent
    {
        string ErrorMessage { get; }
    }
}