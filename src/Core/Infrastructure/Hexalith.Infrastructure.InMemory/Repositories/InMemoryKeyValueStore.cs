namespace Hexalith.Infrastructure.InMemory.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Hexalith.Application.Repositories;

    public class InMemoryKeyValueStore<TState> : IKeyValueStore<TState>
        where TState : new()
    {
        private readonly Dictionary<string, TState> _states = new();

        public InMemoryKeyValueStore()
        {
        }

        public Task Add(string key, TState value)
        {
            _states.Add(key, value);
            return Task.CompletedTask;
        }

        public Task Add(string key, object value)
            => Add(key, (TState)value);

        public Task<bool> Exists(string key)
            => Task.FromResult(_states.ContainsKey(key));

        public async Task<TState?> Find(string key)
            => await Exists(key) ? await Get(key) : default;

        public Task<TState> Get(string key)
            => Task.FromResult(_states[key]);

        public Task Remove(string key)
        {
            _states.Remove(key);
            return Task.CompletedTask;
        }

        async Task<object?> IKeyValueStore.Find(string key)
            => await Find(key);

        async Task<object> IKeyValueStore.Get(string key)
            => (await Get(key))!;
    }
}