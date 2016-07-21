using System.Collections.Generic;
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

        public void ConfigureServices_ServerUri(IServiceCollection services)
        {
            #region code/configuration/aspnetcore/server_uri.cs
            services.AddStormpath(new StormpathConfiguration()
            {
                Web = new WebConfiguration()
                {
                    ServerUri = "http://localhost:5000"
                }
            });
            #endregion
        }

        public void ConfigureServices_VerifyEmailUri(IServiceCollection services)
        {
            #region code/email_verification/aspnetcore/configure_uri.cs
            services.AddStormpath(new StormpathConfiguration
            {
                Web = new WebConfiguration
                {
                    VerifyEmail = new WebVerifyEmailRouteConfiguration
                    {
                        Uri = "/verifyEmail"
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_LoginUri(IServiceCollection services)
        {
            #region code/login/aspnetcore/configure_uri.cs
            services.AddStormpath(new StormpathConfiguration
            {
                Web = new WebConfiguration
                {
                    Login = new WebLoginRouteConfiguration
                    {
                        Uri = "/logMeIn"
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_LoginChangeLabel(IServiceCollection services)
        {
            #region code/login/aspnetcore/configure_labels.cs
            services.AddStormpath(new StormpathConfiguration
            {
                Web = new WebConfiguration
                {
                    Login = new WebLoginRouteConfiguration
                    {
                        Form = new WebLoginRouteFormConfiguration
                        {
                            Fields = new Dictionary<string, WebFieldConfiguration>
                            {
                                ["login"] = new WebFieldConfiguration
                                {
                                    Label = "Email",
                                    Placeholder = "you@yourdomain.com"
                                },
                                ["password"] = new WebFieldConfiguration
                                {
                                    Placeholder = "Tip: Use a strong password!"
                                }
                            }
                        }
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_LogoutUris(IServiceCollection services)
        {
            #region code/logout/aspnetcore/configure_uris.cs
            services.AddStormpath(new StormpathConfiguration
            {
                Web = new WebConfiguration
                {
                    Logout = new WebLogoutRouteConfiguration
                    {
                        Uri = "/logMeOut",
                        NextUri = "/goodbye"
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_MeUri(IServiceCollection services)
        {
            #region code/me_api/aspnetcore/configure_uri.cs
            services.AddStormpath(new StormpathConfiguration
            {
                Web = new WebConfiguration
                {
                    Me = new WebMeRouteConfiguration
                    {
                        Uri = "/userDetails"
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_Oauth2UriAndStrategy(IServiceCollection services)
        {
            #region code/oauth2/aspnetcore/configure_uri_and_strategy.cs
            services.AddStormpath(new StormpathConfiguration
            {
                Web = new WebConfiguration
                {
                    Oauth2 = new WebOauth2RouteConfiguration
                    {
                        Uri = "/api/token",
                        Password = new WebOauth2PasswordGrantConfiguration
                        {
                            ValidationStrategy = WebOauth2TokenValidationStrategy.Stormpath
                        }
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_PasswordResetUris(IServiceCollection services)
        {
            #region code/password_reset/aspnetcore/configure_uris.cs
            services.AddStormpath(new StormpathConfiguration
            {
                Web = new WebConfiguration
                {
                    ForgotPassword = new WebForgotPasswordRouteConfiguration
                    {
                        Uri = "/forgot-password"
                    },
                    ChangePassword = new WebChangePasswordRouteConfiguration
                    {
                        Uri = "/change-password"
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_RegisterUri(IServiceCollection services)
        {
            #region code/registration/aspnetcore/configure_uri.cs
            services.AddStormpath(new StormpathConfiguration
            {
                Web = new WebConfiguration
                {
                    Register = new WebRegisterRouteConfiguration
                    {
                        Uri = "/createAccount"
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_RegisterFormFieldsRequired(IServiceCollection services)
        {
            #region code/registration/aspnetcore/configure_form_fields_required.cs
            services.AddStormpath(new StormpathConfiguration
            {
                Web = new WebConfiguration
                {
                    Register = new WebRegisterRouteConfiguration
                    {
                        Form = new WebRegisterRouteFormConfiguration
                        {
                            Fields = new Dictionary<string, WebFieldConfiguration>
                            {
                                ["givenName"] = new WebFieldConfiguration { Required = false },
                                ["surname"] = new WebFieldConfiguration { Required = false }
                            }
                        }
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_RegisterCustomFormField(IServiceCollection services)
        {
            #region code/registration/aspnetcore/configure_custom_form_field.cs
            services.AddStormpath(new StormpathConfiguration
            {
                Web = new WebConfiguration
                {
                    Register = new WebRegisterRouteConfiguration
                    {
                        Form = new WebRegisterRouteFormConfiguration
                        {
                            Fields = new Dictionary<string, WebFieldConfiguration>
                            {
                                ["favoriteColor"] = new WebFieldConfiguration
                                {
                                    Enabled = true,
                                    Visible = true,
                                    Label = "Favorite Color",
                                    Placeholder = "e.g. red, blue",
                                    Required = true,
                                    Type = "text"
                                }
                            }
                        }
                    }
                }
            });
            #endregion
        }

        public void ConfigureServices_RegisterFieldOrder(IServiceCollection services)
        {
            #region code/registration/aspnetcore/configure_field_order.cs
            services.AddStormpath(new StormpathConfiguration
            {
                Web = new WebConfiguration
                {
                    Register = new WebRegisterRouteConfiguration
                    {
                        Form = new WebRegisterRouteFormConfiguration
                        {
                            FieldOrder = new string[]
                            {
                                "surname",
                                "givenName",
                                "email",
                                "password"
                            }
                        }
                    }
                }
            });
            #endregion
        }
    }
}
