using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stormpath.AspNetCore.Internal;

namespace Stormpath.AspNetCore.Owin
{
    public interface IOwinResponse
    {
        Stream Body { get; set; }
        IDictionary<string, string[]> Headers { get; }
        string ReasonPhrase { set; }
        int StatusCode { set; }

        Task WriteAsync(string text, Encoding encoding, CancellationToken cancellationToken = default(CancellationToken));
    }
}