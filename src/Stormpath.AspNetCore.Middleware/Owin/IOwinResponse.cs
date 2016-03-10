using System.Collections.Generic;
using System.IO;

namespace Stormpath.AspNetCore.Owin
{
    public interface IOwinResponse
    {
        Stream Body { get; set; }
        IDictionary<string, string[]> Headers { get; set; }
        string ReasonPhrase { set; }
        int StatusCode { set; }
    }
}