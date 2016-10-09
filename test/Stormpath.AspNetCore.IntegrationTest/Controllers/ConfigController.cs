using Microsoft.AspNetCore.Mvc;
using Stormpath.Configuration.Abstractions.Immutable;

namespace Stormpath.AspNetCore.IntegrationTest.Controllers
{
    [Route("/config")]
    public class ConfigController : Controller
    {
        public readonly StormpathConfiguration _config;

        public ConfigController(StormpathConfiguration config)
        {
            _config = config;
        }

        public IActionResult Get()
        {
            return Ok(_config.Application.Name);
        }
    }
}
