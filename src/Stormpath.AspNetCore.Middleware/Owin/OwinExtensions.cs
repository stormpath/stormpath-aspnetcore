// Contains code from OWIN, licensed under Apache 2.0.

using System.Collections.Generic;

namespace Stormpath.AspNetCore.Owin
{
    public static class OwinExtensions
    {
        public static T Get<T>(this IDictionary<string, object> environment, string key)
        {
            object value;
            return environment.TryGetValue(key, out value)
                ? (T)value
                : default(T);
        }

        public static void Set<T>(this IDictionary<string, object> environment, string key, T value)
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

        public static string[] GetHeaders(this IDictionary<string, string[]> headers, string name)
        {
            string[] value;
            return headers != null && headers.TryGetValue(name, out value)
                ? value
                : new string[0];
        }

        public static string GetHeader(this IDictionary<string, string[]> headers, string name)
        {
            var values = GetHeaders(headers, name);
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

        public static IDictionary<string, string[]> SetHeader(this IDictionary<string, string[]> headers, string name, string value)
        {
            headers[name] = new[] { value };
            return headers;
        }
    }
}
