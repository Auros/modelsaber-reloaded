using System;
using System.IO;
using ModelSaber.Models;
using ModelSaber.Database;
using ModelSaber.Services;
using ModelSaber.Models.User;
using System.Threading.Tasks;
using ModelSaber.Models.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ModelSaber.Controllers
{
    [ApiController]
    [Route("api/assist/[controller]")]
    public class ModelCollectionController : ControllerBase
    {
        private readonly ModelSaberContext _modelSaber;

        public ModelCollectionController(ModelSaberContext modelSaber)
        {
            _modelSaber = modelSaber;
        }

        /// <summary>
        /// Gets a Collection on its formatted title or ID
        /// </summary>
        /// <param name="collectionIdentifier"></param>
        /// <returns></returns>
        [HttpGet("{collectionIdentifier}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCollection(string collectionIdentifier)
        {
            // Check human readable collection format first.
            ModelCollection modelCollection = await _modelSaber.ModelCollections.FirstOrDefaultAsync(c => c.Name.ToLower() == Utilities.FormattedGameName(collectionIdentifier));
            if (modelCollection != null)
            {
                // We've found the collection object, now return it.
                return Ok(modelCollection);
            }

            // It appears we weren't able to find the collection through its ID. Let's check if the ID put in was its actual ID
            //   Convert the collectionIdentifier to a unsigned integer.
            if (ulong.TryParse(collectionIdentifier, out ulong id))
            {
                modelCollection = await _modelSaber.ModelCollections.FirstOrDefaultAsync(c => c.ID == id);
                if (modelCollection != null)
                {
                    return Ok(modelCollection);
                }
            }
            return NotFound();
        }

        /// <summary>
        /// Creates a Collection from the specified body content.
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [RequestSizeLimit(15000000)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateCollection([FromForm] UploadModelCollection upload)
        {
            User user = await UserService.UserFromContext(HttpContext, _modelSaber);
            if (!user.Role.HasFlag(ModelSaberRole.Admin))
            {
                return Unauthorized();
            }
            string fileExtension = Path.GetExtension(upload.Icon.FileName);
            bool exists = await _modelSaber.ModelCollections.AnyAsync(c => c.Name == upload.Name);
            if (upload.Icon == null || upload.Icon.Length == 0 || exists || !Utilities.VerifyImageFileExtension(upload.Icon.OpenReadStream(), fileExtension))
            {
                return BadRequest();
            }
            bool gameExists = await _modelSaber.Games.AnyAsync(g => g.ID == upload.GameID);
            if (!gameExists)
            {
                return NotFound();
            }

            ModelCollection modelCollection = new ModelCollection
            {
                ID = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Name = upload.Name,
                GameID = upload.GameID,
                Created = DateTime.UtcNow,
                Visibility = upload.Visibility,
                Description = upload.Description,
                InstallPath = upload.InstallPath,
                FileExtension = upload.FileExtension
            };

            string saveFolder = Path.Combine("files", user.ID, "images");
            string saveLocation = Path.Combine("files", modelCollection.ID + fileExtension);

            Directory.CreateDirectory(saveFolder);
            await Utilities.SaveIFormToFile(upload.Icon, saveLocation);
            modelCollection.IconURL = "/" + saveLocation.Replace("\\", "/");
            _modelSaber.ModelCollections.Add(modelCollection);
            await _modelSaber.SaveChangesAsync();
            return Ok(modelCollection);
        }
    }
}