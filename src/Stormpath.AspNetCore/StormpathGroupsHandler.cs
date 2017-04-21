using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Stormpath.Owin.Middleware;

namespace Stormpath.AspNetCore
{
    public sealed class StormpathGroupsHandler : AuthorizationHandler<StormpathGroupsRequirement>
    {
        private readonly ScopedLazyUserAccessor _userAccessor;

        public StormpathGroupsHandler(ScopedLazyUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, StormpathGroupsRequirement requirement)
        {
            var account = _userAccessor.Item?.Value;
            var filter = new RequireGroupsFilter(requirement.Groups);
            var result = filter.IsAuthorized(account);

            if (result)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.FromResult(true);
        }
    }
}
