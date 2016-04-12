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
using Stormpath.Owin.Common;
using Stormpath.Owin.Common.Views.Precompiled;

namespace Stormpath.AspNetCore
{
    public class RazorViewRenderer : IViewRenderer
    {
        public Task RenderAsync(string viewName, object viewModel, IOwinEnvironment context, CancellationToken cancellationToken)
        {
            var view = ViewResolver.GetView(viewName);
            if (view == null)
            {
                // todo defer to razor
                throw new InvalidOperationException($"View '{viewName}' not found.");
            }

            return view.ExecuteAsync(viewModel, context.Response.Body);
        }
    }
}
