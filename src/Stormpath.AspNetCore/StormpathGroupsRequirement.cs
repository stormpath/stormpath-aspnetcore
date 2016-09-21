using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Stormpath.AspNetCore
{
    public sealed class StormpathGroupsRequirement : IAuthorizationRequirement
    {
        public StormpathGroupsRequirement(params string[] groupNamesOrHrefs)
        {
            Groups = groupNamesOrHrefs;
        }

        public IReadOnlyCollection<string> Groups { get; private set; }
    }
}
