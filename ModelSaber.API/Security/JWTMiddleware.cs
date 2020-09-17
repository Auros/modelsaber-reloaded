using System.Linq;
using ModelSaber.Common;
using System.Threading.Tasks;
using ModelSaber.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ModelSaber.API.Security
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJWTService _jwtService;

        public JWTMiddleware(RequestDelegate next, IJWTService jwtService)
        {
            _next = next;
            _jwtService = jwtService;
        }

        public async Task Invoke(HttpContext context, ModelSaberContext modelSaberContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null) AttachUserToContext(token, context, modelSaberContext);
            await _next(context);
        }

        private async void AttachUserToContext(string token, HttpContext context, ModelSaberContext modelSaberContext)
        {
            var response = _jwtService.IDFromToken(token);
            if (response.validated)
            {
                // ??? THE PIPELINE FAILS IF I DONT DO THIS
                _ = modelSaberContext.Users.First();
                User user = await modelSaberContext.Users.FirstOrDefaultAsync(u => u.Id == response.id);
                context.Items["User"] = user;
            }
        }
    }
}