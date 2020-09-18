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

        [Maulth]
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<Model> GetModels(int page = 0, int count = 25)
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

        [Maulth]
        [AllowAnonymous]
        [HttpGet("{id}")]
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
            return Ok();
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
