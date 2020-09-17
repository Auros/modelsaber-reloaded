using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelSaber.API.Security;
using ModelSaber.API.Services;
using ModelSaber.Common;
using System.Threading.Tasks;

namespace ModelSaber.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IJWTService _jwtService;
        private readonly DiscordService _discordService;
        private readonly ModelSaberContext _modelSaberContext;

        public AuthorizationController(IJWTService jwtService, DiscordService discordService, ModelSaberContext modelSaberContext)
        {
            _jwtService = jwtService;
            _discordService = discordService;
            _modelSaberContext = modelSaberContext;
        }

        [HttpGet]
        public IActionResult Authenticate()
        {
            return Redirect($"https://discordapp.com/api/oauth2/authorize?response_type=code&client_id={_discordService.ID}&scope=identify&redirect_uri={_discordService.RedirectURL}");
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery(Name = "code")] string code)
        {
            string token = await _discordService.GetAccessToken(code);
            if (token == null) return NotFound();

            DiscordUser profile = await _discordService.GetProfile(token);

            User user = await _modelSaberContext.Users.FirstOrDefaultAsync(u => u.Profile.Id == profile.Id);
            if (user == null)
            {
                user = new User
                {
                    Biography = "",
                    Profile = profile,
                    Role = Role.Uploader
                };
                user = (await _modelSaberContext.Users.AddAsync(user)).Entity;
            }
            token = _jwtService.GenerateToken(user);
            return Ok(new { token });
        }

        [Maulth]
        [HttpGet("@me")]
        public IActionResult GetSelf()
        {
            if (HttpContext.Items["User"] is not User user)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
