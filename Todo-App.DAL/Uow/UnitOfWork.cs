using System;
using System.Threading.Tasks;
using Todo_App.DAL.Abstract;
using Todo_App.DAL.EfDbContext;
using Todo_App.DAL.Repository;
using Todo_App.Domain.Entities;

namespace Todo_App.DAL.Uow
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly TodoListContext _context;
        private IGenericRepository<Todo> _todoRepository;
        private IGenericRepository<Users> _usersRepository;

        public UnitOfWork(TodoListContext context)
        {
            _context = context;
        }

        // I generate my all repositories according to Singleton Pattern 
        // depending on UnitOfWork class
        // as you see below
        public IGenericRepository<Todo> TodoRepository
        {
            get { return _todoRepository ?? (_todoRepository = new GenericRepository<Todo>(_context)); }
        }

        public IGenericRepository<Users> UserRepository
        {
            get { return _usersRepository ?? (_usersRepository = new GenericRepository<Users>(_context)); }
        }

        // I manage all my database commit and rollback transactions from only one point:
        public async Task Commit()
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    
                }
                catch (Exception ex)
                {
                    _context.Dispose();
                    transaction.Rollback();
                }

            }

        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }
    }
}
