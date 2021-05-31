using Bistrotic.Application.Queries;
using Bistrotic.Users.Application.ModelViews;

namespace Bistrotic.Users.Application.Queries
{
    public sealed class GetUsersummaryInformationsList
        : QueryBase<UsersummaryInformations[]>
    {
        public GetUsersummaryInformationsList(int take = 0, int skip = 0)
        {
            Take = take;
            Skip = skip;
        }

        public int Skip { get; }
        public int Take { get; }
    }
}