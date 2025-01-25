using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Selah.Application.Registration;
using Selah.Application.Validators;
using Selah.Core.ApiContracts.Identity;
using Selah.Core.Models;
using Selah.WebAPI.Controllers;

namespace Selah.WebApi.UnitTests.Controllers;

public class AccountControllerTests
{
    private readonly Mock<IMediator> _mediatorMock = new();

    private AccountController _controller;


    public AccountControllerTests()
    {
        var userId = Guid.NewGuid();
        var appRequestContext = new AppRequestContext { UserId = userId };

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "Bearer my_token";

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext,
        };

        _controller = new AccountController(_mediatorMock.Object, new RegisterAccountValidator())
            { ControllerContext = controllerContext };
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnOkResult()
    {
        _mediatorMock.Setup(x => x.Send(It.IsAny<RegisterAccount.Command>(), CancellationToken.None))
            .ReturnsAsync(new AccessTokenResponse());

        var command = new RegisterAccount.Command
        {
            FirstName = "Hingle",
            LastName = "McCringleberry",
            Email = "testing123@test.com",
            Password = "AStrongPassword!42",
            PasswordConfirmation = "AStrongPassword!42",
        };

        var result = await _controller.Register(command);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task RegisterAsync_ShouldReturnBadRequestResult()
    {
        _mediatorMock.Setup(x => x.Send(It.IsAny<RegisterAccount.Command>(), CancellationToken.None))
            .ReturnsAsync(new AccessTokenResponse());

        var result = await _controller.Register(new RegisterAccount.Command());
        Assert.IsType<BadRequestObjectResult>(result);
    }
}