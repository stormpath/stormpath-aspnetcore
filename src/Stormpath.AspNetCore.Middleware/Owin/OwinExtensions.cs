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
    }
}
