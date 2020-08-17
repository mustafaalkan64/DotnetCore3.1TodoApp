using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Todo_App.Business.Models;
using ToDo_App.Core.Helpers;
using Todo_App.Business.Abstract;
using Todo_App.DAL.Abstract;
using Todo_App.Domain.Entities;
using Todo_App.Business.Validations;

namespace Todo_App.Business.Services
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private string secretKey;
        private readonly ILogger<UserManager> _logger;
        public IConfiguration Configuration { get; }


        public UserManager(IUnitOfWork unit, 
                IMapper mapper, 
                ILogger<UserManager> logger, 
                IConfiguration configuration)
        {
            _uow = unit;
            Configuration = configuration;
            secretKey = Configuration["JwtToken:SecretKey"];
            _mapper = mapper;
            _logger = logger;
        }

        // Login Operation Service Method
        public async Task<WebApiResponse> Authenticate(UserLoginDto userLoginDto)
        {
            try
            {
                // Validate UserLoginDto through Fluent Validation and if it's invalid, return errors.
                var errors = ValidatorUtility.FluentValidate(new UserLoginValidator(), userLoginDto);
                if (!string.IsNullOrEmpty(errors))
                {
                    return new WebApiResponse()
                    {
                        Response = errors,
                        Status = false
                    };
                }
                // Check If Email Exists in DB.
                var user = await _uow.UserRepository.FindByAsync(x => x.Email.Equals(userLoginDto.Email));

                if (user == null)
                {
                    return new WebApiResponse()
                    {
                        Response = "Any Account Could Not Found With This Email",
                        Status = false
                    };
                }
                else
                {
                    // Check Hashed Password Matchs and if password is correct
                    if (UserPasswordHashManager.AreEqual(userLoginDto.Password, user.Hash, user.Salt))
                    {
                        var userDto = _mapper.Map<Users, UserDto>(user);
                        // Generate JWT Token Specialized to current user and return token.
                        var token = JWTTokenManager.CreateToken(userDto, secretKey, Configuration["JwtToken:Issuer"]);
                        return new WebApiResponse()
                        {
                            Response = token,
                            Status = true
                        };
                    }
                    else
                    {
                        return new WebApiResponse()
                        {
                            Response = "Password is Wrong, Please Check Your Password!",
                            Status = false
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Error During Login User", userLoginDto.Email);
                throw ex;
            }

        }

        public async Task<WebApiResponse> Register(UserDto userDto)
        {
            try
            {
                // Validate UserLoginDto through Fluent Validation and if it's invalid, return errors.
                ValidatorUtility.FluentValidate(new UserValidator(), userDto);
                if (await CheckUserName(userDto.UserName))
                {
                    return new WebApiResponse()
                    {
                        Response = "This UserName Allready Exists. Please Type Different UserName",
                        Status = false
                    };
                }

                if (await CheckEmail(userDto.Email))
                {
                    return new WebApiResponse()
                    {
                        Response = "This Email Address Allready Exists. Please Type Different Email Address",
                        Status = false
                    };
                }

                // return null if user not found
                if (userDto == null)
                    return new WebApiResponse()
                    {
                        Response = "User can not be null",
                        Status = false
                    };

                // Generate Hashed Password Through UserPasswordHashManager Helper Class.
                var user = _mapper.Map<Users>(userDto);
                user.Salt = UserPasswordHashManager.CreateSalt(10);
                user.Hash = UserPasswordHashManager.GenerateHash(userDto.Password, user.Salt);

                await _uow.UserRepository.AddAsync(user);
                await _uow.Commit();

                userDto.Id = user.Id;
                // Generate JWT Token Specialized to current user and return token.
                var token = JWTTokenManager.CreateToken(userDto, secretKey, Configuration["JwtToken:Issuer"]);
                return new WebApiResponse()
                {
                    Response = token,
                    Status = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Error During User Register", userDto);
                throw ex;
            }

        }


        public async Task<WebApiResponse> ChangePassword(ChangePasswordModelDto changePasswordModel)
        {
            try
            {
                ValidatorUtility.FluentValidate(new ChangePasswordValidator(), changePasswordModel);

                var user = await _uow.UserRepository.FindByAsync(x => x.Id.Equals(changePasswordModel.UserId));
                if (user == null)
                    return null;
                else
                {
                    if (UserPasswordHashManager.AreEqual(changePasswordModel.CurrentPassword, user.Hash, user.Salt))
                    {
                        user.Salt = UserPasswordHashManager.CreateSalt(10);
                        user.Hash = UserPasswordHashManager.GenerateHash(changePasswordModel.NewPassword, user.Salt);
                        await _uow.UserRepository.UpdateAsync(user);
                        await _uow.Commit();

                        return new WebApiResponse()
                        {
                            Response = "Password Changed Successfuly",
                            Status = true
                        };
                    }
                    else
                    {
                        return new WebApiResponse()
                        {
                            Response = "Current Password is Wrong",
                            Status = false
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception Error During Change Password", changePasswordModel);
                throw ex;
            }

        }

        public async Task<Users> GetUserById(int id)
        {
            return await _uow.UserRepository.FindByAsync(x => x.Id == id);
        }

        /// <summary>
        /// Check If User Name Exists In Users Table
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task<bool> CheckUserName(string username)
        {
            return await _uow.UserRepository.Any(x => x.UserName.Equals(username));
        }


        /// <summary>
        /// Check If Email Exists In Users Table
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task<bool> CheckEmail(string email)
        {
            return await _uow.UserRepository.Any(x => x.Email.Equals(email));
        }

    }
}
