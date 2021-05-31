namespace Hexalith.Users.Application.Queries
{
    using Hexalith.Application.Queries;
    using Hexalith.Users.Domain.ValueTypes;

    public abstract class UserIdQuery<TResult> : QueryBase<UserId, TResult>
    {
        protected UserIdQuery() { }

        protected UserIdQuery(UserId UserId)
            : base(UserId) { }
    }
}