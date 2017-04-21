using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stormpath.Owin.Abstractions;

namespace Stormpath.AspNetCore.DocExamples.Controllers
{

    #region code/request_context/aspnetcore/injecting_lazy_account.cs
    public class InjectLazyAccountController : Controller
    {
        private readonly ICompatibleOktaAccount _account;

        public InjectLazyAccountController(Lazy<ICompatibleOktaAccount> lazyAccount)
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
    // TODO
    //public class UserModificationController : Controller
    //{
    //    private readonly IAccount _account;

    //    public UserModificationController(Lazy<IAccount> lazyAccount)
    //    {
    //        _account = lazyAccount.Value;
    //    }

    //    [HttpPost]
    //    [Authorize]
    //    public async Task<IActionResult> UpdatePassword(string newPassword)
    //    {
    //        if (_account != null)
    //        {
    //            var stormpathAccount = _account;
    //            stormpathAccount.SetPassword(newPassword);
    //            await stormpathAccount.SaveAsync();
    //        }

    //        return RedirectToAction("Index");
    //    }
    //}
    #endregion
}
