using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        public Task<CustomerBasket?> GetBasketAsync(string id);
        public Task<bool> DeleteBasketAsync(string id);
        public Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket? basket , TimeSpan? timeToLive = null);
    }
}
