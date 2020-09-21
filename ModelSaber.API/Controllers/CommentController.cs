using System;
using System.Linq;
using ModelSaber.Common;
using System.Threading.Tasks;
using ModelSaber.API.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ModelSaber.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ModelSaberContext _modelSaberContext;

        public CommentController(ModelSaberContext modelSaberContext)
        {
            _modelSaberContext = modelSaberContext;
        }

        [Maulth]
        [HttpPost("{source}")]
        public async Task<IActionResult> Comment([FromBody] CommentBody body)
        {
            if (string.IsNullOrWhiteSpace(body.Comment))
            {
                return BadRequest(new { error = "Comment cannot be empty." });
            }
            if (!await ItemExistsInDatabase(body.Source))
            {
                return NotFound();
            }
            User commenter = (User)HttpContext.Items["User"];
            Comment comment = new Comment
            {
                Source = body.Source,
                Commenter = commenter,
                Message = body.Comment,
                Time = DateTime.UtcNow,
            };
            await _modelSaberContext.Comments.AddAsync(comment);
            await _modelSaberContext.SaveChangesAsync();
            return Ok(comment);
        }

        [HttpDelete("{source}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            Comment comment = await _modelSaberContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            _modelSaberContext.Comments.Remove(comment);
            await _modelSaberContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{source}")]
        public IActionResult GetComments(Guid source)
        {
            IEnumerable<Comment> comments = _modelSaberContext.Comments.Where(c => c.Source == source);
            return Ok(comments);
        }

        public async Task<bool> ItemExistsInDatabase(Guid id)
        {
            Type[] types = new Type[] { typeof(User), typeof(Model), typeof(Playlist) };
            for (int i = 0; i < types.Length; i++)
            {
                object obj = await _modelSaberContext.FindAsync(types[i], id);
                if (obj != null) return true;
            }
            return false;
        }

        public class CommentBody
        {
            public Guid Source { get; set; }
            public string Comment { get; set; }
        }
    }
}