using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Selah.Core.ApiContracts.Crypto;
using Selah.Infrastructure.Services.Interfaces;
using Selah.WebAPI.Filters;

namespace Selah.WebAPI.Controllers;


//Utility endpoint for doing this in debug mode

[ApiController]
[DebugOnlyAsyncActionFilter]
[Route("api/[controller]")]

public class DebugController: ControllerBase
{

    private readonly ICryptoService _cryptoService;

    public DebugController(ICryptoService cryptoService)
    {
        _cryptoService = cryptoService;
    }
    
    [HttpPost("decrypt")]
    [Consumes("text/plain", "application/json")]
    public IActionResult Decrypt([FromBody] DecryptStringRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.EncryptedString))
        {
            return BadRequest("Missing encrypted string");
        }
        string decrypted = _cryptoService.Decrypt(request.EncryptedString);
        
        return Ok(new DecryptStringResponse{DecryptedString = decrypted});
    }
}