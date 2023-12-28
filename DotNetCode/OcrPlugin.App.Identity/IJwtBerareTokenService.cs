using OcrPlugin.App.Db;

namespace OcrPlugin.App.Identity
{
    public interface IJwtBerareTokenService
    {
        public string CreateJwtBearerToken(ApplicationUser applicationUser);
    }
}
