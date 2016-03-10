using System.Collections.Generic;
using System.IO;

namespace Stormpath.AspNetCore.Owin
{
    public interface IOwinRequest
    {
        Stream Body { get; }
        string Method { get; }
        string Path { get; }
        string PathBase { get; }
        string QueryString { get; }
        IDictionary<string, string[]> Headers { get; }
    }
}