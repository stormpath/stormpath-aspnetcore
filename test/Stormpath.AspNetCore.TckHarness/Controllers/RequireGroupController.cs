using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Stormpath.AspNetCore.TestHarness.Controllers
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
