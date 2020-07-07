using ModelSaber.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelSaber.Models.Discord;
using Microsoft.Extensions.Logging;

namespace ModelSaber.Controllers
{
    [Route("api/assist/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {

        private readonly DiscordService _discordService;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(DiscordService discordService, ILogger<AuthorizationController> logger)
        {
            _logger = logger;
            _discordService = discordService;
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
            DiscordUser profile = await _discordService.GetProfile(token);
            return Ok(profile);
        }
    }
}
