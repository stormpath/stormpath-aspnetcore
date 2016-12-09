using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stormpath.Configuration.Abstractions;

namespace Stormpath.AspNetCore.DocExamples
{
    public class Startup_PostReset
    {
        public void ConfigureServices_Basic(IServiceCollection services)
        {
            #region code/password_reset/aspnetcore/postreset_handler.cs
            services.AddStormpath(new StormpathOptions()
            {
                Configuration = new StormpathConfiguration(), // existing config, if any
                PostChangePasswordHandler = (context, ct) =>
                {
                    return Task.FromResult(0);
                }
            });
            #endregion
        }
    }
}
