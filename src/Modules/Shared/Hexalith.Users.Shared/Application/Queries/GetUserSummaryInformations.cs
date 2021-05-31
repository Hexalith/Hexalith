namespace Hexalith.Users.Application.Queries
{
    using Hexalith.Users.Application.ModelViews;
    using Hexalith.Users.Domain.ValueTypes;

    public sealed class GetUsersummaryInformations : UserIdQuery<UsersummaryInformations>
    {
        public GetUsersummaryInformations(UserId UserId) : base(UserId)
        {
        }
    }
}