using Selah.Core.ApiContracts;

namespace Selah.Application.Services.Interfaces;

public interface IApplicationUserHttpService
{
    Task<BaseHttpResponse<ApplicationUser> >GetById(Guid id);
}
