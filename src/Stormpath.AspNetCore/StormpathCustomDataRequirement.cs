using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Stormpath.AspNetCore
{
    public sealed class StormpathCustomDataRequirement : IAuthorizationRequirement
    {
        public StormpathCustomDataRequirement(string key, object value, IEqualityComparer<object> comparer = null)
        {
            Key = key;
            Value = value;
            Comparer = comparer;
        }

        public string Key { get; private set; }

        public object Value { get; private set; }

        public IEqualityComparer<object> Comparer { get; private set; }
    }
}
