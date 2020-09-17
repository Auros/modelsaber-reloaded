using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace ModelSaber.API.Security
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JWTSettings _jwtSettings;

        public JWTMiddleware(RequestDelegate next, IOptions<JWTSettings> jwtSettings)
        {
            _next = next;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            await _next(context);
        }
    }
}