using System;
using Microsoft.AspNetCore.Mvc;
using Stormpath.SDK.Account;

namespace Stormpath.AspNetCore.IntegrationTest.Controllers
{
    [Route("/user")]
    public class UserController : Controller
    {
        public readonly IAccount _account;

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
