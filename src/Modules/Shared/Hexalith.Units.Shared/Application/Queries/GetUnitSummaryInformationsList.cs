using Hexalith.Application.Queries;
using Hexalith.Units.Application.ModelViews;

namespace Hexalith.Units.Application.Queries
{
    public sealed class GetUnitSummaryInformationsList
        : QueryBase<UnitSummaryInformations[]>
    {
        public GetUnitSummaryInformationsList(int take = 0, int skip = 0)
        {
            Take = take;
            Skip = skip;
        }

        public int Skip { get; }
        public int Take { get; }
    }
}