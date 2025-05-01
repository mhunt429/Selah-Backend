using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.AccountConnector;
using Selah.Core.Models;
using Selah.Core.Models.Plaid;
using Selah.WebAPI.Extensions;
using Selah.WebAPI.Filters;

namespace Selah.WebAPI.Controllers;

[ApiController]
[Authorize]
[ValidAppRequestContextFilter]
[Route("api/[controller]")]
public class ConnectorController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConnectorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("link")]
    public async Task<IActionResult> GetLinkToken()
    {
        AppRequestContext requestContext = Request.GetAppRequestContext();
        Guid userId = requestContext.UserId;

        PlaidLinkToken result = await _mediator.Send(new CreateLinkToken.Command { UserId = userId });

        return Ok(result.ToBaseHttpResponse(HttpStatusCode.OK));
    }

    [HttpPost("exchange")]
    public async Task<IActionResult> ExchangeToken([FromBody] ExchangeLinkToken.Command request)
    {
        AppRequestContext requestContext = Request.GetAppRequestContext();
        Guid userId = requestContext.UserId;

        request.UserId = userId;

        ApiResponseResult<Unit> result = await _mediator.Send(request);

        switch (result.status)
        {
            case ResultStatus.Success:
                return NoContent();
            default:
                return BadRequest();
        }
    }
}