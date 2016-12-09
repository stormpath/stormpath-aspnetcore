using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Stormpath.AspNetCore.DocExamples
{
    public class CustomDataStartup
    {
        public void ConfigureServicesSnippet()
        {
            IServiceCollection services = null;

            #region code/authorization/aspnetcore/require_customData_startup.cs
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("CanPost", policy => policy.AddRequirements(
                    new StormpathCustomDataRequirement("canPost", true)));
            });
            #endregion

            #region code/authorization/aspnetcore/require_multiple_customData.cs
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("CanPostStickies", policy => policy
                .AddRequirements(new StormpathCustomDataRequirement("canPost", true))
                .AddRequirements(new StormpathCustomDataRequirement("userType", "admin")));
            });
            #endregion
        }
    }

    #region code/authorization/aspnetcore/require_customData_controller.cs
    [Authorize(Policy = "CanPost")]
    public class CreatePostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
    #endregion
}
