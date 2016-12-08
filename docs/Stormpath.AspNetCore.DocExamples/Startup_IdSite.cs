using Microsoft.Extensions.DependencyInjection;
using Stormpath.Configuration.Abstractions;

namespace Stormpath.AspNetCore.DocExamples
{
    public class Startup_IdSite
    {
        public void ConfigureServices_IdSite(IServiceCollection services)
        {
            #region code/id_site/aspnetcore/enable_idsite.cs
            services.AddStormpath(new StormpathConfiguration()
            {
                Web = new WebConfiguration()
                {
                    IdSite = new WebIdSiteConfiguration()
                    {
                        Enabled = true
                    }
                }
            });
            #endregion
        }
    }
}
