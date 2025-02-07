using Microsoft.EntityFrameworkCore;
using Npgsql.NameTranslation;
using Selah.Core.Models.Sql.AccountConnector;
using Selah.Core.Models.Sql.ApplicationUser;
using Selah.Core.Models.Sql.FinancialAccount;
using Selah.Core.Models.Sql.UserAccount;

namespace Selah.Infrastructure;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<ApplicationUserSql> ApplicationUsers { get; set; }
    public DbSet<UserAccountSql> UserAccounts { get; set; }
    public DbSet<AccountConnectorSql> AccountConnectors { get; set; }
    public DbSet<FinancialAccountSql?> FinancialAccounts { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply snake_case naming convention globally
        var mapper = new NpgsqlSnakeCaseNameTranslator();
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            // Convert table names to snake_case
            entity.SetTableName(mapper.TranslateMemberName(entity.GetTableName()));

            // Convert column names to snake_case
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(mapper.TranslateMemberName(property.Name));
            }

            // Convert key names to snake_case
            foreach (var key in entity.GetKeys())
            {
                key.SetName(mapper.TranslateMemberName(key.GetName()));
            }

            // Convert foreign key constraint names to snake_case
            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(mapper.TranslateMemberName(fk.GetConstraintName()));
            }

            // Convert index names to snake_case
            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(mapper.TranslateMemberName(index.GetDatabaseName()));
            }
        }
    }
}