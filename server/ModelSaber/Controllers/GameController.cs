using System;
using System.IO;
using System.Linq;
using ModelSaber.Models;
using ModelSaber.Services;
using ModelSaber.Database;
using ModelSaber.Models.User;
using ModelSaber.Models.Game;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ModelSaber.Controllers
{
    [Route("api/assist/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly ModelSaberContext _modelSaber;

        public GameController(ModelSaberContext modelSaber)
        {
            _modelSaber = modelSaber;
        }

        /// <summary>
        /// Gets all the public games.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IAsyncEnumerable<Game> GetGames()
        {
            return _modelSaber.Games.Where(g => g.Visibility == Visibility.Public).AsAsyncEnumerable();
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
            if (ulong.TryParse(gameIdentifier, out ulong id))
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
        /// Creates a Game from the specified body content.
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
            if (!user.Role.HasFlag(ModelSaberRole.Admin))
            {
                return Unauthorized();
            }
            string fileExtension = Path.GetExtension(upload.Icon.FileName);
            bool exists = await _modelSaber.Games.AnyAsync(g => g.Title.ToLower() == upload.Title.ToLower());
            if (upload.Icon == null || upload.Icon.Length == 0 || exists || !Utilities.VerifyImageFileExtension(upload.Icon.OpenReadStream(), fileExtension))
            {
                return BadRequest();
            }

            Game game = new Game
            {
                ID = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Title = upload.Title,
                Created = DateTime.UtcNow,
                Visibility = upload.Visibility,
                Description = upload.Description
            };
            string saveFolder = Path.Combine("files", user.ID, "images");
            string saveLocation = Path.Combine(saveFolder, game.ID + fileExtension);

            Directory.CreateDirectory(saveFolder);
            await Utilities.SaveIFormToFile(upload.Icon, saveLocation);
            game.IconURL = "/" + saveLocation.Replace("\\", "/");
            _modelSaber.Games.Add(game);
            await _modelSaber.SaveChangesAsync();
            return Ok(game);
        }
    }
}