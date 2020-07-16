using ModelSaber.Services;
using ModelSaber.Models.User;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelSaber.Models.Discord;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using ModelSaber.Database;
using Microsoft.EntityFrameworkCore;

namespace ModelSaber.Controllers
{
    [Route("api/assist/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly JWTService _jwtService;
        private readonly UserService _userService;
        private readonly ModelSaberContext _modelSaber;
        private readonly DiscordService _discordService;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(JWTService jwtService, UserService userService, ModelSaberContext modelSaber, DiscordService discordService, ILogger<AuthorizationController> logger)
        {
            _logger = logger;
            _jwtService = jwtService;
            _modelSaber = modelSaber;
            _userService = userService;
            _discordService = discordService;
        }

        [HttpGet]
        public IActionResult Authenticate()
        {
            return Redirect($"https://discordapp.com/api/oauth2/authorize?response_type=code&client_id={_discordService.ID}&scope=identify&redirect_uri={_discordService.RedirectURL}");
        }

        /// <summary>
        /// The callback from Discord which then creates the user if necessary then returns a token.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery(Name = "code")] string code)
        {
            string token = await _discordService.GetAccessToken(code);

            // Code was invalid? Let's stop.
            if (token == null)
                return NotFound();

            DiscordUser profile = await _discordService.GetProfile(token);
           
            // Try to find the user in the database.
            User user = await _modelSaber.Users.FirstOrDefaultAsync(u => u.ID == profile.ID);
            if (user == null)
            {
                // Create the user if there is no user.
                user = new User
                {
                    ID = profile.ID,
                    Profile = profile
                };

                await _modelSaber.Users.AddAsync(user);
                await _modelSaber.SaveChangesAsync();
            }
            token = _jwtService.GenerateUserToken(user);
            return Ok(new { token });
        }

        /// <summary>
        /// Gets an authenticated user's profile.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("@me")]
        public async Task<IActionResult> GetSelf()
        {
            User user = await UserService.UserFromContext(HttpContext, _modelSaber);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
