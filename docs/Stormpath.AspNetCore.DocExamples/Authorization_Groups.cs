using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Stormpath.AspNetCore.DocExamples
{
    public class GroupsStartup
    {
        public void ConfigureServicesSnippet()
        {
            IServiceCollection services = null;

            #region code/authorization/aspnetcore/require_single_group_startup.cs
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AdminOnly", policy => policy.AddRequirements(
                    new StormpathGroupsRequirement("admin")));
            });
            #endregion

            #region code/authorization/aspnetcore/require_group_by_href.cs
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("SpecifyGroupByHref", policy => policy.AddRequirements(
                    new StormpathGroupsRequirement("https://api.stormpath.com/v1/groups/aRaNdOmGrOuPiDhEre")));
            });
            #endregion

            #region code/authorization/aspnetcore/require_any_group.cs
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("PayingMembersOnly", policy => policy.AddRequirements(
                    new StormpathGroupsRequirement("subscriber", "partner")));
            });
            #endregion

            #region code/authorization/aspnetcore/require_multiple_groups.cs
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AdminManagers", policy => policy
                    .AddRequirements(new StormpathGroupsRequirement("admin"))
                    .AddRequirements(new StormpathGroupsRequirement("manager")));
            });
            #endregion
        }
    }

    #region code/authorization/aspnetcore/require_single_group_controller.cs
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
        // Only users in the admin group can access these actions

        public IActionResult Index()
        {
            return View();
        }
    }
    #endregion
}
