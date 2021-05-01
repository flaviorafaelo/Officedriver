using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Altima.Broker.Business;

namespace Altima.Broker.Data
{
    public interface IAsyncRepository<T> where T : BaseModel//, IAggregateRoot
    {
        Task<T> GetByIdAsync(long id);
        IQueryable<T> ListAllAsync();
        //Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity);
        Task<int> UpdateAsync(long id, T entity);
        Task<int> DeleteAsync(long id);
        //Task<int> CountAsync(ISpecification<T> spec);
    }
}
