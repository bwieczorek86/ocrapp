using Microsoft.AspNetCore.Mvc;
using OcrPlugin.App.Integrations.Softlex;

namespace OcrPlugin.App.BlazorClient.Server.Components.Integration;

[Route("api/[controller]")]
[ApiController]
public class IntegrationController : ControllerBase
{
    private readonly ISoftlexCloudIntegration _softlexCloudIntegration;

    public IntegrationController(ISoftlexCloudIntegration softlexCloudIntegration)
    {
        _softlexCloudIntegration = softlexCloudIntegration;
    }

    [HttpGet]
    public IActionResult Softlex()
    {
        _softlexCloudIntegration.IncrementData();
        return Ok("dupa");
    }
}