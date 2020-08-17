using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Todo_App.DAL.Abstract
{
    public interface IGenericRepository<T>
    {
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Expression<Func<T, bool>> identity, params Expression<Func<T, object>>[] includes);
        Task<bool> DeleteAsync(T entity);

        Task<bool> Any(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<List<T>> PageAsync(string sort, bool desc, Expression<Func<T, bool>> predicate, int take, int skip, params Expression<Func<T, object>>[] include);
        Task<List<T>> PageAsync(string sort, bool desc, int take, int skip, params Expression<Func<T, object>>[] include);
        Task<List<T>> SearchByAsync(string sort, bool desc, Expression<Func<T, bool>> searchBy, params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(object id);

    }
}
