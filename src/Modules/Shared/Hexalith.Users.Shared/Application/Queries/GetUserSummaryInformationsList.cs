using Hexalith.Application.Queries;
using Hexalith.Users.Application.ModelViews;

namespace Hexalith.Users.Application.Queries
{
    public sealed class GetUsersummaryInformationsList
        : QueryBase<UsersummaryInformations[]>
    {
        public GetUsersummaryInformationsList(int take = 0, int skip = 0)
            => (Take, Skip) = (take, skip);

        public int Skip { get; }
        public int Take { get; }
    }
}