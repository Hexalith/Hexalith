namespace Bistrotic.Users.Application.Queries
{
    using Bistrotic.Users.Application.ModelViews;
    using Bistrotic.Users.Domain.ValueTypes;

    public sealed class GetUsersummaryInformations : UserIdQuery<UsersummaryInformations>
    {
        public GetUsersummaryInformations(UserId UserId) : base(UserId)
        {
        }
    }
}