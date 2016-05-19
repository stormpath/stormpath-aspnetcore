using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stormpath.Configuration.Abstractions;

namespace Stormpath.AspNetCore.DocExamples
{
    public class Startup_ViewTemplates
    {
        public void ConfigureServices_CustomView(IServiceCollection services)
        {
            #region code/templates/aspnetcore/custom_view.cs
            services.AddStormpath(new StormpathConfiguration
            {
                Web = new WebConfiguration
                {
                    Login = new WebLoginRouteConfiguration
                    {
                        View = "~/Views/Login/MyLogin.cshtml"
                    }
                }
            });
            #endregion
        }
    }
}
