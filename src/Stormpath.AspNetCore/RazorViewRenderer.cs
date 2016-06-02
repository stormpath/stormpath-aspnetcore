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

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Stormpath.Owin.Abstractions;

namespace Stormpath.AspNetCore
{
    public class RazorViewRenderer : IViewRenderer
    {
        private static readonly string MicrosoftHttpContextKey = "Microsoft.AspNetCore.Http.HttpContext";

        public async Task RenderAsync(string viewName, object viewModel, IOwinEnvironment context, CancellationToken cancellationToken)
        {
            var httpContext = context.Request[MicrosoftHttpContextKey] as HttpContext;

            if (httpContext == null)
            {
                throw new InvalidOperationException($"Request dictionary must include item '{MicrosoftHttpContextKey}'");
            }

            var viewEngine = httpContext.RequestServices.GetRequiredService<ICompositeViewEngine>();
            var tempDataProvider = httpContext.RequestServices.GetRequiredService<ITempDataProvider>();
            var httpContextAccessor = httpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();

            bool isFullyQualifiedViewPath = viewName.Contains("/");
            var path = isFullyQualifiedViewPath
                ? viewName
                : $"~/Views/{viewName}.cshtml";

            var viewDataDictionary = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary());
            var actionContext = new ActionContext(httpContextAccessor.HttpContext, new Microsoft.AspNetCore.Routing.RouteData(), new ActionDescriptor());
            viewDataDictionary.Model = viewModel;

            var viewResult = viewEngine.FindView(actionContext, path, isMainPage: true);

            if (!viewResult.Success)
            {
                return;
            }

            using (var writer = new System.IO.StreamWriter(context.Response.Body))
            {
                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDataDictionary,
                    new TempDataDictionary(httpContextAccessor.HttpContext, tempDataProvider),
                    writer,
                    new HtmlHelperOptions());

                cancellationToken.ThrowIfCancellationRequested();
                await viewResult.View.RenderAsync(viewContext);
                writer.Flush();
            }
        }
    }
}
