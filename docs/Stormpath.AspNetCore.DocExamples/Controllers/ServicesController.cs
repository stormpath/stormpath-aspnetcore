using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stormpath.SDK;
using Stormpath.SDK.Account;
using Stormpath.SDK.Application;

namespace Stormpath.AspNetCore.DocExamples.Controllers
{
    #region code/request_objects/aspnetcore/controller_injection.cs
    public class InjectedServicesController : Controller
    {
        private readonly IApplication stormpathApplication;

        public InjectedServicesController(IApplication stormpathApplication)
        {
            this.stormpathApplication = stormpathApplication;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
    #endregion

    #region code/request_objects/aspnetcore/injecting_application.cs
    public class AccountsController : Controller
    {
        private readonly IApplication application;

        public AccountsController(IApplication application)
        {
            this.application = application;
        }
        
        [HttpGet]
        public async Task<IActionResult> FindAccountByEmail(string email)
        {
            var foundAccount = await application.GetAccounts()
                     .Where(a => a.Email == email)
                     .SingleOrDefaultAsync();

            if (foundAccount == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(foundAccount.FullName);
            }
        }
    }
    #endregion

    #region code/request_objects/aspnetcore/update_user_password.cs
    public class UserModificationController : Controller
    {
        private readonly Lazy<IAccount> account;

        public UserModificationController(Lazy<IAccount> account)
        {
            this.account = account;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(string newPassword)
        {
            if (account.Value != null)
            {
                var stormpathAccount = account.Value;
                stormpathAccount.SetPassword(newPassword);
                await stormpathAccount.SaveAsync();
            }

            return RedirectToAction("Index");
        }
    }
    #endregion
}
