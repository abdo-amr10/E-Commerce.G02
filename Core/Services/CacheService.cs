using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Services.Abstraction;

namespace Services
{
    internal class CacheService(ICacheRepository cachingRepository) : ICacheService
    {


        public async Task<string?> GetCachedValueAsync(string key)
       => await cachingRepository.GetAsync(key);



        public async Task SetCacheValueAsync(string key, string value, TimeSpan expirationTime)
          => await cachingRepository.SetAsync(key, value, expirationTime);
    }

}
