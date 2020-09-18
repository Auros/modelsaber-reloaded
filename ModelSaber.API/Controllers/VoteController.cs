using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelSaber.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class VoteController : ControllerBase
    {
        private readonly ModelSaberContext _modelSaberContext;

        public VoteController(ModelSaberContext modelSaberContext)
        {
            _modelSaberContext = modelSaberContext;
        }

    }
}
