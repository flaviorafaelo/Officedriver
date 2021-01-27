using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Altima.Broker.Data;
using Altima.Broker.Business;
using System;

namespace Altima.Broker.AspNet.Mvc.Data
{
    public class AsyncRepository<T> : IAsyncRepository<T> where T : BaseModel
    {
        protected readonly DataContext _dbContext;

        public AsyncRepository(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(long id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        //public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        //{
        //    return await ApplySpecification(spec).ToListAsync();
        //}

        //public async Task<int> CountAsync(ISpecification<T> spec)
        //{
        //    return await ApplySpecification(spec).CountAsync();
        //}

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<int> UpdateAsync(long id, T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(long id)
        {
            var model = await _dbContext.Set<T>().FindAsync(id);
            if (model != null)
            {
                _dbContext.Set<T>().Remove(model);
                return await _dbContext.SaveChangesAsync();
            } 
            else
            {
                return 0;
            }           
        }

        //private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        //{
        //    return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        //}
    }
}