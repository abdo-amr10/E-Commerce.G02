using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Presistence.Data;

namespace Presistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false) =>
            asNoTracking ? await _dbContext.Set<TEntity>().ToListAsync() : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(int id) => await _dbContext.Set<TEntity>().FindAsync(id);

        public async Task AddAsync(TEntity entity) => await _dbContext.Set<TEntity>().AddAsync(entity);

        public void Update(TEntity entity)=>  _dbContext.Set<TEntity>().Update(entity);

        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);

        public async Task<TEntity?> GetByIdAsync(Specifications<TEntity> specifications)
           => await ApplySpecifications(specifications).FirstOrDefaultAsync();

        public async Task<IEnumerable<TEntity>> GetAllAsync(Specifications<TEntity> specifications)
              => await ApplySpecifications(specifications).ToListAsync();

        public async Task<int> CountAsync(Specifications<TEntity> specifications)
            => await ApplySpecifications(specifications).CountAsync();
        private IQueryable<TEntity> ApplySpecifications(Specifications<TEntity> specifications)
            => SpecificationEvaluator.GetQuery<TEntity>(_dbContext.Set<TEntity>(), specifications);

        
    }
}
