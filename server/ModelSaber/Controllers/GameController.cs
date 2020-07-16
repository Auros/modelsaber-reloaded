using ModelSaber.Database;
using ModelSaber.Models.Game;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ModelSaber.Services;
using ModelSaber.Models.User;
using System.Linq;
using System;

namespace ModelSaber.Controllers
{
    [Route("api/assist/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly ModelSaberContext _modelSaber;

        public GameController(UserService userService, ModelSaberContext modelSaber)
        {
            _modelSaber = modelSaber;
            _userService = userService;
        }

        /// <summary>
        /// Get a Game based on its formatted title or ID
        /// </summary>
        /// <param name="gameIdentifier"></param>
        /// <returns></returns>
        [HttpGet("{gameIdentifier}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGame(string gameIdentifier)
        {
            // Check human readable game format first.
            Game game = await _modelSaber.Games.FirstOrDefaultAsync(g => g.Title.ToLower() == Utilities.FormattedGameName(gameIdentifier));
            if (game != null)
            {
                // We've found the game object, now return it.
                return Ok(game);
            }
            
            // It appears we weren't able to find the game through its ID. Let's check if the ID put in was its actual ID
            //   Convert the gameIdentifier to a unsigned integer.
            if (uint.TryParse(gameIdentifier, out uint id))
            {
                game = await _modelSaber.Games.FirstOrDefaultAsync(g => g.ID == id);
                if (game != null)
                {
                    return Ok(game);
                }
            }
            return NotFound();
        }

        /// <summary>
        /// Creates a game from the specified body content.
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [RequestSizeLimit(15000000)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateGame([FromForm] UploadGame upload)
        {
            User user = await UserService.UserFromContext(HttpContext, _modelSaber);
            if (!user.Permissions.Contains("*.*"))
            {
                return Unauthorized();
            }
            bool exists = await _modelSaber.Games.AnyAsync(g => g.Title.ToLower() == upload.Title.ToLower());
            if (upload.Icon == null || upload.Icon.Length == 0 || exists)
            {
                return BadRequest();
            }
            Game game = new Game
            {
                Title = upload.Title,
                Created = DateTime.UtcNow,
                Visibility = upload.Visibility,
                Description = upload.Description
            };
            return Ok(game);
        }
    }
}