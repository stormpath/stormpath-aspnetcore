using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Stormpath.AspNetCore.DocExamples
{
    public class Startup_Setup
    {
        #region code/csharp/setup/configure_services.cs
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStormpath();

            // Add other services
        }
        #endregion

        #region code/csharp/setup/configure.cs
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Logging and static file middleware (if applicable)

            app.UseStormpath();

            // MVC or other framework middleware here
        }
        #endregion
    }
}
