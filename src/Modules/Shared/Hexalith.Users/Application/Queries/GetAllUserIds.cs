using System.Collections.Generic;

using Bistrotic.Application.Queries;

namespace Bistrotic.Users.Application.Queries
{
    public sealed class GetAllUserIds : QueryBase<List<int>>
    {
        public GetAllUserIds(int take, int skip)
        {
            Take = take;
            Skip = skip;
        }

        public int Skip { get; }
        public int Take { get; }
    }
}