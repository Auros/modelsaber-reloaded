using System;
using System.Linq;
using ModelSaber.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ModelSaber.API.Security;
using System.IO;
using ModelSaber.API.Services;
using ModelSaber.API.Interfaces;

namespace ModelSaber.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class PlaylistController : ControllerBase, ISource
    {
        private readonly IAuditor _auditor;
        private readonly ModelSaberContext _modelSaberContext;

        public string SourceName => "Playlist";

        public PlaylistController(IAuditor auditor, ModelSaberContext modelSaberContext)
        {
            _auditor = auditor;
            _modelSaberContext = modelSaberContext;
        }

        [HttpGet]
        public IEnumerable<Playlist> GetPlaylists([FromQuery(Name = "page")] int page = 0, [FromQuery(Name = "count")] int count = 10)
        {
            if (0 > page) page = 0;
            if (count > 10) count = 10;
            if (0 > count) return Array.Empty<Playlist>();

            return _modelSaberContext.Playlists.Skip(page * count).Take(count);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlaylist(Guid id)
        {
            Playlist playlist = await _modelSaberContext.Playlists.FirstOrDefaultAsync(p => p.Id == id);
            if (playlist == null)
            {
                return NotFound();
            }
            return Ok(playlist);
        }

        [HttpPost]
        [Maulth(Role.Uploader)]
        public async Task<IActionResult> CreatePlaylist([FromForm] UploadBody body)
        {
            if (body.Thumbnail == null || body.Thumbnail.Length == 0)
            {
                return BadRequest(new { error = "Thumbnail File Invalid" });
            }
            if (string.IsNullOrWhiteSpace(body.Name))
            {
                return BadRequest(new { error = "Name Required" });
            }
            User uploader = (User)HttpContext.Items["User"];
            Playlist playlist = new Playlist
            {
                User = uploader,
                Name = body.Name,
                Description = body.Description,
            };
            foreach (var modelId in body.InitialModels)
            {
                Model model = await _modelSaberContext.Models.FirstOrDefaultAsync(m => m.Id == modelId);
                if (model != null)
                {
                    playlist.Models.Add(model);
                }
            }
            playlist = (await _modelSaberContext.Playlists.AddAsync(playlist)).Entity;
            await _modelSaberContext.SaveChangesAsync();
            try
            {
                string fileExtension = Path.GetExtension(body.Thumbnail.FileName);
                string saveFolder = Path.Combine("Files", "Playlists", playlist.Id.ToString());
                if (Utilities.IsFileExtensionValid(body.Thumbnail.OpenReadStream(), fileExtension))
                    throw new Exception("Invalid Thumbnail File. Must be a png, jpg, apng, or gif.");
                if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
                string hash = body.Thumbnail.OpenReadStream().ComputeHash(HashType.SHA256);
                string thumbnailPath = Path.Combine(saveFolder, hash + fileExtension);
                await Utilities.SaveIFormToFile(body.Thumbnail, thumbnailPath);
                playlist.ThumbnailURL = "/" + thumbnailPath.Replace("\\", "/").ToLower();
                await _modelSaberContext.SaveChangesAsync();
                _auditor.Audit(this, uploader, $"created a new playlist {playlist.Name}", playlist.Id);
            }
            catch (Exception e)
            {
                _modelSaberContext.Playlists.Remove(playlist);
                await _modelSaberContext.SaveChangesAsync();
                return BadRequest(new { error = e.Message });
            }
            return Ok(playlist);
        }

        [Maulth]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlaylist(Guid id)
        {
            User user = (User)HttpContext.Items["User"];
            Playlist playlist = await _modelSaberContext.Playlists.FirstOrDefaultAsync(p => p.Id == id);
            if (playlist == null)
            {
                return NotFound();
            }
            if (!(user.Role.HasFlag(Role.Admin) || user.Id == playlist.User.Id))
            {
                return Forbid();
            }
            _auditor.Audit(this, user, $"deleted the playlist {playlist.Name}", playlist.Id);
            _modelSaberContext.Playlists.Remove(playlist);
            await _modelSaberContext.SaveChangesAsync();
            return NoContent();
        } 

        public class UploadBody
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public IFormFile Thumbnail { get; set; }
            public Guid[] InitialModels { get; set; } = Array.Empty<Guid>();
        }
    }
}