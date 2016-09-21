using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Stormpath.Owin.Middleware;

namespace Stormpath.AspNetCore
{
    public sealed class StormpathCustomDataHandler : AuthorizationHandler<StormpathCustomDataRequirement>
    {
        private readonly ScopedLazyUserAccessor _userAccessor;

        public StormpathCustomDataHandler(ScopedLazyUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, StormpathCustomDataRequirement requirement)
        {
            var account = _userAccessor.GetItem()?.Value;
            var filter = new RequireCustomDataFilter(requirement.Key, requirement.Value, requirement.Comparer);
            var result = await filter.IsAuthorizedAsync(account, CancellationToken.None);

            if (result)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
