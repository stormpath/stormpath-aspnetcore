using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Stormpath.AspNetCore.DocExamples
{
    public class Startup_Quickstart
    {
        #region code/quickstart/aspnetcore/configure_services.cs
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStormpath();

            // Add other services
        }
        #endregion

        #region code/quickstart/aspnetcore/configure.cs
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Logging and static file middleware (if applicable)

            app.UseStormpath();

            // MVC or other framework middleware here
        }
        #endregion
    }
}
