using OcrPlugin.App.Identity.Models;

namespace OcrPlugin.App.Identity
{
    internal interface IJwtTokenProvider
    {
        public string Get(ApplicationUser user);
    }
}