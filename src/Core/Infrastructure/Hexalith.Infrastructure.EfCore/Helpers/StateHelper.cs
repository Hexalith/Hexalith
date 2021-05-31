using System;

using Hexalith.Infrastructure.EfCore.Repositories;

namespace Hexalith.Infrastructure.EfCore.Helpers
{
    public static class StateHelper
    {
        public static int HashKey(this IRepositoryDbSet? state)
            => state?.Id?.HashKey() ?? 0;

        public static int HashKey(this string? key)
            => key?.GetHashCode(StringComparison.Ordinal) ?? 0;
    }
}