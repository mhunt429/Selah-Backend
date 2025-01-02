using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Services.Interfaces;
using Selah.Core.Models;
using Selah.WebAPI.Extensions;

namespace Selah.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ConnectorController : ControllerBase
{
    private readonly IAccountConnectorHttpService _accountConnectorHttpService;

    public ConnectorController(IAccountConnectorHttpService accountConnectorHttpService)
    {
        _accountConnectorHttpService = accountConnectorHttpService;
    }

    [HttpGet("link")]
    public async Task<IActionResult> LinkAccount()
    {
        AppRequestContext? requestContext = Request.GetAppRequestContext();
        Guid userId = requestContext.UserId;
        
        var result = await _accountConnectorHttpService.CreateLinkToken(userId);
        return result.StatusCode == 200 ? Ok(result) : BadRequest(result);
    }
}