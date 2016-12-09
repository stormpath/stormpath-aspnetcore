using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stormpath.Configuration.Abstractions;

namespace Stormpath.AspNetCore.DocExamples
{
    public class Startup_PostVerify
    {
        public void ConfigureServices_Basic(IServiceCollection services)
        {
            #region code/email_verification/aspnetcore/postverify_handler.cs
            services.AddStormpath(new StormpathOptions()
            {
                Configuration = new StormpathConfiguration(), // existing config, if any
                PostVerifyEmailHandler = (context, ct) =>
                {
                    return Task.FromResult(0);
                }
            });
            #endregion
        }
    }
}
