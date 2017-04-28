using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Stormpath.AspNetCore
{
    public sealed class StormpathGroupsHandler : AuthorizationHandler<StormpathGroupsRequirement>
    {
        private readonly ScopedLazyUserAccessor _userAccessor;
        private readonly ScopedAuthorizationFilterFactoryAccessor _authzFilterFactoryAccessor;

        public StormpathGroupsHandler(
            ScopedLazyUserAccessor userAccessor,
            ScopedAuthorizationFilterFactoryAccessor authzFilterFactoryAccessor)
        {
            _userAccessor = userAccessor;
            _authzFilterFactoryAccessor = authzFilterFactoryAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, StormpathGroupsRequirement requirement)
        {
            var filter = _authzFilterFactoryAccessor?.Item?.CreateGroupFilter(requirement.Groups);
            if (filter == null) throw new InvalidOperationException("An internal error occurred. The AuhorizationFilterFactory was null.");

            var account = _userAccessor.Item?.Value;
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
