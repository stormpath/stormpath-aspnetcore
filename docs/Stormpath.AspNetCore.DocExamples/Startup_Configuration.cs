using Microsoft.Extensions.DependencyInjection;
using Stormpath.Configuration.Abstractions;

namespace Stormpath.AspNetCore.DocExamples
{
    public class Startup_Configuration
    {

        public void ConfigureServices_Model(IServiceCollection services)
        {
            #region code/configuration/aspnetcore/inline_config.cs
            services.AddStormpath(new StormpathConfiguration()
            {
                Application = new ApplicationConfiguration()
                {
                    Name = "My Application"
                },
                Web = new WebConfiguration()
                {
                    Register = new WebRegisterRouteConfiguration()
                    {
                        Enabled = false
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_AnonymousModel(IServiceCollection services)
        {
            #region code/configuration/aspnetcore/anonymous_inline_config.cs
            services.AddStormpath(new
            {
                application = new
                {
                    name = "My Application"
                },
                web = new
                {
                    register = new
                    {
                        enabled = false
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_DisableDefaultFeatures(IServiceCollection services)
        {
            #region code/configuration/aspnetcore/disable_default_features.cs
            services.AddStormpath(new StormpathConfiguration()
            {
                Web = new WebConfiguration()
                {
                    ForgotPassword = new WebForgotPasswordRouteConfiguration()
                    {
                        Enabled = false
                    },
                    ChangePassword = new WebChangePasswordRouteConfiguration()
                    {
                        Enabled = false
                    },
                    Login = new WebLoginRouteConfiguration()
                    {
                        Enabled = false
                    },
                    Logout = new WebLogoutRouteConfiguration()
                    {
                        Enabled = false
                    },
                    Me = new WebMeRouteConfiguration()
                    {
                        Enabled = false
                    },
                    Oauth2 = new WebOauth2RouteConfiguration()
                    {
                        Enabled = false
                    },
                    Register = new WebRegisterRouteConfiguration()
                    {
                        Enabled = false
                    },
                    VerifyEmail = new WebVerifyEmailRouteConfiguration()
                    {
                        Enabled = false
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_ApiCredentials(IServiceCollection services)
        {
            #region code/configuration/aspnetcore/api_credentials.cs
            services.AddStormpath(new StormpathConfiguration()
            {
                Client = new ClientConfiguration()
                {
                    ApiKey = new ClientApiKeyConfiguration()
                    {
                        Id = "YOUR_API_KEY_ID",
                        Secret = "YOUR_API_KEY_SECRET"
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_DisableHtml(IServiceCollection services)
        {
            #region code/configuration/aspnetcore/disable_html_produces.cs
            services.AddStormpath(new StormpathConfiguration()
            {
                Web = new WebConfiguration()
                {
                    Produces = new string[] { "application/json" }
                }
            });
            #endregion
        }
    }
}
