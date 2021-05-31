namespace Hexalith.WorkItems.Application.Models
{
    using System.Collections.Immutable;

    public record ProjectSla(
        string ProjectName,
        ImmutableArray<PrioritySla> PrioritySlas)
    {
    }
}