using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Stormpath.AspNetCore.DocExamples.Controllers
{
    #region code/authorization/aspnetcore/protected_route.cs
    [Authorize]
    public class ProfileController : Controller
    {
        // GET: /profile
        public IActionResult Index()
        {
            // [Authorize] will require a logged-in user for this action
            return View();
        }
    }
    #endregion
}
