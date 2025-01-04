using Selah.Application.Queries.ApplicationUser;
using Selah.Application.Services.Interfaces;
using Selah.Core.ApiContracts;
using Selah.Core.ApiContracts.Identity;

namespace Selah.Application.Services;

public class ApplicationUserHttpService : IApplicationUserHttpService
{
    private readonly IGetUserByIdQuery _getUserByIdQuery;
    private readonly IUserLoginCommand _userLoginCommand;

    public ApplicationUserHttpService(IGetUserByIdQuery getUserByIdQuery, IUserLoginCommand userLoginCommand)
    {
        _getUserByIdQuery = getUserByIdQuery;
        _userLoginCommand = userLoginCommand;
    }

    public async Task<BaseHttpResponse<ApplicationUser>> GetById(Guid id)
    {
        ApplicationUser? user = await _getUserByIdQuery.GetAsync(id);

        return new BaseHttpResponse<ApplicationUser>
        {
            StatusCode = user != null ? 200 : 404,
            Data = user,
        };
    }

    public async Task<BaseHttpResponse<AccessTokenResponse>> LoginUser(LoginRequest loginRequest)
    {
        AccessTokenResponse? accessTokenResponse = await _userLoginCommand.Login(loginRequest);

        if (accessTokenResponse == null)
        {
            return new BaseHttpResponse<AccessTokenResponse>
            {
                StatusCode = 401,
                Data = null,
                Errors = new[] { "Invalid login credentials" }
            };
        }

        return new BaseHttpResponse<AccessTokenResponse>
        {
            StatusCode = 200,
            Data = accessTokenResponse,
        };
    }
}