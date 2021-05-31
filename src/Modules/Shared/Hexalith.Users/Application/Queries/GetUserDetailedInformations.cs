namespace Bistrotic.Users.Application.Queries
{
    using Bistrotic.Users.Application.ModelViews;
    using Bistrotic.Users.Domain.ValueTypes;

    public sealed class GetUserDetailedInformations
        : UserIdQuery<UserDetailedInformations>
    {
        public GetUserDetailedInformations(UserId UserId) : base(UserId)
        {
        }
    }
}