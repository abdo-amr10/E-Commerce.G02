using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presistence.Data;

namespace Presistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ConcurrentDictionary <string,object> _repositories;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new();
        }
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>

               => (IGenericRepository<TEntity, TKey>) _repositories.GetOrAdd(typeof(TEntity).Name,
             _ => new GenericRepository<TEntity, TKey> (_dbContext));
            

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();  
    }
}
