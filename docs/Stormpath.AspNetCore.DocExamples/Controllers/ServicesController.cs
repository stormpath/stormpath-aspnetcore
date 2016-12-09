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
    #region code/request_context/aspnetcore/controller_injection.cs
    public class InjectedServicesController : Controller
    {
        private readonly IApplication _stormpathApplication;

        public InjectedServicesController(IApplication stormpathApplication)
        {
            _stormpathApplication = stormpathApplication;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
    #endregion

    #region code/request_context/aspnetcore/injecting_application.cs
    public class AccountsController : Controller
    {
        private readonly IApplication _application;

        public AccountsController(IApplication application)
        {
            _application = application;
        }
        
        [HttpGet]
        public async Task<IActionResult> FindAccountByEmail(string email)
        {
            var foundAccount = await _application.GetAccounts()
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

    #region code/request_context/aspnetcore/injecting_lazy_account.cs
    public class InjectLazyAccountController : Controller
    {
        private readonly IAccount _account;

        public InjectLazyAccountController(Lazy<IAccount> lazyAccount)
        {
            _account = lazyAccount.Value;
        }

        public IActionResult Index()
        {
            if (_account != null)
            {
                // Do something with the Account
            }

            return View();
        }
    }
    #endregion

    #region code/request_context/aspnetcore/update_user_password.cs
    public class UserModificationController : Controller
    {
        private readonly IAccount _account;

        public UserModificationController(Lazy<IAccount> lazyAccount)
        {
            _account = lazyAccount.Value;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdatePassword(string newPassword)
        {
            if (_account != null)
            {
                var stormpathAccount = _account;
                stormpathAccount.SetPassword(newPassword);
                await stormpathAccount.SaveAsync();
            }

            return RedirectToAction("Index");
        }
    }
    #endregion
}
