using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stormpath.Configuration.Abstractions;

namespace Stormpath.AspNetCore.DocExamples
{
    public class Startup_PreLogout
    {
        public void ConfigureServices_Basic(IServiceCollection services)
        {
            #region code/logout/aspnetcore/prelogout_handler.cs
            services.AddStormpath(new StormpathOptions()
            {
                Configuration = new StormpathConfiguration(), // existing config, if any
                PreLogoutHandler = (context, ct) =>
                {
                    return Task.FromResult(0);
                }
            });
            #endregion
        }
    }
}
