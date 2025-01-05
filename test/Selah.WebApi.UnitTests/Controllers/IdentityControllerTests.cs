using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Selah.Application.Services.Interfaces;
using Selah.Core.ApiContracts;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models;
using Selah.WebAPI.Controllers;

namespace Selah.WebApi.UnitTests;

public class IdentityControllerTests
{
    private readonly Mock<IApplicationUserHttpService> _applicationUserHttpService;

    private IdentityController _controller;

    public IdentityControllerTests()
    {
        var userId = Guid.NewGuid();
        var appRequestContext = new AppRequestContext { UserId = userId };

        _applicationUserHttpService = new Mock<IApplicationUserHttpService>();
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "Bearer my_token";

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext,
        };

        _controller = new IdentityController(_applicationUserHttpService.Object)
            { ControllerContext = controllerContext };
    }

    [Fact]
    public async Task GetUserAsync_ShouldReturnUser()
    {
        _applicationUserHttpService.Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(new BaseHttpResponse<ApplicationUser> { StatusCode = 200 });

        var result = await _controller.GetCurrentUser();
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task GetUserAsync_ShouldUnAuthorized_WhenUserIsNotFound()
    {
        _applicationUserHttpService.Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(new BaseHttpResponse<ApplicationUser> { StatusCode = 401 });

        var result = await _controller.GetCurrentUser();
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Login_ShouldReturnAccessToken()
    {
        _applicationUserHttpService.Setup(x => x.LoginUser(It.IsAny<LoginRequest>()))
            .ReturnsAsync(new BaseHttpResponse<AccessTokenResponse> { StatusCode = 200 });
        var result = await _controller.Login(new LoginRequest());
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task Login_ShouldReturnUnAuthorized_OnInvalidLoginRequest()
    {
        _applicationUserHttpService.Setup(x => x.LoginUser(It.IsAny<LoginRequest>()))
            .ReturnsAsync(new BaseHttpResponse<AccessTokenResponse> { StatusCode = 401 });
        var result = await _controller.Login(new LoginRequest());
        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}