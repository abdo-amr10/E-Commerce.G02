using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Presistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connectionMultiplexer) : ICacheRepository
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
        public async Task<string?> GetAsync(string key)
        {
            var value = await _database.StringGetAsync(key);
            return value.IsNullOrEmpty ? null : value.ToString();

        }

        public async Task SetAsync(string key, object value, TimeSpan duration)
        {
            var serializedOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var serializedValue = JsonSerializer.Serialize(value, serializedOptions);
            await _database.StringSetAsync(key, serializedValue, duration);
        }
    }
}
