// <copyright file="PrecompiledViewRenderer.cs" company="Stormpath, Inc.">
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
using Stormpath.SDK.Logging;

namespace Stormpath.AspNetCore
{
    public class PrecompiledViewRenderer : IViewRenderer
    {
        private readonly ILogger _logger;

        public PrecompiledViewRenderer(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<bool> RenderAsync(string name, object model, IOwinEnvironment context, CancellationToken cancellationToken)
        {
            var view = ViewResolver.GetView(name);
            if (view == null)
            {
                _logger.Trace($"View '{name}' is not a precompiled view", nameof(PrecompiledViewRenderer));
                return false;
            }

            cancellationToken.ThrowIfCancellationRequested();

            _logger.Trace($"Rendering precompiled view '{name}'", nameof(PrecompiledViewRenderer));

            await view.ExecuteAsync(model, context.Response.Body);
            return true;
        }
    }
}
