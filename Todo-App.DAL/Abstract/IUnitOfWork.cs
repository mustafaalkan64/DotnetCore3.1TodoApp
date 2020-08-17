using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo_App.DAL.EfDbContext;
using Todo_App.DAL.Repository;
using Todo_App.Domain.Entities;

namespace Todo_App.DAL.Abstract
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Todo> TodoRepository { get; }
        IGenericRepository<Users> UserRepository { get; }
        Task Commit();
    }
}
