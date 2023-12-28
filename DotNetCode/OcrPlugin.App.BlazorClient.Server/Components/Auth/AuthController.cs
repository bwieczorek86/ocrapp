using Microsoft.AspNetCore.Mvc;
using OcrPlugin.App.Azure.Storage.UserToCompanies;
using OcrPlugin.App.BlazorClient.Client.Common;
using OcrPlugin.App.Common;
using OcrPlugin.App.Core.Website;
using OcrPlugin.App.Db;
using OcrPlugin.App.Identity;

namespace OcrPlugin.App.BlazorClient.Server.Components.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IEmailToCompanyStorage _userStorage;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUrlCreator _urlCreator;
        private readonly IJwtBerareTokenService _jwtBerareTokenService;

        public AuthController(
            IEmailToCompanyStorage userStorage,
            IPasswordHasher passwordHasher,
            IUrlCreator urlCreator,
            IJwtBerareTokenService jwtBerareTokenService)
        {
            _userStorage = userStorage;
            _passwordHasher = passwordHasher;
            _urlCreator = urlCreator;
            _jwtBerareTokenService = jwtBerareTokenService;
        }

        // TODO przywr�ci� zwracanie validate urla i zwr�ci� do widoku
        [HttpPost("login")]
        [RequestSizeLimit(500)]
        public async Task<LoginResult> Login([FromBody]LoginRequest request, [FromQuery] string returnUrl)
        {
            var validUrl = _urlCreator.CreateRelative(returnUrl).Replace("%2f", "/");
            var user = await _userStorage.FindByName(request.UserName);
            if (user == null)
            {
                return new LoginResult { Message = "User does not exist", Success = false };
            }

            if (user.FailedLogInCount >= 10)
            {
                return new LoginResult { Message = "Account locked", Success = false };
            }

            var hashed = _passwordHasher.Hash(request.Password, user.Salt);
            if (hashed != user.PasswordHash)
            {
                await _userStorage.IncreaseFailedCount(user);
                return new LoginResult { Message = "User does not exist", Success = false };
            }

            await _userStorage.ResetFailedCount(user);

            var applicationUser = new ApplicationUser
            {
                UserName = user.Email,
                Email = user.Email,
                Company = user.CompanyTable
            };

            return new LoginResult
            {
                Message = "Login successful",
                JwtBearer = _jwtBerareTokenService.CreateJwtBearerToken(applicationUser),
                Email = applicationUser.Email,
                Success = true,
                ValidateUrl = validUrl
            };
        }
    }
}