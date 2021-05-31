namespace Hexalith.Units.Application.Queries
{
    using System.Collections.Generic;

    using Hexalith.Application.Queries;

    public sealed class GetAllUnitIds : QueryBase<List<int>>
    {
        public GetAllUnitIds(int take, int skip)
        {
            Take = take;
            Skip = skip;
        }

        public int Skip { get; }
        public int Take { get; }
    }
}