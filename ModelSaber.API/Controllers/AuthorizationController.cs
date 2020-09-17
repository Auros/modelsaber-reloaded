using Microsoft.AspNetCore.Mvc;

namespace ModelSaber.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ModelSaberContext _modelSaberContext;

        public AuthorizationController(ModelSaberContext modelSaberContext)
        {
            _modelSaberContext = modelSaberContext;
        }
    }
}
