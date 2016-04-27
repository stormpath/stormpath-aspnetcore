using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Stormpath.SDK.Account;
using Stormpath.SDK.Application;

namespace Stormpath.AspNetCore.DocExamples.Controllers
{
    #region code/csharp/injecting_objects/controller_fromservices.cs
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

    #region code/csharp/injecting_objects/controller_injection.cs
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

    #region code/csharp/injecting_objects/injecting_user.cs
    public class UserController : Controller
    {
        [FromServices]
        public Lazy<IAccount> Account { get; set; }

        public async Task<IActionResult> Index()
        {
            // If the request is authenticated, do something with the account
            // (like get the account's Custom Data):
            if (Account.Value != null)
            {
                var customData = await Account.Value.GetCustomDataAsync();
            }

            return View();
        }
    }
    #endregion

    public class UserModificationController : Controller
    {
        #region code/csharp/stormpath_objects/update_user_password.cs
        [FromServices]
        public Lazy<IAccount> Account { get; set; }

        
        [HttpPost]
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
