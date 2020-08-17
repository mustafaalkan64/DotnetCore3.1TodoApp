using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Todo_App.Core.Consts;
using Todo_App.DAL.Abstract;
using Todo_App.DAL.EfDbContext;

namespace Todo_App.DAL.Repository
{
    /* This is my generic repository class.
     * I manage my all transactional operations within below these methods.
     * It inherits from IGenericRepository interface
     * As it takes generic type, i dont need generate new repository for each dbsets. 
     */
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private TodoListContext context;
        private DbSet<T> dbSet;

        public GenericRepository(TodoListContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public virtual async Task<bool> AddAsync(T entity)
        {
            try
            {
                context.Set<T>().Add(entity);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public virtual async Task<List<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var result = context.Set<T>().Where(i => true);

            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);

            return await result.ToListAsync();
        }

        /// <summary>
        /// Finds by predicate.
        /// http://appetere.com/post/passing-include-statements-into-a-repository
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        public virtual async Task<T> FindByAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var result = context.Set<T>().Where(predicate);

            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);

            return await result.FirstOrDefaultAsync();
        }

        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public virtual async Task<bool> Any(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var result = await context.Set<T>().AnyAsync(predicate);
            return result;
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                context.Set<T>().Attach(entity);
                context.Entry(entity).State = EntityState.Modified;

                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }

        public virtual async Task<bool> DeleteAsync(Expression<Func<T, bool>> identity, params Expression<Func<T, object>>[] includes)
        {
            var results = context.Set<T>().Where(identity);

            foreach (var includeExpression in includes)
                results = results.Include(includeExpression);
            try
            {
                context.Set<T>().RemoveRange(results);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                return await Task.FromResult(false);
            }
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            return await Task.FromResult(true);
        }

        public virtual async Task<List<T>> PageAsync(string sort, bool desc, Expression<Func<T, bool>> predicate, int take, int skip, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> result;
            
            var orderType = desc ? SortType.Desc.ToUpper() : SortType.Asc.ToUpper();
            var order = $"{sort} { orderType }";
            result = context.Set<T>().OrderBy(order).Where(predicate).Skip((skip - 1) * take).Take(take);

            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);

            return await result.ToListAsync();
        }

        public virtual async Task<List<T>> PageAsync(string sort, bool desc, int skip, int take, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> result;

            var orderType = desc ? SortType.Desc.ToUpper() : SortType.Asc.ToUpper();
            var order = $"{sort} { orderType }";
            result = context.Set<T>().OrderBy(order).Skip((skip - 1) * take).Take(take);

            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);

            return await result.ToListAsync();
        }

        public virtual async Task<List<T>> SearchByAsync(string sort, bool desc, Expression<Func<T, bool>> searchBy, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> result;

            var orderType = desc ? SortType.Desc.ToUpper() : SortType.Asc.ToUpper();
            var order = $"{sort} { orderType }";
            result = context.Set<T>().Where(searchBy).OrderBy(order);

            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);

            return await result.ToListAsync();
        }
    }
}
