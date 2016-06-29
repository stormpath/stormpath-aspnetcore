// <copyright file="RazorViewRenderer.cs" company="Stormpath, Inc.">
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
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Stormpath.Configuration.Abstractions.Immutable;
using Stormpath.Owin.Abstractions;
using Stormpath.Owin.Middleware;
using Stormpath.SDK.Account;
using Stormpath.SDK.Logging;

namespace Stormpath.AspNetCore
{
    public class RazorViewRenderer : IViewRenderer
    {
        private const string MicrosoftHttpContextKey = "Microsoft.AspNetCore.Http.HttpContext";

        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly ILogger _logger;

        public RazorViewRenderer(
            ICompositeViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            ILogger logger)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _logger = logger;
        }

        public async Task<bool> RenderAsync(string name, object model, IOwinEnvironment context, CancellationToken cancellationToken)
        {
            var httpContext = context.Request[MicrosoftHttpContextKey] as HttpContext;
            if (httpContext == null)
            {
                _logger.Error($"Request dictionary does not contain '{MicrosoftHttpContextKey}'", nameof(RazorViewRenderer));
                return false;
            }

            // TODO ideally this would be done by the existing middleware pipeline
            // if authentication and view rendering were split up
            GetUserIdentity(httpContext, _logger);

            var actionContext = GetActionContext(httpContext);

            ViewEngineResult viewEngineResult;
            if (IsApplicationRelativePath(name))
            {
                var basePath = Directory.GetCurrentDirectory();
                _logger.Trace($"Getting view '{name}' relative to '{basePath}'");
                viewEngineResult = _viewEngine.GetView(basePath, name, true);
            }
            else
            {
                viewEngineResult = _viewEngine.FindView(actionContext, name, true);
            }

            if (!viewEngineResult.Success)
            {
                _logger.Trace($"Could not find Razor view '{name}'", nameof(RazorViewRenderer));
                return false;
            }

            var view = viewEngineResult.View;

            using (var writer = new StreamWriter(context.Response.Body))
            {
                var viewDataDictionary = new ViewDataDictionary(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                {
                    Model = model
                };

                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    viewDataDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    writer,
                    new HtmlHelperOptions());

                cancellationToken.ThrowIfCancellationRequested();
                await view.RenderAsync(viewContext);

                return true;
            }
        }

        private static void GetUserIdentity(HttpContext httpContext, ILogger logger)
        {
            var config = httpContext.Items.Get<StormpathConfiguration>(OwinKeys.StormpathConfiguration);
            var scheme = httpContext.Items.Get<string>(OwinKeys.StormpathUserScheme);
            var account = httpContext.Items.Get<IAccount>(OwinKeys.StormpathUser);

            var handler = new RouteProtector(config.Web, null, null, null, logger);
            var isAuthenticatedRequest = handler.IsAuthenticated(scheme, scheme, account);

            if (isAuthenticatedRequest)
            {
                httpContext.User = AccountIdentityTransformer.CreatePrincipal(account, scheme);
            }

            if (httpContext.User == null)
            {
                httpContext.User = new GenericPrincipal(new GenericIdentity(""), new string[0]);
            }
        }

        // TODO need to pass the action name
        private static ActionContext GetActionContext(HttpContext httpContext)
        {
            var routeData = new RouteData();
            routeData.Values.Add(ActionRouter.ControllerKey, "Stormpath");
            routeData.Routers.Add(ActionRouter.Instance);

            var actionDescriptor = new ActionDescriptor()
            {
                RouteValues = new Dictionary<string, string>()
                {
                    [ActionRouter.ControllerKey] = "Stormpath",
                }
            };

            return new ActionContext(httpContext, routeData, actionDescriptor);
        }

        private static bool IsApplicationRelativePath(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            return name[0] == '~' || name[0] == '/';
        }
    }
}
