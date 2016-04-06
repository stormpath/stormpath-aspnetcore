// <copyright file="Serializer.cs" company="Stormpath, Inc.">
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

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Stormpath.AspNetCore
{
    internal static class Serializer
    {
        private static readonly JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static string Serialize(object @obj)
        {
            var serialized = JsonConvert.SerializeObject(@obj, Formatting.Indented, settings);
            return serialized; // todo one line
        }
    }
}
