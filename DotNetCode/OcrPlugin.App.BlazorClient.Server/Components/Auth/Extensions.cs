using OcrPlugin.App.Azure.Storage.UserToCompanies;
using OcrPlugin.App.Identity.Models;

namespace OcrPlugin.App.BlazorClient.Server.Components.Auth
{
    public static class Extensions
    {
        public static ApplicationUser ToApplicationUser(this ApplicationUserEntity applicationUserEntity)
            => new()
            {
                Id = applicationUserEntity.Email,
                Email = applicationUserEntity.Email,
                UserName = applicationUserEntity.Email,
                EmailConfirmed = true,
                PasswordHash = applicationUserEntity.PasswordHash,
            };
    }
}