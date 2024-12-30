using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Application.Services.Interfaces;
using Selah.Core.ApiContracts.AccountRegistration;

namespace Selah.WebAPI.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IRegistrationHttpService _registrationHttpService;

        public AccountController(IRegistrationHttpService registrationHttpService)
        {
            _registrationHttpService = registrationHttpService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountRegistrationRequest request)
        {
            var result = await _registrationHttpService.RegisterAccount(request);

            if (result.StatusCode == 400)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}