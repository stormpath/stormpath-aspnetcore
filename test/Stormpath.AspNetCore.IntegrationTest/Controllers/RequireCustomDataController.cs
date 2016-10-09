using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Stormpath.AspNetCore.IntegrationTest.Controllers
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
