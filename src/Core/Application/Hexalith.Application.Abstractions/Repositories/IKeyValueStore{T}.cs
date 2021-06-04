namespace Hexalith.Application.Repositories
{
    using System.Threading.Tasks;

    public interface IKeyValueStore<TState> : IKeyValueStore
    {
        public Task Add(string key, TState value);

        new public Task<TState?> Find(string key);

        new public Task<TState> Get(string key);
    }
}