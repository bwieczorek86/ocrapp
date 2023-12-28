using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using OcrPlugin.App.Azure.Storage.UserToCompanies;
using OcrPlugin.App.Common;
using OcrPlugin.App.Identity;
using System.Security.Claims;

namespace OcrPlugin.App.BlazorClient.Server.Components.Auth
{
    public class RevalidatingIdentityAuthenticationStateProvider
        : RevalidatingServerAuthenticationStateProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RevalidatingIdentityAuthenticationStateProvider(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory scopeFactory,
            IPasswordHasher passwordHasher,
            IHttpContextAccessor httpContextAccessor)
            : base(loggerFactory)
        {
            _scopeFactory = scopeFactory;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        protected override async Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var scope = _scopeFactory.CreateScope();
            try
            {
                var userManager = scope.ServiceProvider.GetRequiredService<IEmailToCompanyStorage>();
                return await ValidateSecurityStampAsync(userManager, authenticationState.User);
            }
            finally
            {
                if (scope is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
                else
                {
                    scope.Dispose();
                }
            }
        }

        public async Task<bool> SignIn(LoginRequest applicationUser)
        {
            var scope = _scopeFactory.CreateScope();
            try
            {
                var emailToCompanyStorage = scope.ServiceProvider.GetRequiredService<IEmailToCompanyStorage>();

                var user = await emailToCompanyStorage.FindByName(applicationUser.UserName);
                if (user == null)
                {
                    return false;
                }

                var hashed = _passwordHasher.Hash(applicationUser.Password, user.Salt);
                if (hashed != user.PasswordHash)
                {
                    await emailToCompanyStorage.IncreaseFailedCount(user);
                    return false;
                }

                var identity = new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, applicationUser.UserName),
                        new Claim(CustomClaimTypes.Company, user.CompanyTable),
                    },
                    "apiauth_type");
                var claimsPrincipal = new ClaimsPrincipal(identity);

                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
                return true;
            }
            finally
            {
                if (scope is IAsyncDisposable asyncDisposable)
                {
                    await asyncDisposable.DisposeAsync();
                }
                else
                {
                    scope.Dispose();
                }
            }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var http = _httpContextAccessor.HttpContext;

            await Task.CompletedTask;

            return new AuthenticationState(http?.User);
        }

        private async Task<bool> ValidateSecurityStampAsync(IEmailToCompanyStorage userManager, ClaimsPrincipal principal)
        {
            var user = await userManager.FindByName(principal.Identity?.Name);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}