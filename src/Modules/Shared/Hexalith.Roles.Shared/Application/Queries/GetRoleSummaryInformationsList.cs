using Hexalith.Application.Queries;
using Hexalith.Roles.Application.ModelViews;

namespace Hexalith.Roles.Application.Queries
{
    public sealed class GetRoleSummaryInformationsList
        : QueryBase<RoleSummaryInformations[]>
    {
        public GetRoleSummaryInformationsList(int take = 0, int skip = 0)
        {
            Take = take;
            Skip = skip;
        }

        public int Skip { get; }
        public int Take { get; }
    }
}