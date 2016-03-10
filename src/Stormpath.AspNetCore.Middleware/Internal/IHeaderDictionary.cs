// Contains code copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0.

using System.Collections.Generic;

namespace Stormpath.AspNetCore.Internal
{
    /// <summary>
    /// Represents HttpRequest and HttpResponse headers
    /// </summary>
    public interface IHeaderDictionary : IDictionary<string, string[]>
    {
        /// <summary>
        /// <c>IHeaderDictionary</c> has a different indexer contract than IDictionary, where it will return <c>new string[] { }</c> for missing entries.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The stored value, or <c>new string[] { }</c> if the key is not present.</returns>
        new string[] this[string key] { get; set; }
    }
}