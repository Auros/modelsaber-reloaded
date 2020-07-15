using ModelSaber.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelSaber.Models.Discord;
using Microsoft.Extensions.Logging;

namespace ModelSaber.Controllers
{
    [Route("api/assist/[controller]")]
    [ApiController]
    public class GraphQLController : ControllerBase
    {
        public GraphQLController()
        {

        }
    }
}