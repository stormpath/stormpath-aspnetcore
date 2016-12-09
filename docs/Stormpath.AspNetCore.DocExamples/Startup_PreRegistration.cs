using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stormpath.Configuration.Abstractions;

namespace Stormpath.AspNetCore.DocExamples
{
    public class Startup_PreRegistration
    {
        public void ConfigureServices_Basic(IServiceCollection services)
        {
            #region code/registration/aspnetcore/preregistration_handler.cs
            services.AddStormpath(new StormpathOptions()
            {
                Configuration = new StormpathConfiguration(), // existing config, if any
                PreRegistrationHandler = (context, ct) =>
                {
                    return Task.FromResult(0);
                }
            });
            #endregion
        }
    }
}
