using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Stormpath.AspNetCore.TestHarness.Controllers
{
    [Route("/requireCustomData")]
    [Authorize(Policy = "CustomDataIT")]
    public class RequireCustomDataController : Controller
    {
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
