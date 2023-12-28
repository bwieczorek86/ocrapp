using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OcrPlugin.App.Common;
using OcrPlugin.App.Core.AppSettings;
using OcrPlugin.App.Core.Models;

namespace OcrPlugin.App.BlazorClient.Server.Components.AppSettings
{
    [ApiController]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SettingsController(
            ISettingsManager settingsManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _settingsManager = settingsManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("getall")]
        [Authorize]
        public async Task<ICollection<CompanySettings>> GetAll()
        {
            return await _settingsManager.GetAll(_httpContextAccessor.GetCompanyName());
        }

        [HttpPost("update")]
        [Authorize]
        public async Task Update([FromBody] CompanySettings setting)
        {
            // TODO Add response
            await _settingsManager.Update(setting, _httpContextAccessor.GetCompanyName());
        }
    }
}