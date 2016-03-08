// <copyright file="Error.cs" company="Stormpath, Inc.">
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
using Microsoft.AspNet.Http;
using Stormpath.AspNetCore.Models.Error;

namespace Stormpath.AspNetCore
{
    public static class Error
    {
        public static Task Create<T>(HttpContext context)
            where T : AbstractError, new()
        {
            var instance = new T();

            return Invoke(context, instance);
        }

        private static Task Invoke(HttpContext context, AbstractError error)
        {
            context.Response.StatusCode = error.StatusCode;

            if (error.Body != null)
            {
                // add cache-control
                // serialize
                // write to response
                throw new NotImplementedException();
            }
            else
            {
                return Task.FromResult(0);
            }
        }
    }
}
