using System;
using System.Linq;
using ModelSaber.Common;
using ModelSaber.API.Models;
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
    public class VoteController : ControllerBase
    {
        private readonly ModelSaberContext _modelSaberContext;

        public VoteController(ModelSaberContext modelSaberContext)
        {
            _modelSaberContext = modelSaberContext;
        }

        [Maulth]
        [HttpPost("{source}")]
        public async Task<IActionResult> Vote(Guid source, [FromQuery(Name = "upvote")] bool isUpvote)
        {
            if (!await ItemExistsInDatabase(source))
            {
                return NotFound();
            }
            User voter = (User)HttpContext.Items["User"];
            Vote vote = await _modelSaberContext.Votes.FirstOrDefaultAsync(v => v.Source == source && v.Voter.Id == voter.Id);
            if (vote == null)
            {
                vote = (await _modelSaberContext.Votes.AddAsync(new Vote { Source = source, Voter = voter, IsUpvote = isUpvote })).Entity;
                await _modelSaberContext.SaveChangesAsync();
            }
            else
            {
                if (isUpvote == vote.IsUpvote)
                {
                    _modelSaberContext.Votes.Remove(vote);
                    await _modelSaberContext.SaveChangesAsync();
                    return NoContent();
                }
                else
                {
                    vote.IsUpvote = !isUpvote;
                    await _modelSaberContext.SaveChangesAsync();
                }
            }
            return Ok(vote);
        }

        [HttpGet("{source}")]
        public IActionResult GetVotes(Guid source)
        {
            IEnumerable<Vote> votes = _modelSaberContext.Votes.Where(v => v.Source == source);
            return Ok(new VoteStats { Upvotes = votes.Where(v => v.IsUpvote).Count(), Downvotes = votes.Where(v => !v.IsUpvote).Count() });
        }

        [HttpGet("@me/{source}")]
        public async Task<IActionResult> GetVoteFromSelf(Guid source)
        {
            User voter = (User)HttpContext.Items["User"];
            Vote vote = await _modelSaberContext.Votes.FirstOrDefaultAsync(v => v.Source == source && v.Voter.Id == voter.Id);
            if (vote == null)
            {
                return NotFound();
            }
            return Ok(vote);
        }

        public async Task<bool> ItemExistsInDatabase(Guid id)
        {
            Type[] types = new Type[] { typeof(User), typeof(Model), typeof(Comment), typeof(Playlist) };
            for (int i = 0; i < types.Length; i++)
            {
                object obj = await _modelSaberContext.FindAsync(types[i], id);
                if (obj != null) return true;
            }
            return false;
        }
    }
}
