using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface ICacheRepository
    {
        //Set [Key , Value , TimeToLive (exp date)]
        Task SetAsync(string key, object value, TimeSpan duration);
        //Get O references
        Task<string?> GetAsync(string key);
    }
}
