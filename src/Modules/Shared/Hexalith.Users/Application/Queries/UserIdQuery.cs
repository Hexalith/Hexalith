namespace Bistrotic.Users.Application.Queries
{
    using Bistrotic.Application.Queries;
    using Bistrotic.Users.Domain.ValueTypes;

    public abstract class UserIdQuery<TResult> : QueryBase<UserId, TResult>
    {
        protected UserIdQuery()
        {
        }

        protected UserIdQuery(UserId UserId)
            : base(UserId)
        {
        }
    }
}