using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.ApplicationUser;
using Selah.Application.Identity;
using Selah.Core.Models;
using Selah.Core.ApiContracts;
using Selah.Core.ApiContracts.Identity;
using Selah.WebAPI.Extensions;

namespace Selah.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediatr;

    public IdentityController(IMediator mediatr)
    {
        _mediatr = mediatr;
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

        var query = new GetUserById.Query { UserId = userId };
        ApplicationUser? result = await _mediatr.Send(query);
        if (result == null)
        {
            return Unauthorized();
        }

        return Ok(result.ToBaseHttpResponse(HttpStatusCode.OK));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new UserLogin.Command { LoginRequest = request };

        var result = await _mediatr.Send(command);

        if (result == null)
        {
            return Unauthorized();
        }

        return Ok(result.ToBaseHttpResponse(HttpStatusCode.OK));
    }
}