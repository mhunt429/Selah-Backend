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
    public async Task<IActionResult> Login([FromBody] UserLogin.Command request)
    {
        UserLogin.Command.Response result = await _mediatr.Send(request);

        if (result == null)
        {
            return Unauthorized();
        }

        Response.Cookies.Append("x_sessionId", result.SessionId.ToString(), new CookieOptions
        {
            HttpOnly = true, 
            Secure = true, 
            SameSite = SameSiteMode.Strict, 
            Expires = result.SessionExpiration
        });

        return Ok(result.AccessToken.ToBaseHttpResponse(HttpStatusCode.OK));
    }
}