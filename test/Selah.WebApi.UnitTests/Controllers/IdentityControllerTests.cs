using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Selah.Application.Identity;
using Selah.Application.ApplicationUser;
using Selah.Core.ApiContracts;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models;
using Selah.WebAPI.Controllers;

namespace Selah.WebApi.UnitTests;

public class IdentityControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();

    private IdentityController _controller;

    public IdentityControllerTests()
    {
        var userId = Guid.NewGuid();
        var appRequestContext = new AppRequestContext { UserId = userId };

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "Bearer my_token";

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext,
        };

        _controller = new IdentityController(_mediatorMock.Object)
            { ControllerContext = controllerContext };
    }

    [Fact]
    public async Task GetUserAsync_ShouldReturnUser()
    {
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetUserById.Query>(), CancellationToken.None))
            .ReturnsAsync(new ApplicationUser
            {
                Id = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                Email = "test@test.com",
                Username = "test",
                FirstName = "Test",
                LastName = "User"
            });
        var result = await _controller.GetCurrentUser();
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetUserAsync_ShouldUnAuthorized_WhenUserIsNotFound()
    {
        _mediatorMock.Setup(x => x.Send(It.IsAny<GetUserById.Query>(), default))
            .ReturnsAsync((ApplicationUser)null);

        var result = await _controller.GetCurrentUser();
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Login_ShouldReturnAccessToken()
    {
        _mediatorMock.Setup(x => x.Send(It.IsAny<UserLogin.Command>(), default)).ReturnsAsync(new AccessTokenResponse());

        var result = await _controller.Login(new LoginRequest());
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Login_ShouldReturnUnAuthorized_OnInvalidLoginRequest()
    {
        _mediatorMock.Setup(x => x.Send(It.IsAny<UserLogin.Command>(), default)).ReturnsAsync((AccessTokenResponse)null);
        var result = await _controller.Login(new LoginRequest());
        Assert.IsType<UnauthorizedResult>(result);
    }
}