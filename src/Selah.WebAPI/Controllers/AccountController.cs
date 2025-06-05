using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Registration;
using Selah.Core.ApiContracts;
using Selah.Core.ApiContracts.AccountRegistration;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterAccount.Command command)
        {
           

            ApiResponseResult<AccessTokenResponse> result = await _mediator.Send(command);

            switch (result.status)
            {
                case ResultStatus.Success:
                    return Ok(new BaseHttpResponse<AccessTokenResponse>
                    {
                        StatusCode = 200,
                        Data = result.data,
                    });
                default:
                    return BadRequest(new BaseHttpResponse<AccessTokenResponse>
                    {
                        StatusCode = 400,
                        Data = null,
                        Errors = result.errors
                    });
            }
        }
    }
}