using System;
using ModelSaber.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ModelSaber.API.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class Maulth : Attribute, IAuthorizationFilter
    {
        public Role Role { get; set; }

        public Maulth(Role role = Role.None)
        {
            Role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user == null || !user.Role.HasFlag(Role))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}