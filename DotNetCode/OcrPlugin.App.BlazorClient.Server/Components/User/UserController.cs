using Microsoft.AspNetCore.Mvc;
using OcrPlugin.App.Common;

namespace OcrPlugin.App.BlazorClient.Server.Components.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("getcompanyname")]
        public IActionResult GetCompanyName()
        {
            var companyName = _httpContextAccessor.GetCompanyName();

            return Ok(companyName);
        }
    }
}