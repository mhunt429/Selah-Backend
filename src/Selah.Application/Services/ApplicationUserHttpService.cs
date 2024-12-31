using Selah.Application.Queries.ApplicationUser;
using Selah.Application.Services.Interfaces;
using Selah.Core.ApiContracts;

namespace Selah.Application.Services;

public class ApplicationUserHttpService : IApplicationUserHttpService
{
    private readonly IGetUserByIdQuery _getUserByIdQuery;

    public ApplicationUserHttpService(IGetUserByIdQuery getUserByIdQuery)
    {
        _getUserByIdQuery = getUserByIdQuery;
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
}