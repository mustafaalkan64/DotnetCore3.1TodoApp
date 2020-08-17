using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Todo_App.Core.Consts;

namespace ToDo_App.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        // Get Current User Id via JWT Claims Identity
        protected int GetUserId()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            return int.Parse(claimsIdentity.FindFirst(StaticVariables.UserId)?.Value ?? "0");
        }
    }
}