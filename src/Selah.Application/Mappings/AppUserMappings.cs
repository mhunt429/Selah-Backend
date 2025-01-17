using Selah.Core.Models.Sql.ApplicationUser;
using Selah.Infrastructure.Services.Interfaces;

namespace Selah.Application.Mappings;

public static class AppUserMappings
{
    /// <summary>
    /// Takes a record from the app_user table and maps it to the API response for a given user
    /// </summary>
    /// <param name="user"></param>
    public static Core.ApiContracts.ApplicationUser MapAppUserDataAccessToApiContract(this ApplicationUserSql user, ICryptoService cryptoService)
    {
        string[] parsedName = cryptoService.Decrypt(user.EncryptedName).Split("|");
        return new Core.ApiContracts.ApplicationUser
        {
            Id = user.Id,
            AccountId = user.AccountId,
            Email = cryptoService.Decrypt(user.EncryptedEmail),
            Username = user.Username,
            FirstName = parsedName[0],
            LastName = parsedName[1],
            PhoneNumber = cryptoService.Decrypt(user.EncryptedPhone),
            CreatedDate = user.CreatedDate
        };
    }
}