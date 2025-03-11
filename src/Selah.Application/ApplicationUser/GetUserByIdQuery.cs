using MediatR;
using Selah.Application.Mappings;
using Selah.Core.Models.Entities.ApplicationUser;
using Selah.Infrastructure.Repository;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.ApplicationUser;

public class GetUserById
{
    public class Query : IRequest<Core.ApiContracts.ApplicationUser>
    {
        public Guid UserId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Core.ApiContracts.ApplicationUser>
    {
        private readonly IApplicationUserRepository _repository;
        private readonly ICryptoService _cryptoService;

        public Handler(IApplicationUserRepository repository, ICryptoService cryptoService)
        {
            _repository = repository;
            _cryptoService = cryptoService;
        }

        public async Task<Core.ApiContracts.ApplicationUser> Handle(Query query, CancellationToken cancellationToken)
        {
            ApplicationUserEntity? userSql = await _repository.GetUserByIdAsync(query.UserId);
            if (userSql == null) return null!;
           
            return userSql.MapAppUserDataAccessToApiContract(_cryptoService);
        }
    }
}