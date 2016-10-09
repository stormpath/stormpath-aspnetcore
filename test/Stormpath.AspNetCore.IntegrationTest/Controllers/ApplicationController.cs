using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stormpath.SDK.Application;

namespace Stormpath.AspNetCore.IntegrationTest.Controllers
{
    [Route("/application")]
    public class ApplicationController : Controller
    {
        public readonly IApplication _application;

        public ApplicationController(IApplication application)
        {
            _application = application;
        }

        public IActionResult Get()
        {
            return Ok(_application.Href);
        }
    }
}
