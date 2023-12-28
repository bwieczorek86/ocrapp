using Microsoft.AspNetCore.Identity;

namespace OcrPlugin.App.Db
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }
        public string Company { get; set; }
    }
}