// <copyright file="CompositeViewRenderer.cs" company="Stormpath, Inc.">
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
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Stormpath.Owin.Abstractions;
using Stormpath.SDK.Logging;

namespace Stormpath.AspNetCore
{
    public class CompositeViewRenderer : IViewRenderer
    {
        private readonly IViewRenderer[] _renderers;
        private readonly IDictionary<IViewRenderer, string> _rendererNames;
        private readonly ILogger _logger;

        public CompositeViewRenderer(ILogger logger, params IViewRenderer[] renderers)
        {
            _logger = logger;
            _renderers = renderers ?? new IViewRenderer[] { };

            if (!_renderers.Any())
            {
                throw new ArgumentException("Must have at least one renderer", nameof(renderers));
            }

            _rendererNames = CacheRendererNames(_renderers);
        }

        private static Dictionary<IViewRenderer, string> CacheRendererNames(IViewRenderer[] renderers)
            => renderers.ToDictionary(r => r, r => r.GetType().Name);

        public async Task<bool> RenderAsync(string name, object model, IOwinEnvironment context, CancellationToken cancellationToken)
        {
            foreach (var renderer in _renderers)
            {
                if (await renderer.RenderAsync(name, model, context, cancellationToken))
                {
                    _logger.Info($"Rendered view '{name}' using {_rendererNames[renderer]}", nameof(CompositeViewRenderer));
                    return true;
                }
            }

            _logger.Error($"Could not render view '{name}' using any available renderer", nameof(CompositeViewRenderer));
            return false;
        }
    }
}
