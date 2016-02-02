// <copyright file="SdkConfiguration.cs" company="Stormpath, Inc.">
// Copyright (c) 2016 Stormpath, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System.Collections.Generic;

namespace Stormpath.Framework.Configuration
{
    public sealed class WebConfiguration
    {
        public string BasePath { get; private set; }

        public WebOauth2RouteConfiguration Oauth2 { get; private set; }

        public WebExpandConfiguration Expand { get; private set; }

        public WebOauth2TokenCookieConfiguration AccessTokenCookie { get; private set; }

        public WebOauth2TokenCookieConfiguration RefreshTokenCookie { get; private set; }

        public IReadOnlyList<string> Produces { get; private set; }

        public WebRegisterRouteConfiguration Register { get; private set; }

        public WebVerifyEmailRouteConfiguration VerifyEmail { get; private set; }

        public WebLoginRouteConfiguration Login { get; private set; }

        public WebLogoutRouteConfiguration Logout { get; private set; }

        public WebForgotPasswordRouteConfiguration ForgotPassword { get; private set; }

        public WebChangePasswordRouteConfiguration ChangePassword { get; private set; }

        public WebIdSiteRouteConfiguration IdSite { get; private set; }

        public WebSocialProvidersConfiguration SocialProviders { get; private set; }

        public WebMeRouteConfiguration Me { get; private set; }

        public WebSpaConfiguration Spa { get; private set; }

        public WebUnauthorizedConfiguration Unauthorized { get; private set; }
    }
}
