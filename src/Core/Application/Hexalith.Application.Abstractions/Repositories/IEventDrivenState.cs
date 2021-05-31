using System.Collections.Generic;

namespace Hexalith.Application.Repositories
{
    public interface IEventDrivenState
    {
        void Apply(IEnumerable<object> events);
    }
}