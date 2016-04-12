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

using System.Threading;
using System.Threading.Tasks;
using Stormpath.Owin.Abstractions;
using Stormpath.Owin.Views.Precompiled;

namespace Stormpath.AspNetCore
{
    public class PrecompiledDeferringViewRenderer : IViewRenderer
    {
        private readonly IViewRenderer backupRenderer;

        // TODO: In RC2+, this class will be refactored to actually use Razor for rendering everything. For now, this is a hack
        // that tries to use the precompiled views first, then defers to Razor.
        public PrecompiledDeferringViewRenderer(IViewRenderer useIfLookupFails)
        {
            this.backupRenderer = useIfLookupFails;
        }

        public Task RenderAsync(string viewName, object viewModel, IOwinEnvironment context, CancellationToken cancellationToken)
        {
            var view = ViewResolver.GetView(viewName);
            if (view == null)
            {
                return this.backupRenderer.RenderAsync(viewName, viewModel, context, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();
            return view.ExecuteAsync(viewModel, context.Response.Body);
        }
    }
}
