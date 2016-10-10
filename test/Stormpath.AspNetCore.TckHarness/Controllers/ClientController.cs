using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stormpath.SDK.Client;

namespace Stormpath.AspNetCore.TestHarness.Controllers
{
    [Route("/client")]
    public class ClientController : Controller
    {
        private readonly IClient _client;

        public ClientController(IClient client)
        {
            _client = client;
        }

        public async Task<IActionResult> Get()
        {
            var tenant = await _client.GetCurrentTenantAsync();

            return Ok(tenant.Href);
        }
    }
}
