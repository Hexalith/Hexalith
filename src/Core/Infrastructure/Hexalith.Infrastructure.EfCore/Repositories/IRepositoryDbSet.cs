namespace Hexalith.Infrastructure.EfCore.Repositories
{
    public interface IRepositoryDbSet
    {
        public string Id { get; }
        public int IdHash { get; }
        public int Version { get; }
    }
}