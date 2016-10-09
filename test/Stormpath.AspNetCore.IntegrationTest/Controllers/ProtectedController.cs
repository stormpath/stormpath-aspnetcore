using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Stormpath.AspNetCore.IntegrationTest.Controllers
{
    [Route("/protected")]
    [Authorize]
    public class ProtectedController : Controller
    {
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
