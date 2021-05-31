namespace Hexalith.Users.Application.Queries
{
    using Hexalith.Users.Application.ModelViews;
    using Hexalith.Users.Domain.ValueTypes;

    public sealed class GetUserDetailedInformations
        : UserIdQuery<UserDetailedInformations>
    {
        public GetUserDetailedInformations(UserId UserId) : base(UserId)
        {
        }
    }
}