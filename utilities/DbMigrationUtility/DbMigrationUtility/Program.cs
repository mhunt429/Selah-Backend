using DbMigrationUtility;
using EvolveDb;
using Npgsql;
public class Program
{
    public static void Main(string[] args)
    {
        var connectionStrings = new ConnectionStrings();
        string location = "migrations";
        try
        {
            var connection = new NpgsqlConnection(connectionStrings.SelahDbLocal ??
                                                  "User ID=postgres;Password=postgres;Host=localhost;Port=65432;Database=postgres");
            var evolve = new Evolve(connection,
                msg => Console.WriteLine($"Beginning database migrations with {msg}"))
            {
                Locations = new[] { location }
            };
            evolve.Migrate();
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace.ToString());
            throw;
        }
    }
}