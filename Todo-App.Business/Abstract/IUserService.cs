using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo_App.Business.Models;
using Todo_App.DAL.EfDbContext;
using Todo_App.Domain.Entities;

namespace Todo_App.Business.Abstract
{
    public interface IUserService
    {
        Task<WebApiResponse> Authenticate(UserLoginDto userLoginDto);
        Task<WebApiResponse> Register(UserDto user);
        Task<WebApiResponse> ChangePassword(ChangePasswordModelDto changePasswordModel);
        Task<Users> GetUserById(int id);
    }
}
