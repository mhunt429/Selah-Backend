using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Npgsql.NameTranslation;
using Selah.Core.Models.Entities.AccountConnector;
using Selah.Core.Models.Entities.ApplicationUser;
using Selah.Core.Models.Entities.FinancialAccount;
using Selah.Core.Models.Entities.Identity;
using Selah.Core.Models.Entities.UserAccount;

namespace Selah.Infrastructure;

[ExcludeFromCodeCoverage]
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUserEntity> ApplicationUsers { get; set; }
    public DbSet<UserAccountEntity> UserAccounts { get; set; }
    public DbSet<AccountConnectorEntity> AccountConnectors { get; set; }
    public DbSet<FinancialAccountEntity> FinancialAccounts { get; set; }

    public DbSet<UserSessionEntity> UserSessions { get; set; }
}