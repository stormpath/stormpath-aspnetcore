using System.Security.Claims;
using System.Threading.Tasks;
using Stormpath.SDK.Account;
using Stormpath.SDK.Client;

namespace Stormpath.AspNetCore
{
    public interface IIdentityTransformer
    {
        Task<ClaimsPrincipal> CreatePrincipalAsync(IClient client, IAccount account, string scheme);
    }
}
