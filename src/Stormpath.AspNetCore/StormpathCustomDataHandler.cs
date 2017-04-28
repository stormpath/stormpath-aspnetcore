using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Stormpath.AspNetCore
{
    public sealed class StormpathCustomDataHandler : AuthorizationHandler<StormpathCustomDataRequirement>
    {
        private readonly ScopedLazyUserAccessor _userAccessor;
        private readonly ScopedAuthorizationFilterFactoryAccessor _authzFilterFactoryAccessor;

        public StormpathCustomDataHandler(
            ScopedLazyUserAccessor userAccessor,
            ScopedAuthorizationFilterFactoryAccessor authzFilterFactoryAccessor)
        {
            _userAccessor = userAccessor;
            _authzFilterFactoryAccessor = authzFilterFactoryAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, StormpathCustomDataRequirement requirement)
        {
            var filter = _authzFilterFactoryAccessor?.Item?.CreateCustomDataFilter(requirement.Key, requirement.Value, requirement.Comparer);
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
