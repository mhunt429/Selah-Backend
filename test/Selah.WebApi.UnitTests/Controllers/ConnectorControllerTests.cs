using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Selah.Application.Services.Interfaces;
using Selah.Core.ApiContracts;
using Selah.Core.Models;
using Selah.Core.Models.Plaid;
using Selah.WebAPI.Controllers;

namespace Selah.WebApi.UnitTests.Controllers;

public class ConnectorControllerTests
{
    private readonly Mock<IAccountConnectorHttpService> _accountConnectorHttpService;

    private ConnectorController _controller;

    public ConnectorControllerTests()
    {
        var userId = Guid.NewGuid();
        var appRequestContext = new AppRequestContext { UserId = userId };

        _accountConnectorHttpService = new Mock<IAccountConnectorHttpService>();
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers.Authorization = "Bearer my_token";

        var controllerContext = new ControllerContext()
        {
            HttpContext = httpContext,
        };

        _controller = new ConnectorController(_accountConnectorHttpService.Object)
            { ControllerContext = controllerContext };
    }

    [Fact]
    public async Task LinkAccount_ShouldReturnSuccess_WhenLinkAccountIsSuccessful()
    {
        _accountConnectorHttpService.Setup(x => x.CreateLinkToken(It.IsAny<Guid>()))
            .ReturnsAsync(new BaseHttpResponse<PlaidLinkToken> { StatusCode = 200 });

        var result = await _controller.GetLinkToken();
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task LinkAccount_ShouldReturnBadRequest_WhenLinkAccountIsUnSuccessful()
    {
        _accountConnectorHttpService.Setup(x => x.CreateLinkToken(It.IsAny<Guid>()))
            .ReturnsAsync(new BaseHttpResponse<PlaidLinkToken> { StatusCode = 400 });

        var result = await _controller.GetLinkToken();
        Assert.IsType<BadRequestObjectResult>(result);
    }
}