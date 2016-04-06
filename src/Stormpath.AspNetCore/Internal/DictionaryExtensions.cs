// <copyright file="DictionaryExtensions.cs" company="Stormpath, Inc.">
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

namespace Stormpath.AspNetCore.Internal
{
    public static class DictionaryExtensions
    {
        public static object GetOrNull(this IDictionary<string, object> source, string key)
        {
            object value = null;
            source.TryGetValue(key, out value);
            return value;
        }

        public static TValue Get<TValue>(this IDictionary<string, TValue> source, string key)
        {
            TValue value;
            return source.TryGetValue(key, out value)
                ? value
                : default(TValue);
        }

        public static T Get<T>(this IDictionary<string, object> source, string key)
        {
            object value;
            return source.TryGetValue(key, out value)
                ? (T)value
                : default(T);
        }

        public static string GetString(this IDictionary<string, string[]> collection, string name)
        {
            var values = Get(collection, name);
            if (values == null)
            {
                return string.Empty;
            }

            switch (values.Length)
            {
                case 0:
                    return string.Empty;
                case 1:
                    return values[0];
                default:
                    return string.Join(",", values);
            }
        }

        public static IDictionary<string, string[]> SetString(this IDictionary<string, string[]> collection, string name, string value)
        {
            collection[name] = new[] { value };
            return collection;
        }

        public static void SetOrRemove<T>(this IDictionary<string, object> environment, string key, T value)
        {
            if (Equals(value, default(T)))
            {
                environment.Remove(key);
            }
            else
            {
                environment[key] = value;
            }
        }
    }
}
