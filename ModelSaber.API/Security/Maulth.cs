using System;
using ModelSaber.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace ModelSaber.API.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Maulth : Attribute, IAuthorizationFilter
    {
        public Role Role { get; set; }
        public bool AllowAnonymous { get; set; }

        public Maulth(Role role = Role.None)
        {
            Role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (AllowAnonymous == false && (user == null || !user.Role.HasFlag(Role)))
            {
                context.Result = new JsonResult(new { error = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}