using System.Threading;

namespace Stormpath.AspNetCore.Owin
{
    public interface IOwinEnvironment
    {
        CancellationToken CancellationToken { get; set; }
        IOwinRequest Request { get; }
        IOwinResponse Response { get; }
    }
}