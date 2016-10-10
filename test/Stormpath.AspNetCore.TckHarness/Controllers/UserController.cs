using System;
using Microsoft.AspNetCore.Mvc;
using Stormpath.SDK.Account;

namespace Stormpath.AspNetCore.TestHarness.Controllers
{
    [Route("/user")]
    public class UserController : Controller
    {
        private readonly IAccount _account;

        public UserController(Lazy<IAccount> account)
        {
            _account = account.Value;
        }

        public IActionResult Get()
        {
            return Ok(_account?.Href);
        }
    }
}
