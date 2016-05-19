using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Stormpath.SDK;
using Stormpath.SDK.Account;
using Stormpath.SDK.Application;

namespace Stormpath.AspNetCore.DocExamples.Controllers
{
    #region code/request_objects/aspnetcore/controller_fromservices.cs
    public class ServicesController : Controller
    {
        [FromServices]
        public IApplication StormpathApplication { get; set; }

        public IActionResult Index()
        {
            return View();
        }
    }
    #endregion

    #region code/request_objects/aspnetcore/controller_injection.cs
    public class InjectedServicesController : Controller
    {
        public IApplication StormpathApplication { get; private set; }

        public InjectedServicesController(IApplication stormpathApplication)
        {
            this.StormpathApplication = stormpathApplication;
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
        [FromServices]
        public IApplication StormpathApplication { get; set; }
        
        [HttpGet]
        public async Task<IActionResult> FindAccountByEmail(string email)
        {
            var foundAccount = await StormpathApplication.GetAccounts()
                     .Where(a => a.Email == email)
                     .SingleOrDefaultAsync();

            if (foundAccount == null)
            {
                return Ok("No accounts found.");
            }
            else
            {
                return Ok(foundAccount.FullName);
            }
        }
    }
    #endregion

    public class UserModificationController : Controller
    {
        #region code/request_objects/aspnetcore/update_user_password.cs
        [FromServices]
        public Lazy<IAccount> Account { get; set; }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(string newPassword)
        {
            if (Account.Value != null)
            {
                var stormpathAccount = Account.Value;
                stormpathAccount.SetPassword(newPassword);
                await stormpathAccount.SaveAsync();
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
