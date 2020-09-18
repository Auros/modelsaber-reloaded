using System;
using System.IO;
using System.Linq;
using ModelSaber.Common;
using System.Threading.Tasks;
using ModelSaber.API.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ModelSaber.API.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ModelSaber.API.Services;

namespace ModelSaber.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollectionsController : ControllerBase, ISource
    {
        private readonly IAuditor _auditor;
        private readonly ModelSaberContext _modelSaberContext;

        public string SourceName => "Collection";

        public CollectionsController(IAuditor auditor, ModelSaberContext modelSaberContext)
        {
            _auditor = auditor;
            _modelSaberContext = modelSaberContext;
        }

        [HttpGet]
        public IEnumerable<Collection> GetAllCollections()
        {
            return _modelSaberContext.Collections.Where(c => c != null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCollectionFromId(Guid id)
        {
            Collection collection = await _modelSaberContext.Collections.FirstOrDefaultAsync(c => c.Id == id);
            if (collection == null)
            {
                return NotFound();
            }
            return Ok(collection);
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetCollectionFromName(string name)
        {
            string toMatch = name.ToLowerInvariant().Replace('-', ' ');
            Collection collection = await _modelSaberContext.Collections.FirstOrDefaultAsync(c => c.Name.ToLower() == toMatch);
            if (collection == null)
            {
                return NotFound();
            }
            return Ok(collection);
        }

        [HttpPost]
        [Maulth(Role.Admin)]
        [RequestSizeLimit(100000000)]
        public async Task<IActionResult> UploadCollection([FromForm] UploadBody body)
        {
            if (body.Icon == null || body.Icon.Length == 0 || body.Name == null)
            {
                return BadRequest(new { error = "Invalid File Upload" });
            }
            if (await _modelSaberContext.Collections.AnyAsync(c => c.Name.ToLower() == body.Name.ToLower()))
            {
                return BadRequest(new { error = "Collection Already Exists" });
            }
            Collection collection = new Collection
            {
                Name = body.Name,
                Description = body.Description,
                DefaultVisibility = body.DefaultVisibility,
                DefaultInstallPath = body.DefaultInstallPath,
                DefaultApprovalStatus = body.DefaultApprovalStatus,
            };
            collection = (await _modelSaberContext.Collections.AddAsync(collection)).Entity;
            await _modelSaberContext.SaveChangesAsync();
            try
            {
                string fileExtension = Path.GetExtension(body.Icon.FileName);
                string saveFolder = Path.Combine("Files", "Collections", collection.Id.ToString());
                if (!Utilities.VerifyFileExtension(body.Icon.OpenReadStream(), fileExtension))
                    throw new Exception("Invalid Icon File. Must be a png, jpg, or gif.");
                if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
                string hash = body.Icon.OpenReadStream().ComputeHash();
                string iconPath = Path.Combine(saveFolder, hash + fileExtension);
                await Utilities.SaveIFormToFile(body.Icon, iconPath);
                collection.IconURL = "/" + iconPath.Replace("\\", "/").ToLower();
                _auditor.Audit(this, (User)HttpContext.Items["User"], $"uploaded a new collection {collection.Name}", collection.Id);
                await _modelSaberContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _modelSaberContext.Collections.Remove(collection);
                return BadRequest(new { error = e.Message });
            }
            return Ok(collection);
        }

        [HttpDelete("{id}")]
        [Maulth(Role.Admin)]
        public async Task<IActionResult> DeleteCollection(Guid id)
        {
            Collection collection = await _modelSaberContext.Collections.FirstOrDefaultAsync(c => c.Id == id);
            if (collection == null)
            {
                return NotFound();
            }
            _auditor.Audit(this, (User)HttpContext.Items["User"], $"deleted the collection {collection.Name}", collection.Id);
            _modelSaberContext.Collections.Remove(collection);
            return NoContent();
        }

        public class UploadBody
        {
            public string Name { get; set; }
            public IFormFile Icon { get; set; }
            public string Description { get; set; }
            public string DefaultInstallPath { get; set; }
            public Visibility DefaultVisibility { get; set; } = Visibility.Private;
            public ApprovalStatus DefaultApprovalStatus { get; set; } = ApprovalStatus.Unapproved;
        }
    }
}