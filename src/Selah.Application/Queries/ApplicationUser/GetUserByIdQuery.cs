using Selah.Application.Mappings;
using Selah.Core.Models.Sql.ApplicationUser;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.Queries.ApplicationUser;

public interface IGetUserByIdQuery
{
    Task<Core.ApiContracts.ApplicationUser> GetAsync(Guid id);
}

public class GetUserByIdQuery: IGetUserByIdQuery
{
    private readonly IApplicationUserRepository _repository;
    private readonly ICryptoService _cryptoService;

    public GetUserByIdQuery(IApplicationUserRepository repository, ICryptoService cryptoService)
    {
        _repository = repository;
        _cryptoService = cryptoService;
    }
    public async Task<Core.ApiContracts.ApplicationUser> GetAsync(Guid id)
    {
        ApplicationUserSql userSql = await _repository.GetUserByIdAsync(id);

        if (userSql == null) return null;
        
        return userSql.MapAppUserDataAccessToApiContract(_cryptoService);
    }
}