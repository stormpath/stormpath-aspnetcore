// <copyright file="ActionRouter.cs" company="Stormpath, Inc.">
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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
namespace Stormpath.AspNetCore
{
    /// <summary>
    /// Simple router that returns virtual paths.
    /// </summary>
    /// <remarks>
    /// When Razor is called outside of a controller, tag helpers and action links
    /// don't work unless a router is added to the ActionContext that can handle
    /// generating virtual paths.
    /// </remarks>
    public class ActionRouter : IRouter
    {
        public const string ControllerKey = "controller";
        public const string ActionKey = "action";

        public static readonly ActionRouter Instance = new ActionRouter();

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            object controller = null;
            object action = null;

            bool containsRouteData =
                context.Values.TryGetValue(ControllerKey, out controller)
                && context.Values.TryGetValue(ActionKey, out action);

            return containsRouteData
                ? new VirtualPathData(this, $"/{controller?.ToString()}/{action?.ToString()}")
                : null;
        }

        public Task RouteAsync(RouteContext context)
        {
            throw new NotImplementedException();
        }
    }
}
