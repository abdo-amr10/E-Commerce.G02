using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IDbIntializer
    {
        public Task IntializeAsync();
        public Task IntializeIdentityAsync();

    }
}
