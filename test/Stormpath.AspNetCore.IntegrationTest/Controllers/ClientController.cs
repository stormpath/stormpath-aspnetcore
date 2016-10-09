using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stormpath.SDK.Client;

namespace Stormpath.AspNetCore.IntegrationTest.Controllers
{
    [Route("/client")]
    public class ClientController : Controller
    {
        public readonly IClient _client;

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
