using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Core.Models;
using Selah.Application.Services.Interfaces;
using Selah.Core.ApiContracts;
using Selah.WebAPI.Extensions;

namespace Selah.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IApplicationUserHttpService _applicationUserHttpService;

    public IdentityController(IApplicationUserHttpService applicationUserHttpService)
    {
        _applicationUserHttpService = applicationUserHttpService;
    }

    /// <summary>
    /// Endpoint to get the authenticated user by subject JWT claim
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("current-user")]
    public async Task<IActionResult> GetCurrentUser()
    {
        AppRequestContext? requestContext = Request.GetAppRequestContext();

        Guid userId = requestContext.UserId;

        BaseHttpResponse<ApplicationUser> result = await _applicationUserHttpService.GetById(userId);

        return result.StatusCode == 200 ? Ok(result) : Forbid();
    }
}