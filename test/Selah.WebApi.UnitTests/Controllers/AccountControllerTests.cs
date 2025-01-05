using Microsoft.AspNetCore.Mvc;
using Moq;
using Selah.Application.Services.Interfaces;
using Selah.Core.ApiContracts;
using Selah.Core.ApiContracts.AccountRegistration;
using Selah.Core.ApiContracts.Identity;
using Selah.WebAPI.Controllers;

namespace Selah.WebApi.UnitTests.Controllers;

public class AccountControllerTests
{
    private readonly Mock<IRegistrationHttpService> _mockHttpService;

    private AccountController _controller; 

    public AccountControllerTests()
    {
        _mockHttpService = new Mock<IRegistrationHttpService>();
        _controller = new AccountController(_mockHttpService.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnOkResult()
    {
        _mockHttpService.Setup(x => x.RegisterAccount(It.IsAny<AccountRegistrationRequest>()))
            .ReturnsAsync(new BaseHttpResponse<AccessTokenResponse>{StatusCode = 200});
        
        var result = await _controller.Register(new AccountRegistrationRequest());
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task RegisterAsync_ShouldReturnBadRequestResult()
    {
        _mockHttpService.Setup(x => x.RegisterAccount(It.IsAny<AccountRegistrationRequest>()))
            .ReturnsAsync(new BaseHttpResponse<AccessTokenResponse>{StatusCode = 400});
        
        var result = await _controller.Register(new AccountRegistrationRequest());
        Assert.IsType<BadRequestObjectResult>(result);
    }
}