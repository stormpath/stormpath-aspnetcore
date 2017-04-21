using System;
using System.Threading;
using System.Threading.Tasks;
using Stormpath.Configuration.Abstractions;
using Stormpath.Owin.Middleware;

namespace Stormpath.AspNetCore
{
    public sealed class StormpathOptions
    {
        public StormpathConfiguration Configuration { get; set; }

        public Func<PreChangePasswordContext, CancellationToken, Task> PreChangePasswordHandler { get; set; }

        public Func<PostChangePasswordContext, CancellationToken, Task> PostChangePasswordHandler { get; set; }

        public Func<PreLoginContext, CancellationToken, Task> PreLoginHandler { get; set; }

        public Func<PostLoginContext, CancellationToken, Task> PostLoginHandler { get; set; }

        public Func<PreLogoutContext, CancellationToken, Task> PreLogoutHandler { get; set; }

        public Func<PostLogoutContext, CancellationToken, Task> PostLogoutHandler { get; set; }

        public Func<PreRegistrationContext, CancellationToken, Task> PreRegistrationHandler { get; set; }

        public Func<PostRegistrationContext, CancellationToken, Task> PostRegistrationHandler { get; set; }

        public Func<PreVerifyEmailContext, CancellationToken, Task> PreVerifyEmailHandler { get; set; }

        public Func<PostVerifyEmailContext, CancellationToken, Task> PostVerifyEmailHandler { get; set; }
    }
}
