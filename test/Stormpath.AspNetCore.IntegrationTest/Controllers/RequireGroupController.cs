using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Stormpath.AspNetCore.IntegrationTest.Controllers
{
    [Route("/requireGroup")]
    [Authorize(Policy = "AdminITGroup")]
    public class RequireGroupController : Controller
    {
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
