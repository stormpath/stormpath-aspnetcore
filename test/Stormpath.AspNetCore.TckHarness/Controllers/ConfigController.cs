using Microsoft.AspNetCore.Mvc;
using Stormpath.Configuration.Abstractions.Immutable;

namespace Stormpath.AspNetCore.TestHarness.Controllers
{
    [Route("/config")]
    public class ConfigController : Controller
    {
        private readonly StormpathConfiguration _config;

        public ConfigController(StormpathConfiguration config)
        {
            _config = config;
        }

        public IActionResult Get()
        {
            return Ok(_config.Application.Id);
        }
    }
}
