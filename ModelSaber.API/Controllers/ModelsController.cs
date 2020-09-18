using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelSaber.API.Interfaces;
using ModelSaber.API.Security;
using ModelSaber.API.Services;
using ModelSaber.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ModelSaber.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase, ISource
    {
        private readonly IAuditor _auditor;
        private readonly ModelSaberContext _modelSaberContext;

        public string SourceName => "Model";

        public ModelsController(IAuditor auditor, ModelSaberContext modelSaberContext)
        {
            _auditor = auditor;
            _modelSaberContext = modelSaberContext;
        }

        [HttpGet]
        [Maulth(AllowAnonymous = true)]
        public IEnumerable<Model> GetModels([FromQuery(Name = "page")] int page = 0, [FromQuery(Name = "page")] int count = 25)
        {
            if (0 > page) page = 0;
            if (count > 25) count = 25;
            if (0 > count) return Array.Empty<Model>();
            User visitor = (User)HttpContext.Items["User"];

            if (visitor != null)
            {
                if (visitor.Role.HasFlag(Role.Manager))
                {
                    _modelSaberContext.Models.Skip(page * count).Take(count);
                }
                return _modelSaberContext.Models.Where(x => (x.Uploader.Id == visitor.Id) || (x.Status == ApprovalStatus.Approved && x.Visibility == Visibility.Public)).Skip(page * count).Take(count);
            }
            return _modelSaberContext.Models.Where(x => x.Status == ApprovalStatus.Approved && x.Visibility == Visibility.Public).Skip(page * count).Take(count);
        }

        [HttpGet("{id}")]
        [Maulth(AllowAnonymous = true)]
        public async Task<IActionResult> GetModel(Guid id)
        {
            User visitor = (User)HttpContext.Items["User"];
            Model model = await _modelSaberContext.Models.FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            if (!visitor.Role.HasFlag(Role.Manager))
            {
                if ((visitor.Id == model.Uploader.Id) || (model.Status == ApprovalStatus.Approved && model.Visibility == Visibility.Public))
                    return Ok(model);
                return Forbid();
            }
            return Ok(model);
        }

        [HttpPost]
        [Maulth(Role.Uploader)]
        [RequestSizeLimit(100000000)]
        public async Task<IActionResult> UploadModel([FromForm] UploadBody body)
        {
            if (body.Model == null || body.Model.Length == 0)
            {
                return BadRequest(new { error = "Model File Invalid" });
            }
            if (body.Thumbnail == null || body.Thumbnail.Length == 0)
            {
                return BadRequest(new { error = "Thumbnail File Invalid" });
            }
            if (string.IsNullOrWhiteSpace(body.Name))
            {
                return BadRequest(new { error = "Name Required" });
            }
            Collection collection = await _modelSaberContext.Collections.FirstOrDefaultAsync(c => c.Id == body.CollectionId);
            if (collection == null)
            {
                return NotFound(new { error = "Collection does not exist" });
            }
            User uploader = (User)HttpContext.Items["User"];
            Model model = new Model
            {
                Name = body.Name,
                Tags = body.Tags,
                DownloadCount = 0,
                Uploader = uploader,
                Collection = collection,
                Description = body.Description ?? ""
            };
            if ((!uploader.Role.HasFlag(Role.Trusted) && body.Status == ApprovalStatus.Approved) ||
                (!uploader.Role.HasFlag(Role.Trusted) && body.Visibility == Visibility.Public) ||  
                (!uploader.Role.HasFlag(Role.Verified) && body.Visibility == Visibility.Unlisted) ||
                (body.Status == ApprovalStatus.Denied))
            {
                return NotFound(new { error = "??? Please stop???" });
            }
            model.Status = body.Status;
            model.Visibility = body.Visibility;
            model = (await _modelSaberContext.Models.AddAsync(model)).Entity;
            await _modelSaberContext.SaveChangesAsync();
            try
            {
                // Save Model File
                string saveFolder = Path.Combine("Files", "Models", model.Id.ToString());
                if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
                string modelFileExtension = Path.GetExtension(body.Model.FileName);
                if (modelFileExtension == ".zip")
                {
                    if (!Utilities.IsZIP(body.Model.OpenReadStream()))
                        throw new Exception("File is not a valid ZIP");
                    model.FileType = FileType.Archive;
                }
                else
                {
                    model.FileType = FileType.Single;
                }
                string modelHash = body.Model.OpenReadStream().ComputeHash(HashType.MD5);
                if (await _modelSaberContext.Models.AnyAsync(m => m.Hash == modelHash))
                {
                    throw new Exception("Model Already Exists");
                }
                string modelPath = Path.Combine(saveFolder, modelHash + modelFileExtension);
                await Utilities.SaveIFormToFile(body.Model, modelPath);
                model.DownloadURL = "/" + modelPath.Replace("\\", "/").ToLower();
                model.Hash = modelHash;

                // Save Thumbnail
                string thumbnailFileExtension = Path.GetExtension(body.Thumbnail.FileName);
                if (!Utilities.IsFileExtensionValid(body.Thumbnail.OpenReadStream(), thumbnailFileExtension))
                    throw new Exception("Invalid Thumbnail File. Must be a png, jpg, apng, or gif.");
                string thumbnailHash = body.Thumbnail.OpenReadStream().ComputeHash(HashType.SHA256);
                string thumbnailPath = Path.Combine(saveFolder, thumbnailHash + thumbnailFileExtension);
                await Utilities.SaveIFormToFile(body.Thumbnail, thumbnailPath);
                model.ThumbnailURL = "/" + thumbnailPath.Replace("\\", "/").ToLower();

                // Save Model
                model.UploadDate = DateTime.UtcNow;
                await _modelSaberContext.SaveChangesAsync();
                _auditor.Audit(this, uploader, $"uploaded a new model.", model.Id);
            }
            catch (Exception e)
            {
                _modelSaberContext.Models.Remove(model);
                await _modelSaberContext.SaveChangesAsync();
                return BadRequest(new { error = e.Message });
            }
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModel(Guid id)
        {
            Model model = await _modelSaberContext.Models.FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            _auditor.Audit(this, (User)HttpContext.Items["User"], $"deleted the model {model.Name}", model.Id);
            _modelSaberContext.Models.Remove(model);
            await _modelSaberContext.SaveChangesAsync();
            return NoContent();
        }

        [Maulth(Role.Manager)]
        [HttpPost("update/{id}")]
        public async Task<IActionResult> ApproveModel(Guid id, [FromQuery(Name = "public")] bool makePublic)
        {
            Model model = await _modelSaberContext.Models.FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            model.Status = ApprovalStatus.Approved;
            if (makePublic)
            {
                model.Visibility = Visibility.Public;
                model.Status = ApprovalStatus.Approved;
            }
            await _modelSaberContext.SaveChangesAsync();
            _auditor.Audit(this, (User)HttpContext.Items["User"], $"approved a model", model.Id);
            return Ok(model);
        }

        public async Task<IActionResult> DenyModel(Guid id)
        {
            Model model = await _modelSaberContext.Models.FirstOrDefaultAsync(m => m.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            model.Status = ApprovalStatus.Denied;
            if (model.Visibility == Visibility.Public)
            {
                model.Visibility = Visibility.Private;
            }
            await _modelSaberContext.SaveChangesAsync();
            _auditor.Audit(this, (User)HttpContext.Items["User"], $"denied a model", model.Id);
            return Ok(model);
        }

        public class UploadBody
        {
            public string Name { get; set; }
            public string[] Tags { get; set; } = Array.Empty<string>();
            public IFormFile Model { get; set; }
            public Guid CollectionId { get; set; }
            public string Description { get; set; }
            public IFormFile Thumbnail { get; set; }
            public Visibility Visibility { get; set; } = Visibility.Private;
            public ApprovalStatus Status { get; set; } = ApprovalStatus.Unapproved;
        }
    }
}
