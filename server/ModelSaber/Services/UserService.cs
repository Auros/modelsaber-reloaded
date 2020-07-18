using System.Linq;
using ModelSaber.Database;
using ModelSaber.Models.User;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ModelSaber.Services
{
    public class UserService
    {
        public static async Task<User> UserFromContext(HttpContext context, ModelSaberContext modelSaber)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var ID = claim.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
            return await modelSaber.Users.FirstAsync(u => u.ID == ID.Value);
        }
    }
}