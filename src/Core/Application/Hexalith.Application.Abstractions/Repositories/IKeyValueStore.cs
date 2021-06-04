namespace Hexalith.Application.Repositories
{
    using System.Threading.Tasks;

    public interface IKeyValueStore
    {
        public Task Add(string key, object value);

        public Task<bool> Exists(string key);

        public Task<object> Get(string key);

        public Task Remove(string key);
    }
}