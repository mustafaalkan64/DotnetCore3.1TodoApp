using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Todo_App.Core.Consts;
using Todo_App.Business.Models;
namespace Todo_App.Business
{
    public class JWTTokenManager
    {
        public static string CreateToken(UserDto user, string secretkey = "", string issuer = "")
        {
            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName?.ToString() ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() ?? ""),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email?.ToString() ?? ""),
                    new Claim(StaticVariables.UserId, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, RoleType.User),
                    new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: issuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(90),
                signingCredentials: creds);

            var usertoken = new JwtSecurityTokenHandler().WriteToken(token);
            return usertoken;
        }
    }
    // Generate Token With JWT Token
    
}
