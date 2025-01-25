using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Registration;
using Selah.Core.ApiContracts;
using Selah.Core.ApiContracts.AccountRegistration;
using Selah.Core.ApiContracts.Identity;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<AccountRegistrationRequest> _accountRegistrationRequestValidator;

        public AccountController(IMediator mediator,
            IValidator<AccountRegistrationRequest> accountRegistrationRequestValidator)
        {
            _mediator = mediator;
            _accountRegistrationRequestValidator = accountRegistrationRequestValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterAccount.Command command)
        {
            ValidationResult? validationResult = _accountRegistrationRequestValidator.Validate(command);
            if (!validationResult.IsValid)
            {
                return BadRequest(new BaseHttpResponse<AccessTokenResponse>
                {
                    StatusCode = 400,
                    Data = null,
                    Errors = validationResult.Errors.Select(x => x.ErrorMessage)
                });
            }

            AccessTokenResponse response = await _mediator.Send(command);

            return Ok(response);
        }
    }
}