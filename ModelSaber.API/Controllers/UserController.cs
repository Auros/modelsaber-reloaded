using System;
using ModelSaber.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ModelSaber.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ModelSaberContext _modelSaberContext;

        public UserController(ModelSaberContext modelSaberContext)
        {
            _modelSaberContext = modelSaberContext;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            bool didParse = Guid.TryParse(id, out Guid guid);
            User user = null;
            if (didParse)
            {
                user = await _modelSaberContext.Users.FirstOrDefaultAsync(u => u.Id == guid);
            }
            else
            {
                user = await _modelSaberContext.Users.FirstOrDefaultAsync(u => id == u.Profile.Id);
            }
            if (user == null)
            {
                return NotFound(new { error = "User not found." });
            }
            return Ok(user);
        }
    }
}