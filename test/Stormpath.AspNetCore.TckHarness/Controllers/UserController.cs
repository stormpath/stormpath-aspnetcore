using System;
using Microsoft.AspNetCore.Mvc;
using Stormpath.Owin.Abstractions;

namespace Stormpath.AspNetCore.TestHarness.Controllers
{
    [Route("/user")]
    public class UserController : Controller
    {
        private readonly ICompatibleOktaAccount _account;

        public UserController(Lazy<ICompatibleOktaAccount> account)
        {
            _account = account.Value;
        }

        public IActionResult Get()
        {
            return Ok(_account?.Href);
        }
    }
}
